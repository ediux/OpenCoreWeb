namespace My.Core.Infrastructures.Implementations.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    [MetadataType(typeof(ApplicationGroupTreeMetaData))]
    public partial class ApplicationGroupTree
    {
    }
    
    public partial class ApplicationGroupTreeMetaData
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int ParentId { get; set; }
        [Required]
        public int ChildId { get; set; }
        [Required]
        public int Level { get; set; }
        [Required]
        public bool Void { get; set; }
    
        public virtual ApplicationGroup ApplicationGroup { get; set; }
        public virtual ApplicationGroup ApplicationGroup1 { get; set; }
        public virtual ApplicationGroup ApplicationGroup2 { get; set; }
    }
}
