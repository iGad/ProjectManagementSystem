using PMS.Model.Models;

namespace ProjectManagementSystem.ViewModels
{
    public class UserInfoViewModel
    {
        public UserInfoViewModel(ApplicationUser user) 
        {
            Id = user.Id;
            Name = user.Name;
            Surname = user.Surname;
            Email = user.UserName;
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
    }
}