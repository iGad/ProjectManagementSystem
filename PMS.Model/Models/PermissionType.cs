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
        CanDeleteWorkItem,
        CanChangeForeignWorkItem,
        CanCreateProject,
        CanCreateStage,
        CanCreatePartition,
        HaveAccessToUsers,
        HaveAccessToSettings,
    }
}
