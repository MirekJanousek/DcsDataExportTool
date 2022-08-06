using DcsExportLib.Models;

namespace DcsExportLib.Exporters;

internal interface IClickableDataExporter
{
    void ExportData(DcsModuleInfo moduleInfo);
}