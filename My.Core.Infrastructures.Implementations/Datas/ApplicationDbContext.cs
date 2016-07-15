using System;
using System.Data.Entity;
using MySql.Data.Entity;

namespace My.Core.Infrastructures.Implementations
{
	[DbConfigurationType(typeof(MySqlEFConfiguration))]
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext() :
		this(System.Configuration.ConfigurationManager.ConnectionStrings["Web"].ConnectionString)
		{
		}

		public ApplicationDbContext(string nameorconnectino) : base(nameorconnectino)
		{
			DbConfiguration.SetConfiguration(new MySqlEFConfiguration());
			base.Configuration.LazyLoadingEnabled = true;
		}

		public virtual IDbSet<ApplicationUser> Users { get; set; }

		public virtual IDbSet<ApplicationRole> Roles { get; set; }

		public virtual IDbSet<ApplicationUserGroup> Groups { get; set; }

		public virtual IDbSet<UserOperationCodeDefine> OperationCodeDefines { get; set; }

		public virtual IDbSet<UserOperationLog> UserOperationLogs { get; set; }

		public virtual IDbSet<ApplicationUserProfile> UserProfiles { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Entity<ApplicationUser>()
						.HasMany(m => m.Roles)
						.WithMany(a => a.Users)
						.Map(m =>
						{
							m.MapLeftKey("UserId");
							m.MapRightKey("RoleId");
							m.ToTable("AspNetUserRole");
						});

			modelBuilder.Entity<ApplicationUser>()
						.HasMany(m => m.Groups)
						.WithMany(p => p.Users)
						.Map(m =>
						{
							m.MapLeftKey("UserId");
							m.MapRightKey("GroupId");
							m.ToTable("AspNetUserGroups");
						});

			modelBuilder.Entity<UserOperationLog>()
						.HasRequired(pk => pk.User)
						.WithMany(mp => mp.OpreationLogs)
						.HasForeignKey(k => k.UserId);

			modelBuilder.Entity<UserOperationLog>()
						.HasRequired(rp => rp.OperationDetail)
						.WithMany(p => p.Logs)
						.HasForeignKey(fk => fk.OpreationCode);

			modelBuilder.Entity<ApplicationUserGroup>()
						.HasOptional(op => op.ParentGroup)
						.WithMany(wp => wp.SubGroups)
						.Map(m => m.MapKey("ParentId"))
						.WillCascadeOnDelete(false);

			base.OnModelCreating(modelBuilder);

		}
	}
}

