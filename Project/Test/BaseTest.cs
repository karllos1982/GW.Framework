
using GW.Membership.Contracts.Domain;
using System.Data;
using System.Data.SqlClient;
using System.Transactions;

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
                //SourceValue = "[INSERT YOUR CONNECTION STRING]"
                SourceValue = @"data source=sql5086.site4now.net;initial catalog=db_a83278_gwmodel;persist security info=True;
                    user id=db_a83278_gwmodel_admin;password=synd_p123;MultipleActiveResultSets=True;App=EntityFramework"
                         
            };

            this.LocalizationLanguage = "pt";
            this.ContextLength = 1; 
         }
            
            
        public SourceConfig[] Sources { get; set; }
        public string SiteURL { get; set; }
        public string ProfileImageDir { get; set; }
        public string ApplicationName { get; set; }
        public MailSettings MailSettings { get; set; }

        public string LocalizationLanguage { get; set; }

        public int ContextLength { get; set; }

    }

    public class TestContextBuilder : IContextBuilder
    {
        public ISettings Settings { get; set; }

        public TestContextBuilder(ISettings settings)
        {
            Settings = settings; 
        }

        public void BuilderContext(IContext context)
        {
           ((DapperContext)context)
                .Connection[0] = new SqlConnection(Settings.Sources[0].SourceValue);

            context.Begin(0, System.Data.IsolationLevel.ReadUncommitted);

        }
    }

    public class Resources
    {
        public Resources()
        {
            Settings = new TesteConfigs();
            Builder = new TestContextBuilder(Settings);
            Context = new DapperContext(Settings);
            Repository = new MembershipRepositorySet(Context);
            Domain = new MembershipManager(Context, Repository);

            Builder.BuilderContext(Context); 
        }

        public ISettings Settings { get; set; }

        public TestContextBuilder Builder {get;set;}

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

        public void Perform_ShouldBeTrue(OperationStatus status)
        {
            if (status.Error != null)
            {
                status.Status.ShouldBeTrue(status.Error.Message);
            }
            else
            {
                status.Status.ShouldBeTrue();
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
