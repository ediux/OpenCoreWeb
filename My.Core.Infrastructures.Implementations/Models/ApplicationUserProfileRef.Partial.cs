namespace My.Core.Infrastructures.Implementations.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    [MetadataType(typeof(ApplicationUserProfileRefMetaData))]
    public partial class ApplicationUserProfileRef
    {
    }
    
    public partial class ApplicationUserProfileRefMetaData
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int ProfileId { get; set; }
        [Required]
        public bool Void { get; set; }
        [Required]
        public System.DateTime CreateTime { get; set; }
        [Required]
        public System.DateTime LastUpdateTime { get; set; }
    
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ApplicationUserProfile ApplicationUserProfile { get; set; }
    }
}