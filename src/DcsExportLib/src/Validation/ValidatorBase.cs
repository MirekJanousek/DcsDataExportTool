namespace DcsExportLib.Validation
{
    /// <summary>
    /// The validator base class
    /// </summary>
    internal abstract class ValidatorBase : IValidator
    {
        public ICollection<string> Errors { get; } = new List<string>();
    }
}
