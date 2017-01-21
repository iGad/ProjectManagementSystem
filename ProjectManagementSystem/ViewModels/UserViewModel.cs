using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMS.Model.Models;

namespace ProjectManagementSystem.ViewModels
{
    public class UserViewModel 
    {
        public UserViewModel()
        {
            Roles = new List<RoleViewModel>();
        }

        public UserViewModel(ApplicationUser user) : this()
        {
            Id = user.Id;
            Name = user.Name;
            Surname = user.Surname;
            Fathername = user.Fathername;
            Birthday = user.Birthday;
            Email = user.UserName;
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Fathername { get; set; }
        public DateTime? Birthday { get; set; }
        public string Email { get; set; }
        public List<RoleViewModel> Roles { get; set; } 
    }

    public static class UserViewModelExtensions
    {
        public static ApplicationUser ToApplicationUser(this UserViewModel userViewModel)
        {
            var user = new ApplicationUser
            {
                UserName = userViewModel.Email,
                Email = userViewModel.Email,
                EmailConfirmed = true,
                Name = userViewModel.Name,
                Surname = userViewModel.Surname,
                Fathername = userViewModel.Fathername,
                Birthday = userViewModel.Birthday
            };
            return user;
        }
    }
}