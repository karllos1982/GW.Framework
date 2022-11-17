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

        public OperationStatus Create(UserRolesModel model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForCreate("sysUserRoles", model,model);
            ret = ((DapperContext)Context).Execute(sql, model);

            return ret;
        }

        public UserRolesModel Read(UserRolesParam param)
        {
            UserRolesModel ret = null;
            
            string sql = query.QueryForGet(null);

            ret = ((DapperContext)Context).ExecuteQueryFirst<UserRolesModel>(sql, param); 
                 
            return ret;
        }

        public OperationStatus Update(UserRolesModel model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForUpdate("sysUserRoles", model, model);
            ret = ((DapperContext)Context).Execute(sql, model);

            return ret;
        }

        public OperationStatus Delete(UserRolesModel model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForDelete("sysUserRoles", model, model);
            ret = ((DapperContext)Context).Execute(sql, model);

            return ret;
        }

        public List<UserRolesList> List(UserRolesParam param)
        {
            List<UserRolesList> ret = null;

            ret = ((DapperContext)Context).ExecuteQueryToList<UserRolesList>(query.QueryForList(null),
                 param); 

            return ret;
        }
             
        public List<UserRolesSearchResult> Search(UserRolesParam param)
        {
            List<UserRolesSearchResult> ret = null;

            ret = ((DapperContext)Context).ExecuteQueryToList<UserRolesSearchResult>(query.QueryForSearch(null),
                 param);

            return ret;
        }

        
    }

}
