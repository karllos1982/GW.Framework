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

         OperationStatus Commit();

         OperationStatus Rollback();

         void Execute(string sql,object data);

         T ExecuteQueryFirst<T>(string sql,  object filter = null);

         List<T> ExecuteQueryToList<T>(string sql,  object filter = null);


        // asyncs:

        Task ExecuteAsync(string sql, object data);

        Task<T> ExecuteQueryFirstAsync<T>(string sql, object filter = null);

        Task<List<T>> ExecuteQueryToListAsync<T>(string sql, object filter = null);


    }
}
