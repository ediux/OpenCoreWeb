//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace My.Core.Infrastructures.Implementations.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ApplicationUser
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ApplicationUser()
        {
            this.ApplicationUserGroup = new HashSet<ApplicationUserGroup>();
            this.ApplicationUserProfileRef = new HashSet<ApplicationUserProfileRef>();
            this.ApplicationUserRole = new HashSet<ApplicationUserRole>();
            this.UserOperationLog = new HashSet<UserOperationLog>();
            this.ApplicationUserLogin = new HashSet<ApplicationUserLogin>();
            this.ApplicationUserClaim = new HashSet<ApplicationUserClaim>();
        }
    
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public bool Void { get; set; }
        public int CreateUserId { get; set; }
        public System.DateTime CreateTime { get; set; }
        public int LastUpdateUserId { get; set; }
        public System.DateTime LastUpdateTime { get; set; }
        public Nullable<System.DateTime> LastActivityTime { get; set; }
        public Nullable<System.DateTime> LastUnlockedTime { get; set; }
        public Nullable<System.DateTime> LastLoginFailTime { get; set; }
        public int AccessFailedCount { get; set; }
        public Nullable<bool> LockoutEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDate { get; set; }
        public string ResetPasswordToken { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApplicationUserGroup> ApplicationUserGroup { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApplicationUserProfileRef> ApplicationUserProfileRef { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApplicationUserRole> ApplicationUserRole { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserOperationLog> UserOperationLog { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApplicationUserLogin> ApplicationUserLogin { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ApplicationUserClaim> ApplicationUserClaim { get; set; }
    }
}
