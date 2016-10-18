namespace My.Core.Infrastructures.Implementations.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    [MetadataType(typeof(ApplicationRoleMetaData))]
    public partial class ApplicationRole
    {
    }
    
    public partial class ApplicationRoleMetaData
    {
        [Required]
        public int Id { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string Name { get; set; }
        [Required]
        public int CreateUserId { get; set; }
        [Required]
        public System.DateTime CreateTime { get; set; }
        [Required]
        public int LastUpdateUserId { get; set; }
        [Required]
        public System.DateTime LastUpdateTime { get; set; }
    
        public virtual ICollection<ApplicationUserRole> ApplicationUserRole { get; set; }
    }
}
