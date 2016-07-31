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

		public UserOperationLogRepository(IUnitofWork unitofwork)
		{
			_unitofwork = unitofwork;
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

		public IList<IUserOperationLog> BatchCreate(IEnumerable<IUserOperationLog> entities)
		{
			throw new NotImplementedException();
		}

		public IUserOperationLog Create(IUserOperationLog entity)
		{
			IUserOperationCodeDefineRepository useropreationrepository = 
				_unitofwork.GetRepository<IUserOperationCodeDefine>() as IUserOperationCodeDefineRepository ;
			
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

			return entity;
		}

		public void Delete(IUserOperationLog entity)
		{
			throw new NotImplementedException();
		}

		public IUserOperationLog Find(Expression<Func<IUserOperationLog, bool>> predicate)
		{
			throw new NotImplementedException();
		}

		public IQueryable<IUserOperationLog> FindAll()
		{
			throw new NotImplementedException();
		}

		public void SaveChanges()
		{
			throw new NotImplementedException();
		}

		public IList<IUserOperationLog> ToList(IQueryable<IUserOperationLog> source)
		{
			throw new NotImplementedException();
		}

		public IUserOperationLog Update(IUserOperationLog entity)
		{
			throw new NotImplementedException();
		}
	}
}

