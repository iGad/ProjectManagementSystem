using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS.Model.Models;
using PMS.Model.Services;

namespace PMS.Model.Repositories
{
    public interface IUserSettingsValueProvider
    {
        Task<string> GetSettingsValueForCurrentUser(UserSettingType settingType);
    }

    public class UserSettingsRepository : IUserSettingsValueProvider
    {
        private readonly ApplicationContext _context;
        private readonly ICurrentUserProvider _currentUserProvider;

        public UserSettingsRepository(ApplicationContext context, ICurrentUserProvider currentUserProvider)
        {
            _context = context;
            _currentUserProvider = currentUserProvider;
        }
        public async Task<string> GetSettingsValueForCurrentUser(UserSettingType settingType)
        {
            var currentUser = await _currentUserProvider.GetCurrentUserAsync();
            var setting = await _context.UserSettings.FirstAsync(x => x.Type == settingType);
            return (await _context.UserSettingValues.FirstOrDefaultAsync(x => x.UserId == currentUser.Id && x.UserSettingId == setting.Id))?.Value ?? setting.DefaultValue;
        }

        
    }
}
