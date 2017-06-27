using PMS.Model.Models;
using PMS.Model.Services;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ProjectManagementSystem.Services
{
    public class DataGenerator
    {
        private const string Password = "123456";
        private const string Space = " ";
        private readonly IUsersService _usersService;
        private readonly UsersApiService _usersApiService;
        private readonly WorkItemApiService _workItemsService;
        private Random _random = new Random();
        private List<UserViewModel>_users;
        private RoleViewModel[] _roles;

        public DataGenerator(IUsersService usersService, UsersApiService usersApiService, WorkItemApiService workItemsService)
        {
            _usersService = usersService;
            _usersApiService = usersApiService;
            _workItemsService = workItemsService;
        }

        public void GenerateUsers()
        {
            var roles = _usersApiService.GetRoles();
            List<RoleType[]> userRoles = new List<RoleType[]>
            {
                new []{RoleType.Director, RoleType.MainProjectEngeneer},
                new []{RoleType.MainProjectEngeneer, RoleType.Manager},
                new []{RoleType.Executor, RoleType.Manager},
                new []{RoleType.Executor},
                new []{RoleType.Executor},
            };
            foreach(var userRole in userRoles)
            {
                var index = userRoles.IndexOf(userRole);
                var user = new UserViewModel
                {
                    Email = $"user{index}@pangea.ru",
                    Name = $"Имя{index}",
                    Surname = $"Иванов{index}",
                    Roles = roles.Where(x => userRole.Contains(x.RoleCode)).ToList()
                };
                _usersApiService.AddUser(user, Password);
            }
        }

        public void GenerateWorkItems(DataGeneratorParameters parameters)
        {
            _users = _usersApiService.GetActualUsers();
            _roles = _usersApiService.GetRoles();
            var projects = GenerateProjects(parameters.ProjectCount);
            var stages = GenerateStages(projects, parameters.StagesPerProjectCount);
            var partitions = GeneratePartitions(stages, parameters.PartitionsPerStateFrom, parameters.PartitionsPerStateTo);
            GenerateTasks(partitions, parameters.TasksPerPartitionFrom, parameters.TasksPerPartitionTo);
        }

        private void GenerateTasks(WorkItem[] partitions, int tasksPerPartitionFrom, int tasksPerPartitionTo)
        {            
            foreach (var partition in partitions) {
                var taskCount = _random.Next(tasksPerPartitionTo, tasksPerPartitionFrom + 1);
                for (int i = 1; i <= taskCount; i++)
                {
                    WorkItem task = CreateItem(WorkItemType.Partition, i);
                    task.ParentId = partition.Id;
                    _workItemsService.Add(task);
                }
            }
        }

        private WorkItem[] GeneratePartitions(WorkItem[] stages, int partitionsPerStateFrom, int partitionsPerStateTo)
        {
            var partitionCount = _random.Next(partitionsPerStateFrom, partitionsPerStateTo + 1);
            var partitions = new WorkItem[partitionCount];
            for (int i = 1; i <= partitionCount; i++)
            {
                WorkItem partition = CreateItem(WorkItemType.Partition, i);
                partitions[i - 1] = _workItemsService.Add(partition);
            }
            return partitions;
        }

        private WorkItem[] GenerateStages(WorkItem[] projects, int stagesPerProjectCount)
        {
            var stages = new WorkItem[stagesPerProjectCount];
            for (int i = 1; i <= stagesPerProjectCount; i++)
            {
                WorkItem stage = CreateItem(WorkItemType.Stage, i);
                stages[i - 1] = _workItemsService.Add(stage);
            }
            return stages;
        }

        private WorkItem[] GenerateProjects(int projectCount)
        {
            var projects = new WorkItem[projectCount];
            for(int i = 1; i <= projectCount; i++)
            {
                WorkItem project = CreateItem(WorkItemType.Project, i);
                projects[i - 1] = _workItemsService.Add(project);
            }
            return projects;
        }

        private WorkItem CreateItem(WorkItemType type, int i)
        {
            var item = new WorkItem
            {
                Type = type,
                Name = GetItemName(type, i),
                Description = GenerateDescription(),
                ExecutorId = GetExecutor(type),
                DeadLine = GetDeadline()
            };
            return item;
        }

        private DateTime GetDeadline()
        {
            return DateTime.Today.AddDays(_random.Next(2, 100)).AddHours(17);
        }

        private string GetExecutor(WorkItemType type)
        {
            var users = _users;
            IEnumerable<string> roleIds;
            switch (type)
            {
                case WorkItemType.Project:
                case WorkItemType.Stage:
                    roleIds = _roles.Where(x => x.RoleCode == RoleType.Director || x.RoleCode == RoleType.MainProjectEngeneer).Select(x => x.Id);
                    users = users.Where(x => x.Roles.Any(r => roleIds.Contains(r.Id))).ToList();
                    break;
                case WorkItemType.Partition:
                case WorkItemType.Task:
                    roleIds = _roles.Where(x => x.RoleCode == RoleType.Manager || x.RoleCode == RoleType.Executor).Select(x => x.Id);
                    users = users.Where(x => x.Roles.Any(r => roleIds.Contains(r.Id))).ToList();
                    break;
            }
            return users[_random.Next(0, users.Count)].Id;
        }

        private string GenerateDescription()
        {
            var wordCount = _random.Next(1, 150);
            var stringBuilder = new StringBuilder();
            for(int i = 0; i < wordCount; i++)
            {
                var wordLength = _random.Next(1, 15);
                for (int j = 0; j < wordLength;j++)
                    stringBuilder.Append((char)('а' + _random.Next(0, 32)));
                stringBuilder.Append(i < wordCount - 1 ? Space : ".");
            }
            return stringBuilder.ToString();
        }

        private string GetItemName(WorkItemType type, int i)
        {            
            var name = LexicalHelper.GetWorkItemTypeInCase(type, "a") + i;
            //switch (type)
            //{
            //    case WorkItemType.Stage: name = LexicalHelper.GetWorkItemTypeInCase(WorkItemType.Project) + 
            //}
            return name;
        
        }

        private PMS.Model.Models.ApplicationUser CurrentUser => _usersService.GetCurrentUser();
    }
}