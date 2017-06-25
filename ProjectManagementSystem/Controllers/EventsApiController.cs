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
        private readonly EventsApiService _eventService;
        private readonly ICurrentUsernameProvider _usernameProvider;
        private readonly IUserRepository _userRepository;
        private readonly INotifyService _notifyService;

        public EventsApiController(EventsApiService eventService, ICurrentUsernameProvider usernameProvider, IUserRepository userRepository,
            INotifyService notifyService)
        {
            _eventService = eventService;
            _usernameProvider = usernameProvider;
            _userRepository = userRepository;
            _notifyService = notifyService;
        }

        [HttpPost]
        public ActionResult GetSeenEventsForCurrentUser(EventFilterModel filterModel)
        {
            var filter = filterModel ?? new EventFilterModel();
            var eventCollection = _eventService.GetEventsForCurrentUser(filter);
            return Json(eventCollection, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetNewEventsForCurrentUser()
        {
            return Json(_eventService.GetNewEventsForCurrentUser(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetUnseenEventCountForCurrentUser()
        {
            return Json(_eventService.GetUnseenEventCountForCurrentUser(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ChangeIsFavorite(int eventId, bool isFavorite)
        {
            _eventService.ChangeEventIsFavorite(eventId, isFavorite);
            return Json("OK");
        }

        [HttpPost]
        public ActionResult ChangeEventState(int eventId, EventState state)
        {
            _eventService.ChangeEventState(eventId, state);
            return Json("OK");
        }

        [HttpGet]
        public ActionResult SendNotification(string userId)
        {
            var user = _userRepository.GetById(userId);

            _notifyService.SendNotifications(new WorkEvent
            {
                ObjectId = 12,
                Type = EventType.WorkItemAdded,
                UserId = _userRepository.GetByUserName(_usernameProvider.GetCurrentUsername()).Id
            }, new[] {user});
            return Json("OK", JsonRequestBehavior.AllowGet);
        }
    }
}