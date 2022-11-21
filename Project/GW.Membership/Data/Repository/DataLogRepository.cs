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

        public async Task Create(DataLogModel model)
        {
            
            string sql = query.QueryForCreate("sysDataLog", model,model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);

        }

        public async Task<DataLogModel> Read(DataLogParam param)
        {
            DataLogModel ret = null;
            
            string sql = query.QueryForGet(null);

            ret = await ((DapperContext)Context).ExecuteQueryFirstAsync<DataLogModel>(sql, param); 
                 
            return ret;
        }

        public async Task Update(DataLogModel model)
        {            
            string sql = query.QueryForUpdate("sysDataLog", model, model);
              await ((DapperContext)Context).ExecuteAsync(sql, model);
         
        }

        public async Task Delete(DataLogModel model)
        {
            
            string sql = query.QueryForDelete("sysDataLog", model, model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);

            
        }

        public async Task<List<DataLogList>> List(DataLogParam param)
        {
            List<DataLogList> ret = null;

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<DataLogList>(query.QueryForList(null),param); 
                 
            return ret;
        }
             
        public async Task<List<DataLogSearchResult>> Search(DataLogParam param)
        {
            List<DataLogSearchResult> ret = null;

            ret = await  ((DapperContext)Context)
                .ExecuteQueryToListAsync<DataLogSearchResult>(query.QueryForSearch(null), param);
                

            return ret;
        }

        public async Task<List<DataLogTimelineModel>> GetDataLogTimeline(Int64 recordID)
        {
            List<DataLogTimelineModel> ret = null;
            DataLogParam param = new DataLogParam() { pID = recordID };

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<DataLogTimelineModel>(query.QueryForGetTimeLine(), param);

            return ret;
        }

        public async Task<List<TabelasValueModel>> GetTableList()
        {
            List<TabelasValueModel> ret = null;

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<TabelasValueModel>(query.QueryForGetTableList(),null);
                  

            return ret;
        }

    }

}
