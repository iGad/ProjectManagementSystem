using System;
using System.Linq;
using PMS.Model.Models;
using PMS.Model.Repositories;
using ValueType = PMS.Model.Models.ValueType;

namespace PMS.Model.Services
{
    public class DataUpdater
    {
        private readonly ISettingRepository _settingsRepository;
        private readonly ApplicationContext _context;

        public DataUpdater(ISettingRepository settingsRepository, ApplicationContext context)
        {
            _settingsRepository = settingsRepository;
            _context = context;
        }

        public void Update()
        {
            UpdateSettings();
            UpdateUserSettings();
        }

        private void UpdateUserSettings()
        {
            var userSettings = new[]
            {
                new UserSetting
                {
                    Name = "Звуковое оповещение о событиях в браузере",
                    Type = UserSettingType.NotificationsSound,
                    DefaultValue = bool.TrueString,
                    ValueType = ValueType.Bool
                },
                new UserSetting
                {
                    Name = "Присылать уведомления о новых событиях на почту",
                    Type = UserSettingType.NotificationsToEmail,
                    DefaultValue = bool.FalseString,
                    ValueType = ValueType.Bool
                },
            };
            var existingSettings = _context.UserSettings.ToList();
            foreach (var userSetting in userSettings)
            {
                if (existingSettings.All(x => x.Type != userSetting.Type))
                {
                    _context.UserSettings.Add(userSetting);
                }
            }
            _context.SaveChanges();
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
            var existingSettings = _settingsRepository.GetSettings(x => true).ToList();
            foreach (var setting in settings)
            {
                var existingSetting = existingSettings.SingleOrDefault(x => x.Type == setting.Type);
                if (existingSetting != null && HaveDifferentProperties(setting, existingSetting))
                    _settingsRepository.UpdateSetting(existingSetting, setting);
                else
                    _settingsRepository.AddSetting(setting);
            }
            _settingsRepository.SaveChanges();
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
