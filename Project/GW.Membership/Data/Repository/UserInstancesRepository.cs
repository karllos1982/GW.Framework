using GW.Common;
using GW.Core;
using GW.Membership.Contracts;
using GW.Membership.Contracts.Data;
using GW.Membership.Models;

namespace GW.Membership.Data
{
    public class UserInstancesRepository : IUserInstancesRepository        
    {
       
        public UserInstancesRepository(IContext context)
        {
            Context = context;            
        }
         
        private UserInstancesQueryBuilder query = new UserInstancesQueryBuilder();

        public IContext Context { get; set; }

        public async Task Create(UserInstancesModel model)
        {
            
            string sql = query.QueryForCreate("sysUserInstances", model,model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);
           
        }

        public async Task<UserInstancesModel> Read(UserInstancesParam param)
        {
            UserInstancesModel ret = null;
            
            string sql = query.QueryForGet(null);

            ret = await ((DapperContext)Context)
                .ExecuteQueryFirstAsync<UserInstancesModel>(sql, param); 
                 
            return ret;
        }

        public async Task Update(UserInstancesModel model)
        {
            
            string sql = query.QueryForUpdate("sysUserInstances", model, model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);
            
        }

        public async Task Delete(UserInstancesModel model)
        {
            
            string sql = query.QueryForDelete("sysUserInstances", model, model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);

          }

        public async Task<List<UserInstancesModel>> List(UserInstancesParam param)
        {
            List<UserInstancesModel> ret = null;

            ret =await  ((DapperContext)Context)
                .ExecuteQueryToListAsync<UserInstancesModel>(query.QueryForList(null), param); 

            return ret;
        }
             
        public async Task<List<UserInstancesModel>> Search(UserInstancesParam param)
        {
            List<UserInstancesModel> ret = null;

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<UserInstancesModel>(query.QueryForSearch(null), param);

            return ret;
        }

        
    }

}
