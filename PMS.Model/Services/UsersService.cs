using System.Collections.Generic;
using System.Linq;
using PMS.Model.Models;
using PMS.Model.Repositories;

namespace PMS.Model.Services
{
    public class UsersService
    {
        private readonly IUserRepository userRepository;
        public UsersService(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
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
                throw new PmsExeption("User with Id " + id + " not found");
            return user;
        }
    }
}
