using System;
using System.Web.Mvc;
using PMS.Model.Services;

namespace ProjectManagementSystem.Controllers
{
    public class BaseController : Controller
    {
        protected ActionResult CreateJsonResult(object data)
        {
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        protected ActionResult TryAction(Func<ActionResult> action)
        {
            try
            {
                return action();
            }
            catch (PmsException ex)
            {
                return CreateJsonResult("Произошла ошибка при выполнении операции. " + ex.Message);
            }
        }

        protected ActionResult TryAction(Action action)
        {
            try
            {
                action();
                return CreateJsonResult("OK");
            }
            catch (PmsException ex)
            {
                return CreateJsonResult("Произошла ошибка при выполнении операции. " + ex.Message);
            }
        }
    }
}