using GW.Membership.Contracts;
using System.Data;
using System.Data.SqlClient;
using GW.Common;
using GW.Core;
using Newtonsoft.Json;
using Dapper;
using GW.Helpers;
using System.Linq;
using System;
using System.Reflection;

namespace GW.Membership.Data
{
    public class DapperContext : IDapperContext
    {
        public DapperContext(ISettings settings)
        {
            Settings = settings;
            ConnStatus = new OperationStatus(true);
            ExecutionStatus = new OperationStatus(true);
            
            Connection = new IDbConnection[settings.ContextLength];
            Transaction = new IDbTransaction[settings.ContextLength];        

        }

        public IDbConnection[] Connection { get; set; }

        public IDbTransaction[] Transaction { get; set; }
        
        public ISettings Settings { get; set; }
        public OperationStatus ConnStatus { get; set; }
     
        public OperationStatus ExecutionStatus { get; set; }
     
        public OperationStatus Begin(int connindex, object isolationlavel)
        {
            OperationStatus ret = new OperationStatus(true);

            try
            {
                Connection[connindex].Open();
                Transaction[connindex]
                = Connection[connindex].BeginTransaction((IsolationLevel)isolationlavel);

            }
            catch (Exception ex)
            {
                ConnStatus = new OperationStatus(false);
                ConnStatus.Error = ex;

            }
           
            return ret;
        }

        public OperationStatus Commit()
        {
            OperationStatus ret = new OperationStatus(true);

            
            for (int i = 0; i < this.Connection.Length; i++)  
            {
                if (this.Connection[i] != null)
                {
                    if (Connection[i].State == ConnectionState.Open)
                    {

                        if (this.Transaction[i] != null)
                        {

                            try
                            {
                                this.Transaction[i].Commit();

                            }
                            catch (System.Exception ex)
                            {
                                ret.Status = false;
                                ret.Error = ex;

                            }

                        }

                    }
                }
            }
                           
            ExecutionStatus = ret;

            return ret;
        }

        public OperationStatus Rollback()
        {
            OperationStatus ret = new OperationStatus(true);
                      
            for (int i = 0; i < this.Connection.Length; i++)
            {
                if (this.Connection[i] != null)
                {
                    if (Connection[i].State == ConnectionState.Open)
                    {

                        if (this.Transaction[i] != null)
                        {

                            try
                            {
                                this.Transaction[i].Rollback();

                            }
                            catch (System.Exception ex)
                            {
                                ret.Status = false;
                                ret.Error = ex;

                            }

                        }

                    }
                }
            }                        

            ExecutionStatus = ret;
            return ret;
        }

        public OperationStatus End()
        {
            OperationStatus ret = new OperationStatus(true);
                        
            if (ExecutionStatus.Status)
            {
                ret = this.Commit(); 
            }
            else
            {
                ret = this.Rollback();
            }                 

            this.Dispose();

            ExecutionStatus = ret;

            return ret;
        }

        //

        public void Execute(string sql, object data, int index = 0 )
        {
            ExecutionStatus.Status = true;
            ExecutionStatus.Error = null;
            ExecutionStatus.Returns = null;

            if (this.Connection[index].State == System.Data.ConnectionState.Open)
            {

                try
                {
                    this.Connection[index].Execute(sql, data, Transaction[index]);

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

        }

        public T ExecuteQueryFirst<T>(string sql,object filter = null, int index = 0)                
        {
            ExecutionStatus.Status = true;
            ExecutionStatus.Error = null;
            ExecutionStatus.Returns = null;

            T ret = (default);

            if (this.Connection[index].State == System.Data.ConnectionState.Open)
            {

                try
                {
                    ret = this.Connection[index].QueryFirst<T>(sql, filter, Transaction[index]);
                 
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

        public List<T> ExecuteQueryToList<T>(string sql, object filter = null, int index = 0)
        {
            List<T> ret = new List<T>();

            ExecutionStatus.Status = true;
            ExecutionStatus.Error = null;

            if (this.Connection[index].State == System.Data.ConnectionState.Open)
            {

                try
                {
                    ret = this.Connection[index]
                        .Query<T>(sql, filter, Transaction[index]).AsList<T>();
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


        public List<T> ExecuteQueryToListMultiple<T,U>(string sql,
                Func<T,U,T> relationship, string splitfields, object filter = null, int index = 0)
        {
            List<T> ret = new List<T>();

            ExecutionStatus.Status = true;
            ExecutionStatus.Error = null;

            if (this.Connection[index].State == System.Data.ConnectionState.Open)
            {

                try
                {
                    ret = this.Connection[index]
                        .Query<T, U, T>(
                          sql,                      
                          relationship ,
                          filter,
                          Transaction[index],
                          false,
                          splitfields).AsList<T>();

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

        // asyncs:

        public async Task ExecuteAsync(string sql, object data, int index = 0)
        {
            ExecutionStatus.Status = true;
            ExecutionStatus.Error = null;
            ExecutionStatus.Returns = null;

            if (this.Connection[index].State == System.Data.ConnectionState.Open)
            {

                try
                {
                  await  this.Connection[index].ExecuteAsync(sql, data, Transaction[index]);

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

        }

        public async Task<T> ExecuteQueryFirstAsync<T>( string sql, 
            object filter = null, int index = 0)            
        {
            ExecutionStatus.Status = true;
            ExecutionStatus.Error = null;
            ExecutionStatus.Returns = null;

            T ret = (default);
        

            if (this.Connection[index].State == System.Data.ConnectionState.Open)
            {

                try
                {

                    ret = await this.Connection[index].QueryFirstAsync<T>(sql, filter, Transaction[index]);                  

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

        public async Task<List<T>> ExecuteQueryToListAsync<T>(string sql, 
            object filter = null, int index = 0)
        {
            List<T> ret = new List<T>();

            ExecutionStatus.Status = true;
            ExecutionStatus.Error = null;

            if (this.Connection[index].State == System.Data.ConnectionState.Open)
            {

                try
                {
                    IEnumerable<T> list = await this.Connection[index]
                        .QueryAsync<T>(sql, filter, Transaction[index]);

                    ret = list.AsList(); 

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


        public async Task<List<T>> ExecuteQueryToListOneToOneAsync<T, U>(string sql,
        Func<T, U, T> mapping, string splitfields, object filter = null, int index = 0)
        {
            List<T> ret = new List<T>();

            ExecutionStatus.Status = true;
            ExecutionStatus.Error = null;

            if (this.Connection[index].State == System.Data.ConnectionState.Open)
            {

                try
                {
                    IEnumerable<T> list = await this.Connection[index]
                        .QueryAsync<T, U, T>(
                          sql,
                          mapping,
                          filter,
                          Transaction[index],
                          false,
                          splitfields);

                    ret = list.AsList();

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

            for (int i = 0; i < this.Connection.Length; i++)
            {
                if (this.Transaction[i] != null)
                {
                    this.Transaction[i].Dispose();
                    this.Transaction[i] = null;
                }

                if (this.Connection[i] != null)
                {
                    this.Connection[i].Close();
                    this.Connection[i].Dispose();
                    this.Connection[i] = null;
                }

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

        public async Task RegisterDataLogAsync(string userid, OPERATIONLOGENUM operation,
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

            await ExecuteAsync(sqltext, obj);


        }

    }
}
