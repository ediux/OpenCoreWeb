using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using My.Core.Infrastructures.DAL;
using My.Core.Infrastructures.Logs;

namespace My.Core.Infrastructures.Implementations
{
	public class ApplicationRoleRepository:IApplicationRoleRepository
	{
		public ApplicationRoleRepository()
		{
		}

		public ILogWriter Logger
		{
			get
			{
				throw new NotImplementedException();
			}

			set
			{
				throw new NotImplementedException();
			}
		}

		public void AddUserToRole(int RoleId, int MemberId)
		{
			throw new NotImplementedException();
		}

		public IList<IApplicationRole> BatchCreate(IEnumerable<IApplicationRole> entities)
		{
			throw new NotImplementedException();
		}

		public IApplicationRole Create(IApplicationRole entity)
		{
			throw new NotImplementedException();
		}

		public void Delete(IApplicationRole entity)
		{
			throw new NotImplementedException();
		}

		public IApplicationRole Find(Expression<Func<IApplicationRole, bool>> predicate)
		{
			throw new NotImplementedException();
		}

		public IQueryable<IApplicationRole> FindAll()
		{
			throw new NotImplementedException();
		}

		public IApplicationRole FindById(int RoleId)
		{
			throw new NotImplementedException();
		}

		public IApplicationRole FindByName(string roleName)
		{
			throw new NotImplementedException();
		}

		public IList<IApplicationRole> FindByUser(int MemberId)
		{
			throw new NotImplementedException();
		}

		public bool IsInRole(int MemberId, string roleName)
		{
			throw new NotImplementedException();
		}

		public void RemoveUserFromRole(int RoleId, int MemberId)
		{
			throw new NotImplementedException();
		}

		public void SaveChanges()
		{
			throw new NotImplementedException();
		}

		public IList<IApplicationRole> ToList(IQueryable<IApplicationRole> source)
		{
			throw new NotImplementedException();
		}

		public IApplicationRole Update(IApplicationRole entity)
		{
			throw new NotImplementedException();
		}
	}
}

