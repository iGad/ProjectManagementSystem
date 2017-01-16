using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS.Model.Models;
using PMS.Model.Repositories;

namespace PMS.Model.Services
{
    public class WorkItemService
    {
        private readonly WorkItemRepository repository;
        public WorkItemService(WorkItemRepository repository)
        {
            this.repository = repository;
        }

        public WorkItem Add(WorkItem workItem)
        {
            return this.repository.Add(workItem);
        }
        
    }
}
