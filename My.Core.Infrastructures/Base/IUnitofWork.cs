﻿using System;
namespace My.Core.Infrastructures
{
	/// <summary>
	/// Unitof work.
	/// </summary>
	public interface IUnitofWork : IDisposable
	{
		/// <summary>
		/// 取得資料庫物件執行個體。
		/// </summary>
		/// <returns>The database object.</returns>
		/// <typeparam name="TDb">The 1st type parameter.</typeparam>
		TDb GetDatabaseObject<TDb>() where TDb : class;
		/// <summary>
		/// Gets the entity.
		/// </summary>
		/// <returns>The entity.</returns>
		/// <typeparam name="TDbSet">The 1st type parameter.</typeparam>
		TDbSet GetEntity<TDbSet>() where TDbSet : class;
		/// <summary>
		/// Gets the repository.
		/// </summary>
		/// <returns>The repository.</returns>
		/// <typeparam name="TEntity">The 1st type parameter.</typeparam>
		TRepository GetRepository<TRepository>() where TRepository : class;
		/// <summary>
		/// Begins the transcation.
		/// </summary>
		/// <returns>The transcation.</returns>
		void BeginTranscation();
		/// <summary>
		/// Saves the changes.
		/// </summary>
		/// <returns>The changes.</returns>
		void SaveChanges();
		/// <summary>
		/// Commits the transcation.
		/// </summary>
		/// <returns>The transcation.</returns>
		void CommitTranscation();
		/// <summary>
		/// Opens the database.
		/// </summary>
		/// <returns>The database.</returns>
		void OpenDatabase();
		/// <summary>
		/// Closes the database.
		/// </summary>
		/// <returns>The database.</returns>
		void CloseDatabase();
	}
}

