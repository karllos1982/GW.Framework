using GW.Common;
using GW.Core;
using GW.Membership.Models;

namespace GW.Membership.Contracts.Data   
{
    public interface ISessionLogRepository :
        IRepository<SessionLogParam, SessionLogEntry, SessionLogResult, SessionLogList>
    {

        Task<OperationStatus> SetDateLogout(SessionLogParam obj);


    }
}
