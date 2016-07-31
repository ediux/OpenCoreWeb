using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using My.Core.Infrastructures.Datas;
using My.Core.Infrastructures.Implementations.Datas;
using My.Core.Infrastructures.Logs;

namespace My.Core.Infrastructures.Implementations.Repositories
{
	public class UserOperationCodeDefineRepository : IUserOperationCodeDefineRepository
	{
		private IUnitofWork _unitofwork;
		private ApplicationDbContext _database;

		public UserOperationCodeDefineRepository(IUnitofWork unitofwork)
		{
			_unitofwork = unitofwork;

		}

		private ILogWriter _logger;

		/// <summary>
		/// Gets or sets the logger.
		/// </summary>
		/// <value>The logger.</value>
		public ILogWriter Logger
		{
			get
			{
				return _logger;
			}

			set
			{
				_logger = value;
			}
		}

		/// <summary>
		/// Resets the database object.
		/// </summary>
		/// <returns>The database object.</returns>
		protected virtual ApplicationDbContext GetDatabase()
		{
			return _unitofwork.GetDatabaseObject<ApplicationDbContext>();
		}

		/// <summary>
		/// Writes the error log.
		/// </summary>
		/// <returns>The error log.</returns>
		/// <param name="ex">Ex.</param>
		protected virtual void WriteErrorLog(Exception ex)
		{
			if (_logger != null)
			{
				_logger.ErrorFormat("{0},{1}", ex.Message, ex.StackTrace);
			}
		}


		public IList<IUserOperationCodeDefine> BatchCreate(IEnumerable<IUserOperationCodeDefine> entities)
		{
			try
			{
				_unitofwork.OpenDatabase();
				_database = GetDatabase();

			}
			catch (Exception ex)
			{
				WriteErrorLog(ex);
			}
			finally
			{
				_unitofwork.CloseDatabase();
			}
		}

		public IUserOperationCodeDefine Create(IUserOperationCodeDefine entity)
		{
			try
			{
				_unitofwork.OpenDatabase();
				_database = GetDatabase();
				_unitofwork.BeginTranscation();
				_database.OperationCodeDefines.Add((UserOperationCodeDefine)entity);
				_unitofwork.SaveChanges();
				_unitofwork.CommitTranscation();
			}
			catch (Exception ex)
			{
				WriteErrorLog(ex);
			}
			finally
			{
				_unitofwork.CloseDatabase();
			}
		}

		public void Delete(IUserOperationCodeDefine entity)
		{
			throw new NotImplementedException();
		}

		public IUserOperationCodeDefine Find(Expression<Func<IUserOperationCodeDefine, bool>> predicate)
		{
			throw new NotImplementedException();
		}

		public IQueryable<IUserOperationCodeDefine> FindAll()
		{
			throw new NotImplementedException();
		}

		public IUserOperationCodeDefine FindByCode(int code)
		{
			throw new NotImplementedException();
		}

		public void SaveChanges()
		{
			throw new NotImplementedException();
		}

		public IList<IUserOperationCodeDefine> ToList(IQueryable<IUserOperationCodeDefine> source)
		{
			throw new NotImplementedException();
		}

		public IUserOperationCodeDefine Update(IUserOperationCodeDefine entity)
		{
			throw new NotImplementedException();
		}
	}
}

