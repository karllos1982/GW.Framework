using GW.Core;
using GW.Membership.Contracts.Domain;
using GW.Membership.Contracts.Data;

namespace GW.Membership.Domain
{
    public class MembershipManager : IMembershipManager
    {
        public MembershipManager(IContext context, IMembershipRepositorySet repositorySet)
        {
            Context = context;
            InitializeDomains(context, repositorySet); 
        }

        public IContext Context { get; set; }

        public IDataLogDomain DataLog { get; set; }

        public IInstanceDomain Instance { get; set; }

        public IObjectPermissionDomain ObjectPermission { get; set; }

        public IPermissionDomain Permission { get; set; }

        public IRoleDomain Role { get; set; }

        public ISessionLogDomain SessionLog { get; set; }

        public IUserDomain User { get; set; }

        
        public void InitializeDomains(IContext context, IRepositorySet repositorySet)
        {
            DataLog = new DataLogDomain(context, (IMembershipRepositorySet)repositorySet);
            Instance = new InstanceDomain(context, (IMembershipRepositorySet)repositorySet);
            ObjectPermission = new ObjectPermissionDomain(context, (IMembershipRepositorySet)repositorySet);
            Permission = new PermissionDomain(context, (IMembershipRepositorySet)repositorySet);
            Role = new RoleDomain(context, (IMembershipRepositorySet)repositorySet);
            SessionLog = new SessionLogDomain(context, (IMembershipRepositorySet)repositorySet);
            User = new UserDomain(context, (IMembershipRepositorySet)repositorySet);

        }

    }
}
