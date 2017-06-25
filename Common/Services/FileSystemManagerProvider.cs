using System.Linq;

namespace Common.Services
{
    /// <summary>
    /// Поставщик объектов, которые могут выполнить сохранение по указанному пути
    /// </summary>
    public class FileSystemManagerProvider
    {
        private readonly IFileSystemManager[] _savers;
        /// <summary>
        /// Конструктор, которому DI-контейнер подставит все зарегистрированные IFileSaver
        /// </summary>
        /// <param name="savers">Объекты-сохранятели</param>
        public FileSystemManagerProvider(IFileSystemManager[] savers)
        {
            this._savers = savers;
        }
        /// <summary>
        /// Получить объект, который может сохранить файл по указанному пути
        /// </summary>
        /// <param name="path">Путь, по которому нужно сохранить файл</param>
        /// <returns></returns>
        public IFileSystemManager GetManagerForPath(string path)
        {
            return this._savers.SingleOrDefault(x => x.IsForPath(path));
        }
    }
}
