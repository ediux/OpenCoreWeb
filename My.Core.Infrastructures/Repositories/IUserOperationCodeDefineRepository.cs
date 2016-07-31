using My.Core.Infrastructures.Datas;

namespace My.Core.Infrastructures
{
	public interface IUserOperationCodeDefineRepository : IRepositoryBase<IUserOperationCodeDefine>
	{
		/// <summary>
		/// Finds the by code.
		/// </summary>
		/// <returns>The by code.</returns>
		/// <param name="code">Code.</param>
		IUserOperationCodeDefine FindByCode(int code);
	}
}

