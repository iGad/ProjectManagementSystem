using System.Web.Mvc;
using PMS.Model.Services;

namespace ProjectManagementSystem.Controllers
{
    [Authorize]
    public class DataUpdaterController : Controller
    {
        private readonly DataUpdater updater;

        public DataUpdaterController(DataUpdater updater)
        {
            this.updater = updater;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Update()
        {
            this.updater.Update();
            return Json("OK");
        }
    }
}