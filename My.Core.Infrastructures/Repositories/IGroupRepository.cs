﻿using System.Collections.Generic;
using My.Core.Infrastructures.Datas;

namespace My.Core.Infrastructures
{
	public interface IGroupRepository : IRepositoryBase<IGroup>
	{
		/// <summary>
		/// Finds the by user.
		/// </summary>
		/// <returns>The by user.</returns>
		/// <param name="id">Identifier.</param>
		List<IGroup> FindByUser(int id);


		/// <summary>
		/// 將指定使用者加入指定群組
		/// </summary>
		/// <param name="username"></param>
		/// <param name="groupname"></param>
		/// <returns></returns>
		void AddUserToGroup(string username, string groupname);

		/// <summary>
		/// Removes the user from group.
		/// </summary>
		/// <returns>The user from group.</returns>
		/// <param name="username">Username.</param>
		/// <param name="groupname">Groupname.</param>
		/// <param name="isCanUndo">Is can undo.</param>
		void RemoveUserFromGroup(string username, string groupname, bool isCanUndo = true);
	}
}

