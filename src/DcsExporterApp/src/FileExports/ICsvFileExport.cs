using DcsExportLib.Models;

namespace DCSExporterApp.FileExports;

internal interface ICsvFileExport
{
    void Export(string directoryPath, DcsModule module);
}