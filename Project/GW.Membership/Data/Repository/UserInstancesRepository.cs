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
        }
         
        private UserInstancesQueryBuilder query = new UserInstancesQueryBuilder();

        public IContext Context { get; set; }

        public OperationStatus Create(UserInstancesModel model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForCreate("sysUserInstances", model,model);
            ret = ((DapperContext)Context).Execute(sql, model);

            return ret;
        }

        public UserInstancesModel Read(UserInstancesParam param)
        {
            UserInstancesModel ret = null;
            
            string sql = query.QueryForGet(null);

            ret = ((DapperContext)Context).ExecuteQueryFirst<UserInstancesModel>(sql, param); 
                 
            return ret;
        }

        public OperationStatus Update(UserInstancesModel model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForUpdate("sysUserInstances", model, model);
            ret = ((DapperContext)Context).Execute(sql, model);

            return ret;
        }

        public OperationStatus Delete(UserInstancesModel model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForDelete("sysUserInstances", model, model);
            ret = ((DapperContext)Context).Execute(sql, model);

            return ret;
        }

        public List<UserInstancesList> List(UserInstancesParam param)
        {
            List<UserInstancesList> ret = null;

            ret = ((DapperContext)Context).ExecuteQueryToList<UserInstancesList>(query.QueryForList(null),
                 param); 

            return ret;
        }
             
        public List<UserInstancesModel> Search(UserInstancesParam param)
        {
            List<UserInstancesModel> ret = null;

            ret = ((DapperContext)Context).ExecuteQueryToList<UserInstancesModel>(query.QueryForSearch(null),
                 param);

            return ret;
        }

        
    }

}
