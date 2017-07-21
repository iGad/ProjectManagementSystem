using System.Collections.Generic;
using Common.Models;
using PMS.Model.Models;
using PMS.Model.Services;
using PMS.Model.Services.Notifications;

namespace ProjectManagementSystem.UnitTests.Fakes
{
    struct NotificationInfo
    {
        public string Name { get; set; }
        public object SendedObject { get; set; }
        public BroadcastType Type { get; set; }
        public string[] Users { get; set; }
    }
    class TestNotificationService : IRealtimeNotificationService, INotificationService
    {
        public List<NotificationInfo> SendedRealtimeEvents { get; } = new List<NotificationInfo>();
        public List<WorkEvent> SendedEventNotifications { get; } = new List<WorkEvent>();
        public void SendEvent(string eventName, object sendedObject, BroadcastType broadcastType, params string[] userNames)
        {
            SendedRealtimeEvents.Add(new NotificationInfo {Name = eventName, SendedObject = sendedObject, Type = broadcastType, Users = null});
        }

        //public void SendNotification(string eventName, object sendedObject, BroadcastType broadcastType, params string[] userNames)
        //{
        //    if (broadcastType != BroadcastType.All && (userNames == null || userNames.All(string.IsNullOrWhiteSpace)))
        //        return;
        //    SendedNotifications.Add(new NotificationInfo {Name = eventName, SendedObject = sendedObject, Type = broadcastType, Users = userNames});
        //}

        //public void SendNotification(WorkEvent workEvent, BroadcastType broadcastType, params string[] userNames)
        //{
        //    SendedNotifications.Add(new NotificationInfo
        //    {
        //        Name = workEvent.Type.ToString().ToLower(),
        //        SendedObject = workEvent,
        //        Type = BroadcastType.Users,
        //        Users = userNames
        //    });
        //}

        //public void SendNotifications(WorkEvent workEvent, ICollection<ApplicationUser> users)
        //{
        //    SendedNotifications.Add(new NotificationInfo
        //    {
        //        Name = workEvent.Type.ToString().ToLower(),
        //        SendedObject = workEvent,
        //        Type = BroadcastType.Users,
        //        Users = users.Select(x => x.Id).ToArray()
        //    });
        //}
        
        //public void SendNotifications(WorkEvent workEvent, ApplicationUser user)
        //{
        //    SendNotifications(workEvent, new [] {user});
            
        //}

        public void SendEventNotifications(WorkEvent @event)
        {
            SendedEventNotifications.Add(@event);
        }
    }
}
