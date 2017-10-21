using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using PMS.Model.CommonModels;
using PMS.Model.Models;
using PMS.Model.Services;

namespace ProjectManagementSystem.Controllers
{
    [Attributes.Authorize]
    public class SettingsApiController : Controller
    {
        private readonly SettingsService _settingsService;
        private readonly IUsersService _usersService;

        public SettingsApiController(SettingsService settingsService, IUsersService usersService)
        {
            _settingsService = settingsService;
            _usersService = usersService;
        }

        private List<Role> GetCurrentUserRoles()
        {
            return _usersService.GetRolesByIds(_usersService.GetCurrentUser().Roles.Select(x => x.RoleId));
        }

        private void CheckPermissions()
        {
            var roles = GetCurrentUserRoles();
            if (roles.All(x => x.RoleCode != RoleType.Admin && x.RoleCode != RoleType.Director))
                throw new PmsException("Нет доступа к данному действию");
        }

        [HttpGet]
        public ActionResult GetSettings()
        {
            CheckPermissions();
            return Json(_settingsService.GetSettings(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetUserSettings()
        {
            CheckPermissions();
            return Json(await _settingsService.GetUserSettings(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateSetting(Setting setting)
        {
            CheckPermissions();
            _settingsService.UpdateSetting(setting);
            return Json("OK");
        }

        [HttpPost]
        public async Task<ActionResult> UpdateUserSetting(UserSettingModel setting)
        {
            CheckPermissions();
            await _settingsService.UpdateUserSetting(setting);
            return Json("OK");
        }
    }
}