using GW.Common;
using GW.Core;
using GW.Membership.Contracts.Data;

namespace GW.Membership.Data
{
    public class MembershipRepositorySet: IMembershipRepositorySet
    {
        public MembershipRepositorySet(IContext context)
        {
            this.InitializeRespositories(context);
        }

        public IDataLogRepository DataLog { get; set; }

        public IInstanceRepository Instance { get; set; }

        public IObjectPermissionRepository ObjectPermission { get; set; }

        public IPermissionRepository Permission { get; set; }

        public IRoleRepository Role { get; set; }

        public ISessionLogRepository SessionLog { get; set; }

        public IUserInstancesRepository UserInstances { get; set; }

        public IUserRepository User { get; set; }

        public IUserRolesRepository UserRoles { get; set; }

        public ILocalizationTextRepository LocalizationText { get; set; }

        public void InitializeRespositories(IContext context)
        {
            DapperContext ctx = (DapperContext)context; 

            this.DataLog = new DataLogRepository(ctx);
            this.Instance = new InstanceRepository(ctx);
            this.ObjectPermission = new ObjectPermissionRepository(ctx);
            this.Permission = new PermissionRepository(ctx);
            this.Role = new RoleRepository(ctx);
            this.SessionLog = new SessionLogRepository(ctx);
            this.UserInstances = new UserInstancesRepository(ctx);
            this.User = new UserRepository(ctx);
            this.UserRoles = new UserRolesRepository(ctx); 
            this.LocalizationText = new LocalizationTextRepository(ctx);   
        }
    }

}
