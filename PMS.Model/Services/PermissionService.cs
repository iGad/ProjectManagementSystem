using PMS.Model.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PMS.Model.Models;

namespace PMS.Model.Services
{
    public class PermissionService
    {
        private readonly IUserPermissionsRepository _permissionsRepository;
        private readonly IUsersService _userService;
        private readonly IWorkItemRepository _workItemRepository;

        public PermissionService(IUserPermissionsRepository permissionsRepository, IUsersService userService, IWorkItemRepository workItemRepository)
        {
            _permissionsRepository = permissionsRepository;
            _userService = userService;
            _workItemRepository = workItemRepository;
        }

        public bool HasCurrentUserPermissions(params PermissionType[] permissionTypes)
        {
            if(permissionTypes == null || !permissionTypes.Any())
                throw new ArgumentException(nameof(permissionTypes));
            var user = _userService.GetCurrentUser();
            var roles = _userService.GetRolesByIds(user.Roles.Select(x => x.RoleId));
            var allowedPermissions = _permissionsRepository.GetPermissionsForRoles(roles.Select(x => x.RoleCode));
            return permissionTypes.All(pt => allowedPermissions.Contains(pt));
        }
    }
}
