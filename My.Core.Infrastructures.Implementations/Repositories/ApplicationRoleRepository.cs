using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using My.Core.Infrastructures.DAL;
using My.Core.Infrastructures.Logs;
using My.Core.Infrastructures.Implementations.Models;

namespace My.Core.Infrastructures.Implementations
{
	public class ApplicationRoleRepository:IApplicationRoleRepository<ApplicationRole>
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

		public IList<ApplicationRole> BatchCreate(IEnumerable<ApplicationRole> entities)
		{
			throw new NotImplementedException();
		}

		public ApplicationRole Create(ApplicationRole entity)
		{
			throw new NotImplementedException();
		}

		public void Delete(ApplicationRole entity)
		{
			throw new NotImplementedException();
		}

		public ApplicationRole Find(params object[] values)
		{
			throw new NotImplementedException();
		}

		public IQueryable<ApplicationRole> FindAll()
		{
			throw new NotImplementedException();
		}

		public ApplicationRole FindById(int RoleId)
		{
			throw new NotImplementedException();
		}

		public ApplicationRole FindByName(string roleName)
		{
			throw new NotImplementedException();
		}

		public IList<ApplicationRole> FindByUser(int MemberId)
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

		public IList<ApplicationRole> ToList(IQueryable<ApplicationRole> source)
		{
			throw new NotImplementedException();
		}

		public ApplicationRole Update(ApplicationRole entity)
		{
			throw new NotImplementedException();
		}
	}
}

