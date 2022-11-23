using GW.Common;
using GW.Core;
using GW.Membership.Models;

namespace GW.Membership.Contracts.Domain
{
    public interface IUserDomain :
        IDomain<UserParam, UserModel, UserList, UserSearchResult>
    {
        Task<UserModel> GetByEmail(string email);

        Task<OperationStatus> UpdateUserLogin(UpdateUserLogin model);

        Task<OperationStatus> SetPasswordRecoveryCode(ChangeUserPassword model);

        Task<OperationStatus> ChangeUserPassword(ChangeUserPassword model);

        Task<OperationStatus> ActiveUserAccount(ActiveUserAccount model);

        Task<OperationStatus> ChangeUserProfileImage(ChangeUserImage model);

        Task<OperationStatus> UpdateLoginFailCounter(UpdateUserLoginFailCounter model);

        Task<OperationStatus> ChangeState(UserChangeState model);

        Task<OperationStatus> SetDateLogout(Int64 userid);

        Task<UserRolesModel> AddRoleToUser(Int64 userid, Int64 roleid, bool gocommit);

        Task<UserRolesModel> RemoveRoleFromUser(Int64 userid, Int64 roleid, bool gocommit);

        Task<UserInstancesModel> AddInstanceToUser(Int64 userid, Int64 instanceid, bool gocommit);

        Task<UserInstancesModel> RemoveInstanceFromUser(Int64 userid, Int64 instanceid, bool gocommit);

    }
}
