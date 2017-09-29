using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            _userRepository = userRepository;
            _currentUsernameProvider = currentUsernameProvider;
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
            return _userRepository.GetUsersByRoles(roles).Where(x => !x.IsDeleted).ToList();
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
            var user = _userRepository.GetByUserName(username);
            if(user == null)
                throw new PmsException($"Пользователь {username} не найден");
            return user;
        }

        public ApplicationUser GetCurrentUser()
        {
            if (_currentUser != null)
                return _currentUser;
            var username = _currentUsernameProvider.GetCurrentUsername();
            if (string.IsNullOrWhiteSpace(username))
                return null;
            return (_currentUser = _userRepository.GetByUserName(username));
        }

        public async Task<ApplicationUser> GetCurrentUserAsync()
        {
            if (_currentUser != null)
                return _currentUser;
            var username = _currentUsernameProvider.GetCurrentUsername();
            if (string.IsNullOrWhiteSpace(username))
                return null;
            return (_currentUser = await _userRepository.GetByUserNameAsync(username));
        }

        public List<Role> GetRolesByIds(IEnumerable<string> roleIds)
        {
            return _userRepository.GetRoles().Where(x => roleIds.Contains(x.Id)).ToList();
        }

        public List<ApplicationUser> GetUsersByRole(RoleType role)
        {
            return _userRepository.GetUsersByRole(role).ToList();
        }
    }
}
