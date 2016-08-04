using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;
using My.Core.Infrastructures.Datas;
using My.Core.Infrastructures.Implementations.Datas;
using My.Core.Infrastructures.Logs;

namespace My.Core.Infrastructures.Implementations.Repositories
{
	public class AccountRepository : IAccountRepository
	{
		private IUnitofWork _unitofwork;

		private DbSet<ApplicationUser> _database;

		private bool isbatchmode;
		public AccountRepository(IUnitofWork unitofwork)
		{
			_unitofwork = unitofwork;
			_database = _unitofwork.GetEntity<DbSet<ApplicationUser>>();
			_logger = null;
			isbatchmode = false;
		}

		#region Helper Functions
		/// <summary>
		/// Generates the salted hash.
		/// </summary>
		/// <returns>The salted hash.</returns>
		/// <param name="plainText">Plain text.</param>
		/// <param name="salt">Salt.</param>
		static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
		{
			HashAlgorithm algorithm = new SHA256Managed();

			byte[] plainTextWithSaltBytes =
			  new byte[plainText.Length + salt.Length];

			for (int i = 0; i < plainText.Length; i++)
			{
				plainTextWithSaltBytes[i] = plainText[i];
			}
			for (int i = 0; i < salt.Length; i++)
			{
				plainTextWithSaltBytes[plainText.Length + i] = salt[i];
			}

			return algorithm.ComputeHash(plainTextWithSaltBytes);
		}

		/// <summary>
		/// Compares the byte arrays.
		/// </summary>
		/// <returns>The byte arrays.</returns>
		/// <param name="array1">Array1.</param>
		/// <param name="array2">Array2.</param>
		public static bool CompareByteArrays(byte[] array1, byte[] array2)
		{
			if (array1.Length != array2.Length)
			{
				return false;
			}

			for (int i = 0; i < array1.Length; i++)
			{
				if (array1[i] != array2[i])
				{
					return false;
				}
			}

			return true;
		}

