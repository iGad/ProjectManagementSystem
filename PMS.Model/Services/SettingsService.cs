using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PMS.Model.CommonModels;
using PMS.Model.Models;
using PMS.Model.Repositories;

namespace PMS.Model.Services
{
    public class SettingsService : ISettingsService
    {
        private readonly ISettingRepository _repository;
        private readonly IUserSettingsRepository _userSettingsRepository;
        private readonly ICurrentUserProvider _userProvider;

        public SettingsService(ISettingRepository repository, IUserSettingsRepository userSettingsRepository, ICurrentUserProvider userProvider)
        {
            _repository = repository;
            _userSettingsRepository = userSettingsRepository;
            _userProvider = userProvider;
        }

        public List<Setting> GetSettings()
        {
            return _repository.GetSettings(x => true).ToList();
        }

        public void UpdateSetting(Setting setting)
        {
            var oldSetting = _repository.Get(setting.Id);
            if(oldSetting == null)
                throw new PmsException($"Настройка с идентификатором {setting.Id} не найдена");
            oldSetting.Value = setting.Value;
            _repository.SaveChanges();
        }

        public async Task InitSettingForUser(string userId)
        {
            var settings = await _userSettingsRepository.GetSettings();
            var existingValues = await _userSettingsRepository.GetUserSettings(userId);
            var newValues = settings.Where(x => existingValues.All(v => v.Id != x.Id))
                .Select(x => new UserSettingValue { UserId = userId, UserSettingId = x.Id, Value = x.DefaultValue });
            _userSettingsRepository.AddUserSettingValues(newValues);
            await _userSettingsRepository.SaveChangesAsync();
        }

        public async Task<List<UserSettingModel>> GetUserSettings()
        {
            var user = await _userProvider.GetCurrentUserAsync();
            return await _userSettingsRepository.GetUserSettings(user.Id);
        }

        public async Task UpdateUserSetting(UserSettingModel settingModel)
        {
            var setting = await _userSettingsRepository.GetSettingValue(settingModel.Id, settingModel.UserId);
            setting.Value = setting.Value;
            await _userSettingsRepository.SaveChangesAsync();
        }
    }
}
