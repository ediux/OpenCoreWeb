using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using My.Core.Infrastructures.Datas;
using My.Core.Infrastructures.Implementations.Datas;
using My.Core.Infrastructures.Logs;

namespace My.Core.Infrastructures.Implementations.Repositories
{
	public class ApplicationUserProfileRepository : IUserProfileRepository
	{
		private IUnitofWork _unitofwork;
		private DbSet<ApplicationUserProfile> _database;

		public ApplicationUserProfileRepository(IUnitofWork unitofwork)
		{
			_unitofwork = unitofwork;
			_database = _unitofwork.GetEntity<DbSet<ApplicationUserProfile>>();
		}

		private ILogWriter _logger;

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
		protected virtual DbSet<ApplicationUserProfile> GetDatabase()
		{
			return _unitofwork.GetEntity<DbSet<ApplicationUserProfile>>();
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

		public IUserProfile AddCustomField(string FieldName)
		{
			throw new NotImplementedException();
		}

		public IList<IUserProfile> BatchCreate(IEnumerable<IUserProfile> entities)
		{
			throw new NotImplementedException();
		}

		public IUserProfile ChangeFieldName(string oldName, string newName)
		{
			throw new NotImplementedException();
		}

		public IUserProfile Create(IUserProfile entity)
		{
			throw new NotImplementedException();
		}

		public void Delete(IUserProfile entity)
		{
			throw new NotImplementedException();
		}

		public IUserProfile Find(Expression<Func<IUserProfile, bool>> predicate)
		{
			throw new NotImplementedException();
		}

		public IQueryable<IUserProfile> FindAll()
		{
			try
			{
				return _database.AsQueryable();
			}
			catch (Exception ex)
			{
				WriteErrorLog(ex);
				return null;
			}
		}

		public int FindUserIdByEmail(string email)
		{
			throw new NotImplementedException();
		}

		public int FindUserIdByPhone(string PhoneNumber)
		{
			throw new NotImplementedException();
		}

		public string GetCustomFieldValue(string FieldName)
		{
			throw new NotImplementedException();
		}

		public int GetEmptyCustomFieldCounts()
		{
			throw new NotImplementedException();
		}

		public bool IsEmailConfirmedByUserId(int UserId)
		{
			throw new NotImplementedException();
		}

		public bool IsPhoneConfirmeByUserId(int UserId)
		{
			throw new NotImplementedException();
		}

		public void RemoveCustomField(string FieldName)
		{
			throw new NotImplementedException();
		}

		public void SaveChanges()
		{
			_unitofwork.SaveChanges();
		}

		public void SetCustomFieldValue(string FieldName)
		{
			throw new NotImplementedException();
		}

		public IList<IUserProfile> ToList(IQueryable<IUserProfile> source)
		{
			throw new NotImplementedException();
		}

		public IUserProfile Update(IUserProfile entity)
		{
			throw new NotImplementedException();
		}
	}
}

