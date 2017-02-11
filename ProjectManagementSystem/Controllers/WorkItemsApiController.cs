using System.Linq;
using System.Web.Mvc;
using Common.Models;
using Microsoft.AspNet.Identity;
using PMS.Model;
using PMS.Model.Models;
using ProjectManagementSystem.Services;
using ProjectManagementSystem.ViewModels;
using Extensions = PMS.Model.Extensions;

namespace ProjectManagementSystem.Controllers
{
    public class WorkItemsApiController : BaseController
    {
        private readonly WorkItemApiService workItemService;
        public WorkItemsApiController(WorkItemApiService workItemService)
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
            this.workItemService.Delete(id, true);
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
            return CreateJsonResult(projects.Select(x => new WorkItemViewModel(x)));
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
            var itemsDictionary = this.workItemService.GetActualWorkItemModels();
            return CreateJsonResult(itemsDictionary);
        }

        [HttpGet]
        public ActionResult GetStates()
        {
            var states =
                this.workItemService.GetStatesViewModels(User.IsInRole(Resources.Director) || User.IsInRole(Resources.MainProjectEngineer) || User.IsInRole(Resources.Admin),
                    User.IsInRole(Resources.Manager));
            return CreateJsonResult(states);
        }

        [HttpGet]
        public ActionResult GetLinkedItemsForItem(int workItemId)
        {
            var itemsCollection = this.workItemService.GetLinkedWorkItemsForItem(workItemId);
            return Json(itemsCollection, JsonRequestBehavior.AllowGet);
            //Parent
            //Children (first-level)
            //depends on
            //depends by
        }

        [HttpGet]
        public ActionResult GetProjectsTree()
        {
            var projects = this.workItemService.GetProjectsTree();
            return Json(projects, JsonRequestBehavior.AllowGet);
        }
    }
}