using NUnit.Framework;
using System;
using System.Collections.ObjectModel;
using My.Core.Infrastructures.Implementations;
using System.Linq;

namespace My.UnitTest
{
	[TestFixture()]
	public class Test
	{
		private ApplicationDbContext GetDatabase()
		{

			string _connstr = "server=192.168.11.10;port=3306;database=OpenCoreWeb;uid=ediux;password=!QAZ2wsx";
			ApplicationDbContext _db = new ApplicationDbContext(_connstr);
			return _db;
		}

		[TestFixtureSetUp]
		public void TestInit()
		{

			ApplicationDbContext _db = GetDatabase();

			_db.Database.CreateIfNotExists();

			_db.Dispose();

		}

		[Test()]
		public void TestUserOpearationCodeDefineCase()
		{
			using (var _db = GetDatabase())
			{
				bool _isexists = (from _g in _db.OperationCodeDefines
								  select _g
								 ).Any();

				if (_isexists)
				{
					for (int code = 0; code < 65535; code++)
					{
						var _codedefine = new UserOperationCodeDefine();

						switch (code)
						{
							case (int)OperationCodeEnum.Account_BatchCreate_End_Success:
								_codedefine.Description = "Account_BatchCreate_End_Success";
								_codedefine.OpreationCode = (int)OperationCodeEnum.Account_BatchCreate_End_Success;
								break;

							case (int)OperationCodeEnum.Account_BatchCreate_End_Fail:
								_codedefine.Description = "Account_BatchCreate_End_Fail";
								_codedefine.OpreationCode = (int)OperationCodeEnum.Account_BatchCreate_End_Fail;
								break;
							case (int)OperationCodeEnum.Account_BatchCreate_Rollback:
								_codedefine.Description = "Account_BatchCreate_Rollback";
								_codedefine.OpreationCode = (int)OperationCodeEnum.Account_BatchCreate_Rollback;
								break;
							case (int)OperationCodeEnum.Account_BatchCreate_Start:
								_codedefine.Description = "Account_BatchCreate_Start";
								_codedefine.OpreationCode = (int)OperationCodeEnum.Account_BatchCreate_Start;
								break;

							case (int)OperationCodeEnum.Account_ChangePassword_End_Fail:
								_codedefine.Description = "Account_ChangePassword_End_Fail";
								_codedefine.OpreationCode = (int)OperationCodeEnum.Account_ChangePassword_End_Fail;
								break;

							case (int)OperationCodeEnum.Account_ChangePassword_End_Success:
								_codedefine.Description = "Account_ChangePassword_End_Success";
								_codedefine.OpreationCode = (int)OperationCodeEnum.Account_ChangePassword_End_Success;
								break;
							case (int)OperationCodeEnum.Account_ChangePassword_Rollback:
								_codedefine.Description = "Account_ChangePassword_Rollback";
								_codedefine.OpreationCode = (int)OperationCodeEnum.Account_ChangePassword_Rollback;
								break;
							case (int)OperationCodeEnum.Account_ChangePassword_Start:
								_codedefine.Description = "Account_ChangePassword_Start";
								_codedefine.OpreationCode = (int)OperationCodeEnum.Account_ChangePassword_Start;
								break;
							case (int)OperationCodeEnum.Account_Create_End_Fail:
								_codedefine.Description = "Account_Create_End_Fail";
								_codedefine.OpreationCode = (int)OperationCodeEnum.Account_Create_End_Fail;
								break;
							case (int)OperationCodeEnum.Account_Create_End_Success:
								_codedefine.Description = "Account_Create_End_Success";
								_codedefine.OpreationCode = (int)OperationCodeEnum.Account_Create_End_Success;
								break;
						}


						_db.OperationCodeDefines.Add(_codedefine);
					}

					_db.SaveChanges();

					_isexists = (from _g in _db.OperationCodeDefines								 
								 select _g
							 ).Any();

					Assert.IsTrue(_isexists, "操作代碼 Account_BatchCreate_End_Fail 建立失敗！");
				}
				else {
					Assert.Pass();
				}
			}
		}

