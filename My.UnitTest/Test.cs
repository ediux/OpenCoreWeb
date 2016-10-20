using NUnit.Framework;
using System;
using System.Collections.ObjectModel;
using My.Core.Infrastructures.Implementations;
using System.Linq;
using My.Core.Infrastructures.Implementations.Models;

namespace My.UnitTest
{
	[TestFixture()]
	public class Test
	{
		[Test()]
		public void TestCase()
		{
            //string connectionstr = "server=localhost;port=3306;database=OpenCoreWeb;uid=root;password=!QAZ2wsx";

            //My.Core.Infrastructures.Implementations.ApplicationDbContext _db = new ApplicationDbContext(connectionstr);
            //_db.Database.CreateIfNotExists();
            //var g = new ApplicationUserGroup { ParentGroup = null, Name = "Users", SubGroups = new Collection<ApplicationUserGroup>(), Users = new Collection<ApplicationUser>() };
            //g = _db.Groups.Add(g);
            //_db.SaveChanges();

            //var gs = (from tt in _db.Groups
            //          where tt.GroupId == g.GroupId
            //          select tt).SingleOrDefault();

            //var newuser = new ApplicationUser() { DisplayName = "Administrator" };
            //newuser.Groups = new Collection<ApplicationUserGroup>();
            //newuser.Roles = new Collection<ApplicationRole>();
            //newuser.OpreationLogs = new Collection<UserOperationLog>();

            //if (gs != null)
            //{
            //    newuser.Groups.Add(gs);
            //}
            //_db.Users.Add(newuser);
            //_db.SaveChanges();
		}
	}
}

