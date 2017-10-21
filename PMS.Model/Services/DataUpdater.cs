using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using PMS.Model.Models;
using PMS.Model.Repositories;
using ValueType = PMS.Model.Models.ValueType;

namespace PMS.Model.Services
{
    public class DataUpdater
    {
        private readonly ApplicationContext _context;

        public DataUpdater(ApplicationContext context)
        {
            _context = context;
        }

        public async Task Update()
        {
            await UpdateSettings();
            await UpdateUserSettings();
        }

        private async Task UpdateUserSettings()
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
                new UserSetting
                {
                    Name = "Адрес электронной почты для получения уведомлений",
                    Type = UserSettingType.NotificationsEmail,
                    DefaultValue = string.Empty,
                    ValueType = ValueType.String,
                    Regex = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$"
                },
            };
            var existingSettings = await _context.UserSettings.ToListAsync();
            foreach (var userSetting in userSettings)
            {
                var existingSetting = existingSettings.FirstOrDefault(x => x.Type == userSetting.Type);
                if (existingSetting == null)
                {
                    _context.UserSettings.Add(userSetting);
                }
                else
                {
                    existingSetting.DefaultValue = userSetting.DefaultValue;
                    existingSetting.Name = userSetting.Name;
                    existingSetting.ValueType = userSetting.ValueType;
                    existingSetting.Regex = userSetting.Regex;
                }
            }
            var users = await _context.Users.ToListAsync();
            var settings = await _context.UserSettings.ToListAsync();
            foreach (var user in users)
            {
                var existingValues = await _context.UserSettingValues.Where(x => x.UserId == user.Id).ToListAsync();
                var newValues = settings.Where(x => existingValues.All(v => v.UserSettingId != x.Id))
                    .Select(x => new UserSettingValue {UserId = user.Id, UserSettingId = x.Id, Value = x.DefaultValue});
                _context.UserSettingValues.AddRange(newValues);
            }
            await _context.SaveChangesAsync();

        }

        private async Task UpdateSettings()
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
            var existingSettings = await _context.Settings.ToListAsync();
            foreach (var setting in settings)
            {
                var existingSetting = existingSettings.SingleOrDefault(x => x.Type == setting.Type);
                if (existingSetting != null)
                {
                    if(HaveDifferentProperties(setting, existingSetting))
                        UpdateSetting(existingSetting, setting);
                }
                else
                    _context.Settings.Add(setting);
            }
            await _context.SaveChangesAsync();
        }

        private void UpdateSetting(Setting existingSetting, Setting setting)
        {
            existingSetting.Type = setting.Type;
            existingSetting.Value = setting.Value;
            existingSetting.DefaultValue = setting.DefaultValue;
            existingSetting.MaxValue = setting.MaxValue;
            existingSetting.MinValue = setting.MinValue;
            existingSetting.ValueType = setting.ValueType;
            existingSetting.ValueRegex = setting.ValueRegex;
            existingSetting.Name = setting.Name;
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
