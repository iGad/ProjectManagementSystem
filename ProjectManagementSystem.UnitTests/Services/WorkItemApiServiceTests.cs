using System;
using System.Linq;
using Common.Services;
using NUnit.Framework;
using PMS.Model;
using PMS.Model.Models;
using PMS.Model.Repositories;
using PMS.Model.Services;
using PMS.Model.UnitTests;
using PMS.Model.UnitTests.Common;
using PMS.Model.UnitTests.Fakes;
using ProjectManagementSystem.Services;
using ProjectManagementSystem.UnitTests.Fakes;

namespace ProjectManagementSystem.UnitTests.Services
{
    [TestFixture]
    public class WorkItemApiServiceTests
    {
        private TestUserRepository CreateUserRepository()
        {
            return new TestUserRepository();
        }

        private TestNotificationService CreateTestNotifyService()
        {
            return new TestNotificationService();
        }
        

        private WorkItemApiService CreateService(IWorkItemRepository workItemRepository, TestUserRepository userRepository)
        {
            return CreateService(workItemRepository, userRepository, CreateTestNotifyService());
        }

        private WorkItemApiService CreateService(IWorkItemRepository workItemRepository, TestUserRepository userRepository, TestNotificationService notifyService)
        {
            var settingsProvider = new TestSettingsValueProvider();
            return new WorkItemApiService(workItemRepository, userRepository, notifyService, notifyService, settingsProvider);
        }

        private WorkItem CreateWorkItem(TestWorkItemRepository repository, string userId, string parentItemExecutor)
        {
            return new WorkItem
            {
                Name = "test",
                Description = "test",
                CreatorId = userId,
                ParentId = repository.WorkItems.First(x=>x.Type == WorkItemType.Partition && x.State == WorkItemState.AtWork && x.ExecutorId == parentItemExecutor).Id,
                DeadLine = DateTime.Today,
                Type = WorkItemType.Task
            };
        }

        [Test]
        [TestCase(10053, WorkItemState.Reviewing, 3, 1, 0, 2)]
        [TestCase(10053, WorkItemState.Done, 0, 1, 2, 3)]//
        [TestCase(10094, WorkItemState.AtWork, 6, 1, 0, 2)]
        [TestCase(10081, WorkItemState.AtWork, 4, 1, 0, 2)]
        [TestCase(10081, WorkItemState.Done, 0, 1, 2, 4)]
        [TestCase(10105, WorkItemState.Done, 3, 1, 0)]
        public void Update_WhenOnlyStateChanged_SendExpectedNotification(int workItemNumber, WorkItemState newState, int currentUserId, int notificationCount, params int[] usersNumbers)
        {
            var userRepository = CreateUserRepository();
            userRepository.SetCurrentUser(new TGuid(currentUserId).ToGuid().ToString());
            var workItemRepository = TestHelper.CreateFilledWorkItemRepository(userRepository);
            var notifyService = CreateTestNotifyService();
            var service = CreateService(workItemRepository, userRepository, notifyService);
            var workItem = new WorkItem(workItemRepository.GetById(workItemNumber));
            workItem.State = newState;
            var usersIds = usersNumbers.Select(x => new TGuid(x).ToGuid().ToString()).OrderBy(x=>x);
          
            service.Update(workItem);

            Assert.AreEqual(notificationCount, notifyService.SendedNotifications.Count);
            if (usersNumbers.Length > 0)
            {
                Assert.IsTrue(notifyService.SendedNotifications[0].Users.OrderBy(x => x).SequenceEqual(usersIds));
                Assert.IsTrue(notifyService.SendedNotifications[0].Name == Constants.WorkItemStateChangedEventName);
            }
        }
        

        [Test]
        [TestCase(10053, WorkItemState.Reviewing, 3, 1, 0, 2)]
        [TestCase(10053, WorkItemState.Done, 0, 1, 2, 3)]
        [TestCase(10094, WorkItemState.AtWork, 6, 1, 0, 2)]
        [TestCase(10081, WorkItemState.AtWork, 4, 1, 0, 2)]
        [TestCase(10081, WorkItemState.Done, 0, 1, 2, 4)]
        [TestCase(10105, WorkItemState.Done, 3, 1, 0)]
        public void Update_WhenStateWithOtherPropertiesChanged_SendExpectedNotification(int workItemNumber, WorkItemState newState, int currentUserId, int notificationCount, params int[] usersNumbers)
        {
            var userRepository = CreateUserRepository();
            userRepository.SetCurrentUser(new TGuid(currentUserId).ToGuid().ToString());
            var workItemRepository = TestHelper.CreateFilledWorkItemRepository(userRepository);
            var notifyService = CreateTestNotifyService();
            var service = CreateService(workItemRepository, userRepository, notifyService);
            var workItem = new WorkItem(workItemRepository.GetById(workItemNumber));
            workItem.State = newState;
            workItem.Name += "test";
            workItem.Description += "test";
            var usersIds = usersNumbers.Select(x => new TGuid(x).ToGuid().ToString()).OrderBy(x => x);
          
            service.Update(workItem);

            Assert.AreEqual(notificationCount, notifyService.SendedNotifications.Count);
            if (usersNumbers.Length > 0)
            {
                Assert.IsTrue(notifyService.SendedNotifications[0].Users.OrderBy(x => x).SequenceEqual(usersIds));
                Assert.IsTrue(notifyService.SendedNotifications[0].Name == Constants.WorkItemChangedEventName);
            }
        }

