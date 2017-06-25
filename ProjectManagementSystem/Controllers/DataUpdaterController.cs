using System.Web.Mvc;
using PMS.Model.Services;

namespace ProjectManagementSystem.Controllers
{
    [Authorize]
    public class DataUpdaterController : Controller
    {
        private readonly DataUpdater _updater;

        public DataUpdaterController(DataUpdater updater)
        {
            _updater = updater;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Update()
        {
            _updater.Update();
            return Json("OK");
        }
    }
}