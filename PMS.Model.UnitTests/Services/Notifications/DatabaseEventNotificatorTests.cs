using System;
using System.Linq;
using NUnit.Framework;
using PMS.Model.Models;
using PMS.Model.Repositories;
using PMS.Model.Services.Notifications;
using PMS.Model.UnitTests.Common;
using PMS.Model.UnitTests.Fakes;

namespace PMS.Model.UnitTests.Services.Notifications
{
    [TestFixture]
    public class DatabaseEventNotificatorTests
    {
        private TestEventRepository CreateEventRepository()
        {
            return new TestEventRepository();
        }

        private DatabaseEventNotificator CreateNotificator(IEventRepository repo)
        {
            return new DatabaseEventNotificator(repo);
        }

        private WorkEvent CreateEvent(string userId)
        {
            return new WorkEvent
            {
                Type = EventType.WorkItemAdded,
                UserId = userId,
                ObjectId = 1001,
                Data = "1001"
            };
        }

        [Test]
        [TestCase(3, new[] { 3, 5, 6 }, 3)]
        [TestCase(0, new[] { 2, 6 }, 3)]
        [TestCase(3, new[] { 3, 0 }, 2)]
        public void Notify_WhenEventIsValid_AddEventForExpectedUsers(int currentUserNumber, int[] users, int expectedEventUserCount)
        {
            var currentUserId = new TGuid(currentUserNumber).ToGuid().ToString();
            var eventRepo = CreateEventRepository();
            var notificator = CreateNotificator(eventRepo);
            var workEvent = CreateEvent(currentUserId);

            notificator.Notify(workEvent, users.Select(x => new ApplicationUser {Id = new TGuid(x).ToGuid().ToString()}).ToArray());

            Assert.AreEqual(1, eventRepo.Events.Count);
            Assert.AreEqual(expectedEventUserCount, eventRepo.EventsUsers.Count);
        }
        
        [Test]
        public void Notify_WhenEventAddToCurrentUser_MakeEventSeen()
        {
            var currentUserId = new TGuid(3).ToGuid().ToString();
            var eventRepo = CreateEventRepository();
            var notificator = CreateNotificator(eventRepo);
            var workEvent = CreateEvent(currentUserId);

            notificator.Notify(workEvent, new[] {new ApplicationUser {Id = currentUserId}});

            Assert.IsTrue(eventRepo.EventsUsers.All(x => x.State == EventState.Seen));
        }
    }
}
