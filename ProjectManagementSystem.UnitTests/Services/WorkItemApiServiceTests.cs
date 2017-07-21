using System;
using System.Linq;
using NUnit.Framework;
using PMS.Model.CommonModels.EventModels;
using PMS.Model.Models;
using PMS.Model.Repositories;
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
        [TestCase(10053, WorkItemState.Reviewing, 3)]
        [TestCase(10053, WorkItemState.Done, 0)]//
        [TestCase(10094, WorkItemState.AtWork, 6)]
        [TestCase(10081, WorkItemState.AtWork, 4)]
        [TestCase(10081, WorkItemState.Done, 0)]
        [TestCase(10105, WorkItemState.Done, 3)]
        public void Update_WhenOnlyStateChanged_SendExpectedEvents(int workItemNumber, WorkItemState newState, int currentUserId)
        {
            var userRepository = CreateUserRepository();
            userRepository.SetCurrentUser(new TGuid(currentUserId).ToGuid().ToString());
            var workItemRepository = TestHelper.CreateFilledWorkItemRepository(userRepository);
            var notifyService = CreateTestNotifyService();
            var service = CreateService(workItemRepository, userRepository, notifyService);
            var workItem = new WorkItem(workItemRepository.GetById(workItemNumber));
            workItem.State = newState;
          
            service.Update(workItem);

            Assert.AreEqual(1, notifyService.SendedRealtimeEvents.Count);
            Assert.AreEqual(1, notifyService.SendedEventNotifications.Count);
            Assert.AreEqual(EventType.WorkItemStateChanged, notifyService.SendedEventNotifications.First().Type);
        }
        

        [Test]
        [TestCase(10053, WorkItemState.Reviewing, 3)]
        [TestCase(10053, WorkItemState.Done, 0)]
        [TestCase(10094, WorkItemState.AtWork, 6)]
        [TestCase(10081, WorkItemState.AtWork, 4)]
        [TestCase(10081, WorkItemState.Done, 0)]
        [TestCase(10105, WorkItemState.Done, 3)]
        public void Update_WhenStateWithOtherPropertiesChanged_SendExpectedNotification(int workItemNumber, WorkItemState newState, int currentUserId)
        {
            var userRepository = CreateUserRepository();
            userRepository.SetCurrentUser(new TGuid(currentUserId).ToGuid().ToString());
            var workItemRepository = TestHelper.CreateFilledWorkItemRepository(userRepository);
            var notifyService = CreateTestNotifyService();
            var service = CreateService(workItemRepository, userRepository, notifyService);
            var workItem = new WorkItem(workItemRepository.GetById(workItemNumber)) {State = newState};
            workItem.Name += "test";
            workItem.Description += "test";
            
            service.Update(workItem);

            Assert.AreEqual(1, notifyService.SendedRealtimeEvents.Count);
            Assert.AreEqual(1, notifyService.SendedEventNotifications.Count);
            Assert.AreEqual(EventType.WorkItemChanged, notifyService.SendedEventNotifications.First().Type);
        }

        [Test]
        [TestCase(10053, 6, 6, EventType.WorkItemDisappointed, EventType.WorkItemChanged)]//новый исполнитель меняет исполнителя
        [TestCase(10053, 0, 5, EventType.WorkItemDisappointed, EventType.WorkItemChanged, EventType.WorkItemAppointed)]//Директор меняет исполнителя
        [TestCase(10094, 2, 5, EventType.WorkItemDisappointed, EventType.WorkItemChanged, EventType.WorkItemAppointed)]//РН меняет исполнителя
        [TestCase(10094, 6, 5, EventType.WorkItemAppointed, EventType.WorkItemChanged)]//текущий исполнитель меняет исполнителя
        [TestCase(10105, 3, 6, EventType.WorkItemAppointed, EventType.WorkItemChanged)]//текущий исполнитель - РН, меняет на другого
        [TestCase(10105, 3, -1, EventType.WorkItemChanged)]//текущий исполнитель - РН, меняет на пусто
        [TestCase(10105, 2, -1, EventType.WorkItemDisappointed, EventType.WorkItemChanged)]//текущий исполнитель - РН, другой пользователь меняет на пусто
        public void Update_WhenExecutorChanged_SendExpectedEvents(int workItemNumber, int currentUserId, int newExecutorId, params EventType[] eventTypes)
        {
            var events = eventTypes.Select(x => x.ToString());
            var userRepository = CreateUserRepository();
            userRepository.SetCurrentUser(new TGuid(currentUserId).ToGuid().ToString());
            var workItemRepository = TestHelper.CreateFilledWorkItemRepository(userRepository);
            var notifyService = CreateTestNotifyService();
            var service = CreateService(workItemRepository, userRepository, notifyService);
            var workItem = new WorkItem(workItemRepository.GetById(workItemNumber)) {ExecutorId = newExecutorId >= 0 ? new TGuid(newExecutorId).ToGuid().ToString() : null};

            service.Update(workItem);

            Assert.IsTrue(events.All(x => notifyService.SendedEventNotifications.Any(ev=>ev.Type.ToString() == x)));
        }

        [Test]
        [TestCase(10053, 6, 6, 3)]//новый исполнитель меняет исполнителя
        [TestCase(10053, 0, 5, 3)]//Директор меняет исполнителя
        [TestCase(10094, 2, 5, 3)]//РН меняет исполнителя
        [TestCase(10094, 6, 5, 3)]//текущий исполнитель меняет исполнителя
        [TestCase(10105, 3, 6, 3)]//текущий исполнитель - РН, меняет на другого
        [TestCase(10105, 3, -1, 2)]//текущий исполнитель - РН, меняет на пусто
        [TestCase(10105, 2, -1, 2)]//текущий исполнитель - РН, другой пользователь меняет на пусто
        [TestCase(10106, 3, 6, 2)]//текущий исполнитель - пусто, РН меняет на другого
        public void Update_WhenExecutorChanged_AddExpectedEvents(int workItemNumber, int currentUserId, int newExecutorId, int eventCount)
        {
            var userRepository = CreateUserRepository();
            userRepository.SetCurrentUser(new TGuid(currentUserId).ToGuid().ToString());
            var workItemRepository = TestHelper.CreateFilledWorkItemRepository(userRepository);
            var notificationService = CreateTestNotifyService();
            var service = CreateService(workItemRepository, userRepository, notificationService);
            var workItem = new WorkItem(workItemRepository.GetById(workItemNumber));
            workItem.ExecutorId = newExecutorId >= 0 ? new TGuid(newExecutorId).ToGuid().ToString() : null;

            service.Update(workItem);

            Assert.AreEqual(1, notificationService.SendedRealtimeEvents.Count);
            Assert.AreEqual(typeof(WorkItemUpdatedModel), notificationService.SendedRealtimeEvents.First().SendedObject.GetType());
            Assert.AreEqual(eventCount, notificationService.SendedEventNotifications.Count);
        }
        
        [Test]
        [TestCase(10053, 0, 1)]//директор удаляет задачу
        [TestCase(10094, 2, 1)]//РН удаляет задачу
        public void Delete_Always_SendExpectedNotifications(int workItemNumber, int currentUserId, int userCount)
        {
            var userRepository = CreateUserRepository();
            userRepository.SetCurrentUser(new TGuid(currentUserId).ToGuid().ToString());
            var workItemRepository = TestHelper.CreateFilledWorkItemRepository(userRepository);
            var notifyService = CreateTestNotifyService();
            var service = CreateService(workItemRepository, userRepository, notifyService);
            var workItem = workItemRepository.GetById(workItemNumber);
            workItem.ExecutorId = null;

            service.Delete(workItemNumber);

            Assert.AreEqual(1, notifyService.SendedRealtimeEvents.Count);
            Assert.AreEqual(EventType.WorkItemDeleted, ((WorkEvent) notifyService.SendedRealtimeEvents[0].SendedObject).Type);
            Assert.AreEqual(1, notifyService.SendedEventNotifications.Count);
            Assert.AreEqual(EventType.WorkItemDeleted, notifyService.SendedEventNotifications[0].Type);
        }

        [Test]
        [TestCase(10053, 0)]//директор удаляет задачу
        [TestCase(10094, 2)]//РН удаляет задачу
        [TestCase(10094, 6)]//текущий исполнитель удаляет задачу
        [TestCase(10105, 3)]//текущий исполнитель - РН, удаляет задачу
        public void Delete_WhenExecutorIsSet_AddExpectedEventNotifications(int workItemNumber, int currentUserId)
        {
            var userRepository = CreateUserRepository();
            userRepository.SetCurrentUser(new TGuid(currentUserId).ToGuid().ToString());
            var workItemRepository = TestHelper.CreateFilledWorkItemRepository(userRepository);
            var notificationService = CreateTestNotifyService();
            var service = CreateService(workItemRepository, userRepository, notificationService);

            service.Delete(workItemNumber);

            Assert.AreEqual(1, notificationService.SendedRealtimeEvents.Count);
        }
        
        [Test]
        [TestCase(3, 6, 3)]//РН добавляет задачу с исполнителем
        [TestCase(3, 3, 3)] //РН добавляет себе задачу
        [TestCase(6, 6, 3)] //исполнитель добавляет себе задачу
        [TestCase(0, 6, 3)] //директор добавляет задачу с исполнителем
        [TestCase(0, 3, 3)] //директор добавляет задачу с исполнителем РН
        [TestCase(0, 0, 0)] //директор добавляет задачу себе, при этом сам РН
        public void Add_WhenExecutorIsSet_SendExpectedEventNotifications(int currentUserNumber, int executorNumber, int parentItemExecutor)
        {
            var userRepository = CreateUserRepository();
            var curentUserId = new TGuid(currentUserNumber).ToGuid().ToString();
            userRepository.SetCurrentUser(curentUserId);
            var workItemRepository = TestHelper.CreateFilledWorkItemRepository(userRepository);
            var notifyService = CreateTestNotifyService();
            var service = CreateService(workItemRepository, userRepository, notifyService);
            var workItem = CreateWorkItem(workItemRepository, curentUserId, new TGuid(parentItemExecutor).ToGuid().ToString());
                workItem.ExecutorId = new TGuid(executorNumber).ToGuid().ToString();

            service.Add(workItem);

            Assert.AreEqual(curentUserId, workItem.CreatorId);
            Assert.AreEqual(1, notifyService.SendedRealtimeEvents.Count);
            Assert.AreEqual(2, notifyService.SendedEventNotifications.Count);
            Assert.IsTrue(notifyService.SendedEventNotifications.Any(x => x.Type == EventType.WorkItemAdded));
            Assert.IsTrue(notifyService.SendedEventNotifications.Any(x=>x.Type == EventType.WorkItemAppointed));
        }

        [Test]
        [TestCase(3, 3)] //РН добавляет задачу без исполнителя
        [TestCase(6, 3)] //исполнитель добавляет задачу без исполнителя
        [TestCase(0, 0)] //директор добавляет задачу без исполнителя в свой раздел
        public void Add_WhenExecutorIsNotSet_SendExpectedEventNotifications(int currentUserNumber, int parentItemExecutor)
        {
            var userRepository = CreateUserRepository();
            var curentUserId = new TGuid(currentUserNumber).ToGuid().ToString();
            userRepository.SetCurrentUser(curentUserId);
            var workItemRepository = TestHelper.CreateFilledWorkItemRepository(userRepository);
            var notifyService = CreateTestNotifyService();
            var service = CreateService(workItemRepository, userRepository, notifyService);
            var workItem = CreateWorkItem(workItemRepository, curentUserId, new TGuid(parentItemExecutor).ToGuid().ToString());

            service.Add(workItem);

            Assert.AreEqual(curentUserId, workItem.CreatorId);
            Assert.AreEqual(1, notifyService.SendedRealtimeEvents.Count);
            Assert.AreEqual(1, notifyService.SendedEventNotifications.Count);
            Assert.IsTrue(notifyService.SendedEventNotifications.All(x=>x.Type == EventType.WorkItemAdded));
        }
        /*  var items =
                workItemRepository.WorkItems.Where(
                    x =>
                        x.Type == WorkItemType.Task && x.ParentId.HasValue &&
                        workItemRepository.WorkItems.Any(wi => wi.Id == x.ParentId.Value && wi.ExecutorId == x.ExecutorId)).OrderBy(x => x.Id);*/
    }
}
