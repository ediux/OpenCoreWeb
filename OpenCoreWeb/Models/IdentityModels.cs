using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using My.Core.Infrastructures.Implementations.Models;
using System;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using My.Core.Infrastructures.DAL;
using My.Core.Infrastructures.Implementations;
using My.Core.Infrastructures;

namespace OpenCoreWeb.Models
{
    // 您可以在 ApplicationUser 類別新增更多屬性，為使用者新增設定檔資料，請造訪 http://go.microsoft.com/fwlink/?LinkID=317594 以深入了解。
    //public class ApplicationUser : global::My.Core.Infrastructures.Implementations.Models.ApplicationUser, IUser<int>
    //{
    //    public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
    //    {
    //        // 注意 authenticationType 必須符合 CookieAuthenticationOptions.AuthenticationType 中定義的項目
    //        var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
    //        // 在這裡新增自訂使用者宣告
    //        return userIdentity;
    //    }
    //}

    //public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    //{
    //    public ApplicationDbContext()
    //        : base("DefaultConnection", throwIfV1Schema: false)
    //    {
    //    }

    //    public static ApplicationDbContext Create()
    //    {
    //        return new ApplicationDbContext();
    //    }
    //}

    public class OpenCoreWebUserStore : IUserStore<ApplicationUser, int>
        , IUserRoleStore<ApplicationUser, int>, IRoleStore<ApplicationRole,int>
        , IUserEmailStore<ApplicationUser, int>, IUserLockoutStore<ApplicationUser, int>
        , IUserLoginStore<ApplicationUser, int>, IUserPasswordStore<ApplicationUser, int>
        , IUserPhoneNumberStore<ApplicationUser, int>, IUserSecurityStampStore<ApplicationUser, int>,
        IUserTokenProvider<ApplicationUser, int>, IUserTwoFactorStore<ApplicationUser, int>
    {
       
        private bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        IUnitofWork uow;
        IAccountRepository<ApplicationUser> accountrepo;

        public OpenCoreWebUserStore(DbContext context)
        {
            uow = (OpenWebSiteEntities)context;
            accountrepo = uow.GetRepository<ApplicationUser>() as IAccountRepository<ApplicationUser>;
        }

        #region 使用者
        public Task CreateAsync(ApplicationUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApplicationUser> FindByIdAsync(int userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            throw new System.NotImplementedException();
        }
       
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                //
            }

            // Free any unmanaged objects here.
            //
            disposed = true;
        }

        #endregion

        #region User Role Store
        public Task AddToRoleAsync(ApplicationUser user, string roleName)
        {
            throw new System.NotImplementedException();
        }

        public Task<System.Collections.Generic.IList<string>> GetRolesAsync(ApplicationUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(ApplicationUser user, string roleName)
        {
            throw new System.NotImplementedException();
        }

        public Task RemoveFromRoleAsync(ApplicationUser user, string roleName)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region EMail Stroe
        public Task<ApplicationUser> FindByEmailAsync(string email)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetEmailAsync(ApplicationUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task SetEmailAsync(ApplicationUser user, string email)
        {
            throw new System.NotImplementedException();
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region Lockout Store
        public Task<int> GetAccessFailedCountAsync(ApplicationUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> GetLockoutEnabledAsync(ApplicationUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task<System.DateTimeOffset> GetLockoutEndDateAsync(ApplicationUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task<int> IncrementAccessFailedCountAsync(ApplicationUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task ResetAccessFailedCountAsync(ApplicationUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled)
        {
            throw new System.NotImplementedException();
        }

        public Task SetLockoutEndDateAsync(ApplicationUser user, System.DateTimeOffset lockoutEnd)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region Login Store
        public Task AddLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            throw new System.NotImplementedException();
        }

        public Task<ApplicationUser> FindAsync(UserLoginInfo login)
        {
            throw new System.NotImplementedException();
        }

        public Task<System.Collections.Generic.IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task RemoveLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region Password Store
        public Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region Phone Number Store
        public Task<string> GetPhoneNumberAsync(ApplicationUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(ApplicationUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task SetPhoneNumberAsync(ApplicationUser user, string phoneNumber)
        {
            throw new System.NotImplementedException();
        }

        public Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region Security Stamp Store
        public Task<string> GetSecurityStampAsync(ApplicationUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task SetSecurityStampAsync(ApplicationUser user, string stamp)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region Token Provider
        public Task<string> GenerateAsync(string purpose, UserManager<ApplicationUser, int> manager, ApplicationUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> IsValidProviderForUserAsync(UserManager<ApplicationUser, int> manager, ApplicationUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task NotifyAsync(string token, UserManager<ApplicationUser, int> manager, ApplicationUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> ValidateAsync(string purpose, string token, UserManager<ApplicationUser, int> manager, ApplicationUser user)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region TwoFactor
        public Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user)
        {
            throw new System.NotImplementedException();
        }

        public Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled)
        {
            throw new System.NotImplementedException();
        } 
        #endregion

        #region Role Store
        public Task CreateAsync(ApplicationRole role)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteAsync(ApplicationRole role)
        {
            throw new System.NotImplementedException();
        }

        Task<ApplicationRole> IRoleStore<ApplicationRole, int>.FindByIdAsync(int roleId)
        {
            throw new System.NotImplementedException();
        }

        Task<ApplicationRole> IRoleStore<ApplicationRole, int>.FindByNameAsync(string roleName)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateAsync(ApplicationRole role)
        {
            throw new System.NotImplementedException();
        } 
        #endregion
    }
}