		/// <summary>
		/// Resets the database object.
		/// </summary>
		/// <returns>The database object.</returns>
		protected virtual DbSet<ApplicationUser> GetDatabase()
		{
			return _unitofwork.GetEntity<DbSet<ApplicationUser>>();
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
		/// Creates the new administrator user.
		/// </summary>
		/// <returns>The new administrator user.</returns>
		protected virtual IAccount CreateNewAdministratorUser()
		{
			return new ApplicationUser()
			{
				DisplayName = "系統管理員",
				Password = HashedPassword(System.Web.Security.Membership.GeneratePassword(16, 1)),
				PasswordHash = HashedPassword(GeneratorDefaultPassword()),
				ResetPasswordToken = Guid.NewGuid().ToString("N"),
				SecurityStamp = DateTime.Now.Ticks.ToString(),
				TwoFactorEnabled = false,
				UserName = "Administrator",
				Void = false
			};
		}

		/// <summary>
		/// Hasheds the password.
		/// </summary>
		/// <returns>The password.</returns>
		/// <param name="password">Password.</param>
		protected virtual string HashedPassword(string password)
		{
			string _adminpwd = password;
			string _salt = "EdiuxnetSoftware";

			byte[] _adminpwdstream = Encoding.Default.GetBytes(_adminpwd);
			byte[] _saltbytes = Encoding.Default.GetBytes(_salt);

			return Encoding.Default.GetString(GenerateSaltedHash(_adminpwdstream, _saltbytes));
		}

		/// <summary>
		/// Generators the default password.
		/// </summary>
		/// <returns>The default password.</returns>
		protected virtual string GeneratorDefaultPassword()
		{
			return System.Web.Security.Membership.GeneratePassword(12, 1);
		}

			///// <summary>
			///// Writes the user operation log.
			///// </summary>
			///// <returns>The user operation log.</returns>
			///// <param name="code">Code.</param>
			///// <param name="User">User.</param>
			//protected virtual void WriteUserOperationLog(OperationCodeEnum code, IAccount User)
			//{
			//	try
			//	{

			//		IUserOperationLogRepository useroperationlog = _unitofwork.GetRepository<UserOperationLog>() as IUserOperationLogRepository;
			//		IUserOperationCodeDefineRepository useropreationrepository = _unitofwork.GetRepository<UserOperationCodeDefine>() as IUserOperationCodeDefineRepository;

			//		string _url = string.Empty;
			//		StringBuilder _bodybuilder = new StringBuilder();
			//		_url = GetLogData(_url, _bodybuilder);

			//		var operationcodeobject = useropreationrepository.FindByCode((int)code);

			//		IUserOperationLog _newlogrecord =
			//			UserOperationLog.CreateNew(
			//				operationcodeobject,
			//				User,
			//				_url,
			//				_bodybuilder.ToString());

			//		useroperationlog.Create(_newlogrecord);

			//	}
			//	catch (Exception ex)
			//	{
			//		WriteErrorLog(ex);
			//	}

			//}

			///// <summary>
			///// Gets the log data.
			///// </summary>
			///// <returns>The log data.</returns>
			///// <param name="_url">URL.</param>
			///// <param name="_bodybuilder">Bodybuilder.</param>
			//protected virtual string GetLogData(string _url, StringBuilder _bodybuilder)
			//{
			//	if (System.Web.HttpContext.Current != null)
			//	{
			//		_url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;

			//		if (System.Web.HttpContext.Current.Request.Form.HasKeys())
			//		{
			//			foreach (string _key in System.Web.HttpContext.Current.Request.Form.Keys)
			//			{
			//				_bodybuilder.Append(string.Format("{0}={1};", _key, System.Web.HttpContext.Current.Request.Form[_key]));
			//			}
			//		}

			//	}

			//	return _url;
			//}



		//protected virtual bool GetIsOnline(int memberid)
		//{
		//	try
		//	{
		//		return (_logdata.LogTime <= DateTime.Now);
		//	}
		//	catch (Exception ex)
		//	{
		//		WriteErrorLog(ex);
		//		return false;
		//	}
		//}
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

		/// <summary>
		/// Batchs the create.
		/// </summary>
		/// <returns>The create.</returns>
		/// <param name="entities">Entities.</param>
		public IList<IAccount> BatchCreate(IEnumerable<IAccount> entities)
		{
			try
			{
				List<IAccount> _insertednewuserlist = new List<IAccount>();

				_unitofwork.BeginTranscation();
				isbatchmode = true;
				foreach (IAccount newuser in entities)
				{
					try
					{
						Create(newuser);
					}
					catch (Exception ex)
					{
						WriteErrorLog(ex);					
					}

				}
				isbatchmode = false;
				SaveChanges();
				_unitofwork.CommitTranscation();
				return _insertednewuserlist;
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

		public IAccount ChangePassword(IAccount UpdatedUserData)
		{
			try
			{
				IAccount _fetchuser = FindUserById(UpdatedUserData.MemberId, true);

				if (_fetchuser != null)
				{

					_fetchuser.Password = UpdatedUserData.Password;
					_fetchuser.PasswordHash = HashedPassword(UpdatedUserData.Password);

					Update(UpdatedUserData);
				}

				return _fetchuser;
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

		public IAccount Create(IAccount entity)
		{
			try
			{
				_unitofwork.OpenDatabase();
				_database = GetDatabase();

				ApplicationUser inserteduser = _database.Add((ApplicationUser)entity);
				SaveChanges();

				return inserteduser;
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

		public void Delete(IAccount entity)
		{
			try
			{
				_unitofwork.OpenDatabase();
				_database = GetDatabase();

				_unitofwork.BeginTranscation();

				_database.Remove((ApplicationUser)entity);
				isbatchmode = false;
				SaveChanges();

				_unitofwork.CommitTranscation();


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

		public IAccount Find(Expression<Func<IAccount, bool>> predicate)
		{
			try
			{
				IAccount founduser = FindAll().Where(predicate).SingleOrDefault();
				return founduser;
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

		public IQueryable<IAccount> FindAll()
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

		public IAccount FindUserById(int MemberId, bool isOnline)
		{
			try
			{

				IAccount _founduser = FindAll().Where(w => w.MemberId == MemberId).SingleOrDefault();
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
				return _founduser.MemberId;
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
				IAccount _founduser = FindUserById(MemberId, true);

				if (_founduser == null)
				{
					return false;
				}

				return !_founduser.Void;

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
				IAccount _founduser = FindUserById(_memberid, true);
				if (_founduser != null)
				{

					_founduser.Password = newPassword;
					_founduser.PasswordHash = HashedPassword(newPassword);

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
			if (isbatchmode == false)
			{
				_unitofwork.SaveChanges();
			}
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
				IAccount _founduser = FindUserById(entity.MemberId, true);
				_unitofwork.BeginTranscation();
				_founduser.DisplayName = entity.DisplayName;
				_founduser.Password = entity.Password;
				_founduser.PasswordHash = entity.PasswordHash;
				_founduser.ResetPasswordToken = entity.ResetPasswordToken;
				_founduser.SecurityStamp = entity.SecurityStamp;
				_founduser.TwoFactorEnabled = entity.TwoFactorEnabled;
				_founduser.UserName = entity.UserName;
				_founduser.Void = entity.Void;
				_founduser.ResetPasswordToken = entity.ResetPasswordToken;
				SaveChanges();
				_unitofwork.CommitTranscation();
				return FindUserById(entity.MemberId, true);
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

