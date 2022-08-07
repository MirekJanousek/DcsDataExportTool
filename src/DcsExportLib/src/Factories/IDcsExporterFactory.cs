namespace DcsExportLib.Factories;

public interface IDcsExporterFactory
{
    IDcsExporter GetExporter(ExportSettings exportSettings);

    IModuleLookup GetModuleLookup();
}