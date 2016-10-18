using System;
using My.Core.Infrastructures.DAL;

namespace My.Core.Infrastructures.Datas
{
	/// <summary>
	/// 定義使用者帳號資料表
	/// </summary>
	public interface IAccount : IDataModel
	{
		/// <summary>
		/// 取得或設定使用者系統識別碼。
		/// </summary>
		/// <value>The identifier.</value>
		int MemberId { get; set; }

		/// <summary>
		/// 使用者登入帳號名稱
		/// </summary>
		string UserName { get; set; }

		/// <summary>
		/// 使用者顯示名稱
		/// </summary>
		string DisplayName { get; set; }

		/// <summary>
		/// 取得或設定登入密碼（未加密）
		/// </summary>
		/// <value>The password.</value>
		string Password { get; set; }

		/// <summary>
		/// 取得或設定登入密碼（已加密）
		/// </summary>
		/// <value>The password hash.</value>
		string PasswordHash { get; set; }

		/// <summary>
		/// 取得或設定使用者安全性戳記。
		/// </summary>
		/// <value>The security stamp.</value>
		string SecurityStamp { get; set; }

		/// <summary>
		/// 取得或設定使用者是否啟用兩步驟驗證？
		/// </summary>
		/// <value>The two factor enabled.</value>
		bool TwoFactorEnabled { get; set; }

		/// <summary>
		/// 取得或設定使用者使否被停用？
		/// </summary>
		/// <value>The void.</value>
		bool Void { get; set; }

		/// <summary>
		/// Gets or sets the reset password token.
		/// </summary>
		/// <value>The reset password token.</value>
		string ResetPasswordToken { get; set; }
	}
}

