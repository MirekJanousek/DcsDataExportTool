using CsvHelper;
using CsvHelper.Configuration;

namespace DCSExporterApp.FileExports
{
    internal sealed class CsvElementExportDataMap : ClassMap<ElementExportData>
    {
        public CsvElementExportDataMap()
        {
            Map(m => m.DeviceName).Convert(ConvertDevice).Name("Device (ID)");
            Map(m => m.ElementName).Name("Element");
            Map(m => m.ActionId).Name("Button ID");
            Map(m => m.ElementType).Name("Type");
            Map(m => m.PartDcsId).Name("DCS ID");
            Map(m => m.StepValue).Name("Click Value");
            Map(m => m.LimitMin).Name("Limit Min");
            Map(m => m.LimitMax).Name("Limit Max");
            Map(m => m.Description).Name("Description");
        }

        /// <summary>
        /// Converts device into the CSV filed
        /// </summary>
        /// <param name="args">Conversion arguments</param>
        /// <returns>String representation of device field</returns>
        // ReSharper disable once MemberCanBeMadeStatic.Local
        private string ConvertDevice(ConvertToStringArgs<ElementExportData> args)
        {
            return args.Value?.DeviceId != null ? $"{args.Value.DeviceName}({args.Value.DeviceId})" : string.Empty;
        }
    }
}