        [Test]
        [TestCase(10053, 6, 6, 2, EventType.WorkItemDisappointed, EventType.WorkItemChanged)]//новый исполнитель меняет исполнителя
        [TestCase(10053, 0, 5, 3, EventType.WorkItemDisappointed, EventType.WorkItemChanged, EventType.WorkItemAppointed)]//Директор меняет исполнителя
        [TestCase(10094, 2, 5, 3, EventType.WorkItemDisappointed, EventType.WorkItemChanged, EventType.WorkItemAppointed)]//РН меняет исполнителя
        [TestCase(10094, 6, 5, 2, EventType.WorkItemAppointed, EventType.WorkItemChanged)]//текущий исполнитель меняет исполнителя
        [TestCase(10105, 3, 6, 2, EventType.WorkItemAppointed, EventType.WorkItemChanged)]//текущий исполнитель - РН, меняет на другого
        [TestCase(10105, 3, -1, 1, EventType.WorkItemChanged)]//текущий исполнитель - РН, меняет на пусто
        [TestCase(10105, 2, -1, 2, EventType.WorkItemDisappointed, EventType.WorkItemChanged)]//текущий исполнитель - РН, другой пользователь меняет на пусто
        public void Update_WhenExecutorChanged_SendExpectedNotifications(int workItemNumber, int currentUserId, int newExecutorId, int notificationCount, params EventType[] evetnTypes)
        {
            var events = evetnTypes.Select(x => x.ToString().ToLower());
            var userRepository = CreateUserRepository();
            userRepository.SetCurrentUser(new TGuid(currentUserId).ToGuid().ToString());
            var workItemRepository = TestHelper.CreateFilledWorkItemRepository(userRepository);
            var notifyService = CreateTestNotifyService();
            var service = CreateService(workItemRepository, userRepository, notifyService);
            var workItem = new WorkItem(workItemRepository.GetById(workItemNumber));
            workItem.ExecutorId = newExecutorId >= 0 ? new TGuid(newExecutorId).ToGuid().ToString() : null;

            service.Update(workItem);

            Assert.AreEqual(notificationCount, notifyService.SendedNotifications.Count);
            if(notificationCount > 0)
                Assert.IsTrue(notifyService.SendedNotifications.All(x => events.Contains(x.Name)));
        }

        [Test]
        [TestCase(10053, 6, 6, 3, 4)]//новый исполнитель меняет исполнителя
        [TestCase(10053, 0, 5, 3, 3)]//Директор меняет исполнителя
        [TestCase(10094, 2, 5, 3, 3)]//РН меняет исполнителя
        [TestCase(10094, 6, 5, 3, 3)]//текущий исполнитель меняет исполнителя
        [TestCase(10105, 3, 6, 3, 2)]//текущий исполнитель - РН, меняет на другого
        [TestCase(10105, 3, -1, 2, 2)]//текущий исполнитель - РН, меняет на пусто
        [TestCase(10105, 2, -1, 2, 3)]//текущий исполнитель - РН, другой пользователь меняет на пусто
        public void Update_WhenExecutorChanged_AddExpectedEvents(int workItemNumber, int currentUserId, int newExecutorId, int eventCount, int expectedNotifyedUserCount)
        {
            var userRepository = CreateUserRepository();
            userRepository.SetCurrentUser(new TGuid(currentUserId).ToGuid().ToString());
            var workItemRepository = TestHelper.CreateFilledWorkItemRepository(userRepository);
            var notificationService = CreateTestNotifyService();
            var service = CreateService(workItemRepository, userRepository, notificationService);
            var workItem = new WorkItem(workItemRepository.GetById(workItemNumber));
            workItem.ExecutorId = newExecutorId >= 0 ? new TGuid(newExecutorId).ToGuid().ToString() : null;

            service.Update(workItem);

            Assert.AreEqual(eventCount, notificationService.SendedEvents.Count);
            var changedEvent = notificationService.SendedEvents.Single(x => ((WorkEvent)x.SendedObject).Type == EventType.WorkItemChanged);
            //Assert.AreEqual(expectedNotifyedUserCount, eventService.EventsUsers.Count(x => x.EventId == changedEvent.Id));
        }

