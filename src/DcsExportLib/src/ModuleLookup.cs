using DcsExportLib.Extensions;
using DcsExportLib.Models;

namespace DcsExportLib
{
    internal class ModuleLookup : IModuleLookup
    {
        private const string DisplayNameProperty = "displayName";
        private const string InfoProperty = "info";

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

            if (moduleDirInfos == null || moduleDirInfos.Length == 0)
                throw new DirectoryNotFoundException(
                    $"No aircraft mods directory was found at path: {airModsDirInfo.FullName}");

            List<DcsModuleInfo> allModulesList = new List<DcsModuleInfo>();

            foreach (var moduleDirectoryInfo in moduleDirInfos)
            {
                DcsModuleInfo? foundModule = GetModuleInfoFromEntry(moduleDirectoryInfo);

                if(foundModule != null)
                    allModulesList.Add(foundModule);
            }

            return allModulesList.OrderBy(i => i.Name).ToList();
        }

        private DcsModuleInfo? GetModuleInfoFromEntry(DirectoryInfo moduleDirectory)
        {
            const string entryFileName = "entry.lua";

            if (moduleDirectory == null)
                throw new ArgumentNullException(nameof(moduleDirectory));

            if (!moduleDirectory.Exists)
                throw new DirectoryNotFoundException($"Can't find directory: {moduleDirectory.FullName}");

            FileInfo[] entryFileInfos = moduleDirectory.GetFiles(entryFileName);

            if (entryFileInfos == null || entryFileInfos.Length != 1)
                throw new FileNotFoundException(
                    $"Cannot find file: {entryFileName} in directory: {moduleDirectory.FullName}");

            bool parsed = TryGetInfoFromEntry(entryFileInfos[0], out var dcsModuleInfo);

            return parsed ? dcsModuleInfo : null;
        }

        private bool TryGetInfoFromEntry(FileInfo entryFileInfo, out DcsModuleInfo? dcsModuleInfo)
        {
            dcsModuleInfo = null;

            if (entryFileInfo == null)
                throw new ArgumentNullException(nameof(entryFileInfo));

            if (!entryFileInfo.Exists)
                throw new FileNotFoundException($"Can't find file: {entryFileInfo.FullName}");

            string entryFileContent;

            using (var fileStream = entryFileInfo.OpenRead())
            {
                using (TextReader tr = new StreamReader(fileStream))
                {
                    entryFileContent = tr.ReadToEnd();
                }
            }

            if (string.IsNullOrEmpty(entryFileContent))
                return false;

            string displayName = GetLuaPropertyValue(entryFileContent, DisplayNameProperty);

            if (string.IsNullOrEmpty(displayName))
                return false;

            string info = GetLuaPropertyValue(entryFileContent, InfoProperty);

            dcsModuleInfo = new DcsModuleInfo { ModulePath = entryFileInfo.DirectoryName, Info = info, Name = displayName};
            return true;
        }

        private string GetLuaPropertyValue(string entryFileContent, string property)
        {
            if (string.IsNullOrEmpty(entryFileContent))
                return string.Empty;

            // Get property start index
            int dnIx = entryFileContent.IndexOf(property, StringComparison.Ordinal);

            if (dnIx == -1)
                return String.Empty;

            // TODO MJ: make sure to search for more variants of line endings
            int eolIx = entryFileContent.IndexOfLineEnd(dnIx);

            // Get the line with the property
            string line = entryFileContent.Substring(dnIx, (eolIx - dnIx));

            return line.FindQuotedToLineEnd();
        }

    }
}
