using GW.Common;
using GW.Core;
using GW.Membership.Models;
using GW.Membership.Contracts.Data;

namespace GW.Membership.Contracts.Domain
{
    public interface IInstanceDomain :
        IDomain<InstanceParam, InstanceModel, InstanceList, InstanceSearchResult>
    {
        IMembershipRepositorySet RepositorySet { get; set; }
    }
}
