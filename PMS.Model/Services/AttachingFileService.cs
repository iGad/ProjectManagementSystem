using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Services;
using PMS.Model.CommonModels;
using PMS.Model.Models;
using PMS.Model.Repositories;

namespace PMS.Model.Services
{
    public class AttachingFileService
    {
        private readonly IWorkItemRepository repository;
        private readonly ISettingsValueProvider settingsProvider;
        private readonly FileSystemManagerProvider fileSystemManagerProvider;

        public AttachingFileService(IWorkItemRepository repository, ISettingsValueProvider settingsProvider, FileSystemManagerProvider fileSystemManagerProvider)
        {
            this.repository = repository;
            this.settingsProvider = settingsProvider;
            this.fileSystemManagerProvider = fileSystemManagerProvider;
        }


        public FileInfoDisplayModel AttachFileToWorkItem(int workItemId, string fullPath, Stream inputStream)
        {
            var fileManager = this.fileSystemManagerProvider.GetManagerForPath(fullPath);
            if (fileManager == null)
                throw new PmsException("Указанный путь не поддерживается");
            var storageFolder = this.settingsProvider.GetSettingValue(SettingType.FileStoragePath);
            if (storageFolder == null)
                throw new PmsException("Не указан путь для сохранения. Поправьте настройки");
            string relativePath;
            if (fullPath.StartsWith(storageFolder))
            {
                relativePath = fullPath.Remove(0, storageFolder.Length);
            }
            else
            {
                relativePath = Path.GetFileName(fullPath);
                var savingPath = Path.Combine(storageFolder, relativePath);
                fileManager.SaveStreamToFile(inputStream, savingPath);
            }
            var file = new AttachedFile
            {
                RelativePath = relativePath,
                WorkItemId = workItemId
            };
            return new FileInfoDisplayModel
            {
                Id = file.Id,
                Name = Path.GetFileName(file.RelativePath),
                FullPath = Path.Combine(storageFolder, file.RelativePath),
                Size = inputStream.Length/1024 + "KB"
            };
        }

        public void DeleteFile(int id)
        {
            
        }

        public string GetFileName(int fileId)
        {
            return string.Empty;
        }

        public byte[] GetFileBytes(int fileId)
        {
            return new byte[0];
        }
    }
}
