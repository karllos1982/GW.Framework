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
            TableName = "sysPermission";
            PKFieldName = "PermissionID";
        }
         
        private PermissionQueryBuilder query = new PermissionQueryBuilder();

        public string TableName { get; set; }

        public string PKFieldName { get; set; }

        public IContext Context { get; set; }

        public async Task Create(PermissionEntry model)
        {
            
            string sql = query.QueryForCreate(TableName, model,model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);
            
        }

        public async Task<PermissionResult> Read(PermissionParam param)
        {
            PermissionResult ret = null;
            
            string sql = query.QueryForGet(null);

            ret = await ((DapperContext)Context)
                .ExecuteQueryFirstAsync<PermissionResult>(sql, param); 
                 
            return ret;
        }

        public async Task Update(PermissionEntry model)
        {
            
            string sql = query.QueryForUpdate(TableName, model, model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);

        }

        public async Task Delete(PermissionEntry model)
        {
            
            string sql = query.QueryForDelete(TableName, model, model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);

          }

        public async Task<List<PermissionList>> List(PermissionParam param)
        {
            List<PermissionList> ret = null;

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<PermissionList>(query.QueryForList(null),param); 
                 

            return ret;
        }
             
        public  async Task<List<PermissionResult>> Search(PermissionParam param)
        {
            List<PermissionResult> ret = null;

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<PermissionResult>(query.QueryForSearch(null),param);
                 

            return ret;
        }

        public async Task<List<PermissionResult>> GetPermissionsByRoleUser(object param)
        {
            List<PermissionResult> ret = null;

            ret = await ((DapperContext)Context).ExecuteQueryToListAsync<PermissionResult>(
                query.QueryForGetPermissionsByRoleUser(param), param);

            return ret;
        }

    }

}
