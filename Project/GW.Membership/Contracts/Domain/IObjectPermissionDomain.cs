using GW.Common;
using GW.Core;
using GW.Membership.Models;

namespace GW.Membership.Contracts.Domain
{
    public interface IObjectPermissionDomain :
        IDomain<ObjectPermissionParam, ObjectPermissionModel, ObjectPermissionList, ObjectPermissionSearchResult>
    {
        new IDapperContext Context { get; set; }

    }
}
