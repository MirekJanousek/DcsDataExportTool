namespace DcsExportLib.Validation;

/// <summary>
/// The validator interface
/// </summary>
internal interface IValidator
{
    /// <summary>
    /// Gets the collection of the validation errors
    /// </summary>
    ICollection<string> Errors { get; }
}