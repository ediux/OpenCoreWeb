﻿using System;
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
    public class ApplicationUserRepository : RepositoryBase<ApplicationUser>, IApplicationUserRepository<ApplicationUser>
    {

        private IUserOperationCodeDefineRepository<UserOperationCodeDefine> logdefinerepo;

        public ApplicationUserRepository(IUnitofWork unitofwork)
            : base(unitofwork)
        {
            logdefinerepo = _unitofwork.GetRepository<UserOperationCodeDefineRepository, UserOperationCodeDefine>() as IUserOperationCodeDefineRepository<UserOperationCodeDefine>;
        }

        #region Helper Functions
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



                var founddefined = logdefinerepo.Find((int)code);


                User.UserOperationLog.Add(new UserOperationLog()
                {
                    Body = _body,
                    UserId = User.Id,
                    LogTime = DateTime.Now,
                    OpreationCode = (int)code,
                    URL = _url
                });


                //_unitofwork.GetEntry<ApplicationUser>(User).State = EntityState.Modified;

            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
            }

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

        public override IList<ApplicationUser> BatchCreate(IEnumerable<ApplicationUser> entities)
        {
            var currentLoginedUser = Find(GetCurrentLoginedUserId());

            if (currentLoginedUser == null)
                currentLoginedUser = ApplicationUser.Create();

            try
            {
                WriteUserOperationLog(OperationCodeEnum.Account_BatchCreate_Start, currentLoginedUser);
                var result = base.BatchCreate(entities);
                WriteUserOperationLog(OperationCodeEnum.Account_BatchCreate_End_Success, currentLoginedUser);

                return result;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                WriteUserOperationLog(OperationCodeEnum.Account_BatchCreate_Rollback, currentLoginedUser);
                WriteUserOperationLog(OperationCodeEnum.Account_BatchCreate_End_Fail, currentLoginedUser);
                throw ex;
            }

        }

        public ApplicationUser ChangePassword(ApplicationUser UpdatedUserData)
        {
            var currentLoginedUser = Find(GetCurrentLoginedUserId());

            if (currentLoginedUser == null)
                currentLoginedUser = ApplicationUser.Create();

            try
            {
                WriteUserOperationLog(OperationCodeEnum.Account_ChangePassword_Start, currentLoginedUser);

                ApplicationUser _updateduser = Update(UpdatedUserData);

                WriteUserOperationLog(OperationCodeEnum.Account_ChangePassword_End_Success, currentLoginedUser);
                return _updateduser;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                WriteUserOperationLog(OperationCodeEnum.Account_ChangePassword_End_Fail, currentLoginedUser);
                throw ex;
            }

        }

        public override ApplicationUser Create(ApplicationUser entity)
        {
            var currentLoginedUser = Find(GetCurrentLoginedUserId());

            if (currentLoginedUser == null)
                currentLoginedUser = ApplicationUser.Create();

            try
            {
                WriteUserOperationLog(OperationCodeEnum.Account_Create_Start, currentLoginedUser);
                entity.LastUpdateTime = entity.CreateTime = DateTime.Now.ToUniversalTime();
                entity.LastUpdateUserId = entity.CreateUserId = currentLoginedUser.Id;
                entity = base.Create(entity);
                WriteUserOperationLog(OperationCodeEnum.Account_Create_End_Success, currentLoginedUser);

                return entity;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                WriteUserOperationLog(OperationCodeEnum.Account_Create_End_Fail, currentLoginedUser);
                WriteUserOperationLog(OperationCodeEnum.Account_Create_Rollback, currentLoginedUser);
                throw ex;
            }

        }

        public override void Delete(ApplicationUser entity)
        {
            var currentLoginedUser = Find(GetCurrentLoginedUserId());

            if (currentLoginedUser == null)
                currentLoginedUser = ApplicationUser.Create();

            try
            {
                WriteUserOperationLog(OperationCodeEnum.Account_Delete_Start, entity);
                base.Delete(entity);
                WriteUserOperationLog(OperationCodeEnum.Account_Delete_End_Success, entity);
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                WriteUserOperationLog(OperationCodeEnum.Account_Delete_End_Fail, entity);
                WriteUserOperationLog(OperationCodeEnum.Account_Delete_Rollback, entity);
                throw ex;
            }

        }

        public override IQueryable<ApplicationUser> FindAll()
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
            var currentLoginedUser = Find(GetCurrentLoginedUserId());

            if (currentLoginedUser == null)
                currentLoginedUser = ApplicationUser.Create();

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
                WriteUserOperationLog(OperationCodeEnum.Account_FindByEmail_End_Fail, currentLoginedUser);
                throw ex;
            }

        }

        public override ApplicationUser Find(params object[] values)
        {
            var result = base.Find(values);
            if (result == null)
                result = ApplicationUser.Create();

            return result;
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
            var currentLoginedUser = Find(GetCurrentLoginedUserId());

            if (currentLoginedUser == null)
                currentLoginedUser = ApplicationUser.Create();

            try
            {
                ApplicationUser _founduser = null;


                var result = from q in _datatable
                             where q.UserName.Equals(LoginAccount, StringComparison.InvariantCultureIgnoreCase)
                             select q;

                _founduser = result.SingleOrDefault();

                if (IsOnline)
                {
                    WriteUserOperationLog(OperationCodeEnum.Account_FLAG_Online, currentLoginedUser);
                    _founduser.LastActivityTime = DateTime.Now;
                    Update(_founduser);
                }
                else
                {
                    WriteUserOperationLog(OperationCodeEnum.Account_FLAG_Offline, currentLoginedUser);
                }
                return _founduser;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }


        }

        public int FindUserIdFromPasswordResetToken(string Token, out ApplicationUser user)
        {
            try
            {


                var result = from q in _datatable
                             where q.ResetPasswordToken.Equals(Token, StringComparison.InvariantCulture)
                             select q;

                user = result.SingleOrDefault();

                return user.Id;
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
                ApplicationUser _founduser = null;
                int _memberid = FindUserIdFromPasswordResetToken(Token, out _founduser);

                if (_founduser != null)
                {
                    _founduser.Password = newPassword;
                    Update(_founduser);
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



        public override ApplicationUser Update(ApplicationUser entity)
        {
            var currentLoginedUser = Find(GetCurrentLoginedUserId());

            if (currentLoginedUser == null)
                currentLoginedUser = ApplicationUser.Create();

            try
            {
                WriteUserOperationLog(OperationCodeEnum.Account_Update_Start, currentLoginedUser);
                entity.LastUpdateTime = DateTime.Now.ToUniversalTime();

                entity.LastUpdateUserId = currentLoginedUser.Id;
                if (entity.Id <= 0 || entity == null)
                    Create(entity);
                else
                    base.Update(entity);
                WriteUserOperationLog(OperationCodeEnum.Account_Update_End_Success, currentLoginedUser);
                return entity;
            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                WriteUserOperationLog(OperationCodeEnum.Account_Update_Rollback, currentLoginedUser);
                WriteUserOperationLog(OperationCodeEnum.Account_Update_End_Fail, currentLoginedUser);
                throw ex;
            }

        }

        public bool ValidateUser(ApplicationUser UserDataToValidated)
        {
            try
            {
                ApplicationUser _validatedUser = null;

                var result = from w in _datatable
                             where w.UserName == UserDataToValidated.UserName
                             select w;

                _validatedUser = result.SingleOrDefault();

                if (_validatedUser != null)
                    return (UserDataToValidated.Password == _validatedUser.Password ||
                        UserDataToValidated.PasswordHash == _validatedUser.PasswordHash);

                return false;

            }
            catch (Exception ex)
            {
                WriteErrorLog(ex);
                throw ex;
            }

        }
    }
}

