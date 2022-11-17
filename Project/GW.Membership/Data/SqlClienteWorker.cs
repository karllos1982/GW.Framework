using GW.Common;
using Dapper;
using System.Data.SqlClient;
using System.Data;

namespace GW.Membership.Data
{
    // OBSOLITE CLASS
    public class SqlClienteWorker : IDataWorker
    {      
        private IDbTransaction _transaction;
        
        public SqlClienteWorker(IDbTransaction transaction)
        {
            _transaction = transaction;
        }

        public OperationStatus Execute(ref OperationStatus executionstatus,
            string sql, object data)
        {
            OperationStatus ret = new OperationStatus(true);
            executionstatus.Status = true;
            executionstatus.Error = null; 

            if (this._transaction.Connection.State== System.Data.ConnectionState.Open)
            {
               
                try
                {
                    this._transaction.Connection.Execute(sql, data, _transaction);
                       
                }
                catch (SqlException ex)
                {
                    ret.Status = false;
                    ret.Error = ex;
                    executionstatus = ret;

                }
                catch (System.Exception ex)
                {
                    ret.Status = false;
                    ret.Error = ex;
                    executionstatus = ret;

                }

            }
            else
            {
                executionstatus.Status = false;
                executionstatus.Error = new Exception("The connection is closed!"); 
            }


            return ret;

        }

        public T ExecuteQueryFirst<T>(ref OperationStatus executionstatus, 
            string sql, object filter = null)
        {
            T ret = (default);

            executionstatus.Status = true;
            executionstatus.Error = null;

            if (this._transaction.Connection.State == System.Data.ConnectionState.Open)
            {
               
                try
                {
                    List<T> list = this._transaction.Connection
                        .Query<T>(sql, filter,_transaction).AsList<T>();
                    if (list != null)
                    {
                        if (list.Count > 0)
                        {
                            ret = list[0];
                        }
                    }

                }
                catch (SqlException ex)
                {
                    executionstatus.Status = false;
                    executionstatus.Error = ex;                    

                }
                catch (System.Exception ex)
                {
                    executionstatus.Status = false;
                    executionstatus.Error = ex;

                }

            }
            else
            {
                executionstatus.Status = false;
                executionstatus.Error = new Exception("The connection is closed!");
            }


            return ret;

        }

        public List<T> ExecuteQueryToList<T>(ref OperationStatus executionstatus, 
            string sql, object filter = null)
        {
            List<T> ret = new List<T>();

            executionstatus.Status = true;
            executionstatus.Error = null;

            if (this._transaction.Connection.State == System.Data.ConnectionState.Open)
            {
                
                try
                {
                    ret = this._transaction.Connection
                        .Query<T>(sql, filter, this._transaction).AsList<T>();
                }
                catch (SqlException ex)
                {
                    executionstatus.Status = false;
                    executionstatus.Error = ex;

                }
                catch (System.Exception ex)
                {
                    executionstatus.Status = false;
                    executionstatus.Error = ex;

                }

            }
            else
            {
                executionstatus.Status = false;
                executionstatus.Error = new Exception("The connection is closed!");
            }


            return ret;

        }
    }
}
