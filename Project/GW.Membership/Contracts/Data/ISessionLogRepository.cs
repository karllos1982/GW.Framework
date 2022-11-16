using GW.Common;
using GW.Core;
using GW.Membership.Models;

namespace GW.Membership.Contracts.Data   
{
    public interface ISessionRepository :
        IRepository<SessionParam, SessionModel, SessionList, SessionSearchResult>
    {

        new IDapperContext Context { get; set; }

    }
}
