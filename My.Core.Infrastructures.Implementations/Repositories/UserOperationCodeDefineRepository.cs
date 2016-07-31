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
		private bool _isBatchMode;
		public UserOperationCodeDefineRepository(IUnitofWork unitofwork)
		{
			_unitofwork = unitofwork;
			_database = null;
			_isBatchMode = false;
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
			List<IUserOperationCodeDefine> _result = new List<IUserOperationCodeDefine>();
			try
			{
				_unitofwork.OpenDatabase();
				_database = GetDatabase();
				_isBatchMode = true;
				_unitofwork.BeginTranscation();

				foreach (var entity in entities)
				{
					_result.Add(	Create(entity));
				}

				SaveChanges();

				return _result;
			}
			catch (Exception ex)
			{
				WriteErrorLog(ex);
				return _result;
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

				if (_isBatchMode == false)
				{
					_unitofwork.BeginTranscation();
				}

				entity = _database.OperationCodeDefines.Add((UserOperationCodeDefine)entity);

				if (_isBatchMode == false)
				{
					_unitofwork.SaveChanges();
				}

				return entity;
			}
			catch (Exception ex)
			{
				WriteErrorLog(ex);
				return entity;
			}
			finally
			{
				if (_isBatchMode == false)
					_unitofwork.CloseDatabase();
			}
		}

		public void Delete(IUserOperationCodeDefine entity)
		{
			try
			{
				_unitofwork.OpenDatabase();
				_database = GetDatabase();
				_unitofwork.BeginTranscation();
				_database.OperationCodeDefines.Remove(entity as UserOperationCodeDefine);
				_unitofwork.SaveChanges();
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

		public IUserOperationCodeDefine Find(Expression<Func<IUserOperationCodeDefine, bool>> predicate)
		{
			try
			{


				IUserOperationCodeDefine founddefined = FindAll().Where(predicate).SingleOrDefault();

				return founddefined;

			}
			catch (Exception ex)
			{
				WriteErrorLog(ex);
				return null;
			}
			finally
			{
				_unitofwork.CloseDatabase();
			}
		}

		public IQueryable<IUserOperationCodeDefine> FindAll()
		{
			try
			{
				_unitofwork.OpenDatabase();
				_database = GetDatabase();


				return (from opreatationcode in _database.OperationCodeDefines
						select opreatationcode);

			}
			catch (Exception ex)
			{
				WriteErrorLog(ex);
				return null;
			}

		}

		public IUserOperationCodeDefine FindByCode(int code)
		{
			try
			{
				IUserOperationCodeDefine founddefined = Find(w => w.OpreationCode == code);

				return founddefined;

			}
			catch (Exception ex)
			{
				WriteErrorLog(ex);
				return null;
			}
			finally
			{
				_unitofwork.CloseDatabase();
			}
		}

		public void SaveChanges()
		{
			_database.SaveChanges();
		}

		public IList<IUserOperationCodeDefine> ToList(IQueryable<IUserOperationCodeDefine> source)
		{
			return source.ToList();
		}

		public IUserOperationCodeDefine Update(IUserOperationCodeDefine entity)
		{
			try
			{
				IUserOperationCodeDefine founddefined = FindByCode(entity.OpreationCode);
				founddefined.Description = entity.Description;
				SaveChanges();
				founddefined = FindByCode(entity.OpreationCode);
				return founddefined;
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
	}
}

