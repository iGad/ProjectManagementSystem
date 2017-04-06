using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace PMS.Model.Models
{
    public class ApplicationUser : IdentityUser<string, IdentityUserLogin, UserRole, IdentityUserClaim>
    {
        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString();
        }
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Fathername { get; set; }
        public DateTime? Birthday { get; set; }
        public bool IsDeleted { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, string> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public override string ToString()
        {
            return Name + (string.IsNullOrWhiteSpace(Surname) ? $"({Email})" : $" {Surname}");
        }
    }
}
