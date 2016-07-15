using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using My.Core.Infrastructures.Datas;

namespace My.Core.Infrastructures.Implementations
{
	public class ApplicationUserProfile : IUserProfile
	{
		public ApplicationUserProfile()
		{
			_address = string.Empty;
			_email = string.Empty;
			_emailconfirmed = false;
			_phonenumber = string.Empty;
			_phoneumberconfirmed = false;
			_id = -1;

			Reserved10_Name = string.Empty;
			Reserved10_Value = string.Empty;

			Reserved9_Name = string.Empty;
			Reserved9_Value = string.Empty;

			Reserved8_Name = string.Empty;
			Reserved8_Value = string.Empty;

			Reserved7_Name = string.Empty;
			Reserved7_Value = string.Empty;

			Reserved6_Name = string.Empty;
			Reserved6_Value = string.Empty;

			Reserved5_Name = string.Empty;
			Reserved5_Value = string.Empty;

			Reserved4_Name = string.Empty;
			Reserved4_Value = string.Empty;

			Reserved3_Name = string.Empty;
			Reserved3_Value = string.Empty;

			Reserved2_Name = string.Empty;
			Reserved2_Value = string.Empty;

			Reserved1_Name = string.Empty;
			Reserved1_Value = string.Empty;
		}

		private string _address;

		public string Address
		{
			get
			{
				return _address;
			}

			set
			{
				_address = value;
			}
		}

		private string _email;
		public string Email
		{
			get
			{
				return _email;
			}

			set
			{
				_email = value;
			}
		}

		private bool _emailconfirmed;

		public bool EmailConfirmed
		{
			get
			{
				return _emailconfirmed;
			}

			set
			{
				_emailconfirmed = value;
			}
		}

		private int _id;
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int MemberId
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

		private string _phonenumber;

		public string PhoneNumber
		{
			get
			{
				return _phonenumber;
			}

			set
			{
				_phonenumber = value;
			}
		}

		private bool _phoneumberconfirmed;

		public bool PhoneNumberConfirmed
		{
			get
			{
				return _phoneumberconfirmed;
			}

			set
			{
				_phoneumberconfirmed = value;
			}
		}

		private string _reserved10name;
		private string _reserved10value;

		public string Reserved10_Name
		{
			get
			{
				return _reserved10name;
			}

			set
			{
				_reserved10name = value;
			}
		}

		public string Reserved10_Value
		{
			get
			{
				return _reserved10value;
			}

			set
			{
				_reserved10value = value;
			}
		}

		private string _reserved1name;
		private string _reserved1value;

		public string Reserved1_Name
		{
			get
			{
				return _reserved1name;
			}

			set
			{
				_reserved1name = value;
			}
		}

		public string Reserved1_Value
		{
			get
			{
				return _reserved1value;
			}

			set
			{
				_reserved1value = value;
			}
		}

		private string _reserved2name;
		private string _reserved2value;

		public string Reserved2_Name
		{
			get
			{
				return _reserved2name;
			}

			set
			{
				_reserved2name = value;
			}
		}

		public string Reserved2_Value
		{
			get
			{
				return _reserved2value;
			}

			set
			{
				_reserved2value = value;
			}
		}

		private string _reserved3name;
		private string _reserved3value;

		public string Reserved3_Name
		{
			get
			{
				return _reserved3name;
			}

			set
			{
				_reserved3name = value;
			}
		}

		public string Reserved3_Value
		{
			get
			{
				return _reserved3value;
			}

			set
			{
				_reserved3value = value;
			}
		}

		private string _reserved4name;
		private string _reserved4value;

		public string Reserved4_Name
		{
			get
			{
				return _reserved4name;
			}

			set
			{
				_reserved4name = value;
			}
		}

		public string Reserved4_Value
		{
			get
			{
				return _reserved4value;
			}

			set
			{
				_reserved4value = value;
			}
		}

		private string _reserved5name;
		private string _reserved5value;

		public string Reserved5_Name
		{
			get
			{
				return _reserved5name;
			}

			set
			{
				_reserved5name = value;
			}
		}

		public string Reserved5_Value
		{
			get
			{
				return _reserved5value;
			}

			set
			{
				_reserved5value = value;
			}
		}

		private string _reserved6name;
		private string _reserved6value;

		public string Reserved6_Name
		{
			get
			{
				return _reserved6name;
			}

			set
			{
				_reserved6name = value;
			}
		}

		public string Reserved6_Value
		{
			get
			{
				return _reserved6value;
			}

			set
			{
				_reserved6value = value;
			}
		}

		private string _reserved7name;
		private string _reserved7value;

		public string Reserved7_Name
		{
			get
			{
				return _reserved7name;
			}

			set
			{
				_reserved7name = value;
			}
		}

		public string Reserved7_Value
		{
			get
			{
				return _reserved7value;
			}

			set
			{
				_reserved7value = value;
			}
		}

		private string _reserved8name;
		private string _reserved8value;

		public string Reserved8_Name
		{
			get
			{
				return _reserved8name;
			}

			set
			{
				_reserved8name = value;
			}
		}

		public string Reserved8_Value
		{
			get
			{
				return _reserved8value;
			}

			set
			{
				_reserved8value = value;
			}
		}

		private string _reserved9name;
		private string _reserved9value;

		public string Reserved9_Name
		{
			get
			{
				return _reserved9name;
			}

			set
			{
				_reserved9name = value;
			}
		}

		public string Reserved9_Value
		{
			get
			{
				return _reserved9value;
			}

			set
			{
				_reserved9value = value;
			}
		}

		private string _telphonenumber;
		/// <summary>
		/// 取得或設定市內電話號碼。
		/// </summary>
		/// <value>The tel phone number.</value>
		public string TelPhoneNumber
		{
			get
			{
				return _telphonenumber;
			}

			set
			{
				_telphonenumber = value;
			}
		}

		private string _themecssname;
		/// <summary>
		/// 取得或設定個人化佈景主題CSS名稱。
		/// </summary>
		/// <value>個人化佈景主題CSS名稱.</value>
		public string ThemeCSSName
		{
			get
			{
				return _themecssname;
			}

			set
			{
				_themecssname = value;
			}
		}

		private string _themecssurl;

		/// <summary>
		/// 取得或設定個人化佈景主題CSS檔案URL位址。
		/// </summary>
		/// <value>個人化佈景主題CSS檔案URL位址.</value>
		public string ThemeCSSURL
		{
			get
			{
				return _themecssurl;
			}

			set
			{
				_themecssurl = value;
			}
		}
			
	}
}

