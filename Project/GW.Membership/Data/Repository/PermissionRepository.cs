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

        public OperationStatus Create(PermissionModel model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForCreate("sysPermission", model,model);
            ret = ((DapperContext)Context).Execute(sql, model);

            return ret;
        }

        public PermissionModel Read(PermissionParam param)
        {
            PermissionModel ret = null;
            
            string sql = query.QueryForGet(null);

            ret = ((DapperContext)Context).ExecuteQueryFirst<PermissionModel>(sql, param); 
                 
            return ret;
        }

        public OperationStatus Update(PermissionModel model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForUpdate("sysPermission", model, model);
            ret = ((DapperContext)Context).Execute(sql, model);

            return ret;
        }

        public OperationStatus Delete(PermissionModel model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForDelete("sysPermission", model, model);
            ret = ((DapperContext)Context).Execute(sql, model);

            return ret;
        }

        public List<PermissionList> List(PermissionParam param)
        {
            List<PermissionList> ret = null;

            ret = ((DapperContext)Context).ExecuteQueryToList<PermissionList>(query.QueryForList(null),
                 param); 

            return ret;
        }
             
        public List<PermissionSearchResult> Search(PermissionParam param)
        {
            List<PermissionSearchResult> ret = null;

            ret = ((DapperContext)Context).ExecuteQueryToList<PermissionSearchResult>(query.QueryForSearch(null),
                 param);

            return ret;
        }

        
    }

}
