using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace PMS.Model.Models.Identity
{
    public class ApplicationRoleManager : RoleManager<Role>
    {
        public ApplicationRoleManager(IRoleStore<Role, string> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(new RoleStore<Role, string, UserRole>(context.Get<ApplicationContext>()));
        }
    }
}