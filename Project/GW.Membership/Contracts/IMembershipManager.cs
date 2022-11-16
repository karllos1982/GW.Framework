using GW.Core;


namespace GW.Membership.Contracts.Domain
{
    public interface IMembershipManager: IManager
    {
        new IDapperContext Context { get; set; }

        IDataLogDomain DataLog { get; set; }

        IInstanceDomain Instance { get; set; }

        IObjectPermissionDomain ObjectPermission { get; set; }

        IPermissionDomain Permission { get; set; }

        IRoleDomain Role { get; set; }

        ISessionLogDomain Session { get; set; }

        IUserDomain User { get; set; }


    }
}
