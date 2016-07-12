using System;
namespace My.Core.Infrastructures.Implementations
{
	public enum OperationCodeEnum
	{
		Account_Create_Start,
		Account_Create_End_Success = 2,
		Account_Create_End_Fail = 3,
		Account_BatchCreate_Start,
		Account_BatchCreate_End_Success,
		Account_BatchCreate_End_Fail,
		Account_BatchCreate_Rollback,
		Account_Update_Start = 4,
		Account_Update_End,
		Account_ChangePassword_Start = 5,
		Account_ChangePassword_End_Success,
		Account_ChangePassword_End_Fail,
		Account_ChangePassword_Rollback,
	}
}

