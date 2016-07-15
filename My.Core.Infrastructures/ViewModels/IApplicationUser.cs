using System;
using My.Core.Infrastructures.Datas;

namespace My.Core.Infrastructures.ViewModels
{
	public interface IApplicationUser : IAccount
	{
		int AccessFailedCount { get; set; }
		bool LockoutEnabled { get; set; }
		DateTime? LockoutEndDateUtc { get; set; }
		DateTime CreateTime { get; set; }
		int? CreateUID { get; set; }
		DateTime? LastUpdateTime { get; set; }
		DateTime? LastActivityTime { get; set; }

		DateTime? LastUnlockedTime { get; set; }

		DateTime? LastLoginFailTime { get; set; }

		bool IsOnline { get; }
	}
}

