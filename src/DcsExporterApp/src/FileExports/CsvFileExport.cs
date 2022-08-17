using CsvHelper;

using DcsExportLib.Models;

using System.Globalization;

namespace DCSExporterApp.FileExports
{
    internal class CsvFileExport : ICsvFileExport
    {
        private static string FileExtension = "csv";
        
        public void Export(string directoryPath, DcsModule module)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(directoryPath);
            
            if(!dirInfo.Exists)
                dirInfo.Create();

            string fullPath = Path.Combine(directoryPath, $"{module.Info.ModuleDirectoryName}.{FileExtension}");

            // TODO remove invalid characters from path
            using (var writer = new StreamWriter(fullPath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<CsvElementExportDataMap>();

                List<ElementExportData> data = new List<ElementExportData>();

                foreach (var element in module.Elements)
                {
                    foreach (var part in element.ElementParts)
                    {
                        ElementExportData dataItem = new ElementExportData()
                        {
                            ElementName = element.Name,
                            Description = element.Hint,
                            DeviceId = element.Device?.DeviceId ?? null,
                            DeviceName = element.Device?.DeviceName ?? string.Empty,
                            ElementType = part.Type,
                            PartDcsId = part.DcsId,
                            ActionId = part.ActionId,
                            StepValue = part.ElementAction.StepValue,
                            LimitMax = part.ElementAction.MaxLimit,
                            LimitMin = part.ElementAction.MinLimit
                        };

                        data.Add(dataItem);
                    }
                }

                csv.WriteRecords(data);
            }
        }
    }
}
