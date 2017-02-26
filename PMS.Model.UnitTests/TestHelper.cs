using System;
using System.Collections.Generic;
using System.Linq;
using PMS.Model.Models;
using PMS.Model.UnitTests.Fakes;

namespace PMS.Model.UnitTests
{
    public static class TestHelper
    {
        /// <summary>
        /// СОздание тестовых рабочих элементов. Для каждого ГИП по проекту. В каждом проекте - 2 стадии
        /// В каждой стадии по 4 раздела для каждого РН. В каждом разделе:
        /// Если раздел не в работе, то по заданию для каждого исполнителя в том же состоянии, что и раздел.
        /// Если раздел в работе, то по 10 заданий для каждого исполнителя в разных состояниях (на одно состояние одно задание, созданное РН
        /// и одно задание созданное самим исполнителем) Задания со статусом новые - без исполнителя.
        /// </summary>
        /// <param name="userRepo"></param>
        /// <returns></returns>
        public static TestWorkItemRepository CreateFilledWorkItemRepository(TestUserRepository userRepo)
        {
            var workItemRepository = new TestWorkItemRepository(userRepo);
            var mainEngeneers = userRepo.GetUsersByRole(Resources.MainProjectEngineer).ToList();
            
            for (int i = 0; i < mainEngeneers.Count; i++)
            {
                var project = CreateWorkItem(workItemRepository.WorkItems, i, WorkItemType.Project, mainEngeneers[i], mainEngeneers[i], WorkItemState.AtWork);
                workItemRepository.WorkItems.Add(project);
                AddStagesForProject(project, workItemRepository, userRepo, mainEngeneers, mainEngeneers[i]);
            }

            return workItemRepository;
        }

        private static void AddStagesForProject(WorkItem project, TestWorkItemRepository workItemRepo, TestUserRepository userRepo, List<ApplicationUser> mainEngeneers, ApplicationUser mainEngeneer)
        {
            var managers = userRepo.GetUsersByRole(Resources.Manager).ToList();
            for (int i = 0; i < 2; i++)
            {
                var stage = CreateWorkItem(workItemRepo.WorkItems, i, WorkItemType.Stage, mainEngeneer, mainEngeneer, WorkItemState.AtWork);
                stage.ParentId = project.Id;
                workItemRepo.WorkItems.Add(stage);
                AddPartitionsForStage(stage, workItemRepo, userRepo, managers, mainEngeneer);
            }
        }

        private static void AddPartitionsForStage(WorkItem stage, TestWorkItemRepository workItemRepo, TestUserRepository userRepo, List<ApplicationUser> managers, ApplicationUser mainEngeneer)
        {
            var executors = userRepo.GetUsersByRole(Resources.Executor).Except(userRepo.GetUsersByRole(Resources.MainProjectEngineer)).ToList();
            for (int i = 0; i < managers.Count; i++)
            {
                var states = new List<WorkItemState> {WorkItemState.AtWork, WorkItemState.Planned, WorkItemState.Done, WorkItemState.New};
                foreach (var state in states)
                {
                    var partition = CreateWorkItem(workItemRepo.WorkItems, states.IndexOf(state), WorkItemType.Partition, mainEngeneer, managers[i], state);
                    partition.ParentId = stage.Id;
                    workItemRepo.WorkItems.Add(partition);
                    AddTasksForPartition(partition, workItemRepo, managers[i], executors);
                }
            }
        }

        private static void AddTasksForPartition(WorkItem partition, TestWorkItemRepository workItemRepo, ApplicationUser manager, List<ApplicationUser> executors)
        {
            for (var i = 0; i < executors.Count; i++)
            {
                if (partition.State != WorkItemState.AtWork)
                {
                    var task = CreateWorkItem(workItemRepo.WorkItems, i, WorkItemType.Task, manager, executors[i], partition.State);
                    task.ParentId = partition.Id;
                    workItemRepo.WorkItems.Add(task);
                }
                else
                {
                    AddTasksForUser(partition.Id, i, workItemRepo, manager, executors[i]);
                    AddTasksForUser(partition.Id, i + executors.Count + 2, workItemRepo, executors[i], executors[i]);
                }
            }
        }

        private static void AddTasksForUser(int partitionId, int number, TestWorkItemRepository workItemRepo, ApplicationUser creator, ApplicationUser executor)
        {
            var states = new List<WorkItemState> { WorkItemState.AtWork, WorkItemState.New, WorkItemState.Planned, WorkItemState.Reviewing, WorkItemState.Done };
            foreach (var state in states)
            {
                var task = CreateWorkItem(workItemRepo.WorkItems, partitionId * 31 + number * 5 + (states.IndexOf(state) + 1), WorkItemType.Task, creator, executor, state);
                if (state == WorkItemState.New)
                    task.ExecutorId = null;
                task.ParentId = partitionId;
                workItemRepo.WorkItems.Add(task);
            }
        } 

        public static WorkItem CreateWorkItem(IEnumerable<WorkItem> workItems,  int number, WorkItemType type, ApplicationUser creator, ApplicationUser executor, WorkItemState state = WorkItemState.New)
        {
            return new WorkItem
            {
                Id = workItems.Count(x=>x.Type == type) + 1 + (int)Math.Pow(10, (int)type),
                Type = type,
                Name = type.ToString() + number,
                //Creator = creator,
                CreatorId = creator.Id,
                //Executor = executor,
                ExecutorId = executor?.Id,
                State = state
            };   
        }
    }
}
