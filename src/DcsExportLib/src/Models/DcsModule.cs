namespace DcsExportLib.Models
{
    /// <summary>
    /// The DCS module
    /// </summary>
    public class DcsModule
    {
        public DcsModuleInfo Info { get; set; } = new DcsModuleInfo();

        public ICollection<ClickableElement> Elements { get; set; } = new List<ClickableElement>();
    }
}
