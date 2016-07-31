using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using My.Core.Infrastructures.Datas;
using My.Core.Infrastructures.Logs;

namespace My.Core.Infrastructures.Implementations.Repositories
{
	public class ApplicationUserProfileRepository : IUserProfileRepository
	{
		private IUnitofWork _unitofwork;
		private ApplicationDbContext _database;

		public ApplicationUserProfileRepository(IUnitofWork unitofwork)
		{
			_unitofwork = unitofwork;
			_database = _unitofwork.GetDatabaseObject<ApplicationDbContext>();
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
			throw new NotImplementedException();
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
			throw new NotImplementedException();
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

