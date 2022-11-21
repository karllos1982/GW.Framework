using GW.Common;
using GW.Core;
using GW.Membership.Contracts;
using GW.Membership.Contracts.Data;
using GW.Membership.Models;

namespace GW.Membership.Data
{
    public class RoleRepository : IRoleRepository        
    {
       
        public RoleRepository(IContext context)
        {
            Context = context;            
        }
         
        private RoleQueryBuilder query = new RoleQueryBuilder();

        public IContext Context { get; set; }

        public async Task Create(RoleModel model)
        {
            
            string sql = query.QueryForCreate("sysRole", model,model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);
            
        }

        public async Task<RoleModel> Read(RoleParam param)
        {
            RoleModel ret = null;
            
            string sql = query.QueryForGet(null);

            ret = await ((DapperContext)Context).ExecuteQueryFirstAsync<RoleModel>(sql, param); 
                 
            return ret;
        }

        public async Task Update(RoleModel model)
        {
            
            string sql = query.QueryForUpdate("sysRole", model, model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);
         
         }

        public async Task Delete(RoleModel model)
        {
            
            string sql = query.QueryForDelete("sysRole", model, model);
           await ((DapperContext)Context).ExecuteAsync(sql, model);
            
        }

        public async Task<List<RoleList>> List(RoleParam param)
        {
            List<RoleList> ret = null;

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<RoleList>(query.QueryForList(null), param); 
                
            return ret;
        }
             
        public async Task<List<RoleSearchResult>> Search(RoleParam param)
        {
            List<RoleSearchResult> ret = null;

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<RoleSearchResult>(query.QueryForSearch(null),param);
                 

            return ret;
        }

        
    }

}
