using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using My.Core.Infrastructures.Datas;
using My.Core.Infrastructures.Implementations.Datas;
using My.Core.Infrastructures.Logs;

namespace My.Core.Infrastructures.Implementations.Repositories
{
	public class ApplicationRoleRepository : IApplicationRoleRepository
	{
		private IUnitofWork _unitofwork;

		private DbSet<ApplicationRole> _database;

		private IAccountRepository accountrepository;

		private bool isbatchmode;

		public ApplicationRoleRepository(IUnitofWork unitofwork)
		{
			_unitofwork = unitofwork;
			_database = _unitofwork.GetEntity<DbSet<ApplicationRole>>();
			accountrepository = _unitofwork.GetRepository<AccountRepository>();	// 可以用Autofac的DI容器框架取代
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
		protected virtual DbSet<ApplicationRole> GetDatabase()
		{
			return _unitofwork.GetEntity<DbSet<ApplicationRole>>();
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

		public void AddUserToRole(int RoleId, int MemberId)
		{
			throw new NotImplementedException();
		}

		public IList<IApplicationRole> BatchCreate(IEnumerable<IApplicationRole> entities)
		{
			try
			{
				List<IApplicationRole> resultset = new List<IApplicationRole>();

				_unitofwork.OpenDatabase();
				_database = GetDatabase();

				isbatchmode = true;

				foreach (var entity in entities)
				{
					try
					{
						resultset.Add(Create(entity));
					}
					catch (Exception ex)
					{
						WriteErrorLog(ex);
					}
				}

				isbatchmode = false;
				SaveChanges();
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

		public IApplicationRole Create(IApplicationRole entity)
		{
			try
			{
				_unitofwork.OpenDatabase();
				_database = GetDatabase();
				entity = _database.Add(entity as ApplicationRole);
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

		public void Delete(IApplicationRole entity)
		{
			try
			{
				_unitofwork.OpenDatabase();
				_database = GetDatabase();
				_database.Remove(entity as ApplicationRole);
				isbatchmode = false;
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

		public IApplicationRole Find(Expression<Func<IApplicationRole, bool>> predicate)
		{
			try
			{
				return FindAll().Where(predicate).SingleOrDefault();
			}
			catch (Exception ex)
			{
				WriteErrorLog(ex);
				return default(ApplicationRole);
			}
			finally
			{
				_unitofwork.CloseDatabase();
			}
		}

		public IQueryable<IApplicationRole> FindAll()
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
				return null;
			}
			finally
			{
				_unitofwork.CloseDatabase();
			}
		}

		public IApplicationRole FindById(int RoleId)
		{
			try
			{
				IApplicationRole _foundrole = (from roles in FindAll()
											   where roles.RoleId == RoleId
											   select roles).SingleOrDefault();

				return _foundrole;
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

		public IApplicationRole FindByName(string roleName)
		{
			try
			{
				IApplicationRole _foundrole = (from roles in FindAll()
											   where roles.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)
											   select roles).SingleOrDefault();

				return _foundrole;
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

		public IList<IApplicationRole> FindByUser(int MemberId)
		{
			try
			{
				IQueryable<IApplicationRole> _foundroles = (from roles in _database
															from users in roles.Users
															where users.MemberId == MemberId
															select roles);

				return ToList(_foundroles);
			}
			catch (Exception ex)
			{
				WriteErrorLog(ex);
				return new List<IApplicationRole>();
			}
			finally
			{
				_unitofwork.CloseDatabase();
			}
		}

		public bool IsInRole(int MemberId, string roleName)
		{
			try
			{
				IQueryable<IApplicationRole> _foundroles = (from roles in _database
															from users in roles.Users
															where users.MemberId == MemberId
															&& roles.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)
															select roles);

				return _foundroles.Any();
			}
			catch (Exception ex)
			{
				WriteErrorLog(ex);
				return false;
			}
		}

		public void RemoveUserFromRole(int RoleId, int MemberId)
		{
			try
			{
				_unitofwork.OpenDatabase();
				_database = GetDatabase();
				ApplicationRole _foundroles = (from roles in _database
											   from users in roles.Users
											   where users.MemberId == MemberId
											   && roles.RoleId == RoleId
											   select roles).SingleOrDefault();

				if (_foundroles != null)
				{
					var founduser = _foundroles.Users.First();
					_foundroles.Users.Remove(founduser);
					isbatchmode = false;
					SaveChanges();
				}
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

		public void SaveChanges()
		{
			try
			{
				if (isbatchmode == false)
				{
					_unitofwork.SaveChanges();
				}

			}
			catch (Exception ex)
			{
				WriteErrorLog(ex);
				throw ex;
			}
		}

		public IList<IApplicationRole> ToList(IQueryable<IApplicationRole> source)
		{
			try
			{
				return FindAll().ToList();
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

		public IApplicationRole Update(IApplicationRole entity)
		{
			try
			{
				ApplicationRole _foundrole = FindById(entity.RoleId) as ApplicationRole;

				if (_foundrole != null)
				{
					_foundrole.Name = entity.Name;

					Collection<ApplicationUser> addusers
					= new Collection<ApplicationUser>(
						((ApplicationRole)entity).Users.Except(_foundrole.Users).ToList());

					Collection<ApplicationUser> removeusers
					= new Collection<ApplicationUser>(
						_foundrole.Users.Except(((ApplicationRole)entity).Users).ToList()
					);

					foreach (var removeuser in removeusers)
					{
						_foundrole.Users.Remove(removeuser);
					}

					foreach (var adduser in addusers)
					{
						_foundrole.Users.Add(adduser);
					}

					isbatchmode = false;
					SaveChanges();

					return FindById(entity.RoleId);
				}
				return entity;
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
	}
}

