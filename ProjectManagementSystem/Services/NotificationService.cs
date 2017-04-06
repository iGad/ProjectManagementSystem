using System;
using System.Linq;
using Common.Models;
using Common.Services;
using Microsoft.AspNet.SignalR.Hubs;
using PMS.Model.Models;
using PMS.Model.Services;
using ProjectManagementSystem.ViewModels;

namespace ProjectManagementSystem.Services
{
    
    public class NotificationService : INotifyService
    {
        private readonly IEventService eventService;
        private readonly SignalrClientsProvider clientsProvider;
        //private ConcurrentDictionary<string, List<Guid>> userConnectionsDictionary = new ConcurrentDictionary<string, List<Guid>>();

        public NotificationService(SignalrClientsProvider clientsProvider, IEventService eventService)
        {
            this.clientsProvider = clientsProvider;
            this.eventService = eventService;
        }

        public IHubConnectionContext<dynamic> Clients => this.clientsProvider.GetClients();

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

        public void SendNotification(string eventName, object sendedObject, BroadcastType broadcastType, params string[] userNames)
        {
            if (broadcastType != BroadcastType.All && (userNames == null || userNames.All(string.IsNullOrEmpty)))
                return;
            //string[] usersNames;// = userNames.Where(x => !string.IsNullOrEmpty(x)).ToArray();
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
                    var users = userNames.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    if (!users.Any())
                        return;
                    clients = Clients.Users(users);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(broadcastType), broadcastType, null);
            }
            clients.recieveNotification(eventName, sendedObject);
        }

        public void SendNotification(WorkEvent workEvent, BroadcastType broadcastType, params string[] userNames)
        {
            if (broadcastType != BroadcastType.All && (userNames == null || userNames.All(string.IsNullOrEmpty)))
                return;
            //string[] usersNames;// = userNames.Where(x => !string.IsNullOrEmpty(x)).ToArray();
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
                    var users = userNames.Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    if (!users.Any())
                        return;
                    clients = Clients.Users(users);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(broadcastType), broadcastType, null);
            }
            clients.recieveNotification(workEvent.Type.ToString().ToLower(), workEvent);
        }

        public void SendNotifications(WorkEvent workEvent, ApplicationUser[] users)
        {
            foreach (var user in users)
            {
                var clients = Clients.Users(new[] {user.UserName});
                var description = this.eventService.GetEventDescription(workEvent, user);
                var model = new NotificationViewModel
                {
                    Text = description,
                    Type = (int)workEvent.Type,
                    WorkItemId = workEvent.ObjectId.Value
                };
                clients.recieveNotification(workEvent.Type.ToString().ToLower(), model);
            }
        }

        public void SendNotifications(WorkEvent workEvent, ApplicationUser user)
        {
            SendNotifications(workEvent, new[] {user});
        }
    }
}