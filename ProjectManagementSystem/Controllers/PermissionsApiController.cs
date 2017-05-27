using System.Linq;
using System.Web.Mvc;
using PMS.Model;
using PMS.Model.Models;
using PMS.Model.Repositories;
using PMS.Model.Services;

namespace ProjectManagementSystem.Controllers
{
    public class PermissionsApiController : Controller
    {
        private readonly IUserPermissionsRepository permissionsRepository;
        private readonly UsersService usersService;

        public PermissionsApiController(IUserPermissionsRepository permissionsRepository, UsersService usersService)
        {
            this.permissionsRepository = permissionsRepository;
            this.usersService = usersService;
        }

        [HttpGet]
        public ActionResult GetUserPermissions()
        {
            var user = this.usersService.GetCurrentUser();
            var allowedPermissions = this.permissionsRepository.GetPermissionsForRoles(
                this.usersService.GetRolesByIds(user.Roles.Select(r => r.RoleId)).Select(r => r.RoleCode).ToArray());
            return Json(Extensions.ToEnumList<PermissionType>().ToDictionary(x => x.ToString(), x => allowedPermissions.Contains(x)), JsonRequestBehavior.AllowGet);
        } 
    }
}