using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMS.Model.Models
{
    public enum PermissionType
    {
        CanMoveToPlanned,
        CanMoveToAtWork,
        CanMoveToReviewing,
        CanMoveToDone,
        CanMoveToArchive,
        
        CanChangeForeignWorkItem,
        CanCreateProject,
        CanCreateStage,
        CanCreatePartition,
        CanCreateTask,
        HaveAccessToUsers,
        HaveAccessToSettings,
    }
}
