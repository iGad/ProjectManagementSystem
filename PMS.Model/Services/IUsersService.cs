using System.Collections.Generic;
using PMS.Model.Models;

namespace PMS.Model.Services
{
    public interface ICurrentUserProvider
    {
        ApplicationUser GetCurrentUser();
    }
    public interface IUsersService : ICurrentUserProvider
    {
        ApplicationUser Get(string id);
        List<ApplicationUser> GetAllowedUsersForWorkItemType(int typeId);
        ApplicationUser GetByUsername(string username);
        List<Role> GetRolesByIds(IEnumerable<string> roleIds);
        List<ApplicationUser> GetUsersByRole(RoleType role);
        ApplicationUser SafeGet(string id);
    }
}