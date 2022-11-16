using GW.Common;
using GW.Core;

namespace GW.Membership.Contracts
{
    public interface IMembershipRepositorySet: IRepositorySet
    {

        IDataLogRepository DataLog { get; set; }
        
        IInstanceRepository Instance { get; set; }

        IObjectPermissionRepository ObjectPermission { get; set; }

        IPermissionRepository Permission { get; set; }

        IRoleRepository Role { get; set; }

        ISessionRepository Session { get; set; }

        IUserInstancesRepository UserInstances { get; set; }

        IUserRepository User { get; set; }

        IUserRolesRepository UserRoles { get; set; }


    }
}
