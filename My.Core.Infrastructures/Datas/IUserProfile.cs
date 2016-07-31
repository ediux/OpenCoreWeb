namespace My.Core.Infrastructures.Datas
{
	/// <summary>
	/// 定義系統使用者額外設定資訊表
	/// </summary>
	public interface IUserProfile : IDataModel
	{
		
		/// <summary>
		/// 取得或設定使用者系統識別碼
		/// </summary>
		/// <value>The identifier.</value>
		int MemberId { get; set; }

		/// <summary>
		/// Gets or sets the email.
		/// </summary>
		/// <value>The email.</value>
		string Email { get; set; }

		/// <summary>
		/// Gets or sets the email confirmed.
		/// </summary>
		/// <value>The email confirmed.</value>
		bool EmailConfirmed { get; set; }

		/// <summary>
		/// Gets or sets the phone number.
		/// </summary>
		/// <value>The phone number.</value>
		string PhoneNumber { get; set; }

		/// <summary>
		/// Gets or sets the phone number confirmed.
		/// </summary>
		/// <value>The phone number confirmed.</value>
		bool PhoneNumberConfirmed { get; set; }

		/// <summary>
		/// Gets or sets the tel phone number.
		/// </summary>
		/// <value>The tel phone number.</value>
		string TelPhoneNumber { get; set; }

		/// <summary>
		/// 取得或設定通訊地址。
		/// </summary>
		/// <value>The address.</value>
		string Address { get; set; }

		/// <summary>
		/// 取得或設定個人化佈景主題CSS名稱。
		/// </summary>
		/// <value>The theme CSSN ame.</value>
		string ThemeCSSName { get; set; }

		string ThemeCSSURL { get; set; }

		string Reserved1_Name { get; set; }

		string Reserved1_Value { get; set; }

		string Reserved2_Name { get; set; }

		string Reserved2_Value { get; set; }

		string Reserved3_Name { get; set; }

		string Reserved3_Value { get; set; }

		string Reserved4_Name { get; set; }

		string Reserved4_Value { get; set; }

		string Reserved5_Name { get; set; }

		string Reserved5_Value { get; set; }

		string Reserved6_Name { get; set; }

		string Reserved6_Value { get; set; }

		string Reserved7_Name { get; set; }

		string Reserved7_Value { get; set; }

		string Reserved8_Name { get; set; }

		string Reserved8_Value { get; set; }

		string Reserved9_Name { get; set; }

		string Reserved9_Value { get; set; }

		string Reserved10_Name { get; set; }

		string Reserved10_Value { get; set; }
	}
}

