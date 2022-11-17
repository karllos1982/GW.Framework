using GW.Common;
using GW.Core;
using GW.Membership.Models;

namespace GW.Membership.Contracts.Data   
{
    public interface ISessionLogRepository :
        IRepository<SessionLogParam, SessionLogModel, SessionLogList, SessionLogSearchResult>
    {

        
    }
}
