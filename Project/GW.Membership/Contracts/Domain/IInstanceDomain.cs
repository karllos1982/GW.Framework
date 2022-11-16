using GW.Common;
using GW.Core;
using GW.Membership.Models;

namespace GW.Membership.Contracts.Domain
{
    public interface IInstanceDomain :
        IDomain<InstanceParam, InstanceModel, InstanceList, InstanceSearchResult>
    {
        new IDapperContext Context { get; set; }

    }
}
