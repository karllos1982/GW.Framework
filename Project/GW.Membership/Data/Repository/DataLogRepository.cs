using GW.Common;
using GW.Core;
using GW.Membership.Contracts;
using GW.Membership.Contracts.Data;
using GW.Membership.Models;

namespace GW.Membership.Data
{
    public class DataLogRepository : IDataLogRepository        
    {
       
        public DataLogRepository(IContext context)
        {
            Context = context;            
        }
         
        private DataLogQueryBuilder query = new DataLogQueryBuilder();

        public IContext Context { get; set; }

        public OperationStatus Create(DataLogModel model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForCreate("sysDataLog", model,model);
            ret = ((DapperContext)Context).Execute(sql, model);

            return ret;
        }

        public DataLogModel Read(DataLogParam param)
        {
            DataLogModel ret = null;
            
            string sql = query.QueryForGet(null);

            ret = ((DapperContext)Context).ExecuteQueryFirst<DataLogModel>(sql, param); 
                 
            return ret;
        }

        public OperationStatus Update(DataLogModel model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForUpdate("sysDataLog", model, model);
            ret = ((DapperContext)Context).Execute(sql, model);

            return ret;
        }

        public OperationStatus Delete(DataLogModel model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForDelete("sysDataLog", model, model);
            ret = ((DapperContext)Context).Execute(sql, model);

            return ret;
        }

        public List<DataLogList> List(DataLogParam param)
        {
            List<DataLogList> ret = null;

            ret = ((DapperContext)Context).ExecuteQueryToList<DataLogList>(query.QueryForList(null),
                 param); 

            return ret;
        }
             
        public List<DataLogSearchResult> Search(DataLogParam param)
        {
            List<DataLogSearchResult> ret = null;

            ret = ((DapperContext)Context).ExecuteQueryToList<DataLogSearchResult>(query.QueryForSearch(null),
                 param);

            return ret;
        }

        public List<DataLogTimelineModel> GetDataLogTimeline(Int64 recordID)
        {
            List<DataLogTimelineModel> ret = null;
            DataLogParam param = new DataLogParam() { pID = recordID };
            ret = ((DapperContext)Context)
                .ExecuteQueryToList<DataLogTimelineModel>(query.QueryForGetTimeLine(),
                  param);

            return ret;
        }

        public List<TabelasValueModel> GetTableList()
        {
            List<TabelasValueModel> ret = null;

            ret = ((DapperContext)Context)
                .ExecuteQueryToList<TabelasValueModel>(query.QueryForGetTableList(),
                  null);

            return ret;
        }

    }

}
