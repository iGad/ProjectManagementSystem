using System.Collections.Generic;
using System.Linq;
using PMS.Model.Models;
using PMS.Model.Repositories;

namespace PMS.Model.Services
{
    public class WorkItemService
    {
        private readonly IWorkItemRepository repository;
        public WorkItemService(IWorkItemRepository repository)
        {
            this.repository = repository;
        }

        public WorkItem Get(int id)
        {
            var workItem = this.repository.GetById(id);
            if(workItem == null)
                throw new PmsExeption(string.Format(Resources.WorkItemNotFound, id));
            return workItem;
        }

        public WorkItem Add(WorkItem workItem)
        {
            var item = this.repository.Add(workItem);
            this.repository.SaveChanges();
            return item;
        }

        public List<WorkItem> GetActualProjects()
        {
            return
                this.repository.Get(
                    x => !x.ParentId.HasValue && x.State != WorkItemState.Archive && x.State != WorkItemState.Deleted && x.State != WorkItemState.Done).ToList();
        }

        public List<WorkItem> GetChildWorkItems(int parentId)
        {
            return
                this.repository.Get(
                    x => x.ParentId == parentId && x.State != WorkItemState.Archive && x.State != WorkItemState.Deleted && x.State != WorkItemState.Done).ToList();
        }

        public void Update(WorkItem workItem)
        {
            var oldWorkItem = Get(workItem.Id);
            oldWorkItem.State = workItem.State;
            oldWorkItem.Status = workItem.Status;
            oldWorkItem.Name = workItem.Name;
            oldWorkItem.Description = workItem.Description;
            oldWorkItem.DeadLine = workItem.DeadLine;
            oldWorkItem.ExecutorId = workItem.ExecutorId;
            this.repository.SaveChanges();
        }
    }
}
