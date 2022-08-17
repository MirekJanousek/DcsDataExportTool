using DcsExportLib.Models;

namespace DCSExporterApp.FileExports
{
    internal class ElementExportData
    {
        public string DeviceName { get; init; } = string.Empty;

        public long? DeviceId { get; init; }
        
        public long ActionId { get; init; }

        public string ElementName { get; init; } = string.Empty;

        public ElementPartType ElementType { get; init; }

        public long PartDcsId { get; init; }

        public decimal StepValue { get; init; }

        public decimal LimitMin { get; init; }

        public decimal LimitMax { get; init; }

        public string Description { get; init; } = string.Empty;
    }
}
