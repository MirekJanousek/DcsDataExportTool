using DcsExportLib.Exporters;
using DcsExportLib.Models;

namespace DcsExportLib.Factories
{
    internal class LoaderFactory : ILoaderFactory
    {
        private readonly IClickableDataLoader _commonClickableDataLoader;

        public LoaderFactory(IClickableDataLoader commonClickableDataLoader)
        {
            _commonClickableDataLoader = commonClickableDataLoader ??
                                         throw new ArgumentNullException(nameof(commonClickableDataLoader));
        }

        public IClickableDataLoader GetClickableDataLoader(DcsModuleInfo moduleInfo)
        {
            return _commonClickableDataLoader;
        }
    }
}
