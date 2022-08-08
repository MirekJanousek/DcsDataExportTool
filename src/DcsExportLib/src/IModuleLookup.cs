using DcsExportLib.Models;

namespace DcsExportLib;

public interface IModuleLookup
{
    /// <summary>
    /// Gets the list of the installed modules
    /// </summary>
    /// <param name="dcsPath">Path to DCS installation folder</param>
    /// <returns>Collection of the installed modules</returns>
    ICollection<DcsModuleInfo> GetInstalledModules(string dcsPath);
}