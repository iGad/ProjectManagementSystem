using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Models;
using Microsoft.AspNet.Identity;
using PMS.Model;
using PMS.Model.Models;
using PMS.Model.Services;

namespace ProjectManagementSystem.Controllers
{
    public class WorkItemsApiController : BaseController
    {
        private readonly WorkItemService workItemService;
        public WorkItemsApiController(WorkItemService workItemService)
        {
            this.workItemService = workItemService;
        }
        [HttpPost]
        public ActionResult AddWorkItem(WorkItem workItem)
        {
            workItem.CreatorId = User.Identity.GetUserId();
            workItem.State = WorkItemState.New;
            workItem.Status = WorkItemStatus.InWork;
                
            return TryAction(() => CreateJsonResult(this.workItemService.Add(workItem)));
        }

        [HttpPost]
        public ActionResult UpdateWorkItem(WorkItem workItem)
        {
            return TryAction(() => this.workItemService.Update(workItem));
        }

        [HttpGet]
        public ActionResult GetWorkItemTypes()
        {
            var types = Extensions.ToEnumList<WorkItemType>().Select(x=>new NamedEntity {Id = (int)x, Name = x.GetDescription()});
            return CreateJsonResult(types);
        }

        [HttpGet]
        public ActionResult GetWorkItem(int id)
        {
            var workItem = this.workItemService.Get(id);
            return CreateJsonResult(workItem);
        }

        [HttpGet]
        public ActionResult GetProjects()
        {
            var projects = this.workItemService.GetActualProjects();
            return CreateJsonResult(projects);
        }

        [HttpGet]
        public ActionResult GetChildWorkItems(int parentId)
        {
            var projects = this.workItemService.GetChildWorkItems(parentId);
            return CreateJsonResult(projects);
        }

        
    }
}