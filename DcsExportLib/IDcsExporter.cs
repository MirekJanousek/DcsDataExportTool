using DcsClickableExportLib.Models;

namespace DcsClickableExportLib;

public interface IDcsExporter
{
    /// <summary>
    /// Exports the clickable data of the given module
    /// </summary>
    /// <param name="moduleInfo">Exported module</param>
    public void ExportClickableData(DcsModuleInfo moduleInfo);
}