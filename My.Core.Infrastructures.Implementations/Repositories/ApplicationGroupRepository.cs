using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using My.Core.Infrastructures.Datas;
using My.Core.Infrastructures.Implementations.Datas;
using My.Core.Infrastructures.Logs;

namespace My.Core.Infrastructures.Implementations.Repositories
{
	public class ApplicationGroupRepository : IGroupRepository
	{
		private IUnitofWork _unitofwork;

		private ApplicationDbContext _database;

		public ApplicationGroupRepository(IUnitofWork unitofwork)
		{
			_unitofwork = unitofwork;
			_database = unitofwork.GetDatabaseObject<ApplicationDbContext>();
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

		public void AddUserToGroup(string username, string groupname)
		{
			try
			{
				IAccountRepository accountrepository = _unitofwork.GetRepository<IAccount>() as IAccountRepository;
				ApplicationUser founduser = accountrepository.FindUserByLoginAccount(username, false) as ApplicationUser;
				IGroup foundgroup = (from g in FindAll()
									 where g.Name == groupname
									 select g).Single();
				ApplicationUserGroup convertentity = (ApplicationUserGroup)foundgroup;
				convertentity.Users.Add(founduser);
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

		public IList<IGroup> BatchCreate(IEnumerable<IGroup> entities)
		{
			try
			{
				List<IGroup> resultset = new List<IGroup>();
				_unitofwork.OpenDatabase();
				_unitofwork.BeginTranscation();
				foreach (IGroup entity in entities)
				{
					resultset.Add(Create(entity));
				}
				_unitofwork.CommitTranscation();
				return resultset;
			}
			catch (Exception ex)
			{
				WriteErrorLog(ex);

				_unitofwork.SaveChanges();

				return new List<IGroup>();
			}
			finally
			{
				_unitofwork.CloseDatabase();
			}
		}

		public IGroup Create(IGroup entity)
		{
			try
			{
				_unitofwork.OpenDatabase();
				_database = GetDatabase();
				_unitofwork.BeginTranscation();
				IGroup newgroup = _database.Groups.Add(entity as ApplicationUserGroup);
				_unitofwork.SaveChanges();
				_unitofwork.CommitTranscation();
				return newgroup;
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

		public void Delete(IGroup entity)
		{
			try
			{
				IGroup grouptoremove = FindAll().Where(w => w.Name == entity.Name).Single();

				_unitofwork.OpenDatabase();
				_database = GetDatabase();
				_unitofwork.BeginTranscation();

				_database.Groups.Remove(grouptoremove);

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

		public IGroup Find(Expression<Func<IGroup, bool>> predicate)
		{
			throw new NotImplementedException();
		}

		public IQueryable<IGroup> FindAll()
		{
			throw new NotImplementedException();
		}

		public List<IGroup> FindByUser(int id)
		{
			throw new NotImplementedException();
		}

		public void RemoveUserFromGroup(string username, string groupname, bool isCanUndo = true)
		{
			throw new NotImplementedException();
		}

		public void SaveChanges()
		{
			throw new NotImplementedException();
		}

		public IList<IGroup> ToList(IQueryable<IGroup> source)
		{
			throw new NotImplementedException();
		}

		public IGroup Update(IGroup entity)
		{
			throw new NotImplementedException();
		}
	}
}

