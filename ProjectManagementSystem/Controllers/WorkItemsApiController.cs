using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PMS.Model;
using PMS.Model.Models;
using PMS.Model.Repositories;
using PMS.Model.Services;

namespace ProjectManagementSystem.Controllers
{
    public class WorkItemsApiController : BaseController
    {
        [HttpPost]
        public ActionResult AddWorkItem(WorkItem workItem)
        {
            using (var context = new ApplicationContext())
            {
                workItem.CreatorId = User.Identity.GetUserId();
                workItem.State = WorkItemState.New;
                workItem.Status = WorkItemStatus.InWork;
                
                var api = new WorkItemService(new WorkItemRepository(context));
                return TryAction(() => CreateJsonResult(api.Add(workItem)));
            }
        }

        [HttpPost]
        public ActionResult UpdateWorkItem(WorkItem workItem)
        {
            using (var context = new ApplicationContext())
            {
                var api = new WorkItemService(new WorkItemRepository(context));
                return TryAction(() => api.Update(workItem));
            }
        }

        [HttpGet]
        public ActionResult GetWorkItemTypes()
        {
            var types = Extensions.ToEnumList<WorkItemType>().Select(x=>new NamedEntity {Id = (int)x, Name = x.GetDescription()});
            return CreateJsonResult(types);
        }

        [HttpGet]
        public ActionResult GetProjects()
        {
            using (var context = new ApplicationContext())
            {
                var api = new WorkItemService(new WorkItemRepository(context));
                var projects = api.GetActualProjects();
                return CreateJsonResult(projects);
            }
        }

        [HttpGet]
        public ActionResult GetChildWorkItems(int parentId)
        {
            using (var context = new ApplicationContext())
            {
                var api = new WorkItemService(new WorkItemRepository(context));
                var projects = api.GetChildWorkItems(parentId);
                return CreateJsonResult(projects);
            }
        }

        
    }
}