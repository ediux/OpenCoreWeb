namespace My.Core.Infrastructures.Implementations.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    [MetadataType(typeof(ApplicationUserProfileMetaData))]
    public partial class ApplicationUserProfile
    {
    }
    
    public partial class ApplicationUserProfileMetaData
    {
        [Required]
        public int Id { get; set; }
        public string Address { get; set; }
        
        [StringLength(512, ErrorMessage="欄位長度不得大於 512 個字元")]
        [Required]
        public string EMail { get; set; }
        [Required]
        public bool EMailConfirmed { get; set; }
        
        [StringLength(10, ErrorMessage="欄位長度不得大於 10 個字元")]
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public bool PhoneConfirmed { get; set; }
        [Required]
        public System.DateTime CreateTime { get; set; }
        [Required]
        public System.DateTime LastUpdateTime { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string DisplayName { get; set; }
    
        public virtual ICollection<ApplicationUserProfileRef> ApplicationUserProfileRef { get; set; }
    }
}
