using GW.Common;
using GW.Core;
using GW.Membership.Contracts;
using GW.Membership.Contracts.Data;
using GW.Membership.Models;

namespace GW.Membership.Data
{
    public class SessionLogRepository : ISessionLogRepository        
    {
       
        public SessionLogRepository(IContext context)
        {
            Context = context;
            TableName = "sysSession";
            PKFieldName = "SessionID";

        }
         
        private SessionQueryBuilder query = new SessionQueryBuilder();

        public string TableName { get; set; }

        public string PKFieldName { get; set; }

        public IContext Context { get; set; }

        public async Task Create(SessionLogEntry model)
        {
            
            string sql = query.QueryForCreate(TableName, model,model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);
            
        }

        public async Task<SessionLogResult> Read(SessionLogParam param)
        {
            SessionLogResult ret = null;
            
            string sql = query.QueryForGet(null);

            ret = await ((DapperContext)Context)
                .ExecuteQueryFirstAsync<SessionLogResult>(sql, param); 
                 
            return ret;
        }

        public async Task Update(SessionLogEntry model)
        {
            
            string sql = query.QueryForUpdate(TableName, model, model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);
            
        }

        public async Task Delete(SessionLogEntry model)
        {
            
            string sql = query.QueryForDelete(TableName, model, model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);
 
        }

        public async Task<List<SessionLogList>> List(SessionLogParam param)
        {
            List<SessionLogList> ret = null;

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<SessionLogList>(query.QueryForList(param),param); 

            return ret;
        }
             
        public async Task<List<SessionLogResult>> Search(SessionLogParam param)
        {
            List<SessionLogResult> ret = null;

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<SessionLogResult>(query.QueryForSearch(param),  param);

            return ret;
        }

        public async Task<OperationStatus> SetDateLogout(SessionLogParam obj)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForSetDateLogout();
             await ((DapperContext)Context).ExecuteAsync(sql, obj);
            ret = Context.ExecutionStatus;

            return ret;
        }

    }

}
