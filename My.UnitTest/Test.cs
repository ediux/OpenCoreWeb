using NUnit.Framework;
using System.Collections.ObjectModel;
using My.Core.Infrastructures.Implementations;
using System.Linq;
using System.Reflection;

namespace My.UnitTest
{
	[TestFixture()]
	public class Test
	{
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

		[TestFixtureSetUp]
		public void TestInit()
		{

			ApplicationDbContext _db = GetDatabase();

			_db.Database.CreateIfNotExists();

			var _alldefines = from _g in _db.OperationCodeDefines
							  select _g;

			if (_alldefines.Any())
			{
				foreach (var _define in _alldefines)
				{
					_db.OperationCodeDefines.Remove(_define);
				}
				_db.SaveChanges();
			}

			_db.Dispose();

		}

		[Test()]
		public void TestUserOpearationCodeDefineCRUDCase()
		{
			using (var _db = GetDatabase())
			{
				var _alldefines = from _g in _db.OperationCodeDefines
								  where _g.OpreationCode == (int)OperationCodeEnum.Account_BatchCreate_Start
								  select _g;

				bool _isexists = _alldefines.Any();

				if (_isexists)
				{
					foreach (var removecode in _alldefines)
					{
						_db.OperationCodeDefines.Remove(removecode);
					}
					_db.SaveChanges();
				}

				Assert.IsFalse(_isexists, "操作代碼定義已經存在！");

				var _codedefine = new UserOperationCodeDefine();

				_codedefine.Description = "Account_BatchCreate_Start";
				_codedefine.OpreationCode = (int)OperationCodeEnum.Account_BatchCreate_Start;

				_db.OperationCodeDefines.Add(_codedefine);
				_db.SaveChanges();



				//Read
				_isexists = (from _g in _db.OperationCodeDefines
							 where _g.OpreationCode == (int)OperationCodeEnum.Account_BatchCreate_Start
							 select _g
						 ).Any();

				Assert.IsTrue(_isexists, "操作代碼 Account_BatchCreate_Start 不存在！");

				//Update
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

				Assert.IsNotNull(_updateitem, "測試項目遺失！");

				Assert.AreSame("批次創立帳號開始", _updateitem.Description);

				//Delete
				_db.OperationCodeDefines.Remove(_updateitem);
				_db.SaveChanges();

				_isexists = (from _g in _db.OperationCodeDefines
							 where _g.OpreationCode == (int)OperationCodeEnum.Account_BatchCreate_Start
							 select _g).Any();

				Assert.IsFalse(_isexists, "測試刪除失敗！");
				Assert.Pass();
			}
		}

		[Test()]
		public void TestGroupCRUDCase()
		{
			using (var _db = GetDatabase())
			{
				var existsgroups = from _g in _db.Groups
								   where _g.Name == "Users"
								   select _g;
				bool _isexists = existsgroups.Any();

				if (_isexists)
				{
					foreach (var removegroup in existsgroups)
					{
						removegroup.Users.Clear();

						_db.Groups.Remove(removegroup);
					}
					_db.SaveChanges();
				}

				Assert.IsFalse(_isexists, "Users 群組已存在！");

				//Create
				var g = new ApplicationUserGroup
				{
					ParentGroup = null,
					Name = "Users",
					SubGroups = new Collection<ApplicationUserGroup>(),
					Users = new Collection<ApplicationUser>()
				};
				g = _db.Groups.Add(g);
				_db.SaveChanges();

				//Read
				_isexists = (from _g in _db.Groups
							 where _g.Name == "Users"
							 select _g
								 ).Any();

				Assert.IsTrue(_isexists, "找不到 Users 群組！");

				//Update

				var currentgroup = (from _g in _db.Groups
									where _g.Name == "Users"
									select _g).SingleOrDefault();

				Assert.IsNotNull(currentgroup, "找不到 Users 群組！");
				currentgroup.Name = "Admins";
				_db.SaveChanges();


				_isexists = (from _g in _db.Groups
							 where _g.Name == "Admins"
							 select _g
								 ).Any();

				Assert.IsTrue(_isexists, "Users 群組名稱變更成 Admins 失敗！");

				//Delete
				currentgroup = (from _g in _db.Groups
								where _g.Name == "Admins"
								select _g).SingleOrDefault();

				Assert.IsNotNull(currentgroup, "找不到 Admins 群組！");

				_db.Groups.Remove(currentgroup);
				_db.SaveChanges();

				_isexists = (from _g in _db.Groups
							 where _g.Name == "Admins"
							 select _g
								 ).Any();

				Assert.IsFalse(_isexists, "刪除 Admins 群組失敗！");
				Assert.Pass();

			}
		}

