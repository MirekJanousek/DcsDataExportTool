namespace DcsExportLib.Models
{
    public class DcsModuleInfo
    {
        public string Name { get; init; }

        public string Info { get; init; }

        public string ModulePath { get; init; }

        public string ModuleDirectoryName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(ModulePath))
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(ModulePath);
                    return dirInfo.Name;
                }

                return string.Empty;
            }
        }
    }
}
