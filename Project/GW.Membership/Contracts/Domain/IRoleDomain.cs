using GW.Common;
using GW.Core;
using GW.Membership.Models;

namespace GW.Membership.Contracts.Domain
{
    public interface IRoleDomain: 
        IDomain<RoleParam, RoleModel, RoleList, RoleSearchResult>
    {
        new IDapperContext Context { get; set; }

    }
}
