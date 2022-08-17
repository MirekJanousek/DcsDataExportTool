using DcsExportLib.Enums;
using DcsExportLib.Extensions;
using DcsExportLib.Factories;
using DcsExportLib.Loaders;
using DcsExportLib.Models;

using NLua;

namespace DcsExportLib.Exporters
{
    internal class CommonClickableDataLoader : IClickableDataLoader
    {
        private readonly IDevicesLoader _devicesLoader;
        private readonly IExecutorFactory _executorFactory;

        public CommonClickableDataLoader(IDevicesLoader deviceLoader, IExecutorFactory executorFactory)
        {
            _devicesLoader = deviceLoader ?? throw new ArgumentNullException(nameof(deviceLoader));
            _executorFactory = executorFactory ?? throw new ArgumentNullException(nameof(executorFactory));
        }

        public DcsModule? GetData(DcsModuleInfo moduleInfo)
        {
            using (Lua lua = new Lua())
            {
                var executor = _executorFactory.GetExecutor(moduleInfo);
                LuaTable elementsTable = executor.ExecuteClickables(lua, moduleInfo);
                
                // dictionary of plane devices
                IDictionary<long, string> devices = new Dictionary<long, string>();

                if(lua[DcsVariables.Devices] is LuaTable devicesResult)
                    devices = _devicesLoader.GetDevices(devicesResult);

                List<ClickableElement> clickElementsCollection = new List<ClickableElement>();

                if (elementsTable is LuaTable resTable)
                {
                    foreach (KeyValuePair<object, object> elementEntry in resTable)
                    {
                        if (elementEntry.Key is string elementEntryStr)
                        {
                            if (elementEntry.Value is LuaTable elementEntryTable)
                            {
                                ClickableElement element =
                                    ExportElementEntry(elementEntryStr, elementEntryTable, devices);
                                clickElementsCollection.Add(element);
                            }
                        }
                    }

                    clickElementsCollection = clickElementsCollection.OrderBy(c => c.Device?.DeviceName).ToList();

                }
                
                return new DcsModule { Elements = clickElementsCollection, Info = moduleInfo };
            }
        }
        
        private static ClickableElement ExportElementEntry(string elementName, LuaTable elementEntryTable,
            IDictionary<long, string> devices)
        {
            AircraftDevice? elementDevice = null;
            
            // get aircraft device, if element has one assigned
            if (elementEntryTable[DcsVariables.Device] != null)
            {
                long deviceId = (long)elementEntryTable[DcsVariables.Device];

                elementDevice = new AircraftDevice
                {
                    DeviceId = deviceId,
                    DeviceName = devices.Single(d => d.Key == deviceId).Value
                };
            }
            
            // get element parts
            ICollection<ClickableElementPart> parts = GetElementParts(elementEntryTable);
            
            ClickableElement clickElement = new ClickableElement
            {
                Device = elementDevice,
                Name = elementName,
                Hint = elementEntryTable[DcsVariables.Hint].ToString() ?? string.Empty,
                ElementParts = parts
            };

            return clickElement;
        }

        private static ICollection<ClickableElementPart> GetElementParts(LuaTable elementEntryTable)
        {
            List<ClickableElementPart> returnParts = new List<ClickableElementPart>();

            if (elementEntryTable["class"] is LuaTable classTable)
            {
                for (int classIx = 1; classIx <= classTable.Keys.Count; classIx++)
                {
                    ClickableElementPart newPart = new ClickableElementPart();

                    // get the type of part
                    int classInt = Convert.ToInt32((long)classTable[classIx]);
                    newPart.Type = (ElementPartType)classInt;

                    // get the dcs id
                    int dcsIdInt = -1;
                    int actionId = -1;

                    if (elementEntryTable["arg"] is LuaTable argsTable)
                    {
                        if (argsTable.Keys.Count >= classIx)
                            dcsIdInt = Convert.ToInt32((long)argsTable[classIx]);
                    }

                    if (elementEntryTable["action"] is LuaTable actionTable)
                    {
                        if (actionTable.Keys.Count >= classIx)
                            actionId = Convert.ToInt32((long)actionTable[classIx]);
                    }

                    // TODO MJ: Some LEV controls have step value in "gain" not in arg_value
                    // ?? Check if the arg_value is 0 and use gain instead ?? Only for LEV controls ?? Try for others to see

                    IElementStepAction stepAction = GetElementStepAction(classIx, elementEntryTable["arg_value"] as LuaTable, (LuaTable)elementEntryTable["arg_lim"]);

                    // get the steps
                    newPart.ElementAction = stepAction;
                    newPart.DcsId = dcsIdInt;
                    newPart.ActionId = actionId;

                    returnParts.Add(newPart);
                }
            }

            return returnParts;
        }

        private static IElementStepAction GetElementStepAction(int classIndex, LuaTable argValuesTable, LuaTable argLimitTable)
        {
            IElementStepAction returnStepAction = new NoElementStepAction();

            // get the argument values
            decimal value = Decimal.Parse(argValuesTable[classIndex].ToString());
            value = value.Trail(15);

            returnStepAction = new ElementStepAction { StepValue = value };

            if (argLimitTable[classIndex] is LuaTable table)
            {
                GetElementStepLimits(table, returnStepAction);
            }
            else
            {
                // this is hit by strange definitions like Harrier Nozzle Control Lever PTN_487
                FillElementStepLimit(1, "0", returnStepAction);
                FillElementStepLimit(2, argLimitTable[1].ToString(), returnStepAction);

#if DEBUG
                throw new InvalidOperationException(
                    "Unexpected limit case, that needs workaround and keep an eye on. This case works for AV8B. This exception is there to give notice in advance.");
#endif
            }

            return returnStepAction;
        }

        private static void GetElementStepLimits(LuaTable argLimitTable, IElementStepAction stepDetail)
        {
            // get the argument values limits
            for (int limitIx = 1; limitIx <= 2; limitIx++)
            {
                // TODO MJ: type agnostic conversion
                // TODO MJ: round doubles to 15 decimal places
                FillElementStepLimit(limitIx, argLimitTable[limitIx].ToString(), stepDetail);
            }
        }

        private static void FillElementStepLimit(int limit, string limitStrValue, IElementStepAction stepDetail)
        {
            decimal value = Decimal.Parse(limitStrValue);
            value = value.Trail(15);

            if (limit == 1)
            {
                stepDetail.MinLimit = value;
            }
            else
            {
                stepDetail.MaxLimit = value;
            }
        }
    }
}
