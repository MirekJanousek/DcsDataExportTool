using NLua;

namespace DcsExportLib.Loaders;

/// <summary>
/// Interface of the object that loads devices from the <see cref="LuaTable"/>
/// </summary>
internal interface IDevicesLoader
{
    /// <summary>
    /// Gets the devices from the <see cref="LuaTable"/>
    /// </summary>
    /// <param name="devicesTable"><see cref="LuaTable"/> containing the devices</param>
    /// <returns>The dictionary of device ids and names</returns>
    IDictionary<long, string> GetDevices(LuaTable devicesTable);
}