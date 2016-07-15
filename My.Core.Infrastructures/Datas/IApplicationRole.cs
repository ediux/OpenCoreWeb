using System;
namespace My.Core.Infrastructures.DAL
{
	/// <summary>
	/// 系統角色資料表
	/// </summary>
	public interface IApplicationRole : IDataModel
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		int RoleId { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>角色名稱</value>
		string Name { get; set; }

	}
}

