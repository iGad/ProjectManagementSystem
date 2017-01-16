using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS.Model.Models;
using PMS.Model.Repositories;

namespace PMS.Model.Services
{
    public class TreeService
    {
        private readonly WorkItemRepository repository;

        public TreeService(WorkItemRepository repository)
        {
            this.repository = repository;
        }

        public ICollection<WorkItem> GetTree()
        {
            var rootItems = this.repository.Get(x => !x.ParentId.HasValue).ToArray();
            var nonRootItems = this.repository.Get(x => x.ParentId.HasValue).ToArray();
            foreach (var rootItem in rootItems)
            {
                
            }
            return rootItems;
        } 


    }
}
