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
    [System.Web.Http.Authorize]
    public class WorkItemsApiController : BaseController
    {
        private readonly WorkItemApiService _workItemService;
        private readonly AttachingFileService _fileService;
        private readonly SignalrNotificator _notifyService;
        public WorkItemsApiController(WorkItemApiService workItemService, AttachingFileService fileService, SignalrNotificator notifyService)
        {
            _workItemService = workItemService;
            _fileService = fileService;
            _notifyService = notifyService;
        }
        [HttpPost]
        public ActionResult AddWorkItem(WorkItem workItem)
        {
            workItem.CreatorId = User.Identity.GetUserId();
            return TryAction(() => CreateJsonResult(_workItemService.Add(workItem)));
        }

        [HttpPost]
        public ActionResult UpdateWorkItem(WorkItem workItem)
        {
            _workItemService.Update(workItem);
            _notifyService.SendEvent("WorkItemChanged", workItem, BroadcastType.All);
            return Json("OK");
        }

        [HttpPost]
        public ActionResult UpdateWorkItemState(int workItemId, WorkItemState newState)
        {
            _workItemService.UpdateWorkItemState(workItemId, newState);
            return Json("OK");
        }

        [HttpPost]
        public ActionResult DeleteWorkItem(int id)
        {
            _workItemService.Delete(id, true);
            return Json("OK");
        }

        [HttpGet]
        public ActionResult GetWorkItemTypes()
        {
            var types = Extensions.ToEnumList<WorkItemType>().Select(x => new EnumViewModel<WorkItemType>(x));
            return CreateJsonResult(types);
        }

        [HttpGet]
        public ActionResult GetWorkItem(int id)
        {
            var workItem = new WorkItemViewModel(_workItemService.GetWithParents(id));
            return CreateJsonResult(workItem);
        }

        [HttpGet]
        public ActionResult GetProjects()
        {
            var projects = _workItemService.GetActualProjects();
            return CreateJsonResult(projects.Select(x => new WorkItemViewModel(x)));
        }

        [HttpGet]
        public ActionResult GetChildWorkItems(int parentId)
        {
            var projects = _workItemService.GetChildWorkItems(parentId);
            return CreateJsonResult(projects);
        }

        [HttpGet]
        public ActionResult GetActualWorkItems()
        {
            var itemsDictionary = _workItemService.GetActualWorkItemModels();
            return CreateJsonResult(itemsDictionary);
        }

        [HttpGet]
        public ActionResult GetUserWorkItemsAggregateInfo()
        {
            return CreateJsonResult(_workItemService.GetUserItemsAggregateInfos());
        }

        [HttpGet]
        public ActionResult GetUserActualWorkItems(string userId)
        {
            var itemsDictionary = _workItemService.GetActualWorkItemModels(userId);
            return CreateJsonResult(itemsDictionary);
        }

        [HttpGet]
        public ActionResult GetStates()
        {
            var states =
                _workItemService.GetStatesViewModels(User.IsInRole(Resources.Director) || User.IsInRole(Resources.MainProjectEngineer) || User.IsInRole(Resources.Admin),
                    User.IsInRole(Resources.Manager));
            return CreateJsonResult(states);
        }

        [HttpGet]
        public ActionResult GetAllStates()
        {
            return Json(_workItemService.GetAllStates(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetLinkedItemsForItem(int workItemId)
        {
            var itemsCollection = _workItemService.GetLinkedWorkItemsForItem(workItemId);
            return Json(itemsCollection, JsonRequestBehavior.AllowGet);
            //Parent
            //Children (first-level)
            //depends on
            //depends by
        }

        [HttpGet]
        public ActionResult GetProjectsTree()
        {
            var projects = _workItemService.GetProjectsTree();
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
            var fileName = _fileService.GetFileName(fileId);
            byte[] fileBytes = _fileService.GetFileBytes(fileId);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        
    }
}