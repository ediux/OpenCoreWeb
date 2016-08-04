namespace My.Core.Infrastructures.IoC
{
	public interface IDynamicLoadingforDIFrameworkService : IServiceBase
	{
		/// <summary>
		/// 取得或設定注入式相依性主機執行個體。
		/// </summary>
		/// <value>The host.</value>
		IDIHost Host { get; set; }
	}
}

