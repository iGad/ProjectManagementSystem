using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using PMS.Model.Models.Identity;
using PMS.Model.Repositories;
using PMS.Model.Services;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Services;

namespace ProjectManagementSystem.Controllers
{
    [Attributes.Authorize]
    public class DataUpdaterController : Controller
    {
        private ApplicationUserManager _userManager;
        private readonly DataUpdater _updater;
        private readonly ISettingsService _settingsService;
        private readonly DataGenerator _dataGenerator;

        public DataUpdaterController(DataUpdater updater, IUserRepository userRepo, IUsersService userService, WorkItemApiService workItemApiService, ISettingsService settingsService)
        {
            _updater = updater;
            _settingsService = settingsService;
            _dataGenerator = new DataGenerator(userService, new UsersApiService(userRepo, UserManager, settingsService), workItemApiService);
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

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Update()
        {
            await _updater.Update();
            return Json("OK");
        }

        [HttpPost]
        public async Task<ActionResult> GenerateUsers()
        {
            await _dataGenerator.GenerateUsers();
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GenerateItems(DataGeneratorParameters parameters)
        {
            _dataGenerator.GenerateWorkItems(parameters);
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
    }
}