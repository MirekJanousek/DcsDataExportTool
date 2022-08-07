using DcsExportLib.Exporters;
using DcsExportLib.Models;

namespace DcsExportLib.Factories;

/// <summary>
/// The interface of factory returning the loader factories
/// </summary>
internal interface ILoaderFactory
{
    /// <summary>
    /// Gets the clickable data loader based on the module
    /// </summary>
    /// <param name="moduleInfo">Module to be loaded</param>
    /// <returns>The clickable data loaded</returns>
    IClickableDataLoader GetClickableDataLoader(DcsModuleInfo moduleInfo);
}