        [Test]
        [TestCase(10053, 0, 2)]//директор удаляет задачу
        [TestCase(10094, 2, 2)]//РН удаляет задачу
        [TestCase(10094, 6, 2)]//текущий исполнитель удаляет задачу
        [TestCase(10105, 3, 1)]//текущий исполнитель - РН, удаляет задачу
        public void Delete_WhenExecutorIsSet_SendExpectedNotifications(int workItemNumber, int currentUserId, int userCount)
        {
            var userRepository = CreateUserRepository();
            userRepository.SetCurrentUser(new TGuid(currentUserId).ToGuid().ToString());
            var workItemRepository = TestHelper.CreateFilledWorkItemRepository(userRepository);
            var notifyService = CreateTestNotifyService();
            var service = CreateService(workItemRepository, userRepository, notifyService);
            
            service.Delete(workItemNumber, true);

            Assert.AreEqual(1, notifyService.SendedNotifications.Count);
            Assert.AreEqual(userCount, notifyService.SendedNotifications[0].Users.Length);
        }

        [Test]
        [TestCase(10053, 0, 1)]//директор удаляет задачу
        [TestCase(10094, 2, 1)]//РН удаляет задачу
        public void Delete_WhenExecutorIsNotSet_SendExpectedNotifications(int workItemNumber, int currentUserId, int userCount)
        {
            var userRepository = CreateUserRepository();
            userRepository.SetCurrentUser(new TGuid(currentUserId).ToGuid().ToString());
            var workItemRepository = TestHelper.CreateFilledWorkItemRepository(userRepository);
            var notifyService = CreateTestNotifyService();
            var service = CreateService(workItemRepository, userRepository, notifyService);
            var workItem = workItemRepository.GetById(workItemNumber);
            workItem.ExecutorId = null;

            service.Delete(workItemNumber, true);

            Assert.AreEqual(1, notifyService.SendedNotifications.Count);
            Assert.AreEqual(userCount, notifyService.SendedNotifications[0].Users.Length);
        }

        [Test]
        [TestCase(10053, 0, 3)]//директор удаляет задачу
        [TestCase(10094, 2, 3)]//РН удаляет задачу
        [TestCase(10094, 6, 3)]//текущий исполнитель удаляет задачу
        [TestCase(10105, 3, 2)]//текущий исполнитель - РН, удаляет задачу
        public void Delete_WhenExecutorIsSet_AddEventForExpectedUsers(int workItemNumber, int currentUserId, int userCount)
        {
            var userRepository = CreateUserRepository();
            userRepository.SetCurrentUser(new TGuid(currentUserId).ToGuid().ToString());
            var workItemRepository = TestHelper.CreateFilledWorkItemRepository(userRepository);
            var notificationService = CreateTestNotifyService();
            var service = CreateService(workItemRepository, userRepository, notificationService);

            service.Delete(workItemNumber, true);

            Assert.AreEqual(1, notificationService.SendedEvents.Count);
        }

        [Test]
        [TestCase(3, 6, 3, 2, EventType.WorkItemAdded, EventType.WorkItemAppointed)]//РН добавляет задачу с исполнителем
        [TestCase(3, 3, 3, 1, EventType.WorkItemAdded)] //РН добавляет себе задачу
        [TestCase(3, -1, 3, 1, EventType.WorkItemAdded)] //РН добавляет задачу без исполнителя
        [TestCase(6, -1, 3, 1, EventType.WorkItemAdded)] //исполнитель добавляет задачу без исполнителя
        [TestCase(6, 6, 3, 1, EventType.WorkItemAdded)] //исполнитель добавляет себе задачу
        [TestCase(0, 6, 3, 2, EventType.WorkItemAdded, EventType.WorkItemAppointed)] //директор добавляет задачу с исполнителем
        [TestCase(0, 3, 3, 2, EventType.WorkItemAdded, EventType.WorkItemAppointed)] //директор добавляет задачу с исполнителем РН
        public void Add_Always_NotifyExpectedUsers(int currentUserNumber, int executorNumber, int parentItemExecutor, int notificationCount, params EventType[] eventTypes)
        {
            var events = eventTypes.Select(x => x.ToString().ToLower()).ToArray();
            var userRepository = CreateUserRepository();
            userRepository.SetCurrentUser(new TGuid(currentUserNumber).ToGuid().ToString());
            var workItemRepository = TestHelper.CreateFilledWorkItemRepository(userRepository);
            var notifyService = CreateTestNotifyService();
            var service = CreateService(workItemRepository, userRepository, notifyService);
            var workItem = CreateWorkItem(workItemRepository, new TGuid(currentUserNumber).ToGuid().ToString(), new TGuid(parentItemExecutor).ToGuid().ToString());
            if (executorNumber >= 0)
                workItem.ExecutorId = new TGuid(executorNumber).ToGuid().ToString();

            service.Add(workItem);

            Assert.AreEqual(notificationCount, notifyService.SendedNotifications.Count);
            if (notificationCount > 0)
                Assert.IsTrue(notifyService.SendedNotifications.All(x => events.Contains(x.Name)));
        }

