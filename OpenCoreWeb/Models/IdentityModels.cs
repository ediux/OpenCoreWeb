using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Linq;
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
        , IUserRoleStore<ApplicationUser, int>, IRoleStore<ApplicationRole, int>
        , IUserEmailStore<ApplicationUser, int>, IUserLockoutStore<ApplicationUser, int>
        , IUserLoginStore<ApplicationUser, int>, IUserPasswordStore<ApplicationUser, int>
        , IUserPhoneNumberStore<ApplicationUser, int>, IUserSecurityStampStore<ApplicationUser, int>
        , IUserTwoFactorStore<ApplicationUser, int>
        , IQueryableUserStore<ApplicationUser, int>, IQueryableRoleStore<ApplicationRole, int>
    {

        private bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        IUnitofWork uow;
        IApplicationUserRepository<ApplicationUser> accountrepo;
        IApplicationRoleRepository<ApplicationRole> rolerepo;


        public OpenCoreWebUserStore(DbContext context)
        {
            uow = (OpenWebSiteEntities)context;
            accountrepo = uow.GetRepository<ApplicationUserRepository, ApplicationUser>();
            rolerepo = uow.GetRepository<ApplicationRoleRepository, ApplicationRole>();
        }

        #region 使用者
        public Task CreateAsync(ApplicationUser user)
        {
            return new Task(() =>
            {
                accountrepo.Create(user);
                accountrepo.SaveChanges();
            });
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            return new Task(() =>
            {
                user.Void = true;
                accountrepo.Update(user);
                accountrepo.SaveChanges();
            });
        }

        public Task<ApplicationUser> FindByIdAsync(int userId)
        {
            return new Task<ApplicationUser>(() =>
            {
                return accountrepo.FindUserById(userId, false);
            });
        }

        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return new Task<ApplicationUser>(() =>
            {
                return accountrepo.FindUserByLoginAccount(userName, false);
            });
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            return new Task(() =>
            {
                accountrepo.Update(user);
                accountrepo.SaveChanges();
            });
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
            return new Task(() =>
            {
                int roleid = rolerepo.FindByName(roleName).Id;
                rolerepo.AddUserToRole(roleid, user.Id);
                rolerepo.SaveChanges();
            });
        }

        public Task<System.Collections.Generic.IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return new Task<System.Collections.Generic.IList<string>>(() =>
            {
                var roles = from q in user.ApplicationUserRole
                            select q.ApplicationRole.Name;
                return roles.ToList();
            });
        }

        public Task<bool> IsInRoleAsync(ApplicationUser user, string roleName)
        {
            return new Task<bool>(() =>
            {
                var userinroles = from q in user.ApplicationUserRole
                                  where q.ApplicationRole.Name.Equals(roleName, StringComparison.InvariantCultureIgnoreCase)
                                  select q;
                return userinroles.Any();
            });
        }

        public Task RemoveFromRoleAsync(ApplicationUser user, string roleName)
        {
            return new Task(() =>
            {
                var userinroles = (from q in user.ApplicationUserRole
                                   where q.ApplicationRole.Name.Equals(roleName, StringComparison.InvariantCultureIgnoreCase)
                                   select q).Single();

                userinroles.Void = true;
                accountrepo.Update(user);
                accountrepo.SaveChanges();
            });
        }
        #endregion

        #region EMail Stroe
        public Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return new Task<ApplicationUser>(() =>
            {
                var findemail = accountrepo.FindByEmail(email);
                return findemail;
            });
        }

        public Task<string> GetEmailAsync(ApplicationUser user)
        {
            return new Task<string>(() =>
            {
                var userinroles = (from q in user.ApplicationUserProfileRef
                                   select q.ApplicationUserProfile.EMail).Single();
                return userinroles;
            });
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user)
        {
            return new Task<bool>(() =>
            {
                var userinroles = (from q in user.ApplicationUserProfileRef
                                   select q.ApplicationUserProfile.EMailConfirmed);
                return userinroles.Single();
            });
        }

        public Task SetEmailAsync(ApplicationUser user, string email)
        {
            return new Task(() =>
            {
                var useremail = user.ApplicationUserProfileRef.Single();
                useremail.ApplicationUserProfile.EMail = email;
                accountrepo.Update(user);
                accountrepo.SaveChanges();
            });
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            return new Task(() =>
            {
                var useremail = user.ApplicationUserProfileRef.Single();
                useremail.ApplicationUserProfile.EMailConfirmed = confirmed;
                accountrepo.Update(user);
                accountrepo.SaveChanges();
            });
        }
        #endregion

        #region Lockout Store
        public Task<int> GetAccessFailedCountAsync(ApplicationUser user)
        {
            return new Task<int>(() =>
            {
                return user.AccessFailedCount;
            });
        }

        public Task<bool> GetLockoutEnabledAsync(ApplicationUser user)
        {
            return new Task<bool>(() =>
            {
                return user.LockoutEnabled.HasValue ? user.LockoutEnabled.Value : false;
            });
        }

        public Task<System.DateTimeOffset> GetLockoutEndDateAsync(ApplicationUser user)
        {
            return new Task<System.DateTimeOffset>(() =>
            {
                return user.LockoutEndDate.HasValue ? user.LockoutEndDate.Value : new System.DateTimeOffset(new DateTime(1754, 1, 1).ToUniversalTime());
            });
        }

        public Task<int> IncrementAccessFailedCountAsync(ApplicationUser user)
        {
            return new Task<int>(() =>
            {
                user.AccessFailedCount += 1;
                user.LastActivityTime =  DateTime.Now;
              
                user = accountrepo.Update(user);
                accountrepo.SaveChanges();
                return user.AccessFailedCount;
            });
        }

        public Task ResetAccessFailedCountAsync(ApplicationUser user)
        {
            return new Task(() =>
            {
                user.AccessFailedCount = 0;
                user.LastActivityTime =  DateTime.Now;
              
                user.LockoutEnabled = false;
                user.LockoutEndDate = DateTime.Now;
               
                accountrepo.Update(user);
                accountrepo.SaveChanges();
            });
        }

        public Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled)
        {
            return new Task(() =>
            {
                user.AccessFailedCount = 0;
                user.LastActivityTime = DateTime.Now;
                user.LastUpdateUserId = user.Id;
                user.LockoutEnabled = enabled;

                accountrepo.Update(user);
                accountrepo.SaveChanges();
            });
        }

        public Task SetLockoutEndDateAsync(ApplicationUser user, System.DateTimeOffset lockoutEnd)
        {
            return new Task(() =>
            {
                user.AccessFailedCount = 0;
                user.LastActivityTime =  DateTime.Now;
             
                user.LockoutEndDate = new DateTime(lockoutEnd.Ticks);
                accountrepo.Update(user);
                accountrepo.SaveChanges();
            });
        }
        #endregion

        #region Login Store
        public Task AddLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            return new Task(() =>
            {
                user.ApplicationUserLogin.Add(new ApplicationUserLogin()
                {
                    LoginProvider = login.LoginProvider,
                    ProviderKey = login.ProviderKey,
                    UserId = user.Id
                });

                accountrepo.Update(user);
                accountrepo.SaveChanges();
            });
        }

        public Task<ApplicationUser> FindAsync(UserLoginInfo login)
        {
            return new Task<ApplicationUser>(() =>
            {
                var founduser = from q in accountrepo.FindAll()
                                from l in q.ApplicationUserLogin
                                where l.LoginProvider == login.LoginProvider
                                && l.ProviderKey == login.ProviderKey
                                select q;

                return founduser.SingleOrDefault();
            });
        }

        public Task<System.Collections.Generic.IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user)
        {
            return new Task<System.Collections.Generic.IList<UserLoginInfo>>(() =>
            {
                var founduser = from q in accountrepo.FindAll()
                                from l in q.ApplicationUserLogin
                                select new UserLoginInfo(l.LoginProvider, l.ProviderKey);

                return founduser.ToList();
            });
        }

        public Task RemoveLoginAsync(ApplicationUser user, UserLoginInfo login)
        {
            return new Task(() =>
            {
                var foundlogininfo = (from q in user.ApplicationUserLogin
                                      where q.LoginProvider == login.LoginProvider
                                      && q.ProviderKey == login.ProviderKey
                                      select q).Single();

                user.ApplicationUserLogin.Remove(foundlogininfo);
                accountrepo.Update(user);
                accountrepo.SaveChanges();
            });
        }
        #endregion

        #region Password Store
        public Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            return new Task<string>(() =>
            {
                return user.PasswordHash;
            });
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            return new Task<bool>(() =>
            {
                if (!string.IsNullOrEmpty(user.Password))
                    return true;
                if (!string.IsNullOrEmpty(user.PasswordHash))
                    return true;
                return false;
            });
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            return new Task(() =>
            {
                user.PasswordHash = passwordHash;
                user.LastActivityTime = user.LastUpdateTime = DateTime.Now.ToUniversalTime();
                accountrepo.Update(user);
                accountrepo.SaveChanges();
            });
        }
        #endregion

        #region Phone Number Store
        public Task<string> GetPhoneNumberAsync(ApplicationUser user)
        {
            return new Task<string>(() =>
            {
                var result = from q in user.ApplicationUserProfileRef
                             select q.ApplicationUserProfile.PhoneNumber;

                return string.Join(";", result.ToArray());
            });
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(ApplicationUser user)
        {
            return new Task<bool>(() =>
            {
                var result = (from q in user.ApplicationUserProfileRef
                              select q.ApplicationUserProfile.PhoneConfirmed).Single();
                return result;
            });
        }

        public Task SetPhoneNumberAsync(ApplicationUser user, string phoneNumber)
        {
            return new Task(() =>
            {
                var profile = (from q in user.ApplicationUserProfileRef
                               select q.ApplicationUserProfile).Single();

                profile.PhoneNumber = phoneNumber;
                profile.PhoneConfirmed = true;

                if (GetTwoFactorEnabledAsync(user).Result)
                    profile.PhoneConfirmed = false;

                accountrepo.Update(user);
                accountrepo.SaveChanges();
            });
        }

        public Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            return new Task(() =>
            {
                var profile = (from q in user.ApplicationUserProfileRef
                               select q.ApplicationUserProfile).Single();

                profile.PhoneConfirmed = confirmed;

                accountrepo.Update(user);
                accountrepo.SaveChanges();
            });
        }
        #endregion

        #region Security Stamp Store
        public Task<string> GetSecurityStampAsync(ApplicationUser user)
        {
            return new Task<string>(() => {
                return user.SecurityStamp;
            });
        }

        public Task SetSecurityStampAsync(ApplicationUser user, string stamp)
        {
            return new Task(() =>
            {
                user.SecurityStamp = stamp;
                accountrepo.Update(user);
                accountrepo.SaveChanges();
            });
        }
        #endregion

        #region TwoFactor
        public Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user)
        {
            return new Task<bool>(() => {
                return user.TwoFactorEnabled;
            });
        }

        public Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled)
        {
            return new Task(() => {
                user.TwoFactorEnabled = enabled;
                accountrepo.Update(user);
                accountrepo.SaveChanges();
            });
        }
        #endregion

        #region Role Store
        public Task CreateAsync(ApplicationRole role)
        {
            return new Task(() => {
                rolerepo.Create(role);
                rolerepo.SaveChanges();
            });
        }

        public Task DeleteAsync(ApplicationRole role)
        {
            return new Task(() =>
            {
                role.Void = true;
                rolerepo.Update(role);
                rolerepo.SaveChanges();
            });
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

        #region 可用來查詢的使用者清單屬性
        public System.Linq.IQueryable<ApplicationUser> Users
        {
            get { return uow.GetEntity<ApplicationUser>(); }
        }
        #endregion

        #region 可用來查詢的角色清單
        public System.Linq.IQueryable<ApplicationRole> Roles
        {
            get { return uow.GetEntity<ApplicationRole>(); }
        }
        #endregion


    }
}