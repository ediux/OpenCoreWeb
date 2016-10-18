namespace My.Core.Infrastructures.Implementations.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    [MetadataType(typeof(UserOperationLogMetaData))]
    public partial class UserOperationLog
    {
    }
    
    public partial class UserOperationLogMetaData
    {
        [Required]
        public long Id { get; set; }
        public string Body { get; set; }
        [Required]
        public System.DateTime LogTime { get; set; }
        [Required]
        public int OpreationCode { get; set; }
        public string URL { get; set; }
        [Required]
        public int UserId { get; set; }
    
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual UserOperationCodeDefine UserOperationCodeDefine { get; set; }
    }
}
