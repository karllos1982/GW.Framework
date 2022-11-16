using GW.Common;
using GW.Core;
using GW.Membership.Models;

namespace GW.Membership.Contracts.Data
{
    public interface IUserRolesRepository :
        IRepository<UserRolesParam, UserRolesModel, UserRolesList, UserRolesSearchResult>
    {

        new IDapperContext Context { get; set; }

    }
}
