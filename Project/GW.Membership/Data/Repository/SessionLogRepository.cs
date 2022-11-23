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
        }
         
        private SessionQueryBuilder query = new SessionQueryBuilder();

        public IContext Context { get; set; }

        public async Task Create(SessionLogModel model)
        {
            
            string sql = query.QueryForCreate("sysSessionLog", model,model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);
            
        }

        public async Task<SessionLogModel> Read(SessionLogParam param)
        {
            SessionLogModel ret = null;
            
            string sql = query.QueryForGet(null);

            ret = await ((DapperContext)Context)
                .ExecuteQueryFirstAsync<SessionLogModel>(sql, param); 
                 
            return ret;
        }

        public async Task Update(SessionLogModel model)
        {
            
            string sql = query.QueryForUpdate("sysSessionLog", model, model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);
            
        }

        public async Task Delete(SessionLogModel model)
        {
            
            string sql = query.QueryForDelete("sysSessionLog", model, model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);
 
        }

        public async Task<List<SessionLogList>> List(SessionLogParam param)
        {
            List<SessionLogList> ret = null;

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<SessionLogList>(query.QueryForList(param),param); 

            return ret;
        }
             
        public async Task<List<SessionLogSearchResult>> Search(SessionLogParam param)
        {
            List<SessionLogSearchResult> ret = null;

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<SessionLogSearchResult>(query.QueryForSearch(param),  param);

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
