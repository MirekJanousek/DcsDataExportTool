using DcsClickableExportLib.Exporters;
using DcsClickableExportLib.Factories;
using DcsClickableExportLib.Models;

using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Loaders;

namespace DcsClickableExportLib
{
    /// TODO MJ:
    /// Get script paths by module (enum?, config file with paths?)

    public class DcsExporter : IDcsExporter
    {
        private readonly string dcsPath_;

        public DcsExporter(string dcsPath)
        {
            dcsPath_ = dcsPath ?? throw new ArgumentNullException(nameof(dcsPath));
        }

        public void ExportClickableData(DcsModuleInfo moduleInfo)
        {
            if(moduleInfo == null)
                throw new ArgumentNullException(nameof(moduleInfo));

            // TODO MJ: validation of the selected module in case someone fills it manually when used as library
            ExporterFactory exporterFactory = new ExporterFactory();
            IClickableDataExporter clickableDataExporter = exporterFactory.GetClickableDataExporter(moduleInfo);

            clickableDataExporter.ExportData(moduleInfo);


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
    }
}
