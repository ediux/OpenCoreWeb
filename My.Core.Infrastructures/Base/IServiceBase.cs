using System;
using My.Core.Infrastructures.IoC;
using My.Core.Infrastructures.Logs;

namespace My.Core.Infrastructures
{
	/// <summary>
	/// 定義一組統一之邏輯服務層介面
	/// </summary>
	public interface IServiceBase
	{
		/// <summary>
		/// Gets or sets the logger.
		/// </summary>
		/// <value>The logger.</value>
		ILogWriter Logger { get; set; }
	}
}

