using DcsExportLib.Builders;
using DcsExportLib.Enums;
using DcsExportLib.Models;

namespace DcsExportLib
{
    internal class ModuleLookup : IModuleLookup
    {
        private readonly IDcsModuleInfoBuilder _moduleBuilder;

        public ModuleLookup(IDcsModuleInfoBuilder moduleBuilder)
        {
            _moduleBuilder = moduleBuilder ?? throw new ArgumentNullException(nameof(moduleBuilder));
        }

        public ICollection<DcsModuleInfo> GetInstalledModules(string dcsPath)
        {
            DirectoryInfo dcsDirInfo = new DirectoryInfo(dcsPath);

            if (!dcsDirInfo.Exists)
                throw new DirectoryNotFoundException($"DCS installation directory was not found! Searched path was:{dcsPath}");

            string aircraftModsPath = Path.Combine(dcsPath, DcsPaths.AircraftModsPath);
            DirectoryInfo airModsDirInfo = new DirectoryInfo(aircraftModsPath);

            // TODO MJ: test and catch exceptions from combining incorrect paths
            if (!airModsDirInfo.Exists)
                throw new DirectoryNotFoundException("DCS aircraft mods directory was not found!");

            DirectoryInfo[] moduleDirInfos = airModsDirInfo.GetDirectories();
            
            List<DcsModuleInfo> allModulesList = new List<DcsModuleInfo>();

            foreach (var moduleDirectoryInfo in moduleDirInfos)
            {
                DcsModuleInfo? foundModule = _moduleBuilder.Build(moduleDirectoryInfo.FullName);

                if (foundModule == null)
                    continue;

                allModulesList.Add(foundModule);
            }            
            return allModulesList.OrderBy(i => i.Name).ToList();
        }
    }
}