using System.Web.Mvc;

namespace ProjectManagementSystem.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //[AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.Title = "Система управления проектами - Пангея";
            return View();
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