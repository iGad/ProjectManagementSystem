using System;
using System.Linq;
using PMS.Model.Models;
using PMS.Model.Repositories;
using ValueType = PMS.Model.Models.ValueType;

namespace PMS.Model.Services
{
    public class DataUpdater
    {
        private readonly ISettingRepository settingsRepository;

        public DataUpdater(ISettingRepository settingsRepository)
        {
            this.settingsRepository = settingsRepository;
        }

        public void Update()
        {
            UpdateSettings();
        }

        private void UpdateSettings()
        {
            var settings = new []
            {
                new Setting
                {
                    Name = "Путь для сохранения и поиска прикрепленных файлов",
                    Type = SettingType.FileStoragePath,
                    ValueType = ValueType.String,
                    DefaultValue = "c:\\Files",
                    ValueRegex = @"^[a-zA-Z]:[\/\\].*"
                },
                new Setting
                {
                    Name = "Максимальное количество отображаемых рабочих элементов в одном столбце",
                    Type = SettingType.MaxDisplayWorkItemCount,
                    ValueType = ValueType.Int,
                    DefaultValue = "30",
                    MinValue = 10,
                    MaxValue = 100
                },
            };
            var existingSettings = this.settingsRepository.GetSettings(x => true).ToList();
            foreach (var setting in settings)
            {
                var existingSetting = existingSettings.SingleOrDefault(x => x.Type == setting.Type);
                if (existingSetting != null && HaveDifferentProperties(setting, existingSetting))
                    this.settingsRepository.UpdateSetting(existingSetting, setting);
                else
                    this.settingsRepository.AddSetting(setting);
            }
            this.settingsRepository.SaveChanges();
        }
        

        private bool HaveDifferentProperties(Setting setting, Setting existingSetting)
        {
            return !setting.Name.Equals(existingSetting.Name, StringComparison.InvariantCultureIgnoreCase) ||
                   setting.ValueRegex != existingSetting.ValueRegex ||
                   setting.MinValue != existingSetting.MinValue ||
                   setting.MaxValue != existingSetting.MaxValue ||
                   setting.DefaultValue != existingSetting.DefaultValue ||
                   setting.ValueType != existingSetting.ValueType;
        }
    }
}
