using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Common.Models;
using Common.Services;

namespace ProjectManagementSystem.UnitTests.Fakes
{
    struct NotificationInfo
    {
        public string Name { get; set; }
        public object SendedObject { get; set; }
        public BroadcastType Type { get; set; }
        public string[] Users { get; set; }
    }
    class TestNotifyService : INotifyService
    {
        public List<NotificationInfo> SendedNotifications { get; } = new List<NotificationInfo>();
        public List<NotificationInfo> SendedEvents { get; } = new List<NotificationInfo>();
        public void SendEvent(string eventName, object sendedObject, BroadcastType broadcastType, params string[] userNames)
        {
            SendedEvents.Add(new NotificationInfo {Name = eventName, SendedObject = sendedObject, Type = broadcastType, Users = null});
        }

        public void SendNotification(string eventName, object sendedObject, BroadcastType broadcastType, params string[] userNames)
        {
            if (broadcastType != BroadcastType.All && (userNames == null || userNames.All(string.IsNullOrWhiteSpace)))
                return;
            SendedNotifications.Add(new NotificationInfo {Name = eventName, SendedObject = sendedObject, Type = broadcastType, Users = userNames});
        }
    }
}
