using PMS.Model.Models;
using PMS.Model.Services;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectManagementSystem.Services
{
    public class DataGenerator
    {
        private const string Password = "123456";
        private const string Space = " ";
        private readonly IUsersService _usersService;
        private readonly UsersApiService _usersApiService;
        private readonly WorkItemApiService _workItemsService;
        private readonly Random _random = new Random();
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
                    Name = $"Иван{index}",
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
            var partitions = GeneratePartitions(stages, parameters.PartitionsPerStageFrom, parameters.PartitionsPerStageTo);
            GenerateTasks(partitions, parameters.TasksPerPartitionFrom, parameters.TasksPerPartitionTo);
        }

        private void GenerateTasks(WorkItem[] partitions, int tasksPerPartitionFrom, int tasksPerPartitionTo)
        {            
            foreach (var partition in partitions) {
               var taskCount = _random.Next(tasksPerPartitionFrom, tasksPerPartitionTo + 1);
                CreateItems(WorkItemType.Task, taskCount, partition);
            }
        }

        private IEnumerable<WorkItem> CreateItems(WorkItemType type, int count, WorkItem parent)
        {
            var items = new List<WorkItem>();
            for (int i = 1; i <= count; i++)
            {
                WorkItem item = CreateItem(type, i, parent);
                _workItemsService.Add(item);
                var state = GenerateState(item);
                _workItemsService.UpdateWorkItemState(item.Id, state);
                items.Add(item);
            }
            return items;
        }

        private WorkItemState GenerateState(WorkItem item)
        {
            if(item.ExecutorId == null)
                return WorkItemState.New;
            var r = _random.Next(0, 4);
            switch (r)
            {
                case 0: return WorkItemState.Planned;
                case 1:
                    return WorkItemState.AtWork;
                case 2:
                    return WorkItemState.Reviewing;
                case 3:
                    return WorkItemState.Done;
            }
            return WorkItemState.Archive;
        }

        private WorkItem[] GeneratePartitions(WorkItem[] stages, int partitionsPerStateFrom, int partitionsPerStateTo)
        {
            var partitions = new List<WorkItem>();
            foreach (var stage in stages)
            {
                var partitionCount = _random.Next(partitionsPerStateFrom, partitionsPerStateTo + 1);
                partitions.AddRange(CreateItems(WorkItemType.Partition, partitionCount, stage));
               
            }
            return partitions.ToArray();
        }

        private WorkItem[] GenerateStages(WorkItem[] projects, int stagesPerProjectCount)
        {
            var stages = new List<WorkItem>(projects.Length * stagesPerProjectCount);
            foreach (var project in projects)
            {
                stages.AddRange(CreateItems(WorkItemType.Stage, stagesPerProjectCount, project));
            }
            
            return stages.ToArray();
        }

        private WorkItem[] GenerateProjects(int projectCount)
        {
            return CreateItems(WorkItemType.Project, projectCount, null).ToArray();
        }

        private WorkItem CreateItem(WorkItemType type, int i, WorkItem parent = null)
        {
            var item = new WorkItem
            {
                Type = type,
                Name = GetItemName(type, i, parent),
                Description = GenerateDescription(),
                ExecutorId = GetExecutor(type),
                DeadLine = GetDeadline()
            };
            if (parent != null)
                item.ParentId = parent.Id;
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
            bool mayBeEmpty = false;
            switch (type)
            {
                case WorkItemType.Project:
                case WorkItemType.Stage:
                    roleIds = _roles.Where(x => x.RoleCode == RoleType.Director || x.RoleCode == RoleType.MainProjectEngeneer).Select(x => x.Id);
                    users = users.Where(x => x.Roles.Any(r => roleIds.Contains(r.Id))).ToList();
                    break;
                case WorkItemType.Partition:
                case WorkItemType.Task:
                    mayBeEmpty = true;
                    roleIds = _roles.Where(x => x.RoleCode == RoleType.Manager || x.RoleCode == RoleType.Executor).Select(x => x.Id);
                    users = users.Where(x => x.Roles.Any(r => roleIds.Contains(r.Id))).ToList();
                    break;
            }
            var isEmpty = mayBeEmpty && _random.Next(0, 4) == 0;
            return isEmpty ? null : users[_random.Next(0, users.Count)].Id;
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

        private string GetItemName(WorkItemType type, int i, WorkItem parent)
        {
            var name = (parent?.Name ?? "") + " " + LexicalHelper.GetWorkItemTypeInCase(type, "n") + i;
           
            return name;
        
        }

        private PMS.Model.Models.ApplicationUser CurrentUser => _usersService.GetCurrentUser();
    }
}