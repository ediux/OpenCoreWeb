﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class OpenWebSiteEntities : DbContext
    {
        public OpenWebSiteEntities()
            : base("name=OpenWebSiteEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<ApplicationGroup> ApplicationGroup { get; set; }
        public virtual DbSet<ApplicationGroupTree> ApplicationGroupTree { get; set; }
        public virtual DbSet<ApplicationRole> ApplicationRole { get; set; }
        public virtual DbSet<ApplicationUser> ApplicationUser { get; set; }
        public virtual DbSet<ApplicationUserGroup> ApplicationUserGroup { get; set; }
        public virtual DbSet<ApplicationUserProfile> ApplicationUserProfile { get; set; }
        public virtual DbSet<ApplicationUserProfileRef> ApplicationUserProfileRef { get; set; }
        public virtual DbSet<ApplicationUserRole> ApplicationUserRole { get; set; }
        public virtual DbSet<UserOperationCodeDefine> UserOperationCodeDefine { get; set; }
        public virtual DbSet<UserOperationLog> UserOperationLog { get; set; }
        public virtual DbSet<ApplicationUserClaim> ApplicationUserClaim { get; set; }
        public virtual DbSet<ApplicationUserLogin> ApplicationUserLogin { get; set; }
    }
}
