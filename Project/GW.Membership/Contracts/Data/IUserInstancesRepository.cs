using GW.Common;
using GW.Core;
using GW.Membership.Models;

namespace GW.Membership.Contracts.Data
{
    public interface IUserInstancesRepository :
        IRepository<UserInstancesParam, UserInstancesEntry, UserInstancesResult, UserInstancesResult>
    {

        Task AlterInstance(UserInstancesParam obj);


    }
}
