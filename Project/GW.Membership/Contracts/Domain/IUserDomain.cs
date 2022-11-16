using GW.Common;
using GW.Core;
using GW.Membership.Models;

namespace GW.Membership.Contracts.Domain
{
    public interface IUserDomain :
        IDomain<UserParam, UserModel, UserList, UserSearchResult>
    {
        new IDapperContext Context { get; set; }

    }
}
