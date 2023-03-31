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
            TableName = "sysRole";
            PKFieldName = "RoleID";
        }
         
        private RoleQueryBuilder query = new RoleQueryBuilder();

        public string TableName { get; set; }

        public string PKFieldName { get; set; }

        public IContext Context { get; set; }

        public async Task Create(RoleEntry model)
        {
            
            string sql = query.QueryForCreate(TableName, model,model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);
            
        }

        public async Task<RoleResult> Read(RoleParam param)
        {
            RoleResult ret = null;
            
            string sql = query.QueryForGet(null);

            ret = await ((DapperContext)Context).ExecuteQueryFirstAsync<RoleResult>(sql, param); 
                 
            return ret;
        }

        public async Task Update(RoleEntry model)
        {
            
            string sql = query.QueryForUpdate(TableName, model, model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);
         
         }

        public async Task Delete(RoleEntry model)
        {
            
            string sql = query.QueryForDelete(TableName, model, model);
           await ((DapperContext)Context).ExecuteAsync(sql, model);
            
        }

        public async Task<List<RoleList>> List(RoleParam param)
        {
            List<RoleList> ret = null;

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<RoleList>(query.QueryForList(null), param); 
                
            return ret;
        }
             
        public async Task<List<RoleResult>> Search(RoleParam param)
        {
            List<RoleResult> ret = null;

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<RoleResult>(query.QueryForSearch(null),param);
                 

            return ret;
        }

        
    }

}
