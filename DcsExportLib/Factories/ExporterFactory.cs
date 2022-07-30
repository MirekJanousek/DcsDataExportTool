using DcsClickableExportLib.Exporters;
using DcsClickableExportLib.Models;

namespace DcsClickableExportLib.Factories
{
    internal class ExporterFactory : IExporterFactory
    {
        public IClickableDataExporter GetClickableDataExporter(DcsModuleInfo moduleInfo)
        {
            throw new NotImplementedException();
        }
    }
}
