﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace PMS.Model.Models.Identity
{

    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<Context>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = false,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = false,
            };
            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;
            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug in here.
            //manager.RegisterTwoFactorProvider("PhoneCode", new PhoneNumberTokenProvider<ApplicationUser>
            //{
            //    MessageFormat = "Your security code is: {0}"
            //});
            //manager.RegisterTwoFactorProvider("EmailCode", new EmailTokenProvider<ApplicationUser>
            //{
            //    Subject = "SecurityCode",
            //    BodyFormat = "Your security code is {0}"
            //});
            //manager.EmailService = new EmailService();
            //manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    // Configure the RoleManager used in the application. RoleManager is defined in the ASP.NET Identity core assembly
    public class ApplicationRoleManager : RoleManager<IdentityRole>
    {
        public ApplicationRoleManager(IRoleStore<IdentityRole, string> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(new RoleStore<IdentityRole>(context.Get<Context>()));
        }
    }

    //public class EmailService : IIdentityMessageService
    //{
    //    public Task SendAsync(IdentityMessage message)
    //    {
    //        // Plug in your email service here to send an email.
    //        return Task.FromResult(0);
    //    }
    //}

    //public class SmsService : IIdentityMessageService
    //{
    //    public Task SendAsync(IdentityMessage message)
    //    {
    //        // Plug in your sms service here to send a text message.
    //        return Task.FromResult(0);
    //    }
    //}

    // This is useful if you do not want to tear down the database each time you run the application.
    // public class ApplicationDbInitializer : DropCreateDatabaseAlways<ApplicationDbContext>
    // This example shows you how to create a new database if the Model changes
    public class ApplicationDbInitializer : CreateDatabaseIfNotExists<Context>
    {
        protected override void Seed(Context context)
        {
            AddRoles(context);
            InitializeIdentityForEF(context);
            base.Seed(context);
        }

        private void AddRoles(Context context)
        {
            
        }

        //Create User=Admin@Admin.com with password=Admin@123456 in the Admin role        
        public static void InitializeIdentityForEF(Context context)
        {
            using (var transaction = context.Database.BeginTransaction())
            {
                List<string> rolesName = new List<string>() { "Руководитель направления", "Главный инженер проекта", "Исполнитель", "Администратор", "Директор" };
                for (int i = 0; i < rolesName.Count; i++)
                {
                    if (!context.Roles.Any(k => k.Name == rolesName[i]))
                        context.Roles.Add(new IdentityRole(rolesName[i]));

                }
                context.SaveChanges();
                var owinContext = new OwinContext();
                owinContext.Set(context);
                var userManager = ApplicationUserManager.Create(null, owinContext);
                var roleManager = ApplicationRoleManager.Create(null, owinContext);
                const string name = "admin@admin.ru";
                const string password = "admin@123456";
                var admin = userManager.FindByName("admin@admin.com");
                if (admin == null)
                {
                    admin = new ApplicationUser {UserName = name, Email = name, EmailConfirmed = true};
                }
                //var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
                //var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
                //const string name = "admin@admin.ru";
                //const string password = "15935712gfdtk";
                //const string roleName = "Директор";

                ////Create Role Admin if it does not exist
                //var role = roleManager.FindByName(roleName);
                //if (role == null)
                //{
                //    role = new IdentityRole(roleName);
                //    var roleresult = roleManager.Create(role);
                //}
            }
           
           
            //const string roleName = "Директор";
            //List<string> rolesName = new List<string>() { "Руководитель направления", "Главный инженер проекта", "Исполнитель", "Администратор", "Директор" };
            //IdentityRole role = null;
            //for (int i = 0; i < rolesName.Count; i++)
            //{
            //    role = roleManager.FindByName(rolesName[i]);
            //    if (role == null)
            //    {
            //        role = new IdentityRole(rolesName[i]);
            //        var roleresult = roleManager.Create(role);
            //    }
            //}
            //Create Role Admin if it does not exist
            //var role = roleManager.FindByName(roleName);
            //if (role == null) {
            //    role = new IdentityRole(roleName);
            //    var roleresult = roleManager.Create(role);
            //}



            //var user = userManager.FindByName(name);
            //if (user == null) {
            //    user = new ApplicationUser { UserName = name, Email = name, Name = "Админ", Surname = "Админ", Birthday = DateTime.Parse("1990-01-01")};
            //    var result = userManager.Create(user, password);
            //    result = userManager.SetLockoutEnabled(user.Id, false);
            //}

            //// Add user admin to Role Admin if not already added
            //var rolesForUser = userManager.GetRoles(user.Id);
            //if (!rolesForUser.Contains(role.Name)) {
            //    var result = userManager.AddToRole(user.Id, role.Name);
            //}
        }
    }

    public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager) :
            base(userManager, authenticationManager)
        { }

        public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
        {
            return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }
    }
}
