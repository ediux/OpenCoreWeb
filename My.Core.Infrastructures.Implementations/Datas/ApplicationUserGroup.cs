using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using My.Core.Infrastructures.Datas;

namespace My.Core.Infrastructures.Implementations.Datas
{
	[Table("AspNetGroups")]
	public class ApplicationUserGroup : IGroup
	{
		public ApplicationUserGroup()
		{
			_id = -1;
			_leftpos = 0;
			_rightpos = 0;
			//_parentid = null;
			_name = string.Empty;
			parentgroup = new Lazy<ApplicationUserGroup>(() => null);
			subgroups = new Lazy<Collection<ApplicationUserGroup>>(()=>new Collection<ApplicationUserGroup>());
			users = new Lazy<Collection<ApplicationUser>>(() => new Collection<ApplicationUser>());
		}

		private int _id;

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int GroupId
		{
			get
			{
				return _id;
			}

			set
			{
				_id = value;
			}
		}
		private int _leftpos;

		public int LeftPos
		{
			get
			{
				return _leftpos;
			}

			set
			{
				_leftpos = value;
			}
		}
		private string _name;

		[MaxLength(256)]
		public string Name
		{
			get
			{
				return _name;
			}

			set
			{
				_name = value;
			}
		}

		//private int? _parentid;

		//public int? ParentId
		//{
		//	get
		//	{
		//		return _parentid;
		//	}

		//	set
		//	{
		//		_parentid = value;

		//	}
		//}

		private int _rightpos;

		public int RightPos
		{
			get
			{
				return _rightpos;
			}

			set
			{
				_rightpos = value;
			}
		}

		private Lazy<ApplicationUserGroup> parentgroup;

		public virtual ApplicationUserGroup ParentGroup { get { return parentgroup.Value; } set { parentgroup = new Lazy<ApplicationUserGroup>(() => value); } }

		private Lazy<Collection<ApplicationUserGroup>> subgroups;

		public virtual Collection<ApplicationUserGroup> SubGroups
		{
			get
			{
				return subgroups.Value;
			}
			set
			{
				subgroups = new Lazy<Collection<ApplicationUserGroup>>(() => value);
			}
		}

		private Lazy<Collection<ApplicationUser>> users;
		public virtual Collection<ApplicationUser> Users
		{
			get
			{
				return users.Value;
			}
			set
			{
				users = new Lazy<Collection<ApplicationUser>>(() => value);
			}
		}
	}
}

