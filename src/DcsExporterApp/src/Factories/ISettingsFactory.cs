namespace DCSExporterApp.Factories;

internal interface ISettingsFactory
{
    T GetSettings<T>() where T : new();
}