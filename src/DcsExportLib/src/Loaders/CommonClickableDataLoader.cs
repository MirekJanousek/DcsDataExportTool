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

                    // order element parts from negative step values to positive
                    clickElementsCollection.ForEach(c =>
                    {
                        c.ElementParts = c.ElementParts.OrderBy(ep => ep.ElementAction.StepValue).ToList();
                    });

                    // order elements by DeviceName and than by Hint
                    clickElementsCollection = clickElementsCollection.OrderBy(c => c.Device?.DeviceName)
                                                                     .ThenBy(c => c.Hint)
                                                                     .ToList();

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
                    long? dcsId = null;
                    long? actionId = null;

                    if (elementEntryTable["arg"] is LuaTable argsTable)
                    {
                        if (argsTable == null)
                            throw new InvalidOperationException("Element doesn't have argument defined!");

                        dcsId = (long?)argsTable[classIx];
                    }

                    if (elementEntryTable["action"] is LuaTable actionTable)
                    {
                        if (actionTable == null)
                            throw new InvalidOperationException("Element doesn't have argument defined!");

                        actionId = (long?)actionTable[classIx];
                    }

                    // TODO MJ: Some LEV controls have step value in "gain" not in arg_value
                    // ?? Check if the arg_value is 0 and use gain instead ?? Only for LEV controls ?? Try for others to see

                    IElementStepAction stepAction;

                    try
                    {
                        stepAction = GetElementStepAction(classIx,
                            elementEntryTable["arg_value"] as LuaTable, (LuaTable)elementEntryTable["arg_lim"]);
                    }
                    catch (Exception ex)
                    {
                        throw new InvalidOperationException($"Can't process the details of element step or limits. Element: {elementEntryTable[DcsVariables.Hint]}", ex);
                    }

                    // get the steps
                    newPart.ElementAction = stepAction;
                    newPart.DcsId = dcsId;
                    newPart.ActionId = actionId;

                    returnParts.Add(newPart);
                }
            }

            return returnParts;
        }

        private static IElementStepAction GetElementStepAction(int classIndex, LuaTable argValuesTable, LuaTable argLimitTable)
        {
            // get the argument values
            decimal value = Decimal.Parse(argValuesTable[classIndex].ToString());
            value = value.Trail(15);

            IElementStepAction returnStepAction = new ElementStepAction { StepValue = value };

            // if there are no limits defined for the element, leave them not filled
            // examples: M-2000C VOR/ILS Test
            if (argLimitTable == null || argLimitTable.Keys.Count == 0)
                return returnStepAction;

            // some modules don't have limits for second element actions defined (F-14)
            // so we will try to set class index to previous and go with its limits
            if (classIndex == 2 && argLimitTable[classIndex] == null)
            {
                classIndex -= 1;

#if DEBUG
                throw new InvalidOperationException("BEWARE BREAKPOINT! Skip manually.");
#endif
            }

            // if limits are table with limits per class
            if (argLimitTable[classIndex] is LuaTable table)
            {
                GetElementStepLimits(table, returnStepAction);
            }
            // if limits are same for all both elements
            else if(argLimitTable.ValuesAreNumbers() && argLimitTable.Keys.Count == 2)
            {
                FillElementStepLimit(1, argLimitTable[1].ToString(), returnStepAction);
                FillElementStepLimit(2, argLimitTable[2].ToString(), returnStepAction);
            }
            // if only one limit value is present
            else if(argLimitTable.ValuesAreNumbers() && argLimitTable.Keys.Count == 1)
            {
                // this is hit by strange definitions like Harrier Nozzle Control Lever PTN_487
                // which has only one limit value instead of 2 for given action
                FillElementStepLimit(1, "0", returnStepAction);
                FillElementStepLimit(2, argLimitTable[1].ToString(), returnStepAction);

#if DEBUG
                throw new InvalidOperationException(
                    "Unexpected limit case, that needs workaround and keep an eye on. This case works for AV8B. This exception is there to give notice in advance.");
#endif
            }
            else
            {
                throw new InvalidOperationException("Unexpected format of limits.");
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
