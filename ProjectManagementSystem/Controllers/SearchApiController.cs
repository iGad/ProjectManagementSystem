using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PMS.Model.CommonModels.FilterModels;
using ProjectManagementSystem.Services;

namespace ProjectManagementSystem.Controllers
{
    [Authorize]
    public class SearchApiController : Controller
    {
        private readonly WorkItemApiService _workItemApiService;

        public SearchApiController(WorkItemApiService workItemApiService)
        {
            _workItemApiService = workItemApiService;
        }

        [HttpPost]
        public ActionResult Find(SearchModel searchModel)
        {
            return Json(_workItemApiService.Find(searchModel), JsonRequestBehavior.AllowGet);
        }
    }
}