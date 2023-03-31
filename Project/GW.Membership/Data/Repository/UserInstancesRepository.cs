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

            TableName = "sysUserInstances";
            PKFieldName = "UserInstanceID";

        }
         
        private UserInstancesQueryBuilder query = new UserInstancesQueryBuilder();

        public string TableName { get; set; }

        public string PKFieldName { get; set; }

        public IContext Context { get; set; }

        public async Task Create(UserInstancesEntry model)
        {
            
            string sql = query.QueryForCreate(TableName, model,model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);
           
        }

        public async Task<UserInstancesResult> Read(UserInstancesParam param)
        {
            UserInstancesResult ret = null;
            
            string sql = query.QueryForGet(null);

            ret = await ((DapperContext)Context)
                .ExecuteQueryFirstAsync<UserInstancesResult>(sql, param); 
                 
            return ret;
        }

        public async Task Update(UserInstancesEntry model)
        {
            
            string sql = query.QueryForUpdate(TableName, model, model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);
            
        }

        public async Task Delete(UserInstancesEntry model)
        {
            
            string sql = query.QueryForDelete(TableName, model, model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);

          }

        public async Task<List<UserInstancesResult>> List(UserInstancesParam param)
        {
            List<UserInstancesResult> ret = null;

            ret =await  ((DapperContext)Context)
                .ExecuteQueryToListAsync<UserInstancesResult>(query.QueryForList(null), param); 

            return ret;
        }
             
        public async Task<List<UserInstancesResult>> Search(UserInstancesParam param)
        {
            List<UserInstancesResult> ret = null;

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<UserInstancesResult>(query.QueryForSearch(null), param);

            return ret;
        }


        public async Task AlterInstance(UserInstancesParam obj)
        {

            string sql = query.QueryForAlterInstance (null);
            await ((DapperContext)Context).ExecuteAsync(sql, obj);

        }

    }

}
