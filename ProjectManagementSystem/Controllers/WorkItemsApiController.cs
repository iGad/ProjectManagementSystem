using System.Linq;
using System.Web.Mvc;
using Common.Models;
using Microsoft.AspNet.Identity;
using PMS.Model;
using PMS.Model.Models;
using ProjectManagementSystem.Services;
using ProjectManagementSystem.ViewModels;
using Extensions = PMS.Model.Extensions;
using System.IO;
using PMS.Model.Services;

namespace ProjectManagementSystem.Controllers
{
    [Authorize]
    public class WorkItemsApiController : BaseController
    {
        private readonly WorkItemApiService workItemService;
        private readonly AttachingFileService fileService;
        private readonly NotificationService notifyService;
        public WorkItemsApiController(WorkItemApiService workItemService, AttachingFileService fileService, NotificationService notifyService)
        {
            this.workItemService = workItemService;
            this.fileService = fileService;
            this.notifyService = notifyService;
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
            this.workItemService.Update(workItem);
            this.notifyService.SendEvent("WorkItemChanged", workItem, BroadcastType.All);
            return Json("OK");
        }

        [HttpPost]
        public ActionResult UpdateWorkItemState(int workItemId, WorkItemState newState)
        {
            var workItem = this.workItemService.Get(workItemId);
            workItem.State = newState;
            return UpdateWorkItem(workItem);
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
            var workItem = this.workItemService.GetWithNoTracking(id);
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
        public ActionResult GetUserWorkItemsAggregateInfo()
        {
            return CreateJsonResult(this.workItemService.GetUserItemsAggregateInfos());
        }

        [HttpGet]
        public ActionResult GetUserActualWorkItems(string userId)
        {
            var itemsDictionary = this.workItemService.GetActualWorkItemModels(userId);
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

        public ActionResult AttachFileToWorkItem(int workItemId)
        {
            Stream inputStream = GetStreamFromRequest();
            return Json("OK");
        }

        private Stream GetStreamFromRequest()
        {
            Stream inputStream = null;
            if (Request.Files.Count > 0)
            {
                var fContent = Request.Files[0];
                if (fContent == null || fContent.ContentLength == 0)
                    throw new PmsException("Не удалось загрузить файл");
                inputStream = fContent.InputStream;
            }
            else
                throw new PmsException("Не выбран файл");
            return inputStream;
        }

        [HttpGet]
        public ActionResult DownloadFile(int fileId)
        {
            //var filePath = _service.GetFilePathForParameter(commandId, parameterId);
            var fileName = this.fileService.GetFileName(fileId);
            byte[] fileBytes = this.fileService.GetFileBytes(fileId);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        
    }
}