using GW.Membership.Contracts;
using System.Data;
using System.Data.SqlClient;
using GW.Common;
using GW.Core;

namespace GW.Membership.Data
{
    public class DapperContext : IDapperContext
    {
        public DapperContext(ISettings settings)
        {
            Settings = settings;    
        }

        public IDbConnection Connection { get; set; }
        public IDbTransaction Transaction { get; set; }
        public IsolationLevel Isolation { get; set; }
        public IDataWorker Worker { get; set; }
        public ISettings Settings { get; set; }
        public OperationStatus ConnStatus { get; set; }
        public OperationStatus ExecutionStatus { get; set; }

        public OperationStatus Begin(int sourceindex)
        {
            OperationStatus ret = new OperationStatus(true);

            Connection = new SqlConnection(Settings.Sources[sourceindex].SourceValue);
            
            try
            {
                Connection.Open();
            }
            catch (Exception ex)
            {
                ret.Status = false;
                // ret.ex
            }

            return ret;
        }

        public OperationStatus Commit()
        {
            OperationStatus ret = new OperationStatus(true);

            return ret;
        }

        public OperationStatus End()
        {
            OperationStatus ret = new OperationStatus(true);

            return ret;
        }

        public void Dispose()
        {
           
        }

      
    }
}
