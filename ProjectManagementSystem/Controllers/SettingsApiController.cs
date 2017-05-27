using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PMS.Model.Models;
using PMS.Model.Services;

namespace ProjectManagementSystem.Controllers
{
    [Authorize]
    public class SettingsApiController : Controller
    {
        private readonly SettingsService settingsService;
        private readonly UsersService usersService;

        public SettingsApiController(SettingsService settingsService, UsersService usersService)
        {
            this.settingsService = settingsService;
            this.usersService = usersService;
        }

        private List<Role> GetCurrentUserRoles()
        {
            return this.usersService.GetRolesByIds(this.usersService.GetCurrentUser().Roles.Select(x => x.RoleId));
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
            return Json(this.settingsService.GetSettings(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateSetting(Setting setting)
        {
            CheckPermissions();
            this.settingsService.UpdateSetting(setting);
            return Json("OK");
        }
    }
}