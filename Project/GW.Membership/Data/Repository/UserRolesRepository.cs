using GW.Common;
using GW.Core;
using GW.Membership.Contracts;
using GW.Membership.Contracts.Data;
using GW.Membership.Models;

namespace GW.Membership.Data
{
    public class UserRolesRepository : IUserRolesRepository        
    {
       
        public UserRolesRepository(IContext context)
        {
            Context = context;            
        }
         
        private UserRolesQueryBuilder query = new UserRolesQueryBuilder();

        public IContext Context { get; set; }

        public async Task Create(UserRolesEntry model)
        {
            
            string sql = query.QueryForCreate("sysUserRoles", model,model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);
            
        }

        public async Task<UserRolesResult> Read(UserRolesParam param)
        {
            UserRolesResult ret = null;
            
            string sql = query.QueryForGet(null);

            ret = await ((DapperContext)Context)
                .ExecuteQueryFirstAsync<UserRolesResult>(sql, param); 
                 
            return ret;
        }

        public async Task Update(UserRolesEntry model)
        {
            
            string sql = query.QueryForUpdate("sysUserRoles", model, model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);
            
        }

        public async Task Delete(UserRolesEntry model)
        {            

            string sql = query.QueryForDelete("sysUserRoles", model, model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);
            
        }

        public async  Task<List<UserRolesResult>> List(UserRolesParam param)
        {
            List<UserRolesResult> ret = null;

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<UserRolesResult>(query.QueryForList(null),param); 
                 
            return ret;
        }
             
        public async Task<List<UserRolesResult>> Search(UserRolesParam param)
        {
            List<UserRolesResult> ret = null;

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<UserRolesResult>(query.QueryForSearch(null),param);
                 
            return ret;
        }

        
    }

}
