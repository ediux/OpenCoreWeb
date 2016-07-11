using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace My.Core.Infrastructures.Implementations
{
	public class UserOperationCodeDefine : IUserOperationCodeDefine
	{
		public UserOperationCodeDefine()
		{
			_opreationcode = -1;
			_description = string.Empty;
		}
		private string _description;

		/// <summary>
		/// Gets or sets the description.
		/// </summary>
		/// <value>The description.</value>
		[MaxLength(2048)]
		public string Description
		{
			get
			{
				return _description;
			}

			set
			{
				_description = value;
			}
		}

		private int _opreationcode;

		/// <summary>
		/// Gets or sets the opreation code.
		/// </summary>
		/// <value>The opreation code.</value>
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
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

		public virtual Collection<UserOperationLog> Logs { get; set; }
	}
}

