﻿using GW.Common;
using GW.Core;
using GW.Membership.Contracts;
using GW.Membership.Contracts.Data;
using GW.Membership.Models;

namespace GW.Membership.Data
{
    public class UserRepository : IUserRepository        
    {
       
        public UserRepository(IContext context)
        {
            Context = context;            
        }
         
        private UserQueryBuilder query = new UserQueryBuilder();

        public IContext Context { get; set; }

        public OperationStatus Create(UserModel model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForCreate("sysUser", model,model);
            ret = ((DapperContext)Context).Execute(sql, model);

            return ret;
        }

        public UserModel Read(UserParam param)
        {
            UserModel ret = null;
            
            string sql = query.QueryForGet(null);

            ret = ((DapperContext)Context).ExecuteQueryFirst<UserModel>(sql, param); 
                 
            return ret;
        }

        public OperationStatus Update(UserModel model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForUpdate("sysUser", model, model);
            ret = ((DapperContext)Context).Execute(sql, model);

            return ret;
        }

        public OperationStatus Delete(UserModel model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForDelete("sysUser", model, model);
            ret = ((DapperContext)Context).Execute(sql, model);

            return ret;
        }

        public List<UserList> List(UserParam param)
        {
            List<UserList> ret = null;

            ret = ((DapperContext)Context).ExecuteQueryToList<UserList>(query.QueryForList(null),
                 param); 

            return ret;
        }
             
        public List<UserSearchResult> Search(UserParam param)
        {
            List<UserSearchResult> ret = null;

            ret = ((DapperContext)Context).ExecuteQueryToList<UserSearchResult>(query.QueryForSearch(null),
                 param);

            return ret;
        }

        
    }

}