using GW.Common;
using GW.Core;
using GW.Membership.Models;

namespace GW.Membership.Contracts.Data
{
    public interface IUserRepository :
        IRepository<UserParam, UserModel, UserList, UserSearchResult>
    {

        UserModel GetByEmail(string email);

        OperationStatus UpdateUserLogin(UpdateUserLogin model);

        OperationStatus SetPasswordRecoveryCode(SetPasswordRecoveryCode model);

        OperationStatus ChangeUserPassword(ChangeUserPassword model);

        OperationStatus ActiveUserAccount(ActiveUserAccount model);

        OperationStatus ChangeUserProfileImage(ChangeUserImage model);

        OperationStatus UpdateLoginFailCounter(UpdateUserLoginFailCounter model);

        OperationStatus ChangeState(UserChangeState model);

    }
}
