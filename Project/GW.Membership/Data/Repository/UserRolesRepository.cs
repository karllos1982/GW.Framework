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

        public async Task Create(UserRolesModel model)
        {
            
            string sql = query.QueryForCreate("sysUserRoles", model,model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);
            
        }

        public async Task<UserRolesModel> Read(UserRolesParam param)
        {
            UserRolesModel ret = null;
            
            string sql = query.QueryForGet(null);

            ret = await ((DapperContext)Context)
                .ExecuteQueryFirstAsync<UserRolesModel>(sql, param); 
                 
            return ret;
        }

        public async Task Update(UserRolesModel model)
        {
            
            string sql = query.QueryForUpdate("sysUserRoles", model, model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);
            
        }

        public async Task Delete(UserRolesModel model)
        {            

            string sql = query.QueryForDelete("sysUserRoles", model, model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);
            
        }

        public async  Task<List<UserRolesModel>> List(UserRolesParam param)
        {
            List<UserRolesModel> ret = null;

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<UserRolesModel>(query.QueryForList(null),param); 
                 
            return ret;
        }
             
        public async Task<List<UserRolesModel>> Search(UserRolesParam param)
        {
            List<UserRolesModel> ret = null;

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<UserRolesModel>(query.QueryForSearch(null),param);
                 
            return ret;
        }

        
    }

}
