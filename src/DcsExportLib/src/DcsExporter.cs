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

        public void ExportClickableData(DcsModuleInfo moduleInfo)
        {
            if(moduleInfo == null)
                throw new ArgumentNullException(nameof(moduleInfo));

            // TODO MJ: check existing Config
            ValidationConfiguration();

            // TODO MJ: validation of the selected module in case someone fills it manually when used as library

            IClickableDataLoader clickableDataExporter = _loaderFactory.GetClickableDataLoader(moduleInfo);
            var exportedModule = clickableDataExporter.GetData(moduleInfo);

            //IList<ClickableElement> clickElements = elements.ToObject<IList<ClickableElement>>();

            //StringBuilder sb = new StringBuilder();

            //foreach(TablePair pair in elements.Pairs)
            //{
            //    Table properties = pair.Value.Table;
            //    sb.Append(properties["element_id"]);
            //    sb.Append(' ');
            //    sb.Append(properties["device"]);
            //    sb.Append(' ');
            //    sb.Append(properties["action"]);
            //    sb.Append(' ');
            //    sb.Append(properties["hint"]);
            //    //sb.Append(properties["class"]);

            //    Console.WriteLine(sb.ToString());
            //    sb.Clear();
            //}

        }

        private void ValidationConfiguration()
        {
            if (string.IsNullOrWhiteSpace(Settings.DcsFolderPath))
                throw new ArgumentException("DCS installation path is missing!");
        }
    }
}
