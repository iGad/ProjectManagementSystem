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
        private readonly IUserPermissionsRepository _permissionsRepository;
        private readonly UsersService _usersService;

        public PermissionsApiController(IUserPermissionsRepository permissionsRepository, UsersService usersService)
        {
            _permissionsRepository = permissionsRepository;
            _usersService = usersService;
        }

        [HttpGet]
        public ActionResult GetUserPermissions()
        {
            var user = _usersService.GetCurrentUser();
            var allowedPermissions = _permissionsRepository.GetPermissionsForRoles(
                _usersService.GetRolesByIds(user.Roles.Select(r => r.RoleId)).Select(r => r.RoleCode).ToArray());
            return Json(Extensions.ToEnumList<PermissionType>().ToDictionary(x => x.ToString(), x => allowedPermissions.Contains(x)), JsonRequestBehavior.AllowGet);
        } 
    }
}