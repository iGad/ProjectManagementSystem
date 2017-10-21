using System.Threading.Tasks;

namespace PMS.Model.Services
{
    public interface ISettingsService
    {
        Task InitSettingForUser(string userId);
    }
}