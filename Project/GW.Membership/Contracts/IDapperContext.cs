using GW.Common;
using GW.Core;
using System.Data;
using System.Data.Common;

namespace GW.Membership.Contracts
{
    public interface IDapperContext : IContext
    {

         IDbConnection Connection { get; set; }

         IDbTransaction Transaction { get; set; }

         IsolationLevel Isolation { get; set; }

         IDataWorker Worker { get; set; }


    }
}
