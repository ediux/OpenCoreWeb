using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My.Core.Infrastructures.Implementations
{
	/// <summary>
	/// 使用者操作紀錄表
	/// </summary>
	public  class UserOperationLog : IUserOperationLog
	{
		public UserOperationLog()
		{
			_body = string.Empty;
			_id = Guid.NewGuid();
			_logtime = DateTime.Now;
			_opreationcode = 0;
			_url = string.Empty;
			_userid = -1;
		}

		#region Body
		private string _body;

		/// <summary>
		/// Gets or sets the body.
		/// </summary>
		/// <value>The body.</value>
		[MaxLength]
		public string Body
		{
			get
			{
				return _body;
			}

			set
			{
				_body = value;
			}
		}
		#endregion

		#region Id
		private Guid _id;

		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id
		{
			get
			{
				return _id;
			}

			set
			{
				_id = value;
			}
		}
		#endregion

		#region logtime
		private DateTime _logtime;
		/// <summary>
		/// Gets or sets the log time.
		/// </summary>
		/// <value>The log time.</value>
		public DateTime LogTime
		{
			get
			{
				return _logtime;
			}

			set
			{
				_logtime = value;
			}
		}
		#endregion

		#region OpreationCode
		private int _opreationcode;
		/// <summary>
		/// Gets or sets the opreation code.
		/// </summary>
		/// <value>The opreation code.</value>
		public int OpreationCode
		{
			get
			{
				return _opreationcode;
			}

			set
			{
				_opreationcode = value;
			}
		}
		#endregion

		#region URL
		private string _url;
		/// <summary>
		/// Gets or sets the URL.
		/// </summary>
		/// <value>The URL.</value>
		public string URL
		{
			get
			{
				return _url;
			}

			set
			{
				_url = value;
			}
		}
		#endregion

		#region UserId
		private int _userid;
		/// <summary>
		/// Gets or sets the user identifier.
		/// </summary>
		/// <value>The user identifier.</value>
		public int UserId
		{
			get
			{
				return _userid;
			}

			set
			{
				_userid = value;
			}
		}
		#endregion

		#region OperationDetail
		/// <summary>
		/// Gets or sets the operation detail.
		/// </summary>
		/// <value>The operation detail.</value>
		[ForeignKey("OpreationCode")]
		public virtual UserOperationCodeDefine OperationDetail { get; set; }
		#endregion

		#region User
		[ForeignKey("UserId")]
		public virtual ApplicationUser User { get; set; }
		#endregion
	}
}

