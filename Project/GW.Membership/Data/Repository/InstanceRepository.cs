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

        public async Task Create(InstanceEntry model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForCreate("sysInstance", model,model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);
        }

        public async Task<InstanceResult> Read(InstanceParam param)
        {
            InstanceResult ret = null;
            
            string sql = query.QueryForGet(null);

            ret = await ((DapperContext)Context).ExecuteQueryFirstAsync<InstanceResult>(sql, param); 
                 
            return ret;
        }

        public async Task Update(InstanceEntry model)
        {
            
            string sql = query.QueryForUpdate("sysInstance", model, model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);

        }

        public async Task Delete(InstanceEntry model)
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
             
        public async Task<List<InstanceResult>> Search(InstanceParam param)
        {
            List<InstanceResult> ret = null;

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<InstanceResult>(query.QueryForSearch(null),  param);
               

            return ret;
        }

        
    }

}
