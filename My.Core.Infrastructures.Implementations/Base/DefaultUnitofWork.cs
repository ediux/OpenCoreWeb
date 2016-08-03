using System;
using System.Collections;
using System.Data.Entity;
using My.Core.Infrastructures.Implementations.Repositories;

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
			//for LINQtoSQL & EF 不需要實作交易因為已經隱含包含了
		}

		public void CloseDatabase()
		{
			_database.Dispose();
			_database = null;
		}

		public void CommitTranscation()
		{
			
		}

		public TDb GetDatabaseObject<TDb>() where TDb : class
		{
			if (typeof(TDb).BaseType == typeof(DbContext))
			{
				return _database as TDb;
			}
			else {
				return null;
			}
		}

		public TDbSet GetEntity<TDbSet>() where TDbSet : class
		{
			if (typeof(TDbSet) != typeof(DbSet<>))
			{
				throw new Exception("Type is not DbSet<>.");
			}

			OpenDatabase();

			return _database.Set<TDbSet>() as TDbSet;
		}

		public TRepository GetRepository<TRepository>() where TRepository : class
		{
			if (_repositories == null)
			{
				_repositories = new Hashtable();
			}


			var type = typeof(TRepository).Name;


			if (!_repositories.ContainsKey(type))
			{
				if (typeof(TRepository) is IAccountRepository)
				{
					var repositoryInstance = new AccountRepository(this);
					_repositories.Add(type, repositoryInstance);
					return repositoryInstance as TRepository;
				}

				if (typeof(TRepository) is IApplicationRoleRepository)
				{
					var repositoryInstance = new ApplicationRoleRepository(this);
					_repositories.Add(type, repositoryInstance);
					return repositoryInstance as TRepository;
				}


				if (typeof(TRepository) is IGroupRepository)
				{
					var repositoryInstance = new ApplicationGroupRepository(this);
					_repositories.Add(type, repositoryInstance);
					return repositoryInstance as TRepository;
				}

				if (typeof(TRepository) is IUserProfileRepository)
				{
					var repositoryInstance = new ApplicationUserProfileRepository(this);
					_repositories.Add(type, repositoryInstance);
					return repositoryInstance as TRepository;
				}

				if (typeof(TRepository) is IUserOperationCodeDefineRepository)
				{
					var repositoryInstance = new UserOperationCodeDefineRepository(this);
					_repositories.Add(type, repositoryInstance);
					return repositoryInstance as TRepository;
				}

				if (typeof(TRepository) is IUserOperationLogRepository)
				{
					var repositoryInstance = new UserOperationLogRepository(this);
					_repositories.Add(type, repositoryInstance);
					return repositoryInstance as TRepository;
				}
			}

			return (TRepository)_repositories[type];
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
				_database.SaveChanges();
			}
			catch
			{
				throw;
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

