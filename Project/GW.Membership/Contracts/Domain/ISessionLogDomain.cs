using GW.Core;
using GW.Membership.Models;

namespace GW.Membership.Contracts.Domain
{
    public interface ISessionLogDomain :
        IDomain<SessionLogParam, SessionLogModel, SessionLogList, SessionLogSearchResult>
    {

    }
}
