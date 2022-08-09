using DcsExportLib.Exporters;
using DcsExportLib.Factories;
using DcsExportLib.Models;

namespace DcsExportLib
{
    internal class DcsExporter : IDcsExporter
    {
        private readonly ILoaderFactory _loaderFactory;

        public DcsExporter(ILoaderFactory loaderFactory)
        {
            _loaderFactory = loaderFactory ?? throw new ArgumentNullException(nameof(loaderFactory));
        }

        public ExportSettings Settings { get; set; } = new();

        public DcsModule? Export(DcsModuleInfo moduleInfo)
        {
            if(moduleInfo == null)
                throw new ArgumentNullException(nameof(moduleInfo));

            // TODO MJ: check existing Config
            ValidationConfiguration();

            // TODO MJ: validation of the selected module in case someone fills it manually when used as library

            IClickableDataLoader clickableDataExporter = _loaderFactory.GetClickableDataLoader(moduleInfo);
            var exportedModule = clickableDataExporter.GetData(moduleInfo);

            return exportedModule;
        }

        private void ValidationConfiguration()
        {
            if (string.IsNullOrWhiteSpace(Settings.DcsFolderPath))
                throw new ArgumentException("DCS installation path is missing!");
        }
    }
}
