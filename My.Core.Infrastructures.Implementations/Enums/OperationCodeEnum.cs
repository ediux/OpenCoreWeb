using System;
namespace My.Core.Infrastructures.Implementations
{
	public enum OperationCodeEnum
	{
		Undefined = 0,
		Account_Create_Start = 1,
		Account_Create_End_Success = 2,
		Account_Create_End_Fail = 3,
		Account_Create_Rollback = 4,
		Account_BatchCreate_Start = 5,
		Account_BatchCreate_End_Success = 6,
		Account_BatchCreate_End_Fail = 7,
		Account_BatchCreate_Rollback = 8,
		Account_Update_Start = 9,
		Account_Update_End_Success = 10,
		Account_Update_End_Fail = 11,
		Account_Update_Rollback = 12,
		Account_Delete_Start = 13,
		Account_Delete_End_Success = 14,
		Account_Delete_End_Fail = 15,
		Account_Delete_Rollback = 16,
		Account_ChangePassword_Start = 17,
		Account_ChangePassword_End_Success = 18,
		Account_ChangePassword_End_Fail = 19,
		Account_ChangePassword_Rollback = 20,
		UserDefined = 65535
	}
}

