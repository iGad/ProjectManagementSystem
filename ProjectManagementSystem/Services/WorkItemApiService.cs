using System;
using System.Collections.Generic;
using System.Linq;
using PMS.Model;
using PMS.Model.Models;
using PMS.Model.Services;
using ProjectManagementSystem.ViewModels;

namespace ProjectManagementSystem.Services
{
    public class WorkItemApiService
    {
        private readonly WorkItemService workItemService;
        public WorkItemApiService(WorkItemService workItemService)
        {
            this.workItemService = workItemService;
        }
        public Dictionary<string, List<WorkItemTileViewModel>> GetActualWorkItems()
        {
            return this.workItemService.GetActualWorkItems()
                .ToDictionary(pair => pair.Key.ToString(), pair => pair.Value.Select(x => new WorkItemTileViewModel(x)).ToList());
        }

        public List<EnumViewModel<WorkItemState>> GetStatesViewModels(bool isMainEngeneer, bool isManager)
        {
            var states = new List<WorkItemState> {WorkItemState.Planned, WorkItemState.AtWork, WorkItemState.Reviewing};
            if (isManager)
                states.Add(WorkItemState.Done);
            else
            {
                if (isMainEngeneer)
                {
                    states.Insert(0, WorkItemState.New);
                    states.Add(WorkItemState.Done);
                    states.Add(WorkItemState.Archive);
                }
            }
            return states.Select(x => new EnumViewModel<WorkItemState>
            {
                Value = x,
                Description = x.GetDescription()
            }).ToList();
        } 
    }
}