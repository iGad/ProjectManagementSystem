using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models;
using Microsoft.AspNet.SignalR.Hubs;
using PMS.Model.Models;
using PMS.Model.Services;
using PMS.Model.Services.Notifications;
using ProjectManagementSystem.ViewModels;

namespace ProjectManagementSystem.Services
{
    
    public class SignalrNotificator : EventNotificator, IRealtimeNotificationService
    {
        private readonly List<WorkEvent> _previousEvents = new List<WorkEvent>(5);
        private readonly SignalrClientsProvider _clientsProvider;
        private readonly IEventService _eventService;

        private readonly ICurrentUserProvider _currentUserProvider;
        //private ConcurrentDictionary<string, List<Guid>> userConnectionsDictionary = new ConcurrentDictionary<string, List<Guid>>();

        public SignalrNotificator(SignalrClientsProvider clientsProvider, IEventService eventService, ICurrentUserProvider currentUserProvider)
        {
            _clientsProvider = clientsProvider;
            _eventService = eventService;
            _currentUserProvider = currentUserProvider;
        }

        public IHubConnectionContext<dynamic> Clients => _clientsProvider.GetClients();

        public void SendEvent(string eventName, object sendedObject, BroadcastType broadcastType, params string[] userNames)
        {
            dynamic clients;
            switch (broadcastType)
            {
                case BroadcastType.All:
                    clients = Clients.All;
                    break;
                case BroadcastType.Others:
                    clients = Clients.AllExcept(userNames);
                    break;
                case BroadcastType.Users:
                    clients = Clients.Users(userNames.ToList());
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(broadcastType), broadcastType, null);
            }
            clients.raiseEvent(eventName, sendedObject);
        }

        //public void SendNotification(string eventName, object sendedObject, BroadcastType broadcastType, params string[] userNames)
        //{
        //    if (broadcastType != BroadcastType.All && (userNames == null || userNames.All(string.IsNullOrEmpty)))
        //        return;
        //    //string[] usersNames;// = userNames.Where(x => !string.IsNullOrEmpty(x)).ToArray();
        //    dynamic clients;
        //    switch (broadcastType)
        //    {
        //        case BroadcastType.All:
        //            clients = Clients.All;
        //            break;
        //        case BroadcastType.Others:
        //            clients = Clients.AllExcept(userNames);
        //            break;
        //        case BroadcastType.Users:
        //            var users = userNames.Where(x => !string.IsNullOrEmpty(x)).ToArray();
        //            if (!users.Any())
        //                return;
        //            clients = Clients.Users(users);
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException(nameof(broadcastType), broadcastType, null);
        //    }
        //    clients.recieveNotification(eventName, sendedObject);
        //}

        //public void SendNotification(WorkEvent workEvent, BroadcastType broadcastType, params string[] userNames)
        //{
        //    if (broadcastType != BroadcastType.All && (userNames == null || userNames.All(string.IsNullOrEmpty)))
        //        return;
        //    //string[] usersNames;// = userNames.Where(x => !string.IsNullOrEmpty(x)).ToArray();
        //    dynamic clients;
        //    switch (broadcastType)
        //    {
        //        case BroadcastType.All:
        //            clients = Clients.All;
        //            break;
        //        case BroadcastType.Others:
        //            clients = Clients.AllExcept(userNames);
        //            break;
        //        case BroadcastType.Users:
        //            var users = userNames.Where(x => !string.IsNullOrEmpty(x)).ToArray();
        //            if (!users.Any())
        //                return;
        //            clients = Clients.Users(users);
        //            break;
        //        default:
        //            throw new ArgumentOutOfRangeException(nameof(broadcastType), broadcastType, null);
        //    }
        //    clients.recieveNotification(workEvent.Type.ToString().ToLower(), workEvent);
        //}

        public void SendNotifications(WorkEvent workEvent, ICollection<ApplicationUser> users)
        {
            foreach (var user in users)
            {
                var clients = Clients.Users(new[] {user.UserName});
                var description = _eventService.GetEventDescription(workEvent, user);
                var model = new NotificationViewModel
                {
                    Text = description,
                    Type = (int)workEvent.Type,
                    WorkItemId = workEvent.ObjectId.Value
                };
                clients.recieveNotification(workEvent.Type.ToString().ToLower(), model);
            }
        }
        

        protected override void NotifyInner(WorkEvent @event, ICollection<ApplicationUser> users)
        {
            SendNotifications(@event, users);
        }

        protected override ICollection<ApplicationUser> GetOnlyResponsableUsers(WorkEvent workEvent,
            ICollection<ApplicationUser> users)
        {
            var currentUser = _currentUserProvider.GetCurrentUser();
            var responsibleUsers = users.Where(x => x.Id != currentUser.Id);
            if (_previousEvents.Count == 5)
                _previousEvents.Clear();
            if (IsSpecialEvent(workEvent))
                _previousEvents.Add(workEvent);
            if (workEvent.Type == EventType.WorkItemChanged)
            {
                var events = _previousEvents.Where(IsSpecialEvent).ToArray();
                if (events.Any())
                {
                    responsibleUsers = responsibleUsers.Where(x => events.All(ev => ev.Data != x.Id));
                    _previousEvents.Clear();
                }
            }
            return responsibleUsers.ToArray();
        }

        private static bool IsSpecialEvent(WorkEvent workEvent)
        {
            return workEvent.Type == EventType.WorkItemAppointed || workEvent.Type == EventType.WorkItemDisappointed;
        }
    }
}