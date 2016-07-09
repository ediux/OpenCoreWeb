using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace My.Core.Infrastructures
{
	public interface IRepositoryBase<T>
		where T : IDataModel
	{
		T Create(T entity);

		/// <summary>
		/// 批次建立多筆資料列於資料表
		/// </summary>
		/// <param name="entities">包含多個新建資料列的列舉。</param>
		/// <returns></returns>
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