		[Test()]
		public void TestCreateGroupCase()
		{
			using (var _db = GetDatabase())
			{
				bool _isexists = (from _g in _db.Groups
								  where _g.Name == "Users"
								  select _g
								 ).Any();

				Assert.IsFalse(_isexists, "Users 群組已存在！");

				if (_isexists)
				{
					return;
				}

				var g = new ApplicationUserGroup
				{
					ParentGroup = null,
					Name = "Users",
					SubGroups = new Collection<ApplicationUserGroup>(),
					Users = new Collection<ApplicationUser>()
				};
				g = _db.Groups.Add(g);
				_db.SaveChanges();

				_isexists = (from _g in _db.Groups
							 where _g.Name == "Users"
							 select _g
								 ).Any();

				Assert.IsTrue(_isexists, "Users 群組建立失敗！");
			}
		}

		[Test()]
		public void TestCreateRoleCase()
		{
			using (var _db = GetDatabase())
			{
				bool _isexists = (from _g in _db.Roles
								  where _g.Name == "Tester"
								  select _g
								 ).Any();

				Assert.IsFalse(_isexists, "Tester 角色已存在！");

				var g = new ApplicationRole
				{
					Name = "Tester",
				};

				g = _db.Roles.Add(g);
				_db.SaveChanges();

				_isexists = (from _g in _db.Roles
							 where _g.Name == "Tester"
							 select _g
								 ).Any();

				Assert.IsTrue(_isexists, "Tester 角色建立失敗！");
			}
		}

		[Test()]
		public void TestCreateUserCase()
		{
			using (var _db = GetDatabase())
			{
				bool _isexists = (from _g in _db.Users
								  where _g.UserName == "Administrator"
								  select _g
								 ).Any();

				Assert.IsFalse(_isexists, "使用者Administrator已存在！");

				if (_isexists)
				{
					return;
				}

				var g = new ApplicationUser
				{
					UserName = "Administrator",
					DisplayName = "系統管理員",
					Password = "!QAZ2wsx",
					PasswordHash = string.Empty,
					ResetPasswordToken = "",
					SecurityStamp = "",
					TwoFactorEnabled = false,
					Void = false
				};

				g = _db.Users.Add(g);

				_db.SaveChanges();

				_isexists = (from _g in _db.Users
							 where _g.UserName == "Administrator"
							 && _g.Void == false
							 select _g
								 ).Any();

				Assert.IsTrue(_isexists, "Tester 角色建立失敗！");
			}

		}

		[Test()]
		public void TestUsertoGroupCase()
		{
			using (var _db = GetDatabase())
			{
				var _founuser = (from _user in _db.Users
								 where _user.UserName == "Administrator" && _user.Void == false
								 select _user).SingleOrDefault();

				var _foundgroup = (from _group in _db.Groups
								   where _group.Name == "Users"
								   select _group).SingleOrDefault();

				bool _isExists = (_founuser != null && _foundgroup != null);

				Assert.IsTrue(_isExists, "帳號與群組不存在！");

				bool _isAdded = (from _g in _founuser.Groups
								 where _g.Name == "Users"
								 select _g).Any();

				Assert.IsFalse(_isAdded, "已經加入群組Users。");


				_founuser.Groups.Add(_foundgroup);
				_db.SaveChanges();

				_isExists = (from _user in _db.Users
							 from _group in _user.Groups
							 where _user.UserName == "Administrator" && _user.Void == false
							 && _group.Name == "Users"
							 select _user).Any();

				Assert.IsTrue(_isExists, "群組加入失敗！");

			}
		}

		[Test]
		public void TestAddUsertoRoleCase()
		{
			using (var _db = GetDatabase())
			{
				var _founuser = (from _user in _db.Users
								 where _user.UserName == "Administrator" && _user.Void == false
								 select _user).SingleOrDefault();

				var _foundgroup = (from _group in _db.Roles
								   where _group.Name == "Tester"
								   select _group).SingleOrDefault();

				bool _isExists = (_founuser != null && _foundgroup != null);

				Assert.IsTrue(_isExists, "帳號與角色不存在！");


				bool _isAdded = (from _g in _founuser.Roles
								 where _g.Name == "Tester"
								 select _g).Any();

				Assert.IsFalse(_isAdded, "已經加入角色Tester。");

				_founuser.Roles.Add(_foundgroup);
				_db.SaveChanges();

				_isExists = (from _user in _db.Users
							 from _group in _user.Roles
							 where _user.UserName == "Administrator" && _user.Void == false
							 && _group.Name == "Tester"
							 select _user).Any();

				Assert.IsTrue(_isExists, "角色Tester加入失敗！");

			}
		}

