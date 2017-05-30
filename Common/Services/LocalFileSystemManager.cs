using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.Services
{
    /// <summary>
    /// Класс для сохранения файлов локально
    /// </summary>
    public class LocalFileSystemManager : IFileSystemManager
    {
        /// <summary>
        /// Создать поток из файла. Этот объект не отвечает за время жизни потока
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        /// <param name="mode">Режим открытия (по умолчанию - открытие)</param>
        /// <returns>Открытый поток</returns>
        public Stream CreateFileStream(string filePath, FileMode mode = FileMode.Open)
        {
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            return File.Open(filePath, mode);
        }

        /// <summary>
        /// Удалить файл
        /// </summary>
        /// <param name="filePath">Путь к файлу</param>
        public void DeleteFile(string filePath)
        {
            if(File.Exists(filePath))
                File.Delete(filePath);
        }

        /// <summary>
        /// Удалить папку и все вложенные подпапки
        /// </summary>
        /// <param name="directoryPath">Путь к папке</param>
        public void DeleteDirectory(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
                Directory.Delete(directoryPath, true);
        }

        /// <summary>
        /// Сохранить весь поток в файл
        /// </summary>
        /// <param name="stream">Входящий поток</param>
        /// <param name="filePath">Файл, в который произведется запись (если файл сужествует, то будет перезаписан)</param>
        /// <returns></returns>
        public async Task SaveStreamToFile(Stream stream, string filePath)
        {
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            using (var writer = File.OpenWrite(filePath))
            {
                await stream.CopyToAsync(writer);
            }
        }

        /// <summary>
        /// Проверка на то, что данный объект может выполнить сохранение по переданному пути
        /// </summary>
        /// <param name="path">Путь, по которому нужно выполнить сохранение</param>
        /// <returns></returns>
        public bool IsForPath(string path)
        {
            var match = Regex.Match(path, @"^[a-zA-Z]:[\/\\].*");
            return match.Success;
        }
        
    }
}
