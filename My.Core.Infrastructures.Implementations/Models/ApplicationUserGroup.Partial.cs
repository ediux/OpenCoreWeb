namespace My.Core.Infrastructures.Implementations.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    [MetadataType(typeof(ApplicationUserGroupMetaData))]
    public partial class ApplicationUserGroup
    {
    }
    
    public partial class ApplicationUserGroupMetaData
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int GroupId { get; set; }
        [Required]
        [UIHint("VoidDisplay")]
        [Display(Name = "Void", ResourceType = typeof(ReslangMUI.MUI))]
        public bool Void { get; set; }
    
        public virtual ApplicationGroup ApplicationGroup { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
