using System;
using My.Core.Infrastructures.DAL;

namespace My.Core.Infrastructures
{
	/// <summary>
	/// 定義使用者操作紀錄表
	/// </summary>
	public interface IUserOperationLog : IDataModel
	{
		/// <summary>
		/// 取得或設定操作紀錄識別碼。
		/// </summary>
		/// <value>The identifier.</value>
		long LogId { get; set; }
		/// <summary>
		/// 取得或設定操作代碼。
		/// </summary>
		/// <value>The opreation code.</value>
		int OpreationCode { get; set; }
		/// <summary>
		/// 取得或設定操作人員識別碼。
		/// </summary>
		/// <value>The user identifier.</value>
		int UserId { get; set; }
		/// <summary>
		/// 取得或設定紀錄時間。
		/// </summary>
		/// <value>The log time.</value>
		DateTime LogTime { get; set; }
		/// <summary>
		/// Gets or sets the URL.
		/// </summary>
		/// <value>The URL.</value>
		string URL { get; set; }
		/// <summary>
		/// Gets or sets the body.
		/// </summary>
		/// <value>The body.</value>
		string Body { get; set; }
	}
}

