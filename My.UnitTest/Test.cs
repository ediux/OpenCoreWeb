using NUnit.Framework;
using System.Collections.ObjectModel;
using My.Core.Infrastructures.Implementations;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace My.UnitTest
{
	[TestFixture()]
	public class Test
	{
		#region 取得資料庫連結的方法
		/// <summary>
		/// Gets the database.
		/// </summary>
		/// <returns>The database.</returns>
		private ApplicationDbContext GetDatabase()
		{

			string _configfilepath = Assembly.GetExecutingAssembly().Location;
			System.Diagnostics.Debug.Write(_configfilepath);
			string _connstr = string.Empty;

			var _config = System.Configuration.ConfigurationManager.OpenExeConfiguration(_configfilepath);

			if (_config.ConnectionStrings != null)
			{
				_connstr = _config.ConnectionStrings.ConnectionStrings["Web"].ConnectionString;
			}

			//_configfilepath = _configfilepath + ".config";

			//if (!File.Exists(_configfilepath))
			//	return null;

			//var appSettings = new NameValueCollection();

			//var doc = XDocument.Load(File.OpenRead(_configfilepath));

			//foreach (XElement element in doc.Element("configuration").Elements("connectionStrings").Elements("add"))
			//{
			//	if (element.Name.LocalName == "add")
			//	{
			//		var key = element.Attribute("key").Value;
			//		var value = element.Attribute("value").Value;
			//		appSettings.Set(key, value);
			//	}

			//}

			var _db = new ApplicationDbContext(_connstr);
			return _db;
		}
		#endregion

		#region 初始化測試環境
		[TestFixtureSetUp]
		public void TestInit()
		{

			ApplicationDbContext _db = GetDatabase();

			_db.Database.CreateIfNotExists();

			Step_CheckOpreationCodeIsExists(_db);

			Step_CheckGroupExists(_db);

			Step_CheckRoleIsExists(_db);

			Step_CheckIsHasUsers(_db);

			_db.Dispose();

		}
		#endregion

		#region Test for UserOpreatinoCodeDefine
		[Test()]
		public void TestUserOpearationCodeDefineCRUDCase()
		{
			using (var _db = GetDatabase())
			{
				bool _isexists;

				_isexists = Step_CheckOpreationCodeIsExists(_db);
				Assert.IsFalse(_isexists, "操作代碼定義已經存在！");

				//Create
				Step_AddOpreationCode(_db);

				//Read
				_isexists = Step_GetOpreationCode(_db);
				Assert.IsTrue(_isexists, "操作代碼 Account_BatchCreate_Start 不存在！");

				//Update
				UserOperationCodeDefine _updateitem = Step_UpdateOperationCode(_db);

				Assert.IsNotNull(_updateitem, "測試項目遺失！");

				Assert.AreSame("批次創立帳號開始", _updateitem.Description, "變更失敗！");

				//Delete
				_isexists = Step_DeleteOpreationCode(_db, _updateitem);

				Assert.IsFalse(_isexists, "測試刪除失敗！");
				Assert.Pass();
			}
		}

		#region Test for OpreationCodeDefine steps
		static bool Step_CheckOpreationCodeIsExists(ApplicationDbContext _db)
		{
			var _alldefines = from _g in _db.OperationCodeDefines
							  where _g.OpreationCode == (int)OperationCodeEnum.Account_BatchCreate_Start
							  select _g;

			bool _isexists = _alldefines.Any();

			Step_RemoveAllOpreationCodes(_db, _alldefines, _isexists);

			return _isexists;
		}

		static void Step_RemoveAllOpreationCodes(ApplicationDbContext _db, IEnumerable<UserOperationCodeDefine> _alldefines, bool isExists = false)
		{
			if (isExists)
			{
				foreach (var removecode in _alldefines)
				{
					_db.OperationCodeDefines.Remove(removecode);
				}
				_db.SaveChanges();
			}
		}

		static void Step_AddOpreationCode(ApplicationDbContext _db)
		{
			var _codedefine = new UserOperationCodeDefine();

			_codedefine.Description = "Account_BatchCreate_Start";
			_codedefine.OpreationCode = (int)OperationCodeEnum.Account_BatchCreate_Start;

			_db.OperationCodeDefines.Add(_codedefine);
			_db.SaveChanges();
		}

		static bool Step_GetOpreationCode(ApplicationDbContext _db)
		{
			bool _isexists = (from _g in _db.OperationCodeDefines
							  where _g.OpreationCode == (int)OperationCodeEnum.Account_BatchCreate_Start
							  select _g
					 ).Any();

			return _isexists;
		}

		static UserOperationCodeDefine Step_UpdateOperationCode(ApplicationDbContext _db)
		{
			var _updateitem = (from _g in _db.OperationCodeDefines
							   where _g.OpreationCode == (int)OperationCodeEnum.Account_BatchCreate_Start
							   select _g
											  ).SingleOrDefault();

			Assert.IsNotNull(_updateitem, "測試項目遺失！");


			_updateitem.Description = "批次創立帳號開始";

			_db.SaveChanges();
			_updateitem = (from _g in _db.OperationCodeDefines
						   where _g.OpreationCode == (int)OperationCodeEnum.Account_BatchCreate_Start
						   select _g
							  ).SingleOrDefault();


			return _updateitem;
		}

		static bool Step_DeleteOpreationCode(ApplicationDbContext _db, UserOperationCodeDefine _updateitem)
		{
			bool _isexists;
			_db.OperationCodeDefines.Remove(_updateitem);
			_db.SaveChanges();

			_isexists = (from _g in _db.OperationCodeDefines
						 where _g.OpreationCode == (int)OperationCodeEnum.Account_BatchCreate_Start
						 select _g).Any();
			return _isexists;
		}
		#endregion
		#endregion

		#region Test for Group
		[Test()]
		public void TestGroupCRUDCase()
		{
			using (var _db = GetDatabase())
			{
				bool _isexists = Step_CheckGroupExists(_db);

				Assert.IsFalse(_isexists, "Users 群組已存在！");

				//Create
				Step_CreateGroupNamedUsers(_db);

				//Read
				_isexists = Step_GetGroupWithNamedUsers(_db).Any();

				Assert.IsTrue(_isexists, "找不到 Users 群組！");

				//Update

				ApplicationUserGroup currentgroup;
				Step_UpdateGroup(_db, out _isexists, out currentgroup);

				Assert.IsTrue(_isexists, "Users 群組名稱變更成 Admins 失敗！");

				//Delete
				Step_DeleteGroup(_db, out _isexists, out currentgroup);

				Assert.IsFalse(_isexists, "刪除 Admins 群組失敗！");
				Assert.Pass();

			}
		}

		#region Test for Groups CRUD steps

		static void Step_DeleteGroup(ApplicationDbContext _db, out bool _isexists, out ApplicationUserGroup currentgroup)
		{
			var _groups = Step_GetGroupWithNamedAdmins(_db);

			currentgroup = null;

			if (_groups.Any())
			{
				if (_groups.Count() > 1)
				{
					currentgroup = _groups.First();

					foreach (var _removeg in _groups)
					{
						_db.Groups.Remove(_removeg);
					}

					_db.SaveChanges();
				}
				else {
					currentgroup = _groups.SingleOrDefault();
					_db.Groups.Remove(currentgroup);
					_db.SaveChanges();
				}
			}

			_isexists = Step_GetGroupWithNamedAdmins(_db).Any();
		}

		static void Step_UpdateGroup(ApplicationDbContext _db, out bool _isexists, out ApplicationUserGroup currentgroup)
		{
			currentgroup = Step_GetGroupWithNamedUsers(_db).SingleOrDefault();
			Assert.IsNotNull(currentgroup, "找不到 Users 群組！");

			currentgroup.Name = "Admins";
			_db.SaveChanges();

			currentgroup = Step_GetGroupWithNamedAdmins(_db).SingleOrDefault();

			_isexists = (currentgroup != null);
		}

		static bool Step_CheckGroupExists(ApplicationDbContext _db)
		{
			IQueryable<ApplicationUserGroup> existsgroups = Step_GetAllGroups(_db);

			bool _isexists = existsgroups.Any();

			if (_isexists)
			{
				foreach (var removegroup in existsgroups)
				{
					_db.Groups.Remove(removegroup);
				}
				_db.SaveChanges();
			}
			//_isexists = Step_GetAllGroups(_db).Any();

			return _isexists;
		}
		static IQueryable<ApplicationUserGroup> Step_GetAllGroups(ApplicationDbContext _db)
		{
			return from _g in _db.Groups
				   select _g;
		}
		static IQueryable<ApplicationUserGroup> Step_GetGroupWithNamedUsers(ApplicationDbContext _db)
		{
			return from _g in _db.Groups
				   where _g.Name == "Users"
				   select _g;
		}

		static IQueryable<ApplicationUserGroup> Step_GetGroupWithNamedAdmins(ApplicationDbContext _db)
		{
			return from _g in _db.Groups
				   where _g.Name == "Admins"
				   select _g;
		}
		static void Step_CreateGroupNamedUsers(ApplicationDbContext _db)
		{
			var g = new ApplicationUserGroup
			{
				ParentGroup = null,
				Name = "Users",
				SubGroups = new Collection<ApplicationUserGroup>(),
				Users = new Collection<ApplicationUser>()
			};
			g = _db.Groups.Add(g);
			_db.SaveChanges();
		}
		#endregion
		#endregion

		#region Test for Role
		[Test]
		public void TestRoleCRUDCase()
		{
			using (var _db = GetDatabase())
			{
				bool _isexists = Step_CheckRoleIsExists(_db);

				Assert.IsFalse(_isexists, "Tester 角色已存在！");

				//Create
				Step_CreateRoleTester(_db);

				//Read
				_isexists = Step_GetRoleTester(_db).Any();

				Assert.IsTrue(_isexists, "找不到 Tester 角色！");

				//Update
				ApplicationRole currentrole;
				Step_UpdateRole(_db, out _isexists,out currentrole);

				Assert.IsTrue(_isexists, "Tester 角色名稱變更成 Master 失敗！");

				//Delete
				 currentrole = Step_DeleteRole(_db, out _isexists);

				Assert.IsFalse(_isexists, "刪除 Master 角色失敗！");


			}


		}

		#region Steps for Role CRUD Test
		static ApplicationRole Step_DeleteRole(ApplicationDbContext _db, out bool _isexists)
		{
			ApplicationRole currentrole = Step_GetRoleforMaster(_db).SingleOrDefault();
			Assert.IsNotNull(currentrole, "找不到 Master 角色！");

			_db.Roles.Remove(currentrole);
			_db.SaveChanges();
			IQueryable<ApplicationRole> currentquery;

			_isexists = Step_CheckRoleforMasterIsExists(_db,out currentquery);
			currentrole = currentquery.SingleOrDefault();
			return currentrole;
		}

		static IQueryable<ApplicationRole> Step_GetRoleforMaster(ApplicationDbContext _db)
		{
			return from _g in _db.Roles
				   where _g.Name == "Master"
				   select _g;
		}

		static bool Step_CheckRoleforMasterIsExists(ApplicationDbContext _db,out IQueryable<ApplicationRole> currquery)
		{
			currquery = Step_GetRoleforMaster(_db);
			return currquery.Any();
		}

		static void Step_UpdateRole(ApplicationDbContext _db, out bool _isExists,out ApplicationRole currentrole)
		{
			 currentrole = Step_GetRoleTester(_db).SingleOrDefault();

			Assert.IsNotNull(currentrole, "找不到 Tester 角色！");

			currentrole.Name = "Master";

			_db.SaveChanges();
			IQueryable<ApplicationRole> _q;
			_isExists = Step_CheckRoleforMasterIsExists(_db,out _q);
			currentrole = _q.SingleOrDefault();

		}

		static void Step_CreateRoleTester(ApplicationDbContext _db)
		{
			var g = new ApplicationRole
			{
				Name = "Tester",
			};

			g = _db.Roles.Add(g);
			_db.SaveChanges();
		}
		static IQueryable<ApplicationRole> Step_GetAllRoles(ApplicationDbContext _db)
		{
			return (from _r in _db.Roles
					select _r);
		}
		static bool Step_CheckRoleIsExists(ApplicationDbContext _db)
		{
			IQueryable<ApplicationRole> existsroles = Step_GetAllRoles(_db);

			bool _isexists = existsroles.Any();

			if (_isexists)
			{
				foreach (var removerole in existsroles)
				{
					_db.Roles.Remove(removerole);
				}
				_db.SaveChanges();
			}
			//_isexists = Step_GetAllRoles(_db).Any();
			return _isexists;
		}

		static IQueryable<ApplicationRole> Step_GetRoleTester(ApplicationDbContext _db)
		{
			return from _g in _db.Roles
				   where _g.Name == "Tester"
				   select _g;
		}
		#endregion
		#endregion

		#region Test for User
		[Test]
		public void TestUserCRUDCase()
		{
			using (var _db = GetDatabase())
			{

				bool _isexists = false;

				_isexists = Step_CheckIsHasUsers(_db);

				Assert.IsFalse(_isexists, "使用者Administrator已存在！");

				//Create
				ApplicationUser _existeduser = Step_CreateUser(_db);

				//Read
				_existeduser = Step_GetUser(_db).SingleOrDefault();

				Assert.IsNotNull(_existeduser, "Administrator 建立失敗！");

				//update

				_existeduser = Step_UpdateUser(_db, _existeduser);

				Assert.IsNotNull(_existeduser, "Administrator 修改失敗！");

				//Delete

				_isexists = Step_DeleteUser(_db, _existeduser);

				Assert.IsFalse(_isexists, "Administrator 刪除失敗！");

				//建立加入角色測試
				Step_TestforUserAddtoRole(_db, out _isexists);

				Assert.IsTrue(_isexists, "加入 Tester 角色失敗！");

				Step_TestforUserRemoveFromRole(_db, out _isexists);

				Assert.IsTrue(_isexists, "移除 Tester 角色失敗！");

				Step_TestforUserAddtoGroup(_db, out _isexists);

				Assert.IsTrue(_isexists, "加入 Users 群組失敗！");

				Step_TestforUserRemovefromGroup(_db, out _isexists);

				Assert.IsTrue(_isexists, "從 Users 群組移除失敗");
			}

		}

		static void Step_TestforUserRemovefromGroup(ApplicationDbContext _db, out bool _isSuccess)
		{
			var _user = Step_GetUser(_db).SingleOrDefault();

			Assert.IsNotNull(_user, "");

			var _group = Step_GetUserInGroups(_user).SingleOrDefault();

			Assert.IsNotNull(_group, "");

			_user.Groups.Remove(_group);
			_db.SaveChanges();

			_group = Step_GetUserInGroups(_user).SingleOrDefault();

			_isSuccess = _group == null;
		}

		static void Step_TestforUserAddtoGroup(ApplicationDbContext _db, out bool _isSuccess)
		{
			var _user = Step_GetUser(_db).SingleOrDefault();

			Assert.IsNotNull(_user, "Administrator 不存在！");
			Step_CreateGroupNamedUsers(_db);

			var _groupforuser = Step_GetGroupWithNamedUsers(_db).SingleOrDefault();

			_user.Groups.Add(_groupforuser);

			_db.SaveChanges();

			_groupforuser = Step_GetUserInGroups(_user).SingleOrDefault();

			_isSuccess = _groupforuser != null;
		}

		static void Step_TestforUserRemoveFromRole(ApplicationDbContext _db, out bool _isSuccess)
		{

			var _user = Step_GetUser(_db).SingleOrDefault();

			Assert.IsNotNull(_user, "Administrator 不存在！");

			var _adminsrole = Step_GetUserInRoles(_user).SingleOrDefault();

			Assert.IsNotNull(_adminsrole, "使用者加入 Tester 角色失敗！");

			_user.Roles.Remove(_adminsrole);
			_db.SaveChanges();

			_user = Step_GetUser(_db).SingleOrDefault();

			Assert.IsNotNull(_user, "找不到 Tester 角色！");

			_adminsrole = Step_GetUserInRoles(_user).SingleOrDefault();

			_isSuccess = (_adminsrole == null);


		}

		static IEnumerable<ApplicationUserGroup> Step_GetUserInGroups(ApplicationUser _user)
		{
			return from _g in _user.Groups
				   where _g.Name == "Users"
				   select _g;
		}

		static IEnumerable<ApplicationRole> Step_GetUserInRoles(ApplicationUser _user)
		{
			return from _r in _user.Roles
				   where _r.Name == "Tester"
				   select _r;
		}

		static void Step_TestforUserAddtoRole(ApplicationDbContext _db, out bool _isSuccess)
		{

			Step_CreateRoleTester(_db);

			var _adminsrole
			= Step_GetRoleTester(_db).SingleOrDefault();

			Assert.IsNotNull(_adminsrole, "Admins 角色建立失敗！");

			Step_CreateUser(_db);

			var _existeduser = Step_GetUser(_db).SingleOrDefault();

			Assert.IsNotNull(_existeduser, "Administrator 建立失敗！");


			_existeduser.Roles.Add(_adminsrole);
			_db.SaveChanges();

			_existeduser = Step_GetUser(_db).SingleOrDefault();

			_isSuccess = _existeduser.Roles.Where(w => w.Name == "Tester").Any();
		}

		static bool Step_DeleteUser(ApplicationDbContext _db, ApplicationUser _existeduser)
		{
			bool _isexists;
			_db.Users.Remove(_existeduser);
			_db.SaveChanges();


			_isexists = (from _g in _db.Users
						 where _g.UserName == "Administrator"
						 select _g).Any();
			return _isexists;
		}

		static ApplicationUser Step_UpdateUser(ApplicationDbContext _db, ApplicationUser _existeduser)
		{
			_existeduser.DisplayName = "Test";
			_db.SaveChanges();

			_existeduser = Step_GetUserByDispalyNameAdmin(_db);
			return _existeduser;
		}

		static ApplicationUser Step_GetUserByDispalyNameAdmin(ApplicationDbContext _db)
		{
			return (from _g in _db.Users
					where _g.UserName == "Administrator"
					&& _g.Void == false
					&& _g.DisplayName == "Test"
					select _g).SingleOrDefault();
		}
		static IQueryable<ApplicationUser> Step_GetUser(ApplicationDbContext _db)
		{
			return from _g in _db.Users
				   where _g.UserName == "Administrator"
				   && _g.Void == false
				   select _g;
		}

		static IQueryable<ApplicationUser> Step_GetAllUser(ApplicationDbContext _db)
		{
			return from _allusers in _db.Users
				   select _allusers;
		}

		static ApplicationUser Step_CreateUser(ApplicationDbContext _db)
		{
			var _existeduser = new ApplicationUser
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
			_existeduser = _db.Users.Add(_existeduser);

			//g.OpreationLogs.Add(new UserOperationLog()
			//{
			//	LogTime = DateTime.Now,
			//	OpreationCode = (int)OperationCodeEnum.Account_Create_Start,
			//	UserId = g.MemberId
			//});

			_db.SaveChanges();
			return _existeduser;
		}

		static bool Step_CheckIsHasUsers(ApplicationDbContext _db)
		{
			bool _isexists;

			IQueryable<ApplicationUser> _users = Step_GetAllUser(_db);

			_isexists = _users.Any();

			if (_isexists)
			{
				foreach (var _u in _users)
				{
					_db.Users.Remove(_u);
				}
				_db.SaveChanges();
			}

			return _isexists;
		}
		#endregion


		[TestFixtureTearDown]
		public void TestFinished()
		{
			//var _db = GetDatabase();
			//_db.Database.Delete();

		}

	}
}

