using System.Web.Mvc;

namespace ProjectManagementSystem.Controllers
{
    
    public class HomeController : Controller
    {
        //[AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.Title = "Система управления проектами - Пангея";
            return View();
        }

        [HttpGet]
        [Attributes.Authorize]
        public ActionResult Ping()
        {
            return Json("OK", JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}