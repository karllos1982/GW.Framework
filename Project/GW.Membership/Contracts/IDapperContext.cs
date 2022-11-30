using GW.Common;
using GW.Core;
using System.Data;
using System.Data.Common;

namespace GW.Membership.Contracts
{
    public interface IDapperContext : IContext
    {

         IDbConnection[] Connection { get; set; }

         IDbTransaction[] Transaction { get; set; }    

         OperationStatus Commit();

         OperationStatus Rollback();

         void Execute(string sql,object data, int index = 0);

         T ExecuteQueryFirst<T>(string sql,  object filter = null, int index = 0);

         List<T> ExecuteQueryToList<T>(string sql,  object filter = null, int index = 0);


        // asyncs:

        Task ExecuteAsync(string sql, object data, int index = 0);

        Task<T> ExecuteQueryFirstAsync<T>(string sql, object filter = null, int index = 0);

        Task<List<T>> ExecuteQueryToListAsync<T>(string sql, object filter = null, int index = 0);


    }
}
