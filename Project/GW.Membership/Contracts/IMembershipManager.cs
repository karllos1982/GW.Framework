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

        ILocalizationTextDomain LocalizationText { get; set; }  

        //




        Task<UserEntry> CreateNewUser(NewUser data, bool gocommit, object userid);

        Task<UserResult> Login(UserLogin model);

        Task<List<UserPermissions>> GetUserPermissions(Int64 roleid, Int64 userid);

        Task<PERMISSION_STATE_ENUM> CheckPermission(List<UserPermissions> permissions,
            string objectcode, PERMISSION_CHECK_ENUM type);

        Task<PermissionsState> GetPermissionsState(List<UserPermissions> permissions,
            string objectcode, bool allownone);

        Task RegisterLoginState(UserLogin model, UpdateUserLogin stateinfo);


        Task Logout(Int64 userid);

        Task<OperationStatus> GetTemporaryPassword(ChangeUserPassword model);

        Task<OperationStatus> GetActiveAccountCode(ActiveUserAccount model);

        Task<OperationStatus> GetChangePasswordCode(ChangeUserPassword model);

        Task<OperationStatus> ChangeUserProfileImage(ChangeUserImage model);


    }
}
