using DcsExportLib.Models;

namespace DcsExportLib.Exporters;

/// <summary>
/// The interface of the clickable data loader
/// </summary>
internal interface IClickableDataLoader
{
    DcsModule GetData(DcsModuleInfo moduleInfo);
}