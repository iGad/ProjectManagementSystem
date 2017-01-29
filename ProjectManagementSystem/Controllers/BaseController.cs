using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PMS.Model.Services;

namespace ProjectManagementSystem.Controllers
{
    public class BaseController : Controller
    {
        protected ActionResult CreateJsonResult(object data)
        {
            return new JsonResult
            {
                Data = data,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                MaxJsonLength = int.MaxValue
            };
        }

        protected ActionResult TryAction(Func<ActionResult> action)
        {
            try
            {
                return action();
            }
            catch (PmsExeption ex)
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
            catch (PmsExeption ex)
            {
                return CreateJsonResult("Произошла ошибка при выполнении операции. " + ex.Message);
            }
        }
    }
}