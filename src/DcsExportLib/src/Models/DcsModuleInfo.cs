namespace DcsExportLib.Models
{
    public class DcsModuleInfo
    {
        public string Name { get; init; }

        public string Info { get; init; }

        public string ModuleBaseFolderPath { get; init; }

        public string ClickableElementsFolderPath { get; init; }

        public string ModuleDirectoryName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ModuleBaseFolderPath))
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(ModuleBaseFolderPath);
                    return dirInfo.Name;
                }

                return string.Empty;
            }
        }
    }
}
