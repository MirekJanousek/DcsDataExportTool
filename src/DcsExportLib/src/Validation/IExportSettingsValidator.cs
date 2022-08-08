namespace DcsExportLib.Validation;

public interface IExportSettingsValidator
{
    /// <summary>
    /// Validates the export settings
    /// </summary>
    /// <param name="exportSettings"><see cref="ExportSettings"/> instance to validate</param>
    /// <returns>Returns true if validation is successful</returns>
    bool Validate(ExportSettings exportSettings);
}