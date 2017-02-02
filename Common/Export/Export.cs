using Common.DI;
using Common.Services;

namespace Common.Export
{
    public class Export : ICompositionExport
    {
        /// <summary>
        /// Метод для регистрации классов
        /// </summary>
        /// <param name="compositionContainer">Контейнер</param>
        public void RegisterExport(ICompositionContainer compositionContainer)
        {
            compositionContainer.RegisterExport<TreeService>();
        }
    }
}
