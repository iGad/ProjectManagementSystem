using System.Linq;
using NUnit.Framework;
using PMS.Model.CommonModels.EventModels;
using PMS.Model.Models;
using PMS.Model.Repositories;
using PMS.Model.Services;
using PMS.Model.UnitTests.Common;
using PMS.Model.UnitTests.Fakes;

namespace PMS.Model.UnitTests.Services
{
    [TestFixture]
    public class EventServiceTests
    {
        private TestUserRepository CreateUserRepository()
        {
            return new TestUserRepository();
        }

        private TestEventRepository CreateTestEventRepository()
        {
            return new TestEventRepository();
        }

        private TestEventRepository CreateFilledEventRepository(TestUserRepository userRepo)
        {
            var repo = CreateTestEventRepository();
            var users = userRepo.GetUsersByRole(RoleType.Manager);
            var @event = CreateEvent(new TGuid(0).ToGuid().ToString());
            @event.Id = 1;
            repo.Events.Add(@event);
            var workEvent = CreateEvent(new TGuid(2).ToGuid().ToString());
            workEvent.Id = 2;
            repo.Events.Add(workEvent);
            foreach (var user in users)
            {
                repo.EventsUsers.Add(new WorkEventUserRelation(@event.Id, user.Id) {State = EventState.New});
                repo.EventsUsers.Add(new WorkEventUserRelation(workEvent.Id, user.Id) { State = EventState.Seen });
            }
            return repo;
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

        private EventService CreateService(IEventRepository eventRepository, TestUserRepository userRepo)
        {
            var describers = new EventDescriber[] {new TestEventDescriber()};
            return new EventService(eventRepository, userRepo, describers);
        }

        [Test]
        [TestCase(3, new [] { 3, 5, 6 })]
        [TestCase(3, new[] { 5, 6 })]
        [TestCase(0, new[] { 2, 6 })]
        [TestCase(3, new[] { 3, 0 })]
        public void AddEvent_WhenEventIsValid_AddEventForExpectedUsers(int currentUserNumber, int[] users)
        {
            var currentUserId = new TGuid(currentUserNumber).ToGuid().ToString();
            var userRepo = CreateUserRepository();
            userRepo.SetCurrentUser(currentUserId);
            var eventRepo = CreateTestEventRepository();
            var service = CreateService(eventRepo, userRepo);
            var workEvent = CreateEvent(currentUserId);

            service.AddEvent(workEvent, users.Select(x => new TGuid(x).ToGuid().ToString()));

            Assert.AreEqual(1, eventRepo.Events.Count);
            Assert.AreEqual(users.Length, eventRepo.EventsUsers.Count);
        }

        [Test]
        public void AddEvent_WhenEventAddToCurrentUser_MakeEventSeen()
        {
            var currentUserId = new TGuid(3).ToGuid().ToString();
            var userRepo = CreateUserRepository();
            userRepo.SetCurrentUser(currentUserId);
            var eventRepo = CreateTestEventRepository();
            var service = CreateService(eventRepo, userRepo);
            var workEvent = CreateEvent(currentUserId);

            service.AddEvent(workEvent, new [] {currentUserId});

            Assert.IsTrue(eventRepo.EventsUsers.All(x => x.State == EventState.Seen));
        }

        [Test]
        public void AddEvent_WhenEventAddToOtherUsers_MakeEventNew()
        {
            var currentUserId = new TGuid(0).ToGuid().ToString();
            var userRepo = CreateUserRepository();
            userRepo.SetCurrentUser(currentUserId);
            var eventRepo = CreateTestEventRepository();
            var service = CreateService(eventRepo, userRepo);
            var workEvent = CreateEvent(currentUserId);

            service.AddEvent(workEvent, (new[] {2, 3, 5, 6}).Select(x => new TGuid(x).ToGuid().ToString()));

            Assert.IsTrue(eventRepo.EventsUsers.All(x => x.State == EventState.New));
        }

        [Test]
        public void ChangeUserEventState_WhenRelationIsExists_SetNewStateAndCallSaveChanges()
        {
            var userRepo = CreateUserRepository();
            var currentUserId = userRepo.GetUsersByRole(RoleType.Manager).First().Id;
            userRepo.SetCurrentUser(currentUserId);
            var eventRepo = CreateFilledEventRepository(userRepo);
            var service = CreateService(eventRepo, userRepo);
            var workEvent = eventRepo.Events.First();
            var eventUserRelation = eventRepo.EventsUsers.Single(x => x.EventId == workEvent.Id && x.UserId == currentUserId);

            service.ChangeUserEventState(workEvent.Id, currentUserId, EventState.Seen);

            Assert.AreEqual(EventState.Seen, eventUserRelation.State);
            Assert.AreEqual(1, eventRepo.SaveChangesCalled);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void ChangeUserEventIsFavorite_WhenRelationIsExists_SetNewValueAndCallSaveChanges(bool expectedValue)
        {
            var userRepo = CreateUserRepository();
            var currentUserId = userRepo.GetUsersByRole(RoleType.Manager).First().Id;
            userRepo.SetCurrentUser(currentUserId);
            var eventRepo = CreateFilledEventRepository(userRepo);
            var service = CreateService(eventRepo, userRepo);
            var workEvent = eventRepo.Events.First();
            var eventUserRelation = eventRepo.EventsUsers.Single(x => x.EventId == workEvent.Id && x.UserId == currentUserId);
            eventUserRelation.IsFavorite = !expectedValue;

            service.ChangeUserEventIsFavorite(workEvent.Id, currentUserId, expectedValue);

            Assert.AreEqual(expectedValue, eventUserRelation.IsFavorite);
            Assert.AreEqual(1, eventRepo.SaveChangesCalled);
        }

        [Test]
        public void GetEventDisplayModel_Always_SetStateAndDescription()
        {
            var userRepo = CreateUserRepository();
            var currentUser = userRepo.GetUsersByRole(RoleType.Manager).First();
            userRepo.SetCurrentUser(currentUser.Id);
            var eventRepo = CreateFilledEventRepository(userRepo);
            var service = CreateService(eventRepo, userRepo);
            var workEvent = eventRepo.Events.First();
            var eventUserRelation = eventRepo.EventsUsers.Single(x => x.EventId == workEvent.Id && x.UserId == currentUser.Id);
            
            var model = service.GetEventDisplayModel(new EventUserModel(workEvent, eventUserRelation), currentUser);

            Assert.AreEqual(eventUserRelation.State, model.State);
            Assert.AreEqual("test", model.Description);
        }
    }
}
