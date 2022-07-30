using DcsClickableExportLib.Models;

namespace DcsClickableExportLib;

public interface IModuleLookup
{
    ICollection<DcsModuleInfo> GetInstalledModules(string dcsPath);
}