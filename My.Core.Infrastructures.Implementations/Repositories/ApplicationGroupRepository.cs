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
	public class ApplicationGroupRepository : IGroupRepository
	{
		private IUnitofWork _unitofwork;

		private DbSet<ApplicationUserGroup> _database;

		private IAccountRepository accountrepository;

		private bool isbatchmode;

		public ApplicationGroupRepository(IUnitofWork unitofwork)
		{
			_unitofwork = unitofwork;
			_database = unitofwork.GetEntity<DbSet<ApplicationUserGroup>>();
			accountrepository = unitofwork.GetRepository<AccountRepository>();
			isbatchmode = false;
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
		protected virtual DbSet<ApplicationUserGroup> GetDatabase()
		{
			return _unitofwork.GetEntity<DbSet<ApplicationUserGroup>>();
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
				ApplicationUser founduser = accountrepository.FindUserByLoginAccount(username, false) as ApplicationUser;
				IGroup foundgroup = (from g in FindAll()
									 where g.Name == groupname
									 select g).Single();
				ApplicationUserGroup convertentity = (ApplicationUserGroup)foundgroup;
				convertentity.Users.Add(founduser);
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

		public IList<IGroup> BatchCreate(IEnumerable<IGroup> entities)
		{
			try
			{
				List<IGroup> resultset = new List<IGroup>();
				_unitofwork.OpenDatabase();
				_unitofwork.BeginTranscation();
				isbatchmode = true;
				foreach (IGroup entity in entities)
				{
					resultset.Add(Create(entity));
				}
				isbatchmode = false;
				SaveChanges();
				_unitofwork.CommitTranscation();
				return resultset;
			}
			catch (Exception ex)
			{
				WriteErrorLog(ex);
				return entities.ToList();
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
				IGroup newgroup = _database.Add(entity as ApplicationUserGroup);
				SaveChanges();
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

				_unitofwork.BeginTranscation();

				_database.Remove((ApplicationUserGroup)grouptoremove);

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
			try
			{
				_unitofwork.OpenDatabase();
				_database = GetDatabase();
				return _database.AsQueryable();
			}
			catch (Exception ex)
			{
				WriteErrorLog(ex);
				throw ex;
			}
			finally
			{
				_unitofwork.CloseDatabase();
			}
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
			if (isbatchmode == false)
			{
				_unitofwork.SaveChanges();
			}
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

