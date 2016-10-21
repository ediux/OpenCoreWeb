namespace My.Core.Infrastructures.Implementations.Models
{
    using Microsoft.AspNet.Identity;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel.DataAnnotations;
    using System.Security.Claims;
    using System.Threading.Tasks;

    [MetadataType(typeof(ApplicationUserMetaData))]
    public partial class ApplicationUser : IUser<int>
    {

        public static ApplicationUser Create()
        {
            return new ApplicationUser()
            {
                Id = -1,
                AccessFailedCount = 0,
                ApplicationUserClaim = new Collection<ApplicationUserClaim>(),
                ApplicationUserGroup = new Collection<ApplicationUserGroup>(),
                ApplicationUserLogin = new Collection<ApplicationUserLogin>(),
                ApplicationUserProfileRef = new Collection<ApplicationUserProfileRef>(),
                ApplicationUserRole = new Collection<ApplicationUserRole>(),
                CreateTime = DateTime.Now.ToUniversalTime(),
                CreateUserId = -1,
                LastActivityTime = DateTime.Now.ToUniversalTime(),
                LastUpdateTime = DateTime.Now.ToUniversalTime(),
                LastUpdateUserId = -1,
                Password = string.Empty,
                PasswordHash = string.Empty,
                LockoutEnabled = false,
                ResetPasswordToken = string.Empty,
                SecurityStamp = string.Empty,
                TwoFactorEnabled = false,
                UserName = string.Empty,
                Void = false,
                UserOperationLog = new Collection<UserOperationLog>()
            };
        }

        public static ApplicationUser CreateKernelUser()
        {
            return new ApplicationUser()
            {
                Id = -1,
                AccessFailedCount = 0,
                ApplicationUserClaim = new Collection<ApplicationUserClaim>(),
                ApplicationUserGroup = new Collection<ApplicationUserGroup>(),
                ApplicationUserLogin = new Collection<ApplicationUserLogin>(),
                ApplicationUserProfileRef = new Collection<ApplicationUserProfileRef>(),
                ApplicationUserRole = new Collection<ApplicationUserRole>(),
                CreateTime = DateTime.Now.ToUniversalTime(),
                CreateUserId = -1,
                LastActivityTime = DateTime.Now.ToUniversalTime(),
                LastUpdateTime = DateTime.Now.ToUniversalTime(),
                LastUpdateUserId = -1,
                Password = string.Empty,
                PasswordHash = string.Empty,
                LockoutEnabled = false,
                ResetPasswordToken = string.Empty,
                SecurityStamp = string.Empty,
                TwoFactorEnabled = false,
                UserName = "System",
                Void = false,
                UserOperationLog = new Collection<UserOperationLog>()
            };
        }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            // 注意 authenticationType 必須符合 CookieAuthenticationOptions.AuthenticationType 中定義的項目
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // 在這裡新增自訂使用者宣告
            return userIdentity;
            
        }
    }

    public partial class ApplicationUserMetaData
    {
        [Display(Name = "Id", ResourceType = typeof(ReslangMUI.MUI))]
        [Required]
        public int Id { get; set; }

        [Display(Name = "UserName", ResourceType = typeof(ReslangMUI.MUI), ShortName = "UserName")]
        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        [Required]
        public string UserName { get; set; }
        [Display(Name = "密碼")]
        [StringLength(50, ErrorMessage = "欄位長度不得大於 50 個字元")]
        public string Password { get; set; }
        [Display(Name = "加密後密碼")]
        public string PasswordHash { get; set; }
        [Display(Name = "安全戳記")]
        public string SecurityStamp { get; set; }
        [Required]
        [Display(Name = "兩步驟驗證")]
        [UIHint("VoidDisplay")]
        public bool TwoFactorEnabled { get; set; }
        [Required]
        [Display(Name = "狀態")]
        [UIHint("VoidDisplay")]
        public bool Void { get; set; }
        [Required]
        [Display(Name = "建立者")]
        [UIHint("UserIDMappingDisplay")]
        public int CreateUserId { get; set; }
        [Required]
        [Display(Name = "建立時間")]
        public System.DateTime CreateTime { get; set; }
        [Required]
        [Display(Name = "最後一次更新者")]
        [UIHint("UserIDMappingDisplay")]
        public int LastUpdateUserId { get; set; }
        [Required]
        [Display(Name = "最後一次更新時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        public System.DateTime LastUpdateTime { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        public Nullable<System.DateTime> LastActivityTime { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        public Nullable<System.DateTime> LastUnlockedTime { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
        public Nullable<System.DateTime> LastLoginFailTime { get; set; }
        [Required]
        [Display(Name = "登入失敗次數")]
        public int AccessFailedCount { get; set; }
        [Display(Name = "鎖定狀態")]
        [UIHint("LockedStateDisplay")]
        public Nullable<bool> LockoutEnabled { get; set; }
        [Display(Name = "鎖定結束日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public Nullable<System.DateTime> LockoutEndDate { get; set; }
        [Display(Name = "密碼重設權杖")]
        [StringLength(512, ErrorMessage = "欄位長度不得大於 512 個字元")]
        public string ResetPasswordToken { get; set; }

        public virtual ICollection<ApplicationUserGroup> ApplicationUserGroup { get; set; }
        public virtual ICollection<ApplicationUserProfileRef> ApplicationUserProfileRef { get; set; }
        public virtual ICollection<ApplicationUserRole> ApplicationUserRole { get; set; }
        public virtual ICollection<UserOperationLog> UserOperationLog { get; set; }
        public virtual ApplicationUserClaim ApplicationUserClaim { get; set; }
        public virtual ICollection<ApplicationUserLogin> ApplicationUserLogin { get; set; }
    }
}
