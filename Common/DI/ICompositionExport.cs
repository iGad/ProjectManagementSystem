namespace Common.DI
{
    /// <summary>
    /// Интерфейс регистрации экспортируемых классов
    /// </summary>
    public interface ICompositionExport
    {
        /// <summary>
        /// Метод для регистрации классов
        /// </summary>
        /// <param name="compositionContainer">Контейнер</param>
        void RegisterExport(ICompositionContainer compositionContainer);
    }
}
