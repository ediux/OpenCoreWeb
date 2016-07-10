using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace My.Core.Infrastructures
{
	/// <summary>
	/// Repository base.
	/// </summary>
	public interface IRepositoryBase<T>
		where T : IDataModel
	{
		/// <summary>
		/// Create the specified entity.
		/// </summary>
		/// <param name="entity">Entity.</param>
		T Create(T entity);

		/// <summary>
		/// Batchs the create.
		/// </summary>
		/// <returns>The create.</returns>
		/// <param name="entities">Entities.</param>
		IList<T> BatchCreate(IEnumerable<T> entities);

		/// <summary>
		/// 取得第一筆符合條件的內容。如果符合條件有多筆，也只取得第一筆。
		/// </summary>
		/// <param name="predicate">要取得的Where條件。</param>
		/// <returns>取得第一筆符合條件的內容。</returns>
		T Find(Expression<Func<T, bool>> predicate);

		/// <summary>
		/// 取得Entity全部筆數的IQueryable。
		/// </summary>
		/// <returns>Entity全部筆數的IQueryable。</returns>
		IQueryable<T> FindAll();

		/// <summary>
		/// 更新一筆資料的內容。
		/// </summary>
		/// <param name="entity">要更新的內容</param>
		T Update(T entity);

		/// <summary>
		/// 刪除一筆資料內容。
		/// </summary>
		/// <param name="entity">要被刪除的Entity。</param>
		void Delete(T entity);

		/// <summary>
		/// 儲存異動。
		/// </summary>
		void SaveChanges();

		/// <summary>
		/// 轉換成清單物件。
		/// </summary>
		/// <returns></returns>
		IList<T> ToList(IQueryable<T> source);
	}
}

