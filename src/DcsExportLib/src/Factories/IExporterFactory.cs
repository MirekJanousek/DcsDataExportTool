using DcsExportLib.Exporters;
using DcsExportLib.Models;

namespace DcsExportLib.Factories;

internal interface IExporterFactory
{
    /// <summary>
    /// Gets the clickable data exporter based on the module
    /// </summary>
    /// <param name="moduleInfo">Module to be exported</param>
    /// <returns>The clickable data exporter</returns>
    IClickableDataExporter GetClickableDataExporter(DcsModuleInfo moduleInfo);
}