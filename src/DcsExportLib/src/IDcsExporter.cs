using DcsExportLib.Models;

namespace DcsExportLib;

public interface IDcsExporter
{
    /// <summary>
    /// Gets or sets the configuration of the exporter
    /// </summary>
    ExportSettings Settings { get; set; }

    /// <summary>
    /// Exports the clickable data of the given module
    /// </summary>
    /// <param name="moduleInfo">Exported module</param>
    public void ExportClickableData(DcsModuleInfo moduleInfo);
}