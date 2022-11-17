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

        public OperationStatus Create(ObjectPermissionModel model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForCreate("sysObjectPermission", model,model);
            ret = ((DapperContext)Context).Execute(sql, model);

            return ret;
        }

        public ObjectPermissionModel Read(ObjectPermissionParam param)
        {
            ObjectPermissionModel ret = null;
            
            string sql = query.QueryForGet(null);

            ret = ((DapperContext)Context).ExecuteQueryFirst<ObjectPermissionModel>(sql, param); 
                 
            return ret;
        }

        public OperationStatus Update(ObjectPermissionModel model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForUpdate("sysObjectPermission", model, model);
            ret = ((DapperContext)Context).Execute(sql, model);

            return ret;
        }

        public OperationStatus Delete(ObjectPermissionModel model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForDelete("sysObjectPermission", model, model);
            ret = ((DapperContext)Context).Execute(sql, model);

            return ret;
        }

        public List<ObjectPermissionList> List(ObjectPermissionParam param)
        {
            List<ObjectPermissionList> ret = null;

            ret = ((DapperContext)Context).ExecuteQueryToList<ObjectPermissionList>(query.QueryForList(null),
                 param); 

            return ret;
        }
             
        public List<ObjectPermissionSearchResult> Search(ObjectPermissionParam param)
        {
            List<ObjectPermissionSearchResult> ret = null;

            ret = ((DapperContext)Context).ExecuteQueryToList<ObjectPermissionSearchResult>(query.QueryForSearch(null),
                 param);

            return ret;
        }

        
    }

}
