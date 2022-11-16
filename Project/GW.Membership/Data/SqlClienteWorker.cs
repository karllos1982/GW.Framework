using GW.Common;
using Dapper;
using System.Data.SqlClient;

namespace GW.Membership.Data
{
    public class SqlClienteWorker : IDataWorker
    {
        private SqlConnection _connection;

        public SqlClienteWorker(SqlConnection connection)
        {
            _connection = connection;
        }

        public OperationStatus Execute(string sql, object obj)
        {
            throw new NotImplementedException();
        }

        public T ExecuteQueryFirst<T>(string sql, object filter = null)
        {
            throw new NotImplementedException();
        }

        public List<T> ExecuteQueryToList<T>(string sql, object filter = null)
        {
            throw new NotImplementedException();
        }
    }
}
