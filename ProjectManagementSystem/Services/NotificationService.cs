using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Common.Models;
using Microsoft.AspNet.SignalR.Hubs;

namespace ProjectManagementSystem.Services
{
    public enum BroadcastType
    {
        All = 0,
        Others = 1,
        Users = 2
    }
    public class NotificationService
    {
        //private ConcurrentDictionary<string, List<Guid>> userConnectionsDictionary = new ConcurrentDictionary<string, List<Guid>>();

        public NotificationService(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
        }

        public IHubConnectionContext<dynamic> Clients { get; }

        public void SendEvent(string eventName, Entity sendedObject, BroadcastType broadcastType, params string[] userNames)
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
    }
}