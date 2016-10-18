using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using My.Core.Infrastructures.DAL;
using My.Core.Infrastructures.Logs;
using My.Core.Infrastructures.Implementations.Models;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using Microsoft.AspNet.Identity;

namespace My.Core.Infrastructures.Implementations
{
    public class AccountRepository : IAccountRepository<ApplicationUser>
    {
        private IUnitofWork _unitofwork;

        private DbSet<ApplicationUser> _datatable;

        public AccountRepository(IUnitofWork unitofwork)
        {
            _unitofwork = unitofwork;
            _datatable = _unitofwork.GetEntity<ApplicationUser>() as DbSet<ApplicationUser>;
        }

        #region Helper Functions

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
        protected virtual void WriteUserOperationLog(OperationCodeEnum code, ApplicationUser User)
        {
            try
            {
                //IRepositoryBase<UserOperationLog> useroperationlog = _unitofwork.GetRepository<UserOperationLog>();

                string _url = string.Empty;
                string _body = string.Empty;

                if (System.Web.HttpContext.Current != null)
                {
                    _url = System.Web.HttpContext.Current.Request.Url.AbsoluteUri;
                    _body = Newtonsoft.Json.JsonConvert.SerializeObject(System.Web.HttpContext.Current.Request.Form);
                }

                if (User == null)
                {
                    return;
                }

                IUserOperationCodeDefineRepository<UserOperationCodeDefine> logdefinerepo = _unitofwork.GetRepository<UserOperationCodeDefine>() as IUserOperationCodeDefineRepository<UserOperationCodeDefine>;

                var founddefined = logdefinerepo.Find((int)code);


                User.UserOperationLog.Add(new UserOperationLog()
                {
                    Body = _body,
                    UserId = User.Id,
                    LogTime = DateTime.Now,
                    OpreationCode = (int)code,
                    URL = _url
                });
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
            }

        }

        protected virtual int GetCurrentLoginedUserId()
        {
            if (System.Web.HttpContext.Current != null)
            {
                return System.Web.HttpContext.Current.User.Identity.GetUserId<int>();
            }
          
            return -1;
        }

