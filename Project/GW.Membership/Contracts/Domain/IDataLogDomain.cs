using GW.Common;
using GW.Core;
using GW.Membership.Models;
using GW.Membership.Contracts.Data;

namespace GW.Membership.Contracts.Domain
{
    public interface IDataLogDomain :
        IDomain<DataLogParam, DataLogModel, DataLogList, DataLogSearchResult>
    {
         IMembershipRepositorySet RepositorySet { get; set;  }

        List<DataLogTimelineModel> GetTimeLine(Int64 recordID);

        List<TabelasValueModel> GetTableList();

    }

}

