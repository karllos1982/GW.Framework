using GW.Common;
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
            TableName = "sysUser";
            PKFieldName = "UserID";

        }
         
        private UserQueryBuilder query = new UserQueryBuilder();

        public string TableName { get; set; }

        public string PKFieldName { get; set; }

        public IContext Context { get; set; }

        public async Task Create(UserEntry model)
        {
           
            string sql = query.QueryForCreate(TableName, model,model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);
         
        }

        public async Task<UserResult> Read(UserParam param)
        {
            UserResult ret = null;
            
            string sql = query.QueryForGet(null);

            ret = await ((DapperContext)Context).ExecuteQueryFirstAsync<UserResult>(sql, param); 
                 
            return ret;
        }

        public async Task Update(UserEntry model)
        {
           
            string sql = query.QueryForUpdate(TableName, model, model);
             await((DapperContext)Context).ExecuteAsync(sql, model);
        
        }

        public async Task Delete(UserEntry model)
        {
           
            string sql = query.QueryForDelete(TableName, model, model);
            await ((DapperContext)Context).ExecuteAsync(sql, model);
           
        }

        public async Task<List<UserList>> List(UserParam param)
        {
            List<UserList> ret = null;

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<UserList>(query.QueryForList(null), param); 
               
            return ret;
        }
             
        public async Task<List<UserResult>> Search(UserParam param)
        {
            List<UserResult> ret = null;

            ret = await ((DapperContext)Context)
                .ExecuteQueryToListAsync<UserResult>(query.QueryForSearch(null),  param);
               
            return ret;
        }

        //

        public async Task<UserResult> GetByEmail(string email)
        {
            UserResult ret = null;

            string sql = query.QueryForGetByEmail();

            ret = await ((DapperContext)Context).ExecuteQueryFirstAsync<UserResult>(sql,
                 new UserParam { pEmail = email });

            return ret;
        }

        public async Task<OperationStatus> UpdateUserLogin(UpdateUserLogin model)
        {
            OperationStatus ret = new OperationStatus(true);
            string sql = query.QueryForUpdateUserLogin();
            await ((DapperContext)Context).ExecuteAsync(sql, model);
            ret = Context.ExecutionStatus;

            return ret;
        }

        public async Task<OperationStatus> SetPasswordRecoveryCode(SetPasswordRecoveryCode model)
        {
            OperationStatus ret = new OperationStatus(true);
            string sql = query.QueryForSetPasswordRecoveryCode();
             await ((DapperContext)Context).ExecuteAsync(sql, model);

            ret = Context.ExecutionStatus; 

            return ret;
        }

        public async Task<OperationStatus> ChangeUserPassword(ChangeUserPassword model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForChangeUserPassword();
            await ((DapperContext)Context).ExecuteAsync(sql, model);

            ret = Context.ExecutionStatus;

            return ret;
        }

        public async Task<OperationStatus> ActiveUserAccount(ActiveUserAccount model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForActiveAccount();
            await ((DapperContext)Context).ExecuteAsync(sql, model);

            ret = Context.ExecutionStatus;

            return ret;
        }

        public async Task<OperationStatus> ChangeUserProfileImage(ChangeUserImage model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForChangeUserProfileImage();
             await ((DapperContext)Context).ExecuteAsync(sql, model);

            ret = Context.ExecutionStatus;

            return ret;
        }

        public async Task<OperationStatus> UpdateLoginFailCounter(UpdateUserLoginFailCounter model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForSetLoginFailCounter(model.Reset);
            await ((DapperContext)Context).ExecuteAsync(sql, model);
            ret = Context.ExecutionStatus;

            return ret;
        }

        public async Task<OperationStatus> ChangeState(UserChangeState model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForChangeUserState();
            await ((DapperContext)Context).ExecuteAsync(sql, model);

            ret = Context.ExecutionStatus;

            return ret;
        }

    }

}
