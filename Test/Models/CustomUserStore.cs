using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Test.Models;

namespace Test.Models
{
    public class CustomUserStore : IUserStore<CustomUser>
    {
        readonly DefaultEntities database;

        public CustomUserStore()
        {
            this.database = new DefaultEntities();
        }

       
        public void Dispose()
        {
            this.database.Dispose();
        }

        public Task CreateAsync(CustomUser user)
        {
            // TODO
            throw new NotImplementedException();
        }

        public Task UpdateAsync(CustomUser user)
        {
            // TODO
            throw new NotImplementedException();
        }

        public Task DeleteAsync(CustomUser user)
        {
            // TODO
            throw new NotImplementedException();
        }

        public async Task<CustomUser> FindByIdAsync(string userId)
        {
            var User = await this.database.Users.Where(c => c.Userid == userId).FirstOrDefaultAsync();
            CustomUser user = new CustomUser()
            {
                UserName = User.Userid,
                Password = User.Password,
                Id= User.Userid
            };
            return user;
        }

        public async Task<CustomUser> FindByNameAsync(string userName)
        {
            var User = await this.database.Users.FindAsync(new object[] {userName});
            CustomUser user = new CustomUser()
            {
                UserName = User.Userid,
                Password = User.Password,
                Id = User.Userid
            };
            return user;
        }
    }

    public class CustomUser : IUser<string>
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Id { get; set; }
    }

    public class CustomUserManager : UserManager<CustomUser>
    {
        public CustomUserManager(IUserStore<CustomUser> store)
            : base(store)
        {
        }

        public static CustomUserManager Create(IdentityFactoryOptions<CustomUserManager> options, IOwinContext context)
        {

            var manager = new CustomUserManager(new CustomUserStore());
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<CustomUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = false
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            //manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            //{
            //    MessageFormat = "Your security code is {0}"
            //});
            //manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            //{
            //    Subject = "Security Code",
            //    BodyFormat = "Your security code is {0}"
            //});
            //manager.EmailService = new EmailService();
            //manager.SmsService = new SmsService();
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider =
                    new DataProtectorTokenProvider<CustomUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }

    public class CustomSignInManager : SignInManager<CustomUser, string>
    {
        public CustomSignInManager(CustomUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public static CustomSignInManager Create(IdentityFactoryOptions<CustomSignInManager> options, IOwinContext context)
        {
            return new CustomSignInManager(context.GetUserManager<CustomUserManager>(), context.Authentication);
        }

        public override async Task<SignInStatus> PasswordSignInAsync(string Email, string Password, bool RememberMe, bool shouldLockout)
        {
            using (var database = new DefaultEntities())
            {
                var user = await database.Users.FindAsync(new object[] {Email});

                if (user == null)
                {
                    return SignInStatus.Failure;
                }
                
                if (user.Password != Password)
                {
                    return SignInStatus.Failure;
                }
            }
            return SignInStatus.Success;
        }

        

    }
}