		[Test()]
		public void TestRoleCRUDCase()
		{
			using (var _db = GetDatabase())
			{
				var existsroles = from _g in _db.Roles
								  where _g.Name == "Tester"
								  select _g;

				bool _isexists = existsroles.Any();

				if (_isexists)
				{
					foreach (var removerole in existsroles)
					{
						removerole.Users.Clear();

						_db.Roles.Remove(removerole);
					}
					_db.SaveChanges();
				}

				Assert.IsFalse(_isexists, "Tester 角色已存在！");

				//Create
				var g = new ApplicationRole
				{
					Name = "Tester",
				};

				g = _db.Roles.Add(g);
				_db.SaveChanges();

				//Read
				_isexists = (from _g in _db.Roles
							 where _g.Name == "Tester"
							 select _g
								 ).Any();

				Assert.IsTrue(_isexists, "找不到 Tester 角色！");

				//Update

				var currentrole = (from _g in _db.Roles
								   where _g.Name == "Tester"
								   select _g).SingleOrDefault();

				Assert.IsNotNull(currentrole, "找不到 Tester 角色！");
				currentrole.Name = "Master";
				_db.SaveChanges();


				_isexists = (from _g in _db.Roles
							 where _g.Name == "Master"
							 select _g
								 ).Any();

				Assert.IsTrue(_isexists, "Tester 角色名稱變更成 Master 失敗！");

				//Delete
				currentrole = (from _g in _db.Roles
							   where _g.Name == "Master"
							   select _g).SingleOrDefault();

				Assert.IsNotNull(currentrole, "找不到 Master 角色！");

				_db.Roles.Remove(currentrole);
				_db.SaveChanges();

				_isexists = (from _g in _db.Roles
							 where _g.Name == "Master"
							 select _g
								 ).Any();

				Assert.IsFalse(_isexists, "刪除 Master 角色失敗！");


			}


		}

		[Test()]
		public void TestUserCRUDCase()
		{
			using (var _db = GetDatabase())
			{
				var _users = from _g in _db.Users
							 where _g.UserName == "Administrator"
							 select _g;
				bool _isexists = _users.Any();

				if (_isexists)
				{
					var _user = _users.SingleOrDefault();

					_db.Users.Remove(_user);
					_db.SaveChanges();
				}

				Assert.IsFalse(_isexists, "使用者Administrator已存在！");

				//Create
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

				//Read
				_existeduser = (from _g in _db.Users
								where _g.UserName == "Administrator"
								&& _g.Void == false
								select _g).SingleOrDefault();

				Assert.IsNotNull(_existeduser, "Administrator 建立失敗！");

				//update

				_existeduser.DisplayName = "Test";


				_db.SaveChanges();

				_existeduser = (from _g in _db.Users
								where _g.UserName == "Administrator"
								&& _g.Void == false
								&& _g.DisplayName == "Test"
								select _g).SingleOrDefault();

				Assert.IsNotNull(_existeduser, "Administrator 修改失敗！");

				//Delete

				_db.Users.Remove(_existeduser);
				_db.SaveChanges();


				_isexists = (from _g in _db.Users
							 where _g.UserName == "Administrator"
							 select _g).Any();

				Assert.IsFalse(_isexists, "Administrator 刪除失敗！");

				//建立加入角色測試
				var _adminsrole
				 = (from _roles in _db.Roles
					where _roles.Name == "Admins"
					select _roles).SingleOrDefault();

				if (_adminsrole == null)
				{
					var _role = new ApplicationRole()
					{
						Name = "Admins"
					};

					_db.Roles.Add(_role);
					_db.SaveChanges();

					_adminsrole = (from _roles in _db.Roles
								   where _roles.Name == "Admins"
								   select _roles).SingleOrDefault();
				}

				Assert.IsNotNull(_adminsrole, "Admins 角色建立失敗！");

				_existeduser = (from _user in _db.Users
								where _user.UserName == "Administrator"
								select _user).SingleOrDefault();

				if (_existeduser == null)
				{
					_existeduser = new ApplicationUser()
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

					_db.Users.Add(_existeduser);
					_db.SaveChanges();
				}

				_existeduser = (from _user in _db.Users
								where _user.UserName == "Administrator"
								select _user).SingleOrDefault();

				Assert.IsNotNull(_existeduser, "Administrator 建立失敗！");

				_existeduser.Roles.Add(_adminsrole);
				_db.SaveChanges();

				_existeduser = (from _user in _db.Users
								where _user.UserName == "Administrator"
								select _user).SingleOrDefault();

				Assert.IsTrue(_existeduser.Roles.Where(w => w.Name == "Admins").Any(),"加入Admins角色失敗！");
				                                       



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

