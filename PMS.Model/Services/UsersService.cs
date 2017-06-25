using System.Collections.Generic;
using System.Linq;
using Common.Services;
using PMS.Model.Models;
using PMS.Model.Repositories;

namespace PMS.Model.Services
{
    public class UsersService : IUsersService
    {
        private ApplicationUser _currentUser;
        private readonly IUserRepository _userRepository;
        private readonly ICurrentUsernameProvider _currentUsernameProvider;

        public UsersService(IUserRepository userRepository, ICurrentUsernameProvider currentUsernameProvider)
        {
            this._userRepository = userRepository;
            this._currentUsernameProvider = currentUsernameProvider;
        }

        public List<ApplicationUser> GetAllowedUsersForWorkItemType(int typeId)
        {
            var type = (WorkItemType)typeId;
            var roles = new List<string> {Resources.Director, Resources.MainProjectEngineer};
            if (type == WorkItemType.Task)
            {
                roles.Add(Resources.Executor);
                roles.Add(Resources.Manager);
            }
            if (type == WorkItemType.Partition)
            {
                roles.Add(Resources.Manager);
            }
            return this._userRepository.GetUsersByRoles(roles).Where(x => !x.IsDeleted).ToList();
        }

        public ApplicationUser Get(string id)
        {
            var user = SafeGet(id);
            if (user == null)
                throw new PmsException("User with Id " + id + " not found");
            return user;
        }

        public ApplicationUser SafeGet(string id)
        {
            return _userRepository.GetById(id);
        }

        public ApplicationUser GetByUsername(string username)
        {
            var user = this._userRepository.GetByUserName(username);
            if(user == null)
                throw new PmsException($"Пользователь {username} не найден");
            return user;
        }

        public ApplicationUser GetCurrentUser()
        {
            if (this._currentUser != null)
                return this._currentUser;
            var username = this._currentUsernameProvider.GetCurrentUsername();
            if (string.IsNullOrWhiteSpace(username))
                return null;
            return (this._currentUser = this._userRepository.GetByUserName(username));
        }

        public List<Role> GetRolesByIds(IEnumerable<string> roleIds)
        {
            return this._userRepository.GetRoles().Where(x => roleIds.Contains(x.Id)).ToList();
        }

        public List<ApplicationUser> GetUsersByRole(RoleType role)
        {
            return _userRepository.GetUsersByRole(role).ToList();
        }
    }
}
