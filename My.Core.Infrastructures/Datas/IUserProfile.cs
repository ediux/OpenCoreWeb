using System;
using My.Core.Infrastructures.DAL;

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
		int Id { get; set; }

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


	}
}

