using DcsExportLib.Enums;
using DcsExportLib.Extensions;
using DcsExportLib.Models;

namespace DcsExportLib.Builders
{
    internal class DcsModuleInfoBuilder : IDcsModuleInfoBuilder
    {
        private const string DisplayNameProperty = "displayName";
        private const string ShortNameProperty = "shortName";
        private const string InfoProperty = "info";

        public DcsModuleInfo? Build(string moduleBaseDirPath)
        {
            if (!IsValidDirectory(moduleBaseDirPath))
                return null;

            // must contain entry.lua
            FileInfo[] entryFileInfos = new DirectoryInfo(moduleBaseDirPath).GetFiles(DcsPaths.EntryFileName);

            if (entryFileInfos.Length != 1)
                return null;
            
            string entryFileContent;

            using (var fileStream = entryFileInfos[0].OpenRead())
            {
                using (TextReader tr = new StreamReader(fileStream))
                {
                    entryFileContent = tr.ReadToEnd();
                }
            }

            if (string.IsNullOrEmpty(entryFileContent))
                return null;

            string moduleName = GetModuleName(entryFileContent);
            string moduleInfo = GetModuleInfo(entryFileContent);
            string clickableScriptPath = GetClickableElementsScriptPath(moduleBaseDirPath);

            if (new[] { moduleName, clickableScriptPath }.Any(i => i == string.Empty))
                return null;

            return new DcsModuleInfo
            {
                Name = moduleName,
                Info = moduleInfo,
                ClickableElementsFolderPath = clickableScriptPath,
                ModuleBaseFolderPath = moduleBaseDirPath
            };
        }

        // TODO: unit test passing this method with param scenarios below
        //  - scenario where path is path to file
        //  - non existing folder path
        //  - wrong syntax path
        //  - path with wrong characters
        private bool IsValidDirectory(string moduleBaseDirPath)
        {
            if (string.IsNullOrWhiteSpace(moduleBaseDirPath))
                return false;

            DirectoryInfo dirInfo;
            try
            {
                dirInfo = new DirectoryInfo(moduleBaseDirPath);
            }
            catch
            {
                return false;
            }

            return dirInfo.Exists;
        }

        /// <summary>
        /// Gets the module name from content of the entry.lua file
        /// </summary>
        /// <param name="entryFileContent">Content of the entry.lua file</param>
        /// <returns>The name of the module</returns>
        private string GetModuleName(string entryFileContent)
        {
            string name = GetFirstLuaPropertyValue(entryFileContent, DisplayNameProperty);

            if (string.IsNullOrWhiteSpace(name))
                name = GetFirstLuaPropertyValue(entryFileContent, ShortNameProperty);

            return string.IsNullOrEmpty(name) ? string.Empty : name;
        }

        /// <summary>
        /// Gets the module info from content of the entry.lua file
        /// </summary>
        /// <param name="entryFileContent">Content of the entry.lua file</param>
        /// <returns>The info string about the module</returns>
        private string GetModuleInfo(string entryFileContent)
        {
            string info = GetFirstLuaPropertyValue(entryFileContent, InfoProperty);
            
            return string.IsNullOrEmpty(info) ? string.Empty : info;
        }

        /// <summary>
        /// Gets the path to clickable data script
        /// </summary>
        /// <param name="moduleBaseDirPath">Module's base folder path</param>
        /// <returns>Path to clickable data script file</returns>
        private string GetClickableElementsScriptPath(string moduleBaseDirPath)
        {
            // search for clickable data script file
            FileInfo[] entryFileInfos = new DirectoryInfo(moduleBaseDirPath).GetFiles(DcsPaths.ClickableDataScriptName, SearchOption.AllDirectories);

            if(entryFileInfos.Length == 0 || entryFileInfos.Length > 1)
                return String.Empty;

            return entryFileInfos[0].FullName;
        }

        /// <summary>
        /// Gets the data of the first occurrence of given property name in the entry file's content
        /// </summary>
        /// <param name="entryFileContent">Content of the entry.lua file</param>
        /// <param name="property">Name of the property</param>
        /// <returns>Value of the property</returns>
        private string GetFirstLuaPropertyValue(string entryFileContent, string property)
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
