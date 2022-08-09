using DcsExportLib.Extensions;
using NLua;

namespace DcsExportLib.Loaders
{
    /// <summary>
    /// Object to load devices from the <see cref="LuaTable"/>
    /// </summary>
    internal class DevicesLoader  : IDevicesLoader
    {
        public IDictionary<long, string> GetDevices(LuaTable devicesTable)
        {
            IDictionary<long, string> devices = new SortedDictionary<long, string>();
            
            if (!devicesTable.AreCorrectTypes<string, long>())
                return devices;

            if (devicesTable.Keys != null && devicesTable.Keys.Count > 0)
            {
                foreach (KeyValuePair<object, object> tableEntry in devicesTable)
                {
                    devices.Add((long)tableEntry.Value, (string)tableEntry.Key);
                }
            }

            if(devices.Count > 0)
                // add NONE device record
                devices.Add(0, "");

            return devices;
        }
    }
}
