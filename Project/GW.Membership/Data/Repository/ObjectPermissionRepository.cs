using GW.Common;
using GW.Core;
using GW.Membership.Contracts;
using GW.Membership.Contracts.Data;
using GW.Membership.Models;

namespace GW.Membership.Data
{
    public class ObjectPermissionRepository : IObjectPermissionRepository        
    {
       
        public ObjectPermissionRepository(IContext context)
        {
            Context = context;            
        }
         
        private ObjectPermissionQueryBuilder query = new ObjectPermissionQueryBuilder();

        public IContext Context { get; set; }

        public async Task Create(ObjectPermissionModel model)
        {
            
            string sql = query.QueryForCreate("sysObjectPermission", model,model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);
            
        }

        public async Task<ObjectPermissionModel> Read(ObjectPermissionParam param)
        {
            ObjectPermissionModel ret = null;
            
            string sql = query.QueryForGet(null);

            ret = await ((DapperContext)Context)
                .ExecuteQueryFirstAsync<ObjectPermissionModel>(sql, param); 
                 
            return ret;
        }

        public async Task Update(ObjectPermissionModel model)
        {
            
            string sql = query.QueryForUpdate("sysObjectPermission", model, model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);

         }

        public async Task Delete(ObjectPermissionModel model)
        {
            
            string sql = query.QueryForDelete("sysObjectPermission", model, model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);

        }

        public async Task<List<ObjectPermissionList>> List(ObjectPermissionParam param)
        {
            List<ObjectPermissionList> ret = null;

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<ObjectPermissionList>(query.QueryForList(null), param); 
                

            return ret;
        }
             
        public async Task<List<ObjectPermissionSearchResult>> Search(ObjectPermissionParam param)
        {
            List<ObjectPermissionSearchResult> ret = null;

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<ObjectPermissionSearchResult>(query.QueryForSearch(null),param);
                 
            return ret;
        }

        
    }

}
