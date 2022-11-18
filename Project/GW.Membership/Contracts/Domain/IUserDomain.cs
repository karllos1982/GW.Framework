using GW.Common;
using GW.Core;
using GW.Membership.Models;

namespace GW.Membership.Contracts.Domain
{
    public interface IUserDomain :
        IDomain<UserParam, UserModel, UserList, UserSearchResult>
    {
        UserModel GetByEmail(string email);

        OperationStatus UpdateUserLogin(UpdateUserLogin model);

        OperationStatus SetPasswordRecoveryCode(ChangeUserPassword model);

        OperationStatus ChangeUserPassword(ChangeUserPassword model);

        OperationStatus ActiveUserAccount(ActiveUserAccount model);

        OperationStatus ChangeUserProfileImage(ChangeUserImage model);

        OperationStatus UpdateLoginFailCounter(UpdateUserLoginFailCounter model);

        OperationStatus ChangeState(UserChangeState model);

        OperationStatus SetDateLogout(Int64 userid);

        OperationStatus AddRoleToUser(Int64 userid, Int64 roleid, bool gocommit);

        OperationStatus RemoveRoleFromUser(Int64 userid, Int64 roleid, bool gocommit);

        OperationStatus AddInstanceToUser(Int64 userid, Int64 instanceid, bool gocommit);

        OperationStatus RemoveInstanceFromUser(Int64 userid, Int64 instanceid, bool gocommit);

    }
}
