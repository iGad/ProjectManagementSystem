﻿using System.Web.Mvc;
using Common.Services;
using PMS.Model.CommonModels.FilterModels;
using PMS.Model.Models;
using PMS.Model.Repositories;
using PMS.Model.Services;
using ProjectManagementSystem.Services;

namespace ProjectManagementSystem.Controllers
{
    [Authorize]
    public class EventsApiController : Controller
    {
        private readonly EventsApiService eventService;
        private readonly ICurrentUsernameProvider usernameProvider;
        private readonly IUserRepository userRepository;
        private readonly INotifyService notifyService;

        public EventsApiController(EventsApiService eventService, ICurrentUsernameProvider usernameProvider, IUserRepository userRepository,
            INotifyService notifyService)
        {
            this.eventService = eventService;
            this.usernameProvider = usernameProvider;
            this.userRepository = userRepository;
            this.notifyService = notifyService;
        }

        [HttpPost]
        public ActionResult GetSeenEventsForCurrentUser(EventFilterModel filterModel)
        {
            var filter = filterModel ?? new EventFilterModel();
            var eventCollection = this.eventService.GetEventsForCurrentUser(filter);
            return Json(eventCollection, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetNewEventsForCurrentUser()
        {
            return Json(this.eventService.GetNewEventsForCurrentUser(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetUnseenEventCountForCurrentUser()
        {
            return Json(this.eventService.GetUnseenEventCountForCurrentUser(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ChangeIsFavorite(int eventId, bool isFavorite)
        {
            this.eventService.ChangeEventIsFavorite(eventId, isFavorite);
            return Json("OK");
        }

        [HttpPost]
        public ActionResult ChangeEventState(int eventId, EventState state)
        {
            this.eventService.ChangeEventState(eventId, state);
            return Json("OK");
        }

        [HttpGet]
        public ActionResult SendNotification(string userId)
        {
            var user = this.userRepository.GetById(userId);

            this.notifyService.SendNotifications(new WorkEvent
            {
                ObjectId = 12,
                Type = EventType.WorkItemAdded,
                UserId = this.userRepository.GetByUserName(this.usernameProvider.GetCurrentUsername()).Id
            }, new[] {user});
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
    }
}