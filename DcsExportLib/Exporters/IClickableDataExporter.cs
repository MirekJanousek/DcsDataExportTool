using DcsClickableExportLib.Models;

namespace DcsClickableExportLib.Exporters;

internal interface IClickableDataExporter
{
    void ExportData(DcsModuleInfo moduleInfo);
}