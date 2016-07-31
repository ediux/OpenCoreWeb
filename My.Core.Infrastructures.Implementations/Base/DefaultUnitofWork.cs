using System;
using System.Collections;
using System.Data.Entity;
using System.Reflection;

namespace My.Core.Infrastructures.Implementations
{
	public class DefaultUnitofWork : IUnitofWork
	{
		private ApplicationDbContext _database;
		private DbContextTransaction _transaction;
		private bool _usedtransaction;
		private Hashtable _repositories;

		public DefaultUnitofWork()
		{
			OpenDatabase();
			_usedtransaction = false;
		}

		public void BeginTranscation()
		{
			_transaction = _database.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
			_usedtransaction = true;
		}

		public void CloseDatabase()
		{
			_database.Dispose();
			_database = null;
		}

		public void CommitTranscation()
		{
			if (_usedtransaction)
			{
				if (_repositories.Count > 1)
				{
					foreach (Type key in _repositories.Keys)
					{
						try
						{
							object _repositoryvalue = _repositories[key];
							MethodInfo _method = key.GetMethod("SaveChanges");
							_method.Invoke(_repositoryvalue, new object[] { });
						}
						catch
						{
							_transaction.Rollback();
							continue;
						}

					}
				}
				_transaction.Commit();

				_usedtransaction = false;
			}
		}

		public TDb GetDatabaseObject<TDb>() where TDb : class
		{
			return _database as TDb;
		}

		public TDbSet GetEntity<TDbSet>() where TDbSet : class
		{
			OpenDatabase();

			return _database.Set<TDbSet>() as TDbSet;
		}

		public IRepositoryBase<TEntity> GetRepository<TEntity>() where TEntity : IDataModel
		{
			if (_repositories == null)
			{
				_repositories = new Hashtable();
			}

			var type = typeof(TEntity).Name;

			if (!_repositories.ContainsKey(type))
			{
				var repositoryType = typeof(IRepositoryBase<>);

				var repositoryInstance =
					Activator.CreateInstance(repositoryType
											 .MakeGenericType(typeof(TEntity)), _database);

				_repositories.Add(type, repositoryInstance);
			}

			return (IRepositoryBase<TEntity>)_repositories[type];
		}

		public void OpenDatabase()
		{
			if (_database == null)
			{
				_database = new ApplicationDbContext();
			}
		}

		public void SaveChanges()
		{
			try
			{
				if (_usedtransaction)
				{
					CommitTranscation();
				}
				else {
					_database.SaveChanges();
				}
			}
			catch
			{
				if (_usedtransaction)
				{
					_transaction.Rollback();
				}
			}

		}

		#region IDisposable Support
		private bool disposedValue = false; // To detect redundant calls

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: dispose managed state (managed objects).
					_database.Dispose();
				}

				// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
				// TODO: set large fields to null.

				disposedValue = true;
			}
		}

		// TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
		// ~DefaultUnitofWork() {
		//   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
		//   Dispose(false);
		// }

		// This code added to correctly implement the disposable pattern.
		public void Dispose()
		{
			// Do not change this code. Put cleanup code in Dispose(bool disposing) above.
			Dispose(true);
			// TODO: uncomment the following line if the finalizer is overridden above.
			// GC.SuppressFinalize(this);
		}
		#endregion


	}
}

