using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using My.Core.Infrastructures.Datas;
using My.Core.Infrastructures.Implementations.Datas;
using My.Core.Infrastructures.Logs;

namespace My.Core.Infrastructures.Implementations.Repositories
{
	public class UserOperationLogRepository : IUserOperationLogRepository
	{
		private IUnitofWork _unitofwork;
		private ApplicationDbContext _database;

		public UserOperationLogRepository(IUnitofWork unitofwork)
		{
			_unitofwork = unitofwork;
		}

		#region Log writer
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
		#endregion


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

		public IList<IUserOperationLog> BatchCreate(IEnumerable<IUserOperationLog> entities)
		{
			throw new NotImplementedException();
		}

		public IUserOperationLog Create(IUserOperationLog entity)
		{
			try
			{

				IUserOperationCodeDefineRepository useropreationrepository =
					_unitofwork.GetRepository<IUserOperationCodeDefine>() as IUserOperationCodeDefineRepository;

				IUserOperationCodeDefine operationcodeobject = useropreationrepository.FindByCode(entity.OpreationCode);

				if (operationcodeobject == null)
				{
					_unitofwork.BeginTranscation();

					operationcodeobject = useropreationrepository.Create(new UserOperationCodeDefine()
					{
						OpreationCode = entity.OpreationCode,
						Description = entity.OpreationCode.ToString()
					});

					_unitofwork.CommitTranscation();
				}

				_unitofwork.OpenDatabase();

				_database = GetDatabase();

				UserOperationLog newlogdata = entity as UserOperationLog;

				_database.UserOperationLogs.Add(newlogdata);

				SaveChanges();

				return entity;
			}
			catch (Exception ex)
			{
				WriteErrorLog(ex);
				return entity;
			}
			finally
			{
				_unitofwork.CloseDatabase();
			}
		}

		public void Delete(IUserOperationLog entity)
		{
			try
			{
				_unitofwork.OpenDatabase();
				_database = GetDatabase();
				UserOperationLog removelogdata = entity as UserOperationLog;
				_database.UserOperationLogs.Remove(removelogdata);
				SaveChanges();
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

		public IUserOperationLog Find(Expression<Func<IUserOperationLog, bool>> predicate)
		{
			try
			{
				return FindAll().Where(predicate).SingleOrDefault();
			}
			catch (Exception ex)
			{
				WriteErrorLog(ex);
				return null;
			}
		}

		public IQueryable<IUserOperationLog> FindAll()
		{
			try
			{
				_unitofwork.OpenDatabase();
				_database = GetDatabase();
				return (from logs in  _database.UserOperationLogs
				        select logs);
			}
			catch (Exception ex)
			{
				WriteErrorLog(ex);
				return null;
			}
		}

		public void SaveChanges()
		{
			_database.SaveChanges();
		}

		public IList<IUserOperationLog> ToList(IQueryable<IUserOperationLog> source)
		{
			return source.ToList();
		}

		public IUserOperationLog Update(IUserOperationLog entity)
		{
			throw new NotImplementedException();
		}
	}
}

