using System;
using System.Linq;
using Common.Models;
using Common.Services;
using Microsoft.AspNet.SignalR.Hubs;

namespace ProjectManagementSystem.Services
{
    
    public class NotificationService : INotifyService
    {
        //private ConcurrentDictionary<string, List<Guid>> userConnectionsDictionary = new ConcurrentDictionary<string, List<Guid>>();

        public NotificationService(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
        }

        public IHubConnectionContext<dynamic> Clients { get; }

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
    }
}