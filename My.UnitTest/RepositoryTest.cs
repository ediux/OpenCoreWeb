using System;
using NUnit.Framework;
using My.Core.Infrastructures;
using My.Core.Infrastructures.Implementations;
using My.Core.Infrastructures.Implementations.Repositories;
using My.Core.Infrastructures.Implementations.Datas;
using My.Core.Infrastructures.Datas;

namespace My.UnitTest
{
	[TestFixture]
	public class RepositoryTest
	{

		[TestFixtureSetUp]
		public void TestInit()
		{
		}

		[Test]
		public void TestCreateUserUsingRepository()
		{
			IUnitofWork _defaultunitofwork = new DefaultUnitofWork();
			Assert.IsNotNull(_defaultunitofwork);
			IAccountRepository _acccountrepositroy = new AccountRepository(_defaultunitofwork);
			Assert.IsNotNull(_acccountrepositroy);

			IAccount _newuser = _acccountrepositroy.Create(new ApplicationUser()
			{
				DisplayName = "系統管理員",
				Password = "",
				TwoFactorEnabled = false,
				UserName = "Administrator",
				Void = false
			});

			Assert.IsNotNull(_newuser);
			Assert.Pass();
		}

	}
}

