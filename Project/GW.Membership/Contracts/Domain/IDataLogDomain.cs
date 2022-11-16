using GW.Common;
using GW.Core;
using GW.Membership.Models;

namespace GW.Membership.Contracts.Domain
{
    public interface IDataLogDomain :
        IDomain<DataLogParam, DataLogModel, DataLogList, DataLogSearchResult>
    {
        new IDapperContext Context { get; set; }

    }
}

