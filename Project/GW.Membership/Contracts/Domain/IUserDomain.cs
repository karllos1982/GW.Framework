using GW.Common;
using GW.Core;
using GW.Membership.Models;

namespace GW.Membership.Contracts.Domain
{
    public interface IUserDomain :
        IDomain<UserParam, UserEntry, UserResult, UserList>
    {
        Task<UserResult> GetByEmail(string email);

        Task<OperationStatus> UpdateUserLogin(UpdateUserLogin model);

        Task<OperationStatus> SetPasswordRecoveryCode(ChangeUserPassword model);

        Task<OperationStatus> ChangeUserPassword(ChangeUserPassword model);

        Task<OperationStatus> ActiveUserAccount(ActiveUserAccount model);

        Task<OperationStatus> ChangeUserProfileImage(ChangeUserImage model);

        Task<OperationStatus> UpdateLoginFailCounter(UpdateUserLoginFailCounter model);

        Task<OperationStatus> ChangeState(UserChangeState model);

        Task<OperationStatus> SetDateLogout(Int64 userid);

        Task<UserRolesEntry> AddRoleToUser(Int64 userid, Int64 roleid);

        Task<UserRolesEntry> RemoveRoleFromUser(Int64 userid, Int64 roleid);

        Task<UserInstancesEntry> AddInstanceToUser(Int64 userid, Int64 instanceid);

        Task<UserInstancesEntry> RemoveInstanceFromUser(Int64 userid, Int64 instanceid);

        Task AlterRole(Int64 userroleid, Int64 newroleid);

        Task AlterInstance(Int64 userinstanceid, Int64 newinstanceid);

    }
}
