using GW.Common;
using GW.Core;
using GW.Membership.Contracts;
using GW.Membership.Contracts.Data;
using GW.Membership.Models;

namespace GW.Membership.Data
{
    public class InstanceRepository : IInstanceRepository        
    {
       
        public InstanceRepository(IContext context)
        {
            Context = context;            
        }
         
        private InstanceQueryBuilder query = new InstanceQueryBuilder();

        public IContext Context { get; set; }

        public async Task Create(InstanceModel model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForCreate("sysInstance", model,model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);
        }

        public async Task<InstanceModel> Read(InstanceParam param)
        {
            InstanceModel ret = null;
            
            string sql = query.QueryForGet(null);

            ret = await ((DapperContext)Context).ExecuteQueryFirstAsync<InstanceModel>(sql, param); 
                 
            return ret;
        }

        public async Task Update(InstanceModel model)
        {
            
            string sql = query.QueryForUpdate("sysInstance", model, model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);

        }

        public async Task Delete(InstanceModel model)
        {
           
            string sql = query.QueryForDelete("sysInstance", model, model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);
      
        }

        public async Task<List<InstanceList>> List(InstanceParam param)
        {
            List<InstanceList> ret = null;

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<InstanceList>(query.QueryForList(null), param); 
                
            return ret;
        }
             
        public async Task<List<InstanceSearchResult>> Search(InstanceParam param)
        {
            List<InstanceSearchResult> ret = null;

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<InstanceSearchResult>(query.QueryForSearch(null),  param);
               

            return ret;
        }

        
    }

}
