using GW.Common;
using GW.Core;
using GW.Membership.Models;

namespace GW.Membership.Contracts.Data
{
    public interface IObjectPermissionRepository :
        IRepository<ObjectPermissionParam, ObjectPermissionModel, ObjectPermissionList, ObjectPermissionSearchResult>
    {

        new IDapperContext Context { get; set; }

    }
}
