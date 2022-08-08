using DcsExportLib.DcsObjects;
using DcsExportLib.Models;

using NLua;

using System.Text;
using DcsExportLib.Enums;
using KeraLua;
using Lua = NLua.Lua;
using LuaFunction = KeraLua.LuaFunction;

namespace DcsExportLib.Exporters
{
    internal class CommonClickableDataLoader : IClickableDataLoader
    {
        private readonly Encoding _encoding = Encoding.UTF8;

        public DcsModule GetData(DcsModuleInfo moduleInfo)
        {
            using (Lua lua = new Lua())
            {
                lua.State.Encoding = Encoding.UTF8;

                LockOnOptions options = new LockOnOptions(moduleInfo.ScriptFolder);
                lua["LockOn_Options"] = options;

                // TODO: pass the info differently
                string clickableDataPath = moduleInfo.ClickableElementsFolderPath;

                lua.DoString("package.path = package.path .. ';Scripts/?.lua'");
                lua.DoFile(ProgramPaths.ExportFunctionsFilePath);

                // dictionary of plane devices
                IDictionary<long, string> devices = GetDeviceNames(moduleInfo);

                lua.DoFile(clickableDataPath);
                var res = lua["elements"];

                List<ClickableElement> clickElementsCollection = new List<ClickableElement>();

                if (res is NLua.LuaTable resTable)
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

                    clickElementsCollection = clickElementsCollection.OrderBy(c => c.Device.DeviceName).ToList();

                }

                PrintElements(clickElementsCollection);
            }

            return null;
        }

        public ICollection<string> InitScriptPaths { get; init; }
        public string ClickableDataScriptPath { get; init; }

        private static void PrintElements(List<ClickableElement> clickElementsCollection)
        {
            var debugCol = clickElementsCollection.Where(c => c.ElementParts.Any(e => e.DcsId == 0)).ToList();
            foreach (var clickableElement in clickElementsCollection)
            {
                foreach (var elementPart in clickableElement.ElementParts.OrderBy(p => p.ActionId))
                {
                    if (!(elementPart.ElementAction is NoElementStepAction))
                    {
                        PrintElementByByActions(clickableElement, elementPart, elementPart.ElementAction);
                    }
                }
            }
        }

        private static void PrintElementByByActions(ClickableElement element, ClickableElementPart part, IElementStepAction stepDetail)
        {
            // DCS ID 0 is valid value
            string dcsIdStr = part.DcsId > -1 ? part.DcsId.ToString() : "NONE";

            Console.WriteLine($"{element.Device.DeviceName}({element.Device.DeviceId})\t{part.ActionId}\t{element.Name}\t{part.Type}\t{dcsIdStr}\t{stepDetail.StepValue}\t{stepDetail.MinLimit}\t{stepDetail.MaxLimit}\t{element.Hint}");
        }

        private static IDictionary<long, string> GetDeviceNames(DcsModuleInfo moduleInfo)
        {
            SortedDictionary<long, string> returnDictionary = new SortedDictionary<long, string>();

            // TODO: pass info differently
            string devicesPath = Path.Combine(moduleInfo.ScriptFolder, DcsPaths.DevicesScriptName);

            using (Lua lua = new Lua())
            {
                lua.State.Encoding = Encoding.UTF8;
                lua.DoFile(devicesPath);
                var devices = lua["devices"];

                if (devices is NLua.LuaTable devicesTable)
                {
                    foreach (KeyValuePair<object, object> deviceRecord in devicesTable)
                    {
                        long deviceId = -1;

                        // index parsing
                        if (deviceRecord.Value is long deviceIdLong)
                        {
                            deviceId = deviceIdLong;
                        }

                        if (deviceRecord.Key is string deviceName && deviceId > -1)
                        {
                            returnDictionary.Add(deviceId, deviceName);
                        }
                    }
                }
            }

            // add NO DEVICE record
            returnDictionary.Add(0, "");

            return returnDictionary;
        }

        private static ClickableElement ExportElementEntry(string elementName, LuaTable elementEntryTable,
            IDictionary<long, string> devices)
        {
            string hintStr = elementEntryTable["hint"].ToString();

            AircraftDevice device = new AircraftDevice
            {
                DeviceId = (long)elementEntryTable["device"],
                DeviceName = devices.Single(d => d.Key == (long)elementEntryTable["device"]).Value
            };
            
            // get element parts
            ICollection<ClickableElementPart> parts = GetElementParts(elementEntryTable);


            ClickableElement clickElement = new ClickableElement
            {
                Device = device,
                Name = elementName,
                Hint = elementEntryTable["hint"].ToString(),
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
