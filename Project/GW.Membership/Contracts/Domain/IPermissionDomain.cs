using GW.Common;
using GW.Core;
using GW.Membership.Models;

namespace GW.Membership.Contracts.Domain
{
    public interface IPermissionDomain :
        IDomain<PermissionParam, PermissionModel, PermissionList, PermissionSearchResult>
    {

       Task<List<PermissionSearchResult>> GetPermissionsByRoleUser(Int64 roleid, Int64 userid);

    }
}
