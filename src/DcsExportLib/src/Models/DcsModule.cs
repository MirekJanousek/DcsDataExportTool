namespace DcsExportLib.Models
{
    public class DcsModuleInfo
    {
        public DcsModuleInfo(string name, string info)
        {
            Name = name;
            Info = info;
        }

        public string Name { get; }

        public string Info { get; }
    }
}
