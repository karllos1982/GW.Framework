using GW.Common;
using GW.Core;
using GW.Membership.Models;

namespace GW.Membership.Contracts.Data
{
    public interface IUserRolesRepository :
        IRepository<UserRolesParam, UserRolesEntry, UserRolesResult, UserRolesResult>
    {

        Task AlterRole(UserRolesParam obj);
    }
}
