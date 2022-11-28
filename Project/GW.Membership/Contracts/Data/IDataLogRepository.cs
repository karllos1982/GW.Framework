using GW.Common;
using GW.Core;
using GW.Membership.Models;

namespace GW.Membership.Contracts.Data
{
    public interface IDataLogRepository :
        IRepository<DataLogParam, DataLogEntry, DataLogResult, DataLogList>
    {
        Task<List<DataLogTimelineModel>> GetDataLogTimeline(Int64 recordID);

        Task<List<TabelasValueModel>> GetTableList();

    }
}
