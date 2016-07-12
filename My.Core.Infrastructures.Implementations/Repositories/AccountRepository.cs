using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using My.Core.Infrastructures.DAL;
using My.Core.Infrastructures.Datas;
using My.Core.Infrastructures.Logs;

namespace My.Core.Infrastructures.Implementations
{
	public class AccountRepository : IAccountRepository
	{
		private IUnitofWork _unitofwork;

		private ApplicationDbContext _database;

		public AccountRepository(IUnitofWork unitofwork)
		{
			_unitofwork = unitofwork;
			_unitofwork.GetDatabaseObject<ApplicationDbContext>();

		}

		#region Helper Functions
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

		/// <summary>
		/// Writes the user operation log.
		/// </summary>
		/// <returns>The user operation log.</returns>
		/// <param name="code">Code.</param>
		/// <param name="User">User.</param>
		protected virtual void WriteUserOperationLog(OperationCodeEnum code, IAccount User)
		{
			try
			{
				IRepositoryBase<UserOperationLog> useroperationlog = _unitofwork.GetRepository<UserOperationLog>();

				string _url = string.Empty;

				if (System.Web.HttpContext.Current != null)
				{
					_url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
				}

				if (User == null)
				{
					useroperationlog.Create(new UserOperationLog()
					{
						Body = string.Empty,
						UserId = GetCurrentLoginedUserId(),
						LogTime = DateTime.Now,
						OpreationCode = (int)code,
						URL = _url
					});

					useroperationlog.SaveChanges();
				}
				else {
					if (User.Id == -1)
					{
						useroperationlog.Create(new UserOperationLog()
						{
							Body = string.Format("{0};{1};{2}", User.UserName, User.Password, User.PasswordHash),
							UserId = GetCurrentLoginedUserId(),
							LogTime = DateTime.Now,
							OpreationCode = (int)code,
							URL = _url
						});

					}
					else {
						useroperationlog.Create(new UserOperationLog()
						{
							Body = string.Empty,
							UserId = -1,
							LogTime = DateTime.Now,
							OpreationCode = (int)code,
							URL = _url
						});
					}
					useroperationlog.SaveChanges();
				}

			}
			catch (Exception ex)
			{
				WriteErrorLog(ex);
			}

		}

		protected virtual int GetCurrentLoginedUserId()
		{
			var founduser= FindUserByLoginAccount(System.Web.HttpContext.Current.User.Identity.Name, true);

			if (founduser != null)
			{
				return founduser.Id;
			}

			return -1;
		}
		#endregion

		#region Logger 記錄器
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

		public IList<IAccount> BatchCreate(IEnumerable<IAccount> entities)
		{
			try
			{
				List<IAccount> _insertednewuserlist = new List<IAccount>();

				_unitofwork.OpenDatabase();

				_database = GetDatabase();

				_unitofwork.BeginTranscation();

				foreach (IAccount newuser in entities)
				{
					try
					{
						ApplicationUser inserteduser = _unitofwork.GetEntity<IDbSet<ApplicationUser>>().Add((ApplicationUser)newuser);
						WriteUserOperationLog(OperationCodeEnum.Account_BatchCreate_Start, inserteduser);
						SaveChanges();
						WriteUserOperationLog(OperationCodeEnum.Account_BatchCreate_End_Success, inserteduser);
						_insertednewuserlist.Add(inserteduser);
					}
					catch (Exception ex)
					{
						WriteErrorLog(ex);
						WriteUserOperationLog(OperationCodeEnum.Account_BatchCreate_End_Fail, newuser);
					}

				}

				_unitofwork.CommitTranscation();

				return _insertednewuserlist;
			}
			catch (Exception ex)
			{
				WriteErrorLog(ex);
				WriteUserOperationLog(OperationCodeEnum.Account_BatchCreate_Rollback, new ApplicationUser() { Id = -1 });
				throw ex;
			}
			finally
			{
				_unitofwork.CloseDatabase();
			}
		}

		public IAccount ChangePassword(IAccount UpdatedUserData)
		{
			try
			{
				WriteUserOperationLog(OperationCodeEnum.Account_ChangePassword_Start, UpdatedUserData);

				_unitofwork.OpenDatabase();
				_database = GetDatabase();

				IAccount _fetchuser = FindUserById(UpdatedUserData.Id, true);

				if (_fetchuser != null)
				{
					_fetchuser.Password = UpdatedUserData.Password;
					_fetchuser.PasswordHash = UpdatedUserData.PasswordHash;
					SaveChanges();
					WriteUserOperationLog(OperationCodeEnum.Account_ChangePassword_End_Success, UpdatedUserData);
				}
				else {
					WriteUserOperationLog(OperationCodeEnum.Account_ChangePassword_End_Fail, UpdatedUserData);
				}

				return _fetchuser;
			}
			catch (Exception ex)
			{
				WriteErrorLog(ex);
				WriteUserOperationLog(OperationCodeEnum.Account_ChangePassword_End_Fail, UpdatedUserData);
				throw ex;
			}
			finally
			{
				_unitofwork.CloseDatabase();
			}
		}

		public IAccount Create(IAccount entity)
		{
			try
			{

				WriteUserOperationLog(OperationCodeEnum.Account_Create_Start, entity);
				ApplicationUser inserteduser = _database.Users.Add((ApplicationUser)entity);
				SaveChanges();
				WriteUserOperationLog(OperationCodeEnum.Account_Create_End_Success, inserteduser);
				return inserteduser;
			}
			catch (Exception ex)
			{
				WriteErrorLog(ex);
				WriteUserOperationLog(OperationCodeEnum.Account_Create_End_Fail, entity);
				WriteUserOperationLog(OperationCodeEnum.Account_Create_Rollback, entity);
				throw ex;
			}
			finally
			{
				_unitofwork.CloseDatabase();
			}
		}

		public void Delete(IAccount entity)
		{
			throw new NotImplementedException();
		}

		public IAccount Find(Expression<Func<IAccount, bool>> predicate)
		{
			throw new NotImplementedException();
		}

		public IQueryable<IAccount> FindAll()
		{
			throw new NotImplementedException();
		}

		public IList<IAccount> FindAllAccounts()
		{
			throw new NotImplementedException();
		}

		public IAccount FindByEmail(string email)
		{
			throw new NotImplementedException();
		}

		public IAccount FindUserById(int MemberId, bool isOnline)
		{
			throw new NotImplementedException();
		}

		public IAccount FindUserByLoginAccount(string LoginAccount, bool IsOnline)
		{
			throw new NotImplementedException();
		}

		public int FindUserIdFromPasswordResetToken(string Token)
		{
			throw new NotImplementedException();
		}

		public bool IsConfirmed(int MemberId)
		{
			throw new NotImplementedException();
		}

		public int ResetPasswordWithToken(string Token, string newPassword)
		{
			throw new NotImplementedException();
		}

		public void SaveChanges()
		{
			throw new NotImplementedException();
		}

		public IList<IAccount> ToList(IQueryable<IAccount> source)
		{
			throw new NotImplementedException();
		}

		public IAccount Update(IAccount entity)
		{
			throw new NotImplementedException();
		}

		public bool ValidateUser(IAccount UserDataToValidated)
		{
			throw new NotImplementedException();
		}
	}
}

