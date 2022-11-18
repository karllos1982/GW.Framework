using GW.Membership.Contracts;
using System.Data;
using System.Data.SqlClient;
using GW.Common;
using GW.Core;
using Newtonsoft.Json;
using Dapper;
using GW.Helpers;

namespace GW.Membership.Data
{
    public class DapperContext : IDapperContext
    {
        public DapperContext(ISettings settings)
        {
            Settings = settings;
            ConnStatus = new OperationStatus(true);
            ExecutionStatus = new OperationStatus(true);
            Isolation = IsolationLevel.ReadCommitted;
        }

        public IDbConnection Connection { get; set; }
        public IDbTransaction Transaction { get; set; }
        public IsolationLevel Isolation { get; set; }   
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
                Transaction = Connection.BeginTransaction(Isolation);                
               
            }
            catch (Exception ex)
            {
                ret.Status = false;
                ret.Error = ex;
                ConnStatus = ret; 

            }

            return ret;
        }

        public OperationStatus Commit()
        {
            OperationStatus ret = new OperationStatus(true);

            if (this.Connection != null)
            {
                if (this.Connection.State == ConnectionState.Open)
                {

                    if (this.Transaction != null)
                    {
                       
                        try
                        {
                            this.Transaction.Commit();

                        }
                        catch (System.Exception ex)
                        {
                            ret.Status = false;
                            ret.Error = ex;
                            ExecutionStatus = ret; 
                        }
                                              
                    }                   

                }

            }

            return ret;
        }

        public OperationStatus Rollback()
        {
            OperationStatus ret = new OperationStatus(true);

            if (this.Connection != null)
            {
                if (this.Connection.State == ConnectionState.Open)
                {

                    if (this.Transaction != null)
                    {

                        try
                        {
                            this.Transaction.Rollback();

                        }
                        catch (System.Exception ex)
                        {
                            ret.Status = false;
                            ret.Error = ex;
                            ExecutionStatus = ret;
                        }

                    }

                }

            }

            return ret;
        }

        public OperationStatus End()
        {
            OperationStatus ret = new OperationStatus(true);

            if (this.Connection != null)
            {
                if (this.Connection.State == ConnectionState.Open)
                {

                    if (this.Transaction != null)
                    {
                        if (ExecutionStatus.Status)
                        {
                            ret = this.Commit(); 
                        }
                        else
                        {
                            ret = this.Rollback();
                        }
                    }

                }

            }

            this.Dispose(); 

            return ret;
        }

        //

        public OperationStatus Execute(string sql, object data)
        {
            OperationStatus ret = new OperationStatus(true);
            ExecutionStatus.Status = true;
            ExecutionStatus.Error = null;

            if (this.Connection.State == System.Data.ConnectionState.Open)
            {

                try
                {
                    this.Connection.Execute(sql, data, Transaction);

                }
                catch (SqlException ex)
                {
                    ret.Status = false;
                    ret.Error = ex;
                    ExecutionStatus = ret;

                }
                catch (System.Exception ex)
                {
                    ret.Status = false;
                    ret.Error = ex;
                    ExecutionStatus = ret;

                }

            }
            else
            {
                ExecutionStatus.Status = false;
                ExecutionStatus.Error = new Exception("The connection is closed!");
            }


            return ret;

        }

        public T ExecuteQueryFirst<T>( string sql, object filter = null)
        {
            T ret = (default);

            ExecutionStatus.Status = true;
            ExecutionStatus.Error = null;

            if (this.Connection.State == System.Data.ConnectionState.Open)
            {

                try
                {
                    List<T> list = this.Connection
                        .Query<T>(sql, filter, Transaction).AsList<T>();
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
                    ExecutionStatus.Status = false;
                    ExecutionStatus.Error = ex;

                }
                catch (System.Exception ex)
                {
                    ExecutionStatus.Status = false;
                    ExecutionStatus.Error = ex;

                }

            }
            else
            {
                ExecutionStatus.Status = false;
                ExecutionStatus.Error = new Exception("The connection is closed!");
            }


            return ret;

        }

        public List<T> ExecuteQueryToList<T>(string sql, object filter = null)
        {
            List<T> ret = new List<T>();

            ExecutionStatus.Status = true;
            ExecutionStatus.Error = null;

            if (this.Connection.State == System.Data.ConnectionState.Open)
            {

                try
                {
                    ret = this.Connection
                        .Query<T>(sql, filter, Transaction).AsList<T>();
                }
                catch (SqlException ex)
                {
                    ExecutionStatus.Status = false;
                    ExecutionStatus.Error = ex;

                }
                catch (System.Exception ex)
                {
                    ExecutionStatus.Status = false;
                    ExecutionStatus.Error = ex;

                }

            }
            else
            {
                ExecutionStatus.Status = false;
                ExecutionStatus.Error = new Exception("The connection is closed!");
            }


            return ret;

        }


        //


        public void Dispose()
        {
            if (this.Transaction != null)
            {
                this.Transaction.Dispose();
                this.Transaction = null;
            }

            if (this.Connection != null)
            {
                this.Connection.Close();
                this.Connection.Dispose();
                this.Connection = null;
            }
        }


        public void RegisterDataLog(string userid, OPERATIONLOGENUM operation,
                 string tableaname, string objID, object olddata, object currentdata)
        {

            DataLogObject obj = new DataLogObject();

            string s_olddata = "";
            string s_currentdata = "";

            obj.DataLogID = Helpers.Utilities.GenerateId();
            obj.UserID = Int64.Parse(userid);
            obj.Date = DateTime.Now;

            switch (operation)
            {
                case OPERATIONLOGENUM.INSERT:
                    obj.Operation = "I";

                    s_currentdata = JsonConvert.SerializeObject(currentdata);

                    break;

                case OPERATIONLOGENUM.UPDATE:
                    obj.Operation = "U";

                    s_olddata = JsonConvert.SerializeObject(olddata);
                    s_currentdata = JsonConvert.SerializeObject(currentdata);

                    break;

                case OPERATIONLOGENUM.DELETE:
                    obj.Operation = "D";

                    s_olddata = JsonConvert.SerializeObject(olddata);
                    break;
            }

            obj.TableName = tableaname;
            obj.ID = Int64.Parse(objID);
            obj.LogOldData = s_olddata;
            obj.LogCurrentData = s_currentdata;

            DataLogQueryBuilder qw = new DataLogQueryBuilder(); 

            string sqltext =                
                    qw.BuildInsertCommand("[sysdatalog]", obj, null);

            Execute(sqltext, obj);


        }

    }
}
