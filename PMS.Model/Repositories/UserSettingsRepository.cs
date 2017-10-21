using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS.Model.CommonModels;
using PMS.Model.Models;
using PMS.Model.Services;

namespace PMS.Model.Repositories
{
    public interface IUserSettingsRepository : IRepository
    {
        Task<string> GetSettingsValueForUser(UserSettingType settingType, string userId);
        Task<List<UserSettingModel>> GetUserSettings(string userId);
        Task<UserSettingValue> GetSettingValue(int settingId, string userId);
        Task InitSettingForUser(string userId);
        Task<List<UserSetting>> GetSettings();
        void AddUserSettingValues(IEnumerable<UserSettingValue> values);
    }

    public class UserSettingsRepository : IUserSettingsRepository
    {
        private readonly ApplicationContext _context;

        public UserSettingsRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<UserSettingValue> GetSettingValue(int settingId, string userId)
        {
            return await _context.UserSettingValues.FirstOrDefaultAsync(x => x.UserId == userId && x.UserSettingId == settingId);
        }

        public async Task<List<UserSetting>> GetSettings()
        {
            return await _context.UserSettings.ToListAsync();
        }

        public void AddUserSettingValues(IEnumerable<UserSettingValue> values)
        {
            _context.UserSettingValues.AddRange(values);
        }

        public async Task InitSettingForUser(string userId)
        {
            var settings = await _context.UserSettings.ToListAsync();
            var existingValues = await _context.UserSettingValues.Where(x => x.UserId == userId).ToListAsync();
            var newValues = settings.Where(x => existingValues.All(v => v.UserSettingId != x.Id))
                .Select(x => new UserSettingValue { UserId = userId, UserSettingId = x.Id, Value = x.DefaultValue });
            _context.UserSettingValues.AddRange(newValues);
        }

        public async Task<string> GetSettingsValueForUser(UserSettingType settingType, string userId)
        {
            var setting = await _context.UserSettings.FirstAsync(x => x.Type == settingType);
            return (await _context.UserSettingValues.FirstOrDefaultAsync(x => x.UserId == userId && x.UserSettingId == setting.Id))?.Value ?? setting.DefaultValue;
        }

        public async Task<List<UserSettingModel>> GetUserSettings(string userId)
        {
            var data = await _context.UserSettingValues.Where(x => x.UserId == userId)
                .Join(_context.UserSettings, v => v.UserSettingId, s => s.Id, (v, s) => new {Value = v, Setting = s})
                .ToListAsync();
            return data.Select(x => new UserSettingModel(x.Setting, x.Value)).ToList();
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
