using DcsExportLib.Exporters;
using DcsExportLib.Models;

namespace DcsExportLib.Factories
{
    internal class ExporterFactory : IExporterFactory
    {
        public IClickableDataExporter GetClickableDataExporter(DcsModuleInfo moduleInfo)
        {
            throw new NotImplementedException();
        }
    }
}
