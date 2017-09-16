using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PMS.Model;
using PMS.Model.CommonModels.FilterModels;
using PMS.Model.Models;
using ProjectManagementSystem.Services;
using ProjectManagementSystem.ViewModels;

namespace ProjectManagementSystem.Controllers
{
    [Authorize]
    public class AutofillApiController : Controller
    {
        private readonly AutofillService _autofillService;

        public AutofillApiController(AutofillService autofillService)
        {
            _autofillService = autofillService;
        }

        [HttpGet]
        public ActionResult GetWorkItemTypes()
        {
            var types = Extensions.ToEnumList<WorkItemType>().Where(x => x != WorkItemType.Project).Select(x => new EnumViewModel<WorkItemType>(x));
            return Json(types, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var result = await _autofillService.GetAllAutofills();
            return Json(result, JsonRequestBehavior.AllowGet);
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