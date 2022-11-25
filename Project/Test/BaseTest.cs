

using GW.Membership.Contracts.Domain;

namespace Test
{
    public class TesteConfigs : ISettings
    {
        public TesteConfigs()
        {
            this.Sources = new SourceConfig[1];
            Sources[0] = new SourceConfig()
            {
                SourceName = "Default",
                SourceValue = "[INSERT YOUR CONNECTION STRING]"

            };
         }
            
            
        public SourceConfig[] Sources { get; set; }
        public string SiteURL { get; set; }
        public string ProfileImageDir { get; set; }
        public string ApplicationName { get; set; }
        public MailSettings MailSettings { get; set; }
    }

    public class Resources
    {
        public Resources()
        {
            Settings = new TesteConfigs();
            Context = new DapperContext(Settings);
            Repository = new MembershipRepositorySet(Context);
            Domain = new MembershipManager(Context, Repository);
            Context.Begin(0);
        }

        public ISettings Settings { get; set; }

        public IDapperContext Context { get; set; }

        public MembershipRepositorySet Repository { get; set; }

        public MembershipManager Domain { get; set; }

        public void finalize()
        {
            OperationStatus ret =  Context.End();

            Console.Write("Exec Status => " + ret.Status.ToString());
            if (ret.Error != null)  
            {
                Console.Write("Msg => " + ret.Error.Message.ToString());
            }
        }

        public void Perform_ShouldBeTrue()
        {
            if (Domain.Context.ExecutionStatus.Error != null)
            {
                Domain.Context.ExecutionStatus.Status.ShouldBeTrue(Domain.Context.ExecutionStatus.Error.Message);
            }
            else
            {
                Domain.Context.ExecutionStatus.Status.ShouldBeTrue();
            }
        }

        public void Perform_ShouldBeFalse(OperationStatus status )
        {
            if (status.Error != null)
            {
                status.Status.ShouldBeFalse(status.Error.Message);
            }
            else
            {
                status.Status.ShouldBeFalse();
            }
        }


        //public static ISettings BuildSettings()
        //{
        //    return new TesteConfigs();
        //}

        //public static IDapperContext BuildContext()
        //{           

        //    return new DapperContext(BuildSettings());
        //}

        //public static IMembershipRepositorySet GetMembershipRepositorySet(IDapperContext context)
        //{            

        //    return new MembershipRepositorySet(context);
        //}

        //public static IMembershipManager GetMembershipManager()
        //{
        //    IDapperContext context = BuildContext();
        //    return new MembershipManager(context, GetMembershipRepositorySet(context)); 
        //}


    }

    public class BaseTest
    {
        public Int64 SysDefaultUser
        {
            get
            {
                return 1001;
            }
        }

        protected Resources Resource { get; set; }

        protected IMembershipManager Domain;

        protected OperationStatus status;


        public void init()
        {
            //Resource = new Resources();

            //Domain = Resource.Domain;

           // Domain.Context.Begin(0); 
        }


        public void finalize()
        {
            try
            {

               Domain.Context.End();             

            }
            catch (Exception ex)
            {

            }

        }

        protected void Perform_ShouldBeTrue()
        {
            if (Domain.Context.ExecutionStatus.Error != null)
            {
                Domain.Context.ExecutionStatus.Status.ShouldBeTrue(Domain.Context.ExecutionStatus.Error.Message);
            }
            else
            {
                Domain.Context.ExecutionStatus.Status.ShouldBeTrue();
            }
        }

        protected void Perform_ShouldBeFalse()
        {
            if (Domain.Context.ExecutionStatus.Error != null)
            {
                Domain.Context.ExecutionStatus.Status.ShouldBeFalse(Domain.Context.ExecutionStatus.Error.Message);
            }
            else
            {
                Domain.Context.ExecutionStatus.Status.ShouldBeFalse();
            }
        }
    }

}
