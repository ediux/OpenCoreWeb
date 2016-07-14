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
			var founduser = FindUserByLoginAccount(System.Web.HttpContext.Current.User.Identity.Name, true);

			if (founduser != null)
			{
				return founduser.Id;
			}

			return -1;
		}

		protected virtual bool GetIsOnline(int memberid)
		{
			try
			{
				IRepositoryBase<IUserOperationLog> operationlog = _unitofwork.GetRepository<IUserOperationLog>();
				IUserOperationLog _logdata = operationlog
					.FindAll()
					.Where(w => w.UserId == memberid && w.OpreationCode == (int)OperationCodeEnum.Account_Update_End_Success)
					.OrderByDescending(o => o.LogTime)
					.FirstOrDefault();
				return (_logdata.LogTime <= DateTime.Now);
			}
			catch (Exception ex)
			{
				WriteErrorLog(ex);
				return false;
			}
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

				IAccount _fetchuser = FindUserById(UpdatedUserData.Id, true);

				if (_fetchuser != null)
				{
					Update(UpdatedUserData);
					WriteUserOperationLog(OperationCodeEnum.Account_ChangePassword_End_Success, UpdatedUserData);
				}
				else
				{
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
			try
			{

				WriteUserOperationLog(OperationCodeEnum.Account_Delete_Start, entity);
				_unitofwork.OpenDatabase();
				_database = GetDatabase();
				_unitofwork.BeginTranscation();
				IAccount founduser = FindUserById(entity.Id, true);
				_database.Users.Remove((ApplicationUser)founduser);
				SaveChanges();
				_unitofwork.CommitTranscation();
				WriteUserOperationLog(OperationCodeEnum.Account_Delete_End_Success, entity);

			}
			catch (Exception ex)
			{
				WriteErrorLog(ex);
				WriteUserOperationLog(OperationCodeEnum.Account_Delete_End_Fail, entity);
				WriteUserOperationLog(OperationCodeEnum.Account_Delete_Rollback, entity);
				throw ex;
			}
			finally
			{
				_unitofwork.CloseDatabase();
			}
		}

		public IAccount Find(Expression<Func<IAccount, bool>> predicate)
		{
			int _memberid = GetCurrentLoginedUserId();
			IAccount _currentexecutinguser = FindUserById(_memberid, GetIsOnline(_memberid));

			try
			{
				WriteUserOperationLog(OperationCodeEnum.Account_Find_Start, _currentexecutinguser);

				IAccount founduser = FindAll().Where(predicate).SingleOrDefault();

				WriteUserOperationLog(OperationCodeEnum.Account_Find_End_Success, _currentexecutinguser);

				return founduser;
			}
			catch (Exception ex)
			{
				WriteErrorLog(ex);
				WriteUserOperationLog(OperationCodeEnum.Accpimt_Find_End_Fail, _currentexecutinguser);
				throw ex;
			}
			finally
			{
				_unitofwork.CloseDatabase();
			}
		}

		public IQueryable<IAccount> FindAll()
		{
			try
			{
				_unitofwork.OpenDatabase();
				_database = _unitofwork.GetDatabaseObject<ApplicationDbContext>();
				return _database.Users.AsQueryable();
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

		public IList<IAccount> FindAllAccounts()
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

		public IAccount FindByEmail(string email)
		{
			try
			{
				int _currentexecutionuserid = GetCurrentLoginedUserId();
				IAccount _currentexecutionuser = FindUserById(_currentexecutionuserid, 
				                                              GetIsOnline(_currentexecutionuserid));
				WriteUserOperationLog(OperationCodeEnum.Account_FindByEmail_Start,
									  _currentexecutionuser);

				IRepositoryBase<IUserProfile> _userprofile = _unitofwork.GetRepository<IUserProfile>();

				IUserProfile _userprofileitem = _userprofile.FindAll().Where(w => w.Email == email).SingleOrDefault();
				WriteUserOperationLog(OperationCodeEnum.Account_FindByEmail_End_Success, _currentexecutionuser);

				return FindUserById(_userprofileitem.Id, GetIsOnline(_userprofileitem.Id));
			}
			catch (Exception ex)
			{
				WriteErrorLog(ex);
				WriteUserOperationLog(OperationCodeEnum.Account_FindByEmail_End_Fail, null);
				throw ex;
			}
			finally
			{
				_unitofwork.CloseDatabase();
			}
		}

		public IAccount FindUserById(int MemberId, bool isOnline)
		{
			try
			{
				
				IAccount _founduser = FindAll().Where(w => w.Id == MemberId).SingleOrDefault();
				WriteUserOperationLog(OperationCodeEnum.Account_FindById_Start,_founduser);
				if (isOnline)
				{
					WriteUserOperationLog(OperationCodeEnum.Account_FLAG_Online, _founduser);
					WriteUserOperationLog(OperationCodeEnum.Account_Update_End_Success, _founduser);
				}
				else
				{
					WriteUserOperationLog(OperationCodeEnum.Account_FLAG_Offline, _founduser);
				}
				WriteUserOperationLog(OperationCodeEnum.Account_FindById_End_Success, _founduser);
				return _founduser;
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

		public IAccount FindUserByLoginAccount(string LoginAccount, bool IsOnline)
		{
			try
			{
				IAccount _founduser = FindAll().Where(w => w.UserName == LoginAccount).SingleOrDefault();
				if (IsOnline)
				{
					WriteUserOperationLog(OperationCodeEnum.Account_FLAG_Online, _founduser);
					WriteUserOperationLog(OperationCodeEnum.Account_Update_End_Success, _founduser);
				}
				else
				{
					WriteUserOperationLog(OperationCodeEnum.Account_FLAG_Offline, _founduser);
				}
				return _founduser;
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

		public int FindUserIdFromPasswordResetToken(string Token)
		{
			try
			{
				IAccount _founduser = Find(w => w.ResetPasswordToken == Token);
				return _founduser.Id;
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

		public bool IsConfirmed(int MemberId)
		{
			try
			{
				IRepositoryBase<IUserProfile> _userprofilerepository = _unitofwork.GetRepository<IUserProfile>();
				IUserProfile _profiledata = _userprofilerepository.Find(w => w.Id == MemberId &&
																		(w.EmailConfirmed || w.PhoneNumberConfirmed));
				IAccount _founduser = Find(w => w.Id == MemberId);
				return (_userprofilerepository != null) || (_founduser != null);
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

		public int ResetPasswordWithToken(string Token, string newPassword)
		{
			try
			{
				int _memberid = FindUserIdFromPasswordResetToken(Token);
				IAccount _founduser = FindUserById(_memberid, GetIsOnline(_memberid));
				if (_founduser != null)
				{

					_founduser.Password = newPassword;

					return (int)OperationCodeEnum.Account_ChangePassword_End_Success;
				}
				return (int)OperationCodeEnum.Account_ChangePassword_End_Fail;
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
			if (_database != null)
				_database.SaveChanges();
			else
				_unitofwork.SaveChanges();
		}

		public IList<IAccount> ToList(IQueryable<IAccount> source)
		{
			try
			{
				return source.ToList();
			}
			catch (Exception ex)
			{
				WriteErrorLog(ex);
				throw ex;
			}

		}

		public IAccount Update(IAccount entity)
		{
			try
			{
				IAccount _founduser = FindUserById(entity.Id, GetIsOnline(entity.Id));
				_unitofwork.BeginTranscation();
				_founduser.DisplayName = entity.DisplayName;
				_founduser.Password = entity.Password;
				_founduser.PasswordHash = entity.PasswordHash;
				_founduser.ResetPasswordToken = entity.ResetPasswordToken;
				_founduser.SecurityStamp = entity.SecurityStamp;
				_founduser.TwoFactorEnabled = entity.TwoFactorEnabled;
				_founduser.UserName = entity.UserName;
				_founduser.Void = entity.Void;
				SaveChanges();
				_unitofwork.CommitTranscation();
				return FindUserById(entity.Id, true);
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

		public bool ValidateUser(IAccount UserDataToValidated)
		{
			try
			{
				return Find(w => w.UserName == UserDataToValidated.UserName &&
						 (UserDataToValidated.Password == w.Password || UserDataToValidated.PasswordHash == w.PasswordHash)) != null;
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

