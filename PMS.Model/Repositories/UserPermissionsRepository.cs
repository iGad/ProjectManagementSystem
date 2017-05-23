using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS.Model.Models;

namespace PMS.Model.Repositories
{
    public class UserPermissionsRepository : IUserPermissionsRepository
    {
        private readonly Dictionary<PermissionType, RoleType[]> permissions = new Dictionary<PermissionType, RoleType[]>
        {
            { PermissionType.CanMoveToPlanned, new []{RoleType.Director, RoleType.Executor, RoleType.MainProjectEngeneer, RoleType.Manager }},
            { PermissionType.CanMoveToAtWork, new []{RoleType.Director, RoleType.Executor, RoleType.MainProjectEngeneer, RoleType.Manager }},
            { PermissionType.CanMoveToReviewing, new []{RoleType.Director, RoleType.Executor, RoleType.MainProjectEngeneer, RoleType.Manager }},
            { PermissionType.CanMoveToDone, new []{RoleType.Director, RoleType.MainProjectEngeneer, RoleType.Manager }},
            { PermissionType.CanMoveToArchive, new []{RoleType.Director, RoleType.MainProjectEngeneer }},
            { PermissionType.CanDeleteWorkItem, new []{RoleType.Director, RoleType.MainProjectEngeneer }},
            { PermissionType.CanCreatePartition, new []{RoleType.Director, RoleType.MainProjectEngeneer, RoleType.Manager }},
            { PermissionType.CanCreateStage, new []{RoleType.Director, RoleType.MainProjectEngeneer }},
            { PermissionType.CanCreateProject, new []{RoleType.Director, RoleType.MainProjectEngeneer }},
            { PermissionType.CanChangeForeignWorkItem, new []{RoleType.Director, RoleType.MainProjectEngeneer, RoleType.Manager }},
            { PermissionType.HaveAccessToUsers, new []{RoleType.Director, RoleType.Admin }},
            { PermissionType.HaveAccessToSettings, new []{RoleType.Director, RoleType.Admin }},
        }; 
        public int SaveChanges()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<PermissionType> GetPermissionsForRoles(ICollection<RoleType> roleCodes)
        {
            return this.permissions.Where(pair => pair.Value.Any(roleCodes.Contains)).Select(x => x.Key);
        }
    }
}