        [Test]
        [TestCase(3, 6, 3, 3)]//РН добавляет задачу с исполнителем
        [TestCase(3, 3, 3, 2)] //РН добавляет себе задачу
        [TestCase(6, 6, 3, 3)] //исполнитель добавляет себе задачу
        [TestCase(0, 6, 3, 3)] //директор добавляет задачу с исполнителем
        [TestCase(0, 3, 3, 2)] //директор добавляет задачу с исполнителем РН
        [TestCase(0, 0, 0, 1)] //директор добавляет задачу себе, при этом сам РН
        public void Add_WhenExecutorIsSet_AddExpectedEventsForExpectedUsers(int currentUserNumber, int executorNumber, int parentItemExecutor, int userCount)
        {
            var userRepository = CreateUserRepository();
            var curentUserId = new TGuid(currentUserNumber).ToGuid().ToString();
            userRepository.SetCurrentUser(curentUserId);
            var workItemRepository = TestHelper.CreateFilledWorkItemRepository(userRepository);
            var notifyService = CreateTestNotifyService();
            var service = CreateService(workItemRepository, userRepository, notifyService);
            var workItem = CreateWorkItem(workItemRepository, curentUserId, new TGuid(parentItemExecutor).ToGuid().ToString());
            //if (executorNumber.HasValue)
                workItem.ExecutorId = new TGuid(executorNumber).ToGuid().ToString();

            service.Add(workItem);

            Assert.AreEqual(2, notifyService.SendedEvents.Count);
            var addedEvent = notifyService.SendedEvents.First().SendedObject as WorkEvent;
            Assert.AreEqual(EventType.WorkItemAdded, addedEvent.Type);
            Assert.AreEqual(EventType.WorkItemAppointed, ((WorkEvent)notifyService.SendedEvents.Last().SendedObject).Type);
           // Assert.AreEqual(userCount, eventService.EventsUsers.Count(x => x.EventId == addedEvent.Id));
        }

        [Test]
        [TestCase(3, 3, 2)] //РН добавляет задачу без исполнителя
        [TestCase(6, 3, 3)] //исполнитель добавляет задачу без исполнителя
        [TestCase(0, 0, 1)] //директор добавляет задачу без исполнителя в свой раздел
        public void Add_WhenExecutorIsNotSet_AddOnlyAddEventForExpectedUsers(int currentUserNumber, int parentItemExecutor, int userCount)
        {
            var userRepository = CreateUserRepository();
            var curentUserId = new TGuid(currentUserNumber).ToGuid().ToString();
            userRepository.SetCurrentUser(curentUserId);
            var workItemRepository = TestHelper.CreateFilledWorkItemRepository(userRepository);
            var notifyService = CreateTestNotifyService();
            var service = CreateService(workItemRepository, userRepository, notifyService);
            var workItem = CreateWorkItem(workItemRepository, curentUserId, new TGuid(parentItemExecutor).ToGuid().ToString());

            service.Add(workItem);

            Assert.AreEqual(1, notifyService.SendedEvents.Count);
            var addedEvent = notifyService.SendedEvents.First().SendedObject as WorkEvent;
            Assert.AreEqual(EventType.WorkItemAdded, addedEvent.Type);
            //Assert.AreEqual(userCount, eventService.EventsUsers.Count);
        }
        /*  var items =
                workItemRepository.WorkItems.Where(
                    x =>
                        x.Type == WorkItemType.Task && x.ParentId.HasValue &&
                        workItemRepository.WorkItems.Any(wi => wi.Id == x.ParentId.Value && wi.ExecutorId == x.ExecutorId)).OrderBy(x => x.Id);*/
    }
}
