using System;
namespace My.Core.Infrastructures.DAL
{
	/// <summary>
	/// 使用者群組
	/// </summary>
	public interface IGroup : IDataModel
	{
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		int GroupId { get; set; }

		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		string Name { get; set; }

		/// <summary>
		/// Gets or sets the left position.
		/// </summary>
		/// <value>The left position.</value>
		int LeftPos { get; set; }

		/// <summary>
		/// Gets or sets the parent identifier.
		/// </summary>
		/// <value>The parent identifier.</value>
		//int? ParentId { get; set; }

		/// <summary>
		/// Gets or sets the right position.
		/// </summary>
		/// <value>The right position.</value>
		int RightPos { get; set; }
	}
}

