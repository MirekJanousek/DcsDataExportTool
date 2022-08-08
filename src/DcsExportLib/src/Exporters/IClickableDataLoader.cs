using DcsExportLib.Models;

namespace DcsExportLib.Exporters;

/// <summary>
/// The interface of the clickable data loader
/// </summary>
internal interface IClickableDataLoader
{
    DcsModule GetData(DcsModuleInfo moduleInfo);

    /// <summary>
    /// Gets the collection of path for scripts that need to run before clickable data script
    /// </summary>
    ICollection<string> InitScriptPaths { get; init; }

    /// <summary>
    /// Gets the clickable data script path
    /// </summary>
    string ClickableDataScriptPath { get; init; }
}