		[Test]
		public void TestAddOperationLogByAdministratorCase()
		{
			using (var _db = GetDatabase())
			{
				var _founuser = (from _user in _db.Users
								 where _user.UserName == "Administrator" && _user.Void == false
								 select _user).SingleOrDefault();

				var _foundefine = (from _codedef in _db.OperationCodeDefines
								   where _codedef.OpreationCode == (int)OperationCodeEnum.Account_FLAG_Online
								   select _codedef).SingleOrDefault();

				bool _isFoundCode = (_foundefine != null);

				if (_isFoundCode == false)
				{
					_db.OperationCodeDefines.Add(new UserOperationCodeDefine()
					{
						Description = "使用者已上線",
						OpreationCode = (int)OperationCodeEnum.Account_FLAG_Online
					});

					_db.SaveChanges();

					_foundefine = (from _codedef in _db.OperationCodeDefines
								   where _codedef.OpreationCode == (int)OperationCodeEnum.Account_FLAG_Online
								   select _codedef).SingleOrDefault();

					_isFoundCode = (_foundefine != null);

					Assert.IsTrue(_isFoundCode, "操作代碼新增失敗！");

				}

				bool _isExists = (_founuser != null && _isFoundCode);

				Assert.IsTrue(_isExists, "帳號與操作代碼不存在！");

				var _newlog = new UserOperationLog();

				_newlog.Body = "Testing.";
				_newlog.LogTime = DateTime.Now;
				_newlog.OpreationCode = _foundefine.OpreationCode;
				_newlog.URL = "";
				_newlog.UserId = _founuser.MemberId;

				_founuser.OpreationLogs.Add(_newlog);

				_db.SaveChanges();

				_isExists = (from _user in _db.Users
							 from _group in _user.OpreationLogs
							 where _user.UserName == "Administrator" && _user.Void == false
							 select _user.OpreationLogs).Any();

				Assert.IsTrue(_isExists, "查無LOG！");

			}
		}

		[Test]
		public void TestDeleteGroupCase()
		{
			using (var _db = GetDatabase())
			{
				var _founuser = (from _user in _db.Users
								 where _user.UserName == "Administrator" && _user.Void == false
								 select _user).SingleOrDefault();

				bool _isUserExist = (_founuser != null);

				Assert.IsTrue(_isUserExist, "使用者 Administrator 不存在！");

				_founuser.Groups.Clear();
				_db.SaveChanges();


				var _groups = from _g in _db.Groups
							  select _g;

				if (_groups.Any())
				{
					foreach (var _removeg in _groups)
					{
						_db.Groups.Remove(_removeg);
					}
					_db.SaveChanges();

#if MYSQL
					_db.Database.ExecuteSqlCommand("ALTER TABLE AspNetGroups AUTO_INCREMENT = 1");
#endif
					Assert.IsFalse(_db.Groups.Any(), "清除群組失敗！");
				}

				Assert.Pass();
			}
		}

		[Test]
		public void TestDeleteRoleCase()
		{
			using (var _db = GetDatabase())
			{
				var _founuser = (from _user in _db.Users
								 where _user.UserName == "Administrator" && _user.Void == false
								 select _user).SingleOrDefault();

				bool _isUserExist = (_founuser != null);

				Assert.IsTrue(_isUserExist, "使用者 Administrator 不存在！");

				_founuser.Roles.Clear();
				_db.SaveChanges();


				var _groups = from _g in _db.Roles
							  select _g;

				if (_groups.Any())
				{
					foreach (var _removeg in _groups)
					{
						_db.Roles.Remove(_removeg);
					}
					_db.SaveChanges();

#if MYSQL
					_db.Database.ExecuteSqlCommand("ALTER TABLE AspNetRoles AUTO_INCREMENT = 1");
#endif
					Assert.IsFalse(_db.Roles.Any(), "清除群組失敗！");
				}

				Assert.Pass();
			}
		}

		[TestFixtureTearDown]
		public void TestFinished()
		{
			//var _db = GetDatabase();
			//_db.Database.Delete();

		}

	}
}

