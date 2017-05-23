using System.Collections.Generic;
using System.Linq;
using Common.Services;
using PMS.Model.Models;
using PMS.Model.Repositories;

namespace PMS.Model.Services
{
    public class UsersService
    {
        private readonly IUserRepository userRepository;
        private readonly ICurrentUsernameProvider currentUsernameProvider;

        public UsersService(IUserRepository userRepository, ICurrentUsernameProvider currentUsernameProvider)
        {
            this.userRepository = userRepository;
            this.currentUsernameProvider = currentUsernameProvider;
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
            return this.userRepository.GetUsersByRoles(roles).Where(x => !x.IsDeleted).ToList();
        }

        public ApplicationUser Get(string id)
        {
            var user = this.userRepository.GetById(id);
            if (user == null)
                throw new PmsException("User with Id " + id + " not found");
            return user;
        }

        public ApplicationUser GetByUsername(string username)
        {
            var user = this.userRepository.GetByUserName(username);
            if(user == null)
                throw new PmsException($"Пользователь {username} не найден");
            return user;
        }

        public ApplicationUser GetCurrentUser()
        {
            var username = this.currentUsernameProvider.GetCurrentUsername();
            if (string.IsNullOrWhiteSpace(username))
                return null;
            return this.userRepository.GetByUserName(username);
        }

        public List<Role> GetRolesByIds(IEnumerable<string> roleIds)
        {
            return this.userRepository.GetRoles().Where(x => roleIds.Contains(x.Id)).ToList();
        } 
    }
}
