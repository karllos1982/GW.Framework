using GW.Common;
using GW.Core;
using GW.Membership.Models;

namespace GW.Membership.Contracts.Data
{
    public interface IPermissionRepository :
        IRepository<PermissionParam, PermissionModel, PermissionList, PermissionSearchResult>
    {

       
    }
}