        protected virtual bool GetIsOnline(int memberid)
        {
            try
            {
                ApplicationUser _founduser = Find(memberid);

                if (_founduser != null)
                {
                    DateTime ExpireTime = DateTime.Now.AddMinutes(-30);

                    if (_founduser.LastActivityTime < ExpireTime)
                    {
                        return false;
                    }

                    return true;
                }

                throw new Exception("Not found.");
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
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

        public IList<ApplicationUser> BatchCreate(IEnumerable<ApplicationUser> entities)
        {
            try
            {
                IList<ApplicationUser> result = ((DbSet<ApplicationUser>)_datatable).AddRange(entities).ToList();
                SaveChanges();
                return result;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);

                WriteUserOperationLog(OperationCodeEnum.Account_BatchCreate_Rollback, new ApplicationUser() { Id = -1 });
                throw ex;
            }

        }

        public ApplicationUser ChangePassword(ApplicationUser UpdatedUserData)
        {
            try
            {
                WriteUserOperationLog(OperationCodeEnum.Account_ChangePassword_Start, UpdatedUserData);

                ApplicationUser _updateduser = Update(UpdatedUserData);


                return _updateduser;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                WriteUserOperationLog(OperationCodeEnum.Account_ChangePassword_End_Fail, UpdatedUserData);
                throw ex;
            }

        }

        public ApplicationUser Create(ApplicationUser entity)
        {
            try
            {
                WriteUserOperationLog(OperationCodeEnum.Account_Create_Start, entity);
                ApplicationUser inserteduser = _datatable.Add((ApplicationUser)entity);

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

        }

        public void Delete(ApplicationUser entity)
        {
            try
            {
                WriteUserOperationLog(OperationCodeEnum.Account_Delete_Start, entity);
                ApplicationUser founduser = Find(entity.Id);
                _datatable.Remove(founduser);
                WriteUserOperationLog(OperationCodeEnum.Account_Delete_End_Success, entity);
                SaveChanges();
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                WriteUserOperationLog(OperationCodeEnum.Account_Delete_End_Fail, entity);
                WriteUserOperationLog(OperationCodeEnum.Account_Delete_Rollback, entity);
                throw ex;
            }

        }

        public ApplicationUser Find(params object[] values)
        {

            ApplicationUser _currentexecutinguser = null;

            try
            {
                _currentexecutinguser = _datatable.Find(values);
                return _currentexecutinguser;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                WriteUserOperationLog(OperationCodeEnum.Accpimt_Find_End_Fail, _currentexecutinguser);
                throw ex;
            }

        }

        public IQueryable<ApplicationUser> FindAll()
        {
            try
            {
                return _datatable
                    .Include(w => w.ApplicationUserGroup)
                    .Include(w => w.ApplicationUserProfileRef)
                    .Include(w => w.ApplicationUserRole).AsQueryable();
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }

        }

        public IList<ApplicationUser> FindAllAccounts()
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

        }

        public ApplicationUser FindByEmail(string email)
        {
            try
            {
                IQueryable<ApplicationUser> queryset = _datatable.Include(i => i.ApplicationUserProfileRef);

                var result = from q in queryset
                             from profilerefs in q.ApplicationUserProfileRef
                             where profilerefs.ApplicationUserProfile.EMail.Equals(email, StringComparison.InvariantCultureIgnoreCase)
                             && q.Void == false
                             select q;

                ApplicationUser founduser = result.SingleOrDefault();

                return founduser;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                WriteUserOperationLog(OperationCodeEnum.Account_FindByEmail_End_Fail, null);
                throw ex;
            }

        }

        public ApplicationUser FindUserById(int MemberId, bool isOnline)
        {
            try
            {
                ApplicationUser _founduser = Find(MemberId);

                if (_founduser != null)
                {
                    if (_founduser.LastActivityTime.HasValue)
                    {
                        if (_founduser.LastActivityTime.Value.AddMinutes(30) > DateTime.Now)
                        {
                            if (isOnline)
                            {
                                _founduser.LastActivityTime = DateTime.Now;
                                Update(_founduser);
                                return _founduser;
                            }
                        }
                    }
                }
                //WriteUserOperationLog(OperationCodeEnum.Account_FindById_Start, _founduser);
                //if (isOnline)
                //{
                //    WriteUserOperationLog(OperationCodeEnum.Account_FLAG_Online, _founduser);
                //    WriteUserOperationLog(OperationCodeEnum.Account_Update_End_Success, _founduser);
                //}
                //else
                //{
                //    WriteUserOperationLog(OperationCodeEnum.Account_FLAG_Offline, _founduser);
                //}
                //WriteUserOperationLog(OperationCodeEnum.Account_FindById_End_Success, _founduser);
                return _founduser;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }

        }

        public ApplicationUser FindUserByLoginAccount(string LoginAccount, bool IsOnline)
        {
            try
            {
                ApplicationUser _founduser = null;


                var result = from q in _datatable
                             where q.UserName.Equals(LoginAccount, StringComparison.InvariantCultureIgnoreCase)
                             select q;

                _founduser = result.SingleOrDefault();

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


        }

        public int FindUserIdFromPasswordResetToken(string Token)
        {
            try
            {
                ApplicationUser _founduser = null;

                var result = from q in _datatable
                             where q.ResetPasswordToken.Equals(Token, StringComparison.InvariantCulture)
                             select q;

                _founduser = result.Single();
                return _founduser.Id;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }

        }

        public bool IsConfirmed(int MemberId)
        {
            try
            {

                //IUserProfile _profiledata = _userprofilerepository.Find(w => w.MemberId == MemberId &&
                //                                                        (w.EmailConfirmed || w.PhoneNumberConfirmed));
                ApplicationUser _founduser = Find(MemberId);

                if (_founduser != null)
                {
                    ApplicationUserProfileRef _profile = _founduser.ApplicationUserProfileRef.Single();
                    return (_profile.ApplicationUserProfile.EMailConfirmed
                        || _profile.ApplicationUserProfile.PhoneConfirmed);
                }

                return false;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }

        }

        public int ResetPasswordWithToken(string Token, string newPassword)
        {
            try
            {
                int _memberid = FindUserIdFromPasswordResetToken(Token);
                ApplicationUser _founduser = FindUserById(_memberid, GetIsOnline(_memberid));
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

        }

        public void SaveChanges()
        {
            _unitofwork.SaveChanges();
        }

        public IList<ApplicationUser> ToList(IQueryable<ApplicationUser> source)
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

        public ApplicationUser Update(ApplicationUser entity)
        {
            try
            {
                ApplicationUser _founduser = Find(entity.Id);

                if (entity.Password != _founduser.Password
                    || entity.PasswordHash != _founduser.PasswordHash)
                {
                    WriteUserOperationLog(OperationCodeEnum.Account_ChangePassword_Start, entity);
                    _founduser.Password = entity.Password;
                    _founduser.PasswordHash = entity.PasswordHash;
                    _founduser.LastUpdateTime = DateTime.Now;
                    _founduser.LastUpdateUserId = GetCurrentLoginedUserId();
                    WriteUserOperationLog(OperationCodeEnum.Account_ChangePassword_End_Success, entity);
                }
                //_unitofwork.BeginTranscation();
                ////_founduser.DisplayName = entity.DisplayName;
                //_founduser.Password = entity.Password;
                //_founduser.PasswordHash = entity.PasswordHash;
                //_founduser.ResetPasswordToken = entity.ResetPasswordToken;
                //_founduser.SecurityStamp = entity.SecurityStamp;
                //_founduser.TwoFactorEnabled = entity.TwoFactorEnabled;
                //_founduser.UserName = entity.UserName;
                //_founduser.Void = entity.Void;
                //_founduser.ResetPasswordToken = entity.ResetPasswordToken;

                SaveChanges();

                return FindUserById(entity.Id, true);
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }

        }

        public bool ValidateUser(ApplicationUser UserDataToValidated)
        {
            try
            {
                ApplicationUser _validatedUser = null;
                var result = from w in _datatable
                             where w.UserName == UserDataToValidated.UserName &&
                         (UserDataToValidated.Password == w.Password || UserDataToValidated.PasswordHash == w.PasswordHash)
                             select w;

                _validatedUser = result.SingleOrDefault();

                if (_validatedUser != null)
                    return true;

                return false;

            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }

        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private bool disposed = false;
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _unitofwork.Dispose();
                }
            }

            disposed = true;

        }
    }
}

