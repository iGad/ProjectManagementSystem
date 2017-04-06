using System;
using PMS.Model.Models;

namespace PMS.Model.Services
{
    public static class LexicalHelper
    {
        public static string GetWorkItemTypeInCase(WorkItemType type, string @case)
        {
            var lowerCase = @case.ToLower();
            switch (type)
            {
                case WorkItemType.Project:
                    if (lowerCase == "n" || lowerCase == "a")
                        return NotificationResources.ProjectN.ToLower();
                    return NotificationResources.ProjectG.ToLower();
                case WorkItemType.Stage:
                    if (lowerCase == "n")
                        return NotificationResources.StageN.ToLower();
                    if (lowerCase == "a")
                        return NotificationResources.StageA.ToLower();
                    return NotificationResources.StageG.ToLower();
                case WorkItemType.Partition:
                    if (lowerCase == "n" || lowerCase == "a")
                        return NotificationResources.PartitionN.ToLower();
                    return NotificationResources.PartitionG.ToLower();
                case WorkItemType.Task:
                    if (lowerCase == "n")
                        return NotificationResources.TaskN.ToLower();
                    if (lowerCase == "a")
                        return NotificationResources.TaskA.ToLower();
                    return NotificationResources.TaskG.ToLower();
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}
