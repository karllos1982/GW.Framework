using GW.Common;
using GW.Core;
using GW.Membership.Models;

namespace GW.Membership.Contracts.Domain
{
    public interface IMembershipManager: IManager
    {
       
        IDataLogDomain DataLog { get; set; }

        IInstanceDomain Instance { get; set; }

        IObjectPermissionDomain ObjectPermission { get; set; }

        IPermissionDomain Permission { get; set; }

        IRoleDomain Role { get; set; }

        ISessionLogDomain SessionLog { get; set; }

        IUserDomain User { get; set; }

        //




        OperationStatus CreateNewUser(NewUser data, bool gocommit, object userid);

        OperationStatus Login(UserLogin model);

        List<UserPermissions> GetUserPermissions(Int64 roleid, Int64 userid);

        PERMISSION_STATE_ENUM CheckPermission(List<UserPermissions> permissions,
            string objectcode, PERMISSION_CHECK_ENUM type);

        PermissionsState GetPermissionsState(List<UserPermissions> permissions,
            string objectcode, bool allownone);

        void RegisterLoginState(UserLogin model, UpdateUserLogin stateinfo);


        void Logout(Int64 userid);

        OperationStatus GetTemporaryPassword(ChangeUserPassword model);

        OperationStatus GetActiveAccountCode(ActiveUserAccount model);

        OperationStatus GetChangePasswordCode(ChangeUserPassword model);

        OperationStatus ChangeUserProfileImage(ChangeUserImage model);


    }
}
