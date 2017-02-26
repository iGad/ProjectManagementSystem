using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common.Models;
using PMS.Model.Models;

namespace PMS.Model.Services
{
    public static class WorkItemExtensions
    {
        public static TreeNode ToTreeNode(this WorkItem workItem)
        {
            return new TreeNode
            {
                Id = workItem.Id,
                Name = workItem.Name,
                Type = workItem.Type.ToString()
            };
        }

        public static string[] GetDifferentProperties(this WorkItem item, WorkItem other)
        {
            var properties = new List<string>();
            if(item.ExecutorId != other.ExecutorId)
                properties.Add(nameof(WorkItem.ExecutorId));
            if (item.Description != other.Description)
                properties.Add(nameof(WorkItem.Description));
            if (item.Name != other.Name)
                properties.Add(nameof(WorkItem.Name));
            if (item.ParentId != other.ParentId)
                properties.Add(nameof(WorkItem.ParentId));
            if (item.State != other.State)
                properties.Add(nameof(WorkItem.State));
            if (item.DeadLine != other.DeadLine)
                properties.Add(nameof(WorkItem.DeadLine));
            return properties.ToArray();
        }
    }
}