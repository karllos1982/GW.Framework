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

        public OperationStatus Create(RoleModel model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForCreate("sysRole", model,model);
            ret = ((DapperContext)Context).Execute(sql, model);

            return ret;
        }

        public RoleModel Read(RoleParam param)
        {
            RoleModel ret = null;
            
            string sql = query.QueryForGet(null);

            ret = ((DapperContext)Context).ExecuteQueryFirst<RoleModel>(sql, param); 
                 
            return ret;
        }

        public OperationStatus Update(RoleModel model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForUpdate("sysRole", model, model);
            ret = ((DapperContext)Context).Execute(sql, model);

            return ret;
        }

        public OperationStatus Delete(RoleModel model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForDelete("sysRole", model, model);
            ret = ((DapperContext)Context).Execute(sql, model);

            return ret;
        }

        public List<RoleList> List(RoleParam param)
        {
            List<RoleList> ret = null;

            ret = ((DapperContext)Context).ExecuteQueryToList<RoleList>(query.QueryForList(null),
                 param); 

            return ret;
        }
             
        public List<RoleSearchResult> Search(RoleParam param)
        {
            List<RoleSearchResult> ret = null;

            ret = ((DapperContext)Context).ExecuteQueryToList<RoleSearchResult>(query.QueryForSearch(null),
                 param);

            return ret;
        }

        
    }

}
