using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Common.Services;
using NUnit.Framework;
using PMS.Model;
using PMS.Model.Models;
using PMS.Model.Repositories;
using PMS.Model.UnitTests;
using PMS.Model.UnitTests.Common;
using PMS.Model.UnitTests.Fakes;
using ProjectManagementSystem.Services;
using ProjectManagementSystem.UnitTests.Fakes;
using ProjectManagementSystem.ViewModels;

namespace ProjectManagementSystem.UnitTests.Services
{
    [TestFixture]
    public class WorkItemApiServiceTests
    {
        private TestUserRepository CreateUserRepository()
        {
            return new TestUserRepository();
        }

        private TestNotifyService CreateTestNotifyService()
        {
            return new TestNotifyService();
        }

        private WorkItemApiService CreateService(IWorkItemRepository workItemRepository, TestUserRepository userRepository, INotifyService notifyService)
        {
            return new WorkItemApiService(workItemRepository, userRepository, userRepository, notifyService);
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
        [TestCase(10053, WorkItemState.Reviewing, 3, 1, 2)]
        [TestCase(10053, WorkItemState.Done, 0, 1, 2, 3)]
        [TestCase(10094, WorkItemState.AtWork, 6, 1, 2)]
        [TestCase(10081, WorkItemState.AtWork, 4, 1, 2)]
        [TestCase(10081, WorkItemState.Done, 0, 1, 2, 4)]
        [TestCase(10105, WorkItemState.Done, 3, 0, new int[0])]
        public void Update_WhenOnlyStateChanged_SendExpectedNotification(int workItemNumber, WorkItemState newState, int currentUserId, int notificationCount, params int[] usersNumbers)
        {
            var userRepository = CreateUserRepository();
            userRepository.SetCurrentUser(new TGuid(currentUserId).ToGuid().ToString());
            var workItemRepository = TestHelper.CreateFilledWorkItemRepository(userRepository);
            var notifyService = CreateTestNotifyService();
            var service = CreateService(workItemRepository, userRepository, notifyService);
            var workItem = new WorkItem(workItemRepository.GetById(workItemNumber));
            workItem.State = newState;
            var usersIds = usersNumbers.Select(x => userRepository.GetById(new TGuid(x).ToGuid().ToString()).UserName).OrderBy(x=>x);
          
            service.Update(workItem);

            Assert.AreEqual(notificationCount, notifyService.SendedNotifications.Count);
            if (usersNumbers.Length > 0)
            {
                Assert.IsTrue(notifyService.SendedNotifications[0].Users.OrderBy(x => x).SequenceEqual(usersIds));
                Assert.IsTrue(notifyService.SendedNotifications[0].Name == Constants.WorkItemStateChangedEventName);
                Assert.That(((WorkItemNotificationViewModel)notifyService.SendedNotifications[0].SendedObject).Data, Has.Property("Old"));
                Assert.That(((WorkItemNotificationViewModel)notifyService.SendedNotifications[0].SendedObject).Data, Has.Property("New"));
            }
        }
        

        [Test]
        [TestCase(10053, WorkItemState.Reviewing, 3, 1, 2)]
        [TestCase(10053, WorkItemState.Done, 0, 1, 2, 3)]
        [TestCase(10094, WorkItemState.AtWork, 6, 1, 2)]
        [TestCase(10081, WorkItemState.AtWork, 4, 1, 2)]
        [TestCase(10081, WorkItemState.Done, 0, 1, 2, 4)]
        [TestCase(10105, WorkItemState.Done, 3, 0, new int[0])]
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
            var usersIds = usersNumbers.Select(x => userRepository.GetById(new TGuid(x).ToGuid().ToString()).UserName).OrderBy(x => x);
          
            service.Update(workItem);

            Assert.AreEqual(notificationCount, notifyService.SendedNotifications.Count);
            if (usersNumbers.Length > 0)
            {
                Assert.IsTrue(notifyService.SendedNotifications[0].Users.OrderBy(x => x).SequenceEqual(usersIds));
                Assert.IsTrue(notifyService.SendedNotifications[0].Name == Constants.WorkItemChangedEventName);
            }
        }

        [Test]
        [TestCase(10053, 6, 6, 2, Constants.DisappointEventName, Constants.WorkItemChangedEventName)]//новый исполнитель меняет исполнителя
        [TestCase(10053, 0, 5, 3, Constants.DisappointEventName, Constants.AppointEventName, Constants.WorkItemChangedEventName)]//Директор меняет исполнителя
        [TestCase(10094, 2, 5, 2, Constants.DisappointEventName, Constants.AppointEventName)]//РН меняет исполнителя
        [TestCase(10094, 6, 5, 2, Constants.WorkItemChangedEventName, Constants.AppointEventName)]//текущий исполнитель меняет исполнителя
        [TestCase(10105, 3, 6, 1, Constants.AppointEventName)]//текущий исполнитель - РН, меняет на другого
        [TestCase(10105, 3, -1, 0, new string[0])]//текущий исполнитель - РН, меняет на пусто
        [TestCase(10105, 2, -1, 1, Constants.DisappointEventName)]//текущий исполнитель - РН, другой пользователь меняет на пусто
        public void Update_WhenExecutorChanged_SendExpectedNotifications(int workItemNumber, int currentUserId, int newExecutorId, int notificationCount, params string[] events)
        {
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
        [TestCase(3, 6, 3, 1, Constants.AppointEventName)]//РН добавляет задачу с исполнителем
        [TestCase(3, 3, 3, 0, new string[0])] //РН добавляет себе задачу
        [TestCase(3, -1, 3, 0, new string[0])] //РН добавляет задачу без исполнителя
        [TestCase(6, -1, 3, 1, Constants.WorkItemAddedEventName)] //исполнитель добавляет задачу без исполнителя
        [TestCase(6, 6, 3, 1, Constants.WorkItemAddedEventName)] //исполнитель добавляет себе задачу
        [TestCase(0, 6, 3, 2, Constants.WorkItemAddedEventName, Constants.AppointEventName)] //директор добавляет задачу с исполнителем
        [TestCase(0, 3, 3, 1, Constants.AppointEventName)] //директор добавляет задачу с исполнителем РН
        public void Add_Always_NotifyExpectedUsers(int currentUserNumber, int executorNumber, int parentItemExecutor, int notificationCount, params string[] events)
        {
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

        /*  var items =
                workItemRepository.WorkItems.Where(
                    x =>
                        x.Type == WorkItemType.Task && x.ParentId.HasValue &&
                        workItemRepository.WorkItems.Any(wi => wi.Id == x.ParentId.Value && wi.ExecutorId == x.ExecutorId)).OrderBy(x => x.Id);*/
    }
}
