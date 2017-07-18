using System.Linq;
using NUnit.Framework;
using PMS.Model.Models;
using PMS.Model.Repositories;
using PMS.Model.Services;
using PMS.Model.Services.Notifications;
using PMS.Model.UnitTests.Common;
using PMS.Model.UnitTests.Fakes;

namespace PMS.Model.UnitTests.Services.Notifications
{
    [TestFixture]
    public class EventNotificatorsUsersProviderTests
    {
        private TestUserRepository CreateUserRepository(int currentUserId)
        {
            var repo = new TestUserRepository();
            repo.SetCurrentUser(new TGuid(currentUserId).ToGuid().ToString());
            return repo;
        }

        private EventNotificatorsUsersProvider CreateProvider(IUsersService usersService, IWorkItemRepository workItemRepository)
        {
            return new EventNotificatorsUsersProvider(usersService, workItemRepository);
        }

        private WorkEvent CreateEvent(int currentUserId, int workItemNumber, EventType type, object data)
        {
            return new WorkEvent{ UserId = new TGuid(currentUserId).ToGuid().ToString(), ObjectId = workItemNumber, Type = type, Data = data.ToString() };
        }

        [Test]
        [TestCase(10053, EventType.WorkItemCommentAdded, 3, 3, 0, 2)]
        [TestCase(10053, EventType.WorkItemAdded, 3, 3, 0, 2)]
        [TestCase(10053, EventType.WorkItemChanged, 3, 3, 0, 2)]
        [TestCase(10053, EventType.WorkItemDeleted, 3, 3, 0, 2)]
        [TestCase(10053, EventType.WorkItemStateChanged, 3, 3, 0, 2)]
        [TestCase(10053, EventType.WorkItemAdded, 0, 0, 2, 3)]
        [TestCase(10094, EventType.WorkItemChanged, 6, 6, 0, 2)]
        [TestCase(10081, EventType.WorkItemDeleted, 4, 4, 0, 2)]
        [TestCase(10081, EventType.WorkItemStateChanged, 0, 0, 2, 4)]
        [TestCase(10105, EventType.WorkItemChanged, 3, 3, 0)]
        public void GetUsersForEventNotification_WhenDifferentEventType_ReturnExpectedUsers(int workItemNumber, EventType eventType, int currentUserId, params int[] usersNumbers)
        {
            var usersService = CreateUserRepository(currentUserId);
            var workItemRepository = TestHelper.CreateFilledWorkItemRepository(usersService);
            var provider = CreateProvider(usersService, workItemRepository);

            var users = provider.GetUsersForEventNotification(CreateEvent(currentUserId, workItemNumber, eventType, ""));

            Assert.AreEqual(usersNumbers.Length, users.Count);
            Assert.IsTrue(usersNumbers.All(x=>users.Any(u=>u.Id == new TGuid(x).ToGuid().ToString())));
        }


        [Test]
        [TestCase(10053, EventType.WorkItemAppointed, 3, 3)]
        [TestCase(10053, EventType.WorkItemAppointed, 1, 3)]
        [TestCase(10053, EventType.WorkItemDisappointed, 3, 3)]
        [TestCase(10053, EventType.WorkItemDisappointed, 1, 3)]
        public void GetUsersForEventNotification_WhenEventTypeIsAppoitedOrDisappointedWorkItem_ReturnExpectedUser(int workItemNumber, EventType eventType, int relatedUserNumber, int currentUserId)
        {
            var relatedUserId = new TGuid(relatedUserNumber).ToGuid().ToString();
            var usersService = CreateUserRepository(currentUserId);
            var workItemRepository = TestHelper.CreateFilledWorkItemRepository(usersService);
            var provider = CreateProvider(usersService, workItemRepository);
            
            var users = provider.GetUsersForEventNotification(CreateEvent(currentUserId, workItemNumber, eventType, relatedUserId));

            Assert.AreEqual(1, users.Count);
            Assert.AreEqual(relatedUserId, users.Single().Id);
        }
        
    }
}
