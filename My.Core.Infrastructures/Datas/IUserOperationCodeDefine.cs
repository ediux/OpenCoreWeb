namespace My.Core.Infrastructures.Datas
{
	/// <summary>
	/// 定義使用者操作代碼對應表。
	/// </summary>
	public interface IUserOperationCodeDefine : IDataModel
	{
		/// <summary>
		/// 取得或設定操作代碼
		/// </summary>
		/// <value>The opreation code.</value>
		int OpreationCode { get; set; }
		/// <summary>
		/// 取得或設定操作代碼描述說明。
		/// </summary>
		/// <value>The description.</value>
		string Description { get; set; }
	}
}

