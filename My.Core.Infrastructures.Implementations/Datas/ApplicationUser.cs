using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using My.Core.Infrastructures.Datas;

namespace My.Core.Infrastructures.Implementations.Datas
{
	[Table("AspNetUsers")]
	public class ApplicationUser : IAccount
	{
		public ApplicationUser()
		{
			_id = -1;
			_displayname = string.Empty;
			_password = string.Empty;
			_passwordhash = string.Empty;
			_securitystamp = string.Empty;
			_twofactorenabled = false;
			_username = string.Empty;
			_void = false;

			Roles = new Collection<ApplicationRole>();
			Groups = new Collection<ApplicationUserGroup>();
			OpreationLogs = new Collection<UserOperationLog>();
		}
		private string _displayname;
		/// <summary>
		/// Gets or sets the display name.
		/// </summary>
		/// <value>The display name.</value>
		public virtual string DisplayName { get { return _displayname; } set { _displayname = value; } }

		private int _id;
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public virtual int MemberId { get { return _id; } set { _id = value; } }

		private string _password;
		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
		[MaxLength(50)]
		public virtual string Password { get { return _password; } set { _password = value; } }

		private string _passwordhash;
		/// <summary>
		/// Gets or sets the password hash.
		/// </summary>
		/// <value>The password hash.</value>
		[MaxLength]
		public virtual string PasswordHash { get { return _passwordhash; } set { _passwordhash = value; } }

		private string _securitystamp;
		/// <summary>
		/// Gets or sets the security stamp.
		/// </summary>
		/// <value>The security stamp.</value>
		[MaxLength]
		public virtual string SecurityStamp { get { return _securitystamp; } set { _securitystamp = value; } }

		private bool _twofactorenabled;

		/// <summary>
		/// Gets or sets the two factor enabled.
		/// </summary>
		/// <value>The two factor enabled.</value>
		public virtual bool TwoFactorEnabled { get { return _twofactorenabled; } set { _twofactorenabled = value; } }

		private string _username;
		/// <summary>
		/// Gets or sets the name of the user.
		/// </summary>
		/// <value>The name of the user.</value>
		[MaxLength(50)]
		public virtual string UserName { get { return _username; } set { _username = value; } }

		private bool _void;
		/// <summary>
		/// Gets or sets the void.
		/// </summary>
		/// <value>The void.</value>
		public virtual bool Void { get { return _void; } set { _void = value; } }

		/// <summary>
		/// 取得或設定使用者擁有的角色。
		/// </summary>
		/// <value>傳回使用者擁有的角色清單。</value>
		public virtual Collection<ApplicationRole> Roles { get; set; }

		/// <summary>
		/// Gets or sets the groups.
		/// </summary>
		/// <value>The groups.</value>
		public virtual Collection<ApplicationUserGroup> Groups { get; set; }
		/// <summary>
		/// Gets or sets the opreation logs.
		/// </summary>
		/// <value>The opreation logs.</value>
		public virtual Collection<UserOperationLog> OpreationLogs { get; set; }

		private string _resetpasswordtoken;

		public string ResetPasswordToken
		{
			get
			{
				return _resetpasswordtoken;
			}

			set
			{
				_resetpasswordtoken = value;
			}
		}
	}
}

