using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PMS.Model.CommonModels.FilterModels;
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

        [HttpPost]
        public async Task<ActionResult> GetAutofillList(AutofillFilterModel filterModel)
        {
            return Json(await _autofillService.GetAutofillList(filterModel), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> Add(Autofill autofill)
        {
            return Json(await _autofillService.AddAutofill(autofill), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> Update(Autofill autofill)
        {
            await _autofillService.Update(autofill);
            return Json("OK");
        }

        public async Task<ActionResult> Delete(int id)
        {
            await _autofillService.Delete(id);
            return Json("OK");
        }
    }
}