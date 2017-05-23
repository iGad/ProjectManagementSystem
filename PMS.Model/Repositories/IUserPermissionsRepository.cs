using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS.Model.Models;

namespace PMS.Model.Repositories
{
    public interface IUserPermissionsRepository : IRepository
    {
        IEnumerable<PermissionType> GetPermissionsForRoles(ICollection<RoleType> roleCodes);
         
    }
}
