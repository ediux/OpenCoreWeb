﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using My.Core.Infrastructures.DAL;

namespace My.Core.Infrastructures.Implementations
{
	[Table("AspNetRoles")]
	public class ApplicationRole : IApplicationRole
	{
		public ApplicationRole()
		{
			_id = -1;
			_name = string.Empty;
		}

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		private int _id;
		/// <summary>
		/// Gets or sets the identifier.
		/// </summary>
		/// <value>The identifier.</value>
		public virtual int Id { get { return _id; } set { _id = value; } }

		private string _name;
		/// <summary>
		/// Gets or sets the name.
		/// </summary>
		/// <value>The name.</value>
		[MaxLength(256)]
		public virtual string Name { get { return _name; } set { _name = value; } }

		public virtual Collection<ApplicationUser> Users { get;set;}

	}
}
