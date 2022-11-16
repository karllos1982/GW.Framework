using GW.Common;
using GW.Core;
using GW.Membership.Models;

namespace GW.Membership.Contracts.Data
{
    public interface IInstanceRepository: 
        IRepository<InstanceParam, InstanceModel, InstanceList, InstanceSearchResult>
    {
        new IDapperContext Context { get; set; }

    }
}
