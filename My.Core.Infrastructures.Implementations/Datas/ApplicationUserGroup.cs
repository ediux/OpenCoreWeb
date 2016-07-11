using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using My.Core.Infrastructures.DAL;

namespace My.Core.Infrastructures.Implementations
{
	[Table("AspNetGroups")]
	public class ApplicationUserGroup : IGroup
	{
		public ApplicationUserGroup()
		{
			_id = -1;
			_leftpos = 0;
			_rightpos = 0;
			_parentid = null;
			_name = string.Empty;

		}

		private int _id;

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id
		{
			get
			{
				return _id;
			}

			set
			{
				_id =	value;
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

		private int? _parentid;

		public int? ParentId
		{
			get
			{
				return _parentid;
			}

			set
			{
				_parentid = value;
					
			}
		}

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

		public virtual ApplicationUserGroup ParentGroup { get; set; }

		public virtual Collection<ApplicationUserGroup> SubGroups { get; set; }

		public virtual Collection<ApplicationUser> Users { get; set; }
	}
}

