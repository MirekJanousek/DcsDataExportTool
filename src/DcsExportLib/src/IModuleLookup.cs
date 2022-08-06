using DcsExportLib.Models;

namespace DcsExportLib;

public interface IModuleLookup
{
    ICollection<DcsModuleInfo> GetInstalledModules(string dcsPath);
}