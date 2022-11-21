using GW.Common;
using GW.Core;
using GW.Membership.Contracts;
using GW.Membership.Contracts.Data;
using GW.Membership.Models;

namespace GW.Membership.Data
{
    public class PermissionRepository : IPermissionRepository        
    {
       
        public PermissionRepository(IContext context)
        {
            Context = context;            
        }
         
        private PermissionQueryBuilder query = new PermissionQueryBuilder();

        public IContext Context { get; set; }

        public async Task Create(PermissionModel model)
        {
            
            string sql = query.QueryForCreate("sysPermission", model,model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);
            
        }

        public async Task<PermissionModel> Read(PermissionParam param)
        {
            PermissionModel ret = null;
            
            string sql = query.QueryForGet(null);

            ret = await ((DapperContext)Context)
                .ExecuteQueryFirstAsync<PermissionModel>(sql, param); 
                 
            return ret;
        }

        public async Task Update(PermissionModel model)
        {
            
            string sql = query.QueryForUpdate("sysPermission", model, model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);

        }

        public async Task Delete(PermissionModel model)
        {
            
            string sql = query.QueryForDelete("sysPermission", model, model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);

          }

        public async Task<List<PermissionList>> List(PermissionParam param)
        {
            List<PermissionList> ret = null;

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<PermissionList>(query.QueryForList(null),param); 
                 

            return ret;
        }
             
        public  async Task<List<PermissionSearchResult>> Search(PermissionParam param)
        {
            List<PermissionSearchResult> ret = null;

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<PermissionSearchResult>(query.QueryForSearch(null),param);
                 

            return ret;
        }

        public async Task<List<PermissionSearchResult>> GetPermissionsByRoleUser(object param)
        {
            List<PermissionSearchResult> ret = null;

            ret = await ((DapperContext)Context).ExecuteQueryToListAsync<PermissionSearchResult>(
                query.QueryForGetPermissionsByRoleUser(param), param);

            return ret;
        }

    }

}
