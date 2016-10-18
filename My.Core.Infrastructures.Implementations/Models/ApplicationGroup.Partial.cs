namespace My.Core.Infrastructures.Implementations.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    [MetadataType(typeof(ApplicationGroupMetaData))]
    public partial class ApplicationGroup
    {
    }
    
    public partial class ApplicationGroupMetaData
    {
        [Required]
        public int Id { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string Name { get; set; }
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
    
        public virtual ICollection<ApplicationGroupTree> ApplicationGroupTree { get; set; }
        public virtual ICollection<ApplicationGroupTree> ApplicationGroupTree1 { get; set; }
        public virtual ICollection<ApplicationGroupTree> ApplicationGroupTree2 { get; set; }
        public virtual ICollection<ApplicationUserGroup> ApplicationUserGroup { get; set; }
    }
}
