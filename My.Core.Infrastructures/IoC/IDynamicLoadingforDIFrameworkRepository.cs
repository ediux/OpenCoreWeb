namespace My.Core.Infrastructures.IoC
{
	public interface IDynamicLoadingforDIFrameworkRepository<TRepository> : IRepositoryBase<TRepository>
		where TRepository:IDataModel
	{
		/// <summary>
		/// 取得或設定注入式相依性主機執行個體。
		/// </summary>
		/// <value>The host.</value>
		IDIHost Host { get; set; }
	}
}

