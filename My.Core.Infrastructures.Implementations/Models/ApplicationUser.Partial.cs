namespace My.Core.Infrastructures.Implementations.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    [MetadataType(typeof(ApplicationUserMetaData))]
    public partial class ApplicationUser
    {
    }
    
    public partial class ApplicationUserMetaData
    {
        [Required]
        public int Id { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string UserName { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        public string Password { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        [Required]
        public bool TwoFactorEnabled { get; set; }
        [Required]
        public bool Void { get; set; }
        [Required]
        public int CreateUserId { get; set; }
        [Required]
        public System.DateTime CreateTime { get; set; }
        [Required]
        public int LastUpdateUserId { get; set; }
        [Required]
        public System.DateTime LastUpdateTime { get; set; }
    
        public virtual ICollection<ApplicationUserGroup> ApplicationUserGroup { get; set; }
        public virtual ICollection<ApplicationUserProfileRef> ApplicationUserProfileRef { get; set; }
        public virtual ICollection<ApplicationUserRole> ApplicationUserRole { get; set; }
    }
}
