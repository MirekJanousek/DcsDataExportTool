using DcsExportLib.Models;

namespace DcsExportLib.Builders;

internal interface IDcsModuleInfoBuilder
{
    DcsModuleInfo? Build(string moduleBaseDirPath);
}