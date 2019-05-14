using Leagues.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Leagues.Models
{
    public class CustomUserStore : IUserStore<CustomUser>
    {
       readonly private Entities database;

        public CustomUserStore()
        {
            this.database = new Entities();
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
            var User = database.Users.Find(user.Id);
            if (User != null)
            {
                User.Password = user.Password;
                database.SaveChanges();
                return null;
            }

            return null;
        }

        public Task DeleteAsync(CustomUser user)
        {
            // TODO
            throw new NotImplementedException();
        }

        public async Task<CustomUser> FindByIdAsync(string userId)
        {
            var User = await database.Users.Where(c => c.Userid == userId).FirstOrDefaultAsync();
            CustomUser user = new CustomUser()
            {
                UserName = User.Userid,
                Password = User.Password,
                Id = User.Userid
            };
            return user;
        }

        public async Task<CustomUser> FindByNameAsync(string userName)
        {
            var User = await this.database.Users.Where(c => c.Userid == userName).FirstOrDefaultAsync();
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

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(CustomUser manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await this.CreateIdentityAsync(manager, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public override async Task<IdentityResult> ResetPasswordAsync(string Id, string Code, string Password)
        {
            var customeUser = new CustomUser()
            {
                Id = Id,
                Password = Password,
                UserName = Id
            };

            await ((CustomUserStore) this.Store).UpdateAsync(customeUser);
            return IdentityResult.Success;
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


            var customuser = await ((CustomUserManager) this.UserManager).FindByIdAsync(Email);
                
            if (customuser == null)
            {
                return SignInStatus.Failure;
            }
                
            if (customuser.Password != Password)
            {
                return SignInStatus.Failure;
            }

            return SignInStatus.Success;
        }

        

    }
}