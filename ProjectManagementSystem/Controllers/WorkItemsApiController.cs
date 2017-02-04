using System.Linq;
using System.Web.Mvc;
using Common.Models;
using Microsoft.AspNet.Identity;
using PMS.Model;
using PMS.Model.Models;
using PMS.Model.Services;
using ProjectManagementSystem.Services;
using Extensions = PMS.Model.Extensions;

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
            return TryAction(() => CreateJsonResult(this.workItemService.Add(workItem)));
        }

        [HttpPost]
        public ActionResult UpdateWorkItem(WorkItem workItem)
        {
            return TryAction(() => this.workItemService.Update(workItem));
        }

        [HttpPost]
        public ActionResult DeleteWorkItem(int id)
        {
            this.workItemService.Delete(id);
            return Json("OK");
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

        [HttpGet]
        public ActionResult GetActualWorkItems()
        {
            var api = new WorkItemApiService(this.workItemService);
            var itemsDictionary = api.GetActualWorkItems();
            return CreateJsonResult(itemsDictionary);
        }

        [HttpGet]
        public ActionResult GetStates()
        {
            var api = new WorkItemApiService(this.workItemService);
            var states =
                api.GetStatesViewModels(User.IsInRole(Resources.Director) || User.IsInRole(Resources.MainProjectEngineer) || User.IsInRole(Resources.Admin),
                    User.IsInRole(Resources.Manager));
            return CreateJsonResult(states);
        }
    }
}