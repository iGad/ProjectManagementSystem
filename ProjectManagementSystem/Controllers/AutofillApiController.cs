using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PMS.Model.Models;
using ProjectManagementSystem.Services;

namespace ProjectManagementSystem.Controllers
{
    
    public class AutofillApiController : Controller
    {
        private readonly AutofillService _autofillService;

        public AutofillApiController(AutofillService autofillService)
        {
            _autofillService = autofillService;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            return Json(_autofillService.GetAll(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Add(Autofill autofill)
        {
            return Json(_autofillService.AddAutofill(autofill), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Update(Autofill autofill)
        {
            _autofillService.Update(autofill);
            return Json("OK");
        }

        public ActionResult Delete(int id)
        {
            _autofillService.Delete(id);
            return Json("OK");
        }
    }
}