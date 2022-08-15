using DcsExportLib.Executors;
using DcsExportLib.Models;

namespace DcsExportLib.Factories
{
    internal class ExecutorFactory : IExecutorFactory
    {
        public IExecutor GetExecutor(DcsModuleInfo moduleInfo)
        {
            return moduleInfo.Name switch
            {
                "Ka-50 Black Shark" => new Ka50Executor(),
                _ => new CommonExecutor()
            };
        }
    }
}
