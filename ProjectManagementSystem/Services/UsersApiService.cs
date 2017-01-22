using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PMS.Model.Models;
using PMS.Model.Repositories;
using PMS.Model.Services;
using ProjectManagementSystem.ViewModels;

namespace ProjectManagementSystem.Services
{
    public class UsersApiService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserRepository userRepository;

        public UsersApiService(IUserRepository repository, UserManager<ApplicationUser> manager)
        {
            this.userManager = manager;
            this.userRepository = repository;
        }

        public UserViewModel AddUser(UserViewModel userViewModel, string password)
        {
            var user = userViewModel.ToApplicationUser();
            this.userManager.Create(user, password);
            this.userManager.AddToRoles(user.Id, userViewModel.Roles.Select(x => x.Name).ToString());
            userViewModel.Id = user.Id;
            return userViewModel;
        }

        public List<UserViewModel> GetActualUsers()
        {
            return this.userRepository.GetUsers(x => !x.IsDeleted).ToArray().Select(CreateUserViewModel).ToList();
        }

        private UserViewModel CreateUserViewModel(ApplicationUser user)
        {
            var viewModel = new UserViewModel(user);
            var rolesIds = user.Roles.Select(x => x.RoleId).ToArray();
            var roles = GetRolesByIds(rolesIds);
            viewModel.Roles.AddRange(roles.Select(x => new RoleViewModel
            {
                Id = x.Id,
                Name = x.Name
            }));
            return viewModel;
        }

        public void UpdateUser(UserViewModel userViewModel)
        {
            var user = GetUser(userViewModel.Id);
            if (IsEmailInvalid(userViewModel.Email, userViewModel.Id))
                throw new PmsExeption($"Логин {userViewModel.Email} уже занят");
            user.Name = userViewModel.Name;
            user.UserName = userViewModel.Email;
            user.Email = userViewModel.Email;
            user.Name = userViewModel.Name;
            user.Surname = userViewModel.Surname;
            user.Fathername = userViewModel.Fathername;
            user.Birthday = userViewModel.Birthday;
            this.userRepository.SaveChanges();
            UpdateUserRoles(userViewModel.Roles, user);
        }

        private IdentityRole[] GetRolesByIds(IEnumerable<string> ids)
        {
            var allRoles = this.userRepository.GetRoles().ToArray();
            return allRoles.Where(x => ids.Contains(x.Id)).ToArray();
        }

        private void UpdateUserRoles(IEnumerable<RoleViewModel> newRoles, ApplicationUser user)
        {
            var oldRolesIds = user.Roles.Select(x => x.RoleId);
            var oldRoles = GetRolesByIds(oldRolesIds);
            this.userManager.RemoveFromRoles(user.Id, oldRoles.Select(x => x.Name).ToArray());
            this.userManager.AddToRoles(user.Id, newRoles.Select(x => x.Name).ToArray());
        }

        private ApplicationUser GetUser(string id)
        {
            var user = this.userRepository.GetById(id);
            if (user == null)
                throw new PmsExeption($"Пользователь с id {id} не найден");
            return user;
        }

        private bool IsEmailInvalid(string email, string id)
        {
            return string.IsNullOrWhiteSpace(email) ||
                   this.userRepository.GetUsers(x => !x.IsDeleted && x.Id != id && x.UserName.Equals(email, StringComparison.InvariantCultureIgnoreCase)).Any();

        }

        public void DeleteUser(string id)
        {
            var user = GetUser(id);
            user.IsDeleted = true;
            user.UserName += Guid.NewGuid();
            this.userRepository.SaveChanges();
        }

        public RoleViewModel[] GetRoles()
        {
            return this.userRepository.GetRoles().Select(x => new RoleViewModel
            {
                Name = x.Name,
                Id = x.Id
            }).ToArray();
        }
    }
}