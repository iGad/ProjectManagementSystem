using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PMS.Model.Models;
using PMS.Model.Models.Identity;
using PMS.Model.Repositories;
using PMS.Model.Services;
using ProjectManagementSystem.Services;
using ProjectManagementSystem.ViewModels;

namespace ProjectManagementSystem.Controllers
{
    [Authorize]
    public class UsersApiController : Controller
    {
        private ApplicationUserManager userManager;
        private readonly UsersService usersService;

        public UsersApiController(UsersService usersService)
        {
            this.usersService = usersService;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return this.userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                this.userManager = value;
            }
        }
        private IUserRepository CreateRepository()
        {
            return new UserRepository(new ApplicationContext());
        }

        [HttpGet]
        public ActionResult GetUsers()
        {
            using (var context = new ApplicationContext())
            {
                var api = new UsersApiService(new UserRepository(context), UserManager);
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
                var api = new UsersApiService(new UserRepository(context), UserManager);
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
        public ActionResult AddUser(UserViewModel user, string password)
        {
            using (var context = new ApplicationContext())
            {
                var api = new UsersApiService(new UserRepository(context), UserManager);
                var newUser = api.AddUser(user, password);
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
                var api = new UsersApiService(new UserRepository(context), UserManager);
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
                var api = new UsersApiService(new UserRepository(context), UserManager);
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
            var users = this.usersService.GetAllowedUsersForWorkItemType(typeId).Select(x => new UserViewModel(x));
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
        public ActionResult HasPermissionsForWorkItem(IEnumerable<int> actions, int workItemId)
        {
            IEnumerable<bool> result = new bool[0];
            if (actions != null)
            {
                result = Enumerable.Repeat(true, actions.Count());
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}