using GW.Common;
using GW.Core;
using GW.Membership.Models;

namespace GW.Membership.Contracts.Data
{
    public interface IUserRepository :
        IRepository<UserParam, UserModel, UserList, UserSearchResult>
    {

        Task<UserModel> GetByEmail(string email);

        Task<OperationStatus> UpdateUserLogin(UpdateUserLogin model);

        Task<OperationStatus> SetPasswordRecoveryCode(SetPasswordRecoveryCode model);

        Task<OperationStatus> ChangeUserPassword(ChangeUserPassword model);

        Task<OperationStatus> ActiveUserAccount(ActiveUserAccount model);

        Task<OperationStatus> ChangeUserProfileImage(ChangeUserImage model);

        Task<OperationStatus> UpdateLoginFailCounter(UpdateUserLoginFailCounter model);

        Task<OperationStatus> ChangeState(UserChangeState model);

    }
}
