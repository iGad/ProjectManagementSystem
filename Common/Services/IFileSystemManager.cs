using System.IO;
using System.Threading.Tasks;

namespace Common.Services
{
    public interface IFileSystemManager
    {
        /// <summary>
        /// Создать поток из файла. Этот объект не отвечает за время жизни потока
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <param name="mode">Режим открытия (по умолчанию - открытие)</param>
        /// <returns>Открытый поток</returns>
        Stream CreateFileStream(string filePath, FileMode mode = FileMode.Open);
        /// <summary>
        /// Удалить папку и все вложенные подпапки
        /// </summary>
        /// <param name="directoryPath">Путь к папке</param>
        void DeleteDirectory(string directoryPath);
        /// <summary>
        /// Удалить файл
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        void DeleteFile(string filePath);

        /// <summary>
        /// Сохранить весь поток в файл
        /// </summary>
        /// <param name="stream">Входящий поток</param>
        /// <param name="filePath">Файл, в который произведется запись (если файл сужествует, то будет перезаписан)</param>
        /// <returns></returns>
        Task SaveStreamToFile(Stream stream, string filePath);
        /// <summary>
        /// Проверка на то, что данный объект может выполнить сохранение по переданному пути
        /// </summary>
        /// <param name="path">Путь, по которому нужно выполнить сохранение</param>
        /// <returns></returns>
        bool IsForPath(string path);
    }
}