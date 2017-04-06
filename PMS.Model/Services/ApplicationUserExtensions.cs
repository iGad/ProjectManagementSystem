using PMS.Model.Models;

namespace PMS.Model.Services
{
    public static class ApplicationUserExtensions
    {
        public static string GetUserIdentityText(this ApplicationUser user)
        {
            var text = user.Name;
            if (!string.IsNullOrWhiteSpace(user.Surname))
                text += " " + user.Surname;
            else
                text += $" ({user.UserName})";
            return text;
        }
    }
}
