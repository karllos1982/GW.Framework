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

        public OperationStatus Create(SessionLogModel model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForCreate("sysSessionLog", model,model);
            ret = ((DapperContext)Context).Execute(sql, model);

            return ret;
        }

        public SessionLogModel Read(SessionLogParam param)
        {
            SessionLogModel ret = null;
            
            string sql = query.QueryForGet(null);

            ret = ((DapperContext)Context).ExecuteQueryFirst<SessionLogModel>(sql, param); 
                 
            return ret;
        }

        public OperationStatus Update(SessionLogModel model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForUpdate("sysSessionLog", model, model);
            ret = ((DapperContext)Context).Execute(sql, model);

            return ret;
        }

        public OperationStatus Delete(SessionLogModel model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForDelete("sysSessionLog", model, model);
            ret = ((DapperContext)Context).Execute(sql, model);

            return ret;
        }

        public List<SessionLogList> List(SessionLogParam param)
        {
            List<SessionLogList> ret = null;

            ret = ((DapperContext)Context).ExecuteQueryToList<SessionLogList>(query.QueryForList(null),
                 param); 

            return ret;
        }
             
        public List<SessionLogSearchResult> Search(SessionLogParam param)
        {
            List<SessionLogSearchResult> ret = null;

            ret = ((DapperContext)Context).ExecuteQueryToList<SessionLogSearchResult>(query.QueryForSearch(null),
                 param);

            return ret;
        }

        public OperationStatus SetDateLogout(SessionLogParam obj)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForSetDateLogout();
            ret = ((DapperContext)Context).Execute(sql, obj);

            return ret;
        }

    }

}
