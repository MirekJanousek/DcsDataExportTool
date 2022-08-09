using CsvHelper;
using CsvHelper.Configuration;

using DcsExportLib.Models;

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

        private string ConvertElementPartFunction(ConvertToStringArgs<ClickableElement> args)
        {
            return args.Value.Hint;
        }

        private string ConvertDevice(ConvertToStringArgs<ElementExportData> args)
        {
            return $"{args.Value.DeviceName}({args.Value.DeviceId})";
        }
    }
}
