using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using PMS.Model.Models;
using PMS.Model.Models.Identity;
using PMS.Model.Repositories;
using PMS.Model.Services;
using ProjectManagementSystem.Services;
using ProjectManagementSystem.ViewModels;

namespace ProjectManagementSystem.Controllers
{
    [Attributes.Authorize]
    public class UsersApiController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly IUsersService _usersService;
        private readonly IUserPermissionsRepository _permissionsRepository;
        private readonly ISettingsService _userSettingsService;

        public UsersApiController(IUsersService usersService, IUserPermissionsRepository permissionsRepository, ISettingsService userSettingsService)
        {
            _usersService = usersService;
            _permissionsRepository = permissionsRepository;
            _userSettingsService = userSettingsService;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
        private IUserRepository CreateRepository()
        {
            return new UserRepository(new ApplicationContext());
        }

        [HttpGet]
        public ActionResult IsUserExists(string username)
        {
            return Json(_usersService.GetByUsername(username));
        }

        [HttpPost]
        public async Task<ActionResult> ChangePassword(string userId, string password)
        {
            var currentUser = _usersService.GetCurrentUser();
            if (currentUser.Id != userId)
                throw new PmsException("Нельзя изменить пароль другого пользователя");
            var code = await UserManager.GeneratePasswordResetTokenAsync(userId);
            var result = await UserManager.ResetPasswordAsync(userId, code, password);
            if (!result.Succeeded)
                throw new PmsException(result.Errors.Aggregate(string.Empty, (s, s1) => s + "\n" + s1));
            return Json("OK");
        }

        [HttpGet]
        public ActionResult GetCurrentUser()
        {
            var currentUser = _usersService.GetCurrentUser();
            using (var context = new ApplicationContext())
            {
                var api = CreateService(context);
                return Json(api.GetUserViewModel(currentUser),JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetUsers()
        {
            using (var context = new ApplicationContext())
            {
                var api = CreateService(context);
                return new JsonResult
                {
                    Data= api.GetActualUsers(),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    MaxJsonLength = int.MaxValue
                };
            }
        }

        [HttpGet]
        public ActionResult GetRoles()
        {
            using (var context = new ApplicationContext())
            {
                var api = CreateService(context);
                var roles = api.GetRoles();
                return new JsonResult
                {
                    Data = roles,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    MaxJsonLength = int.MaxValue
                };
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddUser(UserViewModel user, string password)
        {
            using (var context = new ApplicationContext())
            {
                var api = CreateService(context);
                var newUser = await api.AddUser(user, password);
                return new JsonResult
                {
                    Data = newUser,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    MaxJsonLength = int.MaxValue
                };
            }
        }

        [HttpPost]
        public ActionResult UpdateUser(UserViewModel user)
        {
            using (var context = new ApplicationContext())
            {
                var api = CreateService(context);
                api.UpdateUser(user);
                return new JsonResult
                {
                    Data = user,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    MaxJsonLength = int.MaxValue
                };
            }
        }

        [HttpPost]
        public ActionResult DeleteUser(string userId)
        {
            using (var context = new ApplicationContext())
            {
                var api = CreateService(context);
                api.DeleteUser(userId);
                return new JsonResult
                {
                    Data = "OK",
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    MaxJsonLength = int.MaxValue
                };
            }
        }

        [HttpGet]
        public ActionResult IsEmailUnique(string email)
        {
            using (var context = new ApplicationContext())
            {
                var api = new UserRepository(context);
                var isUnique = !api.GetUsers(x => x.UserName.Equals(email, StringComparison.OrdinalIgnoreCase)).Any();
                return new JsonResult
                {
                    Data = isUnique,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    MaxJsonLength = int.MaxValue
                };
            }
        }

        [HttpGet]
        public ActionResult GetAllowedUsersForWorkItemType(int typeId)
        {
            var users = _usersService.GetAllowedUsersForWorkItemType(typeId).Select(x => new UserViewModel(x));
            return Json(users, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult HasPermissions(IEnumerable<int> actions)
        {
            IEnumerable<bool> result = new bool[0];
            if (actions != null)
            {
                result = Enumerable.Repeat(true, actions.Count());
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult HasPermissionsForWorkItem(IEnumerable<int> actions, int? workItemId)
        {
            //var queriedPermissions = actions.Select(x => (PermissionType) x).ToArray();
            //var user = this.usersService.GetCurrentUser();
            //var roles = this.usersService.GetRolesByIds(user.Roles.Select(x => x.RoleId));
            //var allowedPermissions = this.permissionsRepository.GetPermissionsForRoles(roles.Select(r=>r.RoleCode));
            IEnumerable<bool> result = new bool[actions.Count()];
            //result = queriedPermissions.Select(x=> allowedPermissions.Contains(x) && (x == ))
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        private UsersApiService CreateService(ApplicationContext context)
        {
            return new UsersApiService(new UserRepository(context), UserManager, _userSettingsService);
        }

    }
}