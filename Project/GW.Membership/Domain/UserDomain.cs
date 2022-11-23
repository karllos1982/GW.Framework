using GW.Common;
using GW.Core;
using GW.Membership.Contracts;
using GW.Membership.Contracts.Domain;
using GW.Membership.Models;
using GW.Membership.Contracts.Data;
using GW.Helpers;

namespace GW.Membership.Domain
{
    public class UserDomain : IUserDomain
    {
        public UserDomain(IContext context, IMembershipRepositorySet repositorySet)
        {
            Context = context;
            RepositorySet =repositorySet;  
        }

        public IContext Context { get; set; }

        public IMembershipRepositorySet RepositorySet { get; set; }

        public async Task<UserModel> FillChields( UserModel obj)
        {
            UserRolesParam param = new UserRolesParam();
            param.pUserID = obj.UserID;

            List<UserRolesModel> roles
                = await  RepositorySet.UserRoles.List(param);

            if (roles != null)
            {
                if (roles.Count > 0)
                {
                    obj.RoleList = roles;
                    obj.RoleID = roles[0].RoleID;

                    RoleModel r = await RepositorySet.Role
                        .Read(new RoleParam() { pRoleID = obj.RoleID });
                    obj.Role = r;
                }
            }

            //

            UserInstancesParam param2 = new UserInstancesParam();
            param2.pUserID = obj.UserID;

            List<UserInstancesModel> instances
                = await RepositorySet.UserInstances.List(param2);

            if (instances != null)
            {
                if (instances.Count > 0)
                {
                    obj.InstanceList = instances;
                    obj.InstanceID = instances[0].InstanceID;

                    InstanceModel i = await RepositorySet.Instance
                        .Read(new InstanceParam() { pInstanceID = obj.InstanceID });
                    obj.Instance = i;
                }
            }

            return obj;
        }

        public async Task<UserModel> Get(UserParam param)
        {
            UserModel ret = null;

            ret = await RepositorySet.User.Read(param); 

            if (ret != null)
            {
                ret = await FillChields(ret); 
            }
            
            return ret;
        }

        public async Task<List<UserList>> List(UserParam param)
        {
            List<UserList> ret = null;

            ret = await RepositorySet.User.List(param);           

            return ret;
        }

        public async Task<List<UserSearchResult>> Search(UserParam param)
        {
            List<UserSearchResult> ret = null;

            ret = await RepositorySet.User.Search(param);

            return ret;
        }

        public async Task EntryValidation(UserModel obj)
        {
            OperationStatus ret = null;

            ret = PrimaryValidation.Execute(obj, new List<string>());

            if (obj.RoleID == 0)
            {
                ret.AddInnerException("RoleID", GW.Localization.GetItem("Profile-NotBe-Null").Text);
                ret.Status = false;
            }

            if (!ret.Status)
            {
                ret.Error = new Exception(GW.Localization.GetItem("Validation-Error").Text);
            }

            Context.ExecutionStatus = ret;
            
        }

        public async Task InsertValidation(UserModel obj)
        {
            OperationStatus ret = new OperationStatus(true);
            UserParam param = new UserParam()
            {
                pEmail = obj.Email,
            };

            List<UserList> list
                = await RepositorySet.User.List(param);

            if (list != null)
            {
                if (list.Count > 0)
                {
                    ret.Status = false;
                    ret.Error = new Exception(GW.Localization.GetItem("Email-Exists").Text);
                }
            }

            Context.ExecutionStatus = ret;
         
        }

        public async Task UpdateValidation(UserModel obj)
        {
            OperationStatus ret = new OperationStatus(true);
            UserParam param = new UserParam() { pEmail = obj.Email };

            List<UserList> list
                = await RepositorySet.User.List(param);

            if (list != null)
            {
                if (list.Count > 0)
                {
                    if (list[0].UserID != obj.UserID)
                    {
                        ret.Status = false;
                        ret.Error = new Exception(GW.Localization.GetItem("Email-Exists").Text);
                    }
                }
            }

            Context.ExecutionStatus = ret;      

        }

        public async Task DeleteValidation(UserModel obj)
        {
            Context.ExecutionStatus = new OperationStatus(true);
        }

        public async Task<UserModel> Set(UserModel model, object userid)
        {
            UserModel ret = null;
            OPERATIONLOGENUM operation = OPERATIONLOGENUM.INSERT;

           await EntryValidation(model);

            if (Context.ExecutionStatus.Status)
            {

                UserModel old 
                    = await RepositorySet.User.Read(new UserParam() { pUserID = model.UserID });

                if (old == null)
                {
                    await InsertValidation(model);

                    if (Context.ExecutionStatus.Status)
                    {
                        model.CreateDate = DateTime.Now;
                        if (model.UserID == 0) { model.UserID = GW.Helpers.Utilities.GenerateId(); }
                        await RepositorySet.User.Create(model);
                    }
                }
                else
                {
                    model.CreateDate = old.CreateDate;
                    operation = OPERATIONLOGENUM.UPDATE;

                    await UpdateValidation(model);

                    if (Context.ExecutionStatus.Status)
                    {
                        await RepositorySet.User.Update(model);
                    }

                }

                if (Context.ExecutionStatus.Status && userid != null)
                {
                    await RepositorySet.User.Context
                        .RegisterDataLogAsync(userid.ToString(), operation, "SYSUSER",
                        model.UserID.ToString(), old, model);

                    ret = model;
                }

            }     

            return ret;
        }

     

        public async Task<UserModel> Delete(UserModel model, object userid)
        {
            UserModel ret = null;

            UserModel old 
                = await this.Get(new UserParam() { pUserID = model.UserID });

            if (old != null)
            {
                await DeleteValidation(model);

                if (Context.ExecutionStatus.Status)
                {
                    if (Context.ExecutionStatus.Status && old.RoleList != null)
                    {
                        foreach (UserRolesModel u in old.RoleList)
                        {
                            await RepositorySet.UserRoles.Delete(u);

                            if (!Context.ExecutionStatus.Status)
                            {
                                break;
                            }
                        }
                    }

                    if (Context.ExecutionStatus.Status && old.InstanceList != null)
                    {
                        foreach (UserInstancesModel u in old.InstanceList)
                        {
                           await RepositorySet.UserInstances.Delete(u);
                            if (!Context.ExecutionStatus.Status)
                            {
                                break;
                            }
                        }
                    }

                    if (Context.ExecutionStatus.Status)
                    {
                        await RepositorySet.User.Delete(old);
                        ret = model;
                    }                   
                }
            }
            else
            {
                Context.ExecutionStatus.Status = false;
                Context.ExecutionStatus.Error = new System.Exception(GW.Localization.GetItem("Record-NotFound").Text);

            }

            return ret;
        }

    
        public async  Task<UserModel> GetByEmail(string email)
        {
            UserModel ret = null;

            ret = await RepositorySet.User.GetByEmail(email);

            if (ret != null)
            {
                if (Context.ExecutionStatus.Status)
                {
                    ret = await FillChields(ret);
                }
            }       

            return ret;
        }

        public async Task<OperationStatus> UpdateUserLogin(UpdateUserLogin model)
        {
            OperationStatus ret = new OperationStatus(true);

            UserModel obj = null;

            obj = await RepositorySet.User.Read(new UserParam() { pUserID = model.UserID });

            if (obj != null)
            {
                ret = await RepositorySet.User.UpdateUserLogin(model);
            }
            else
            {
                ret.Status = false;
                ret.Error = new Exception(GW.Localization.GetItem("Login-User-NotFound").Text);
            }

            return ret;

        }

        public async Task<OperationStatus> SetPasswordRecoveryCode(ChangeUserPassword model)
        {
            OperationStatus ret = new OperationStatus(true);

            string errmsg = "";
            string code = "";

            UserModel usermatch = null;
            usermatch = await RepositorySet.User.GetByEmail(model.Email);

            if (Context.ExecutionStatus.Status)
            {

                if (usermatch != null)
                {
                    if (!model.ToActivate)
                    {
                        if (usermatch.IsActive == 1)
                        {
                            code = Utilities.GenerateCode(6);

                        }
                        else
                        {
                            errmsg = GW.Localization.GetItem("Login-Inactive-Account").Text;

                        }
                    }
                    else
                    {
                        code = Utilities.GenerateCode(6);
                    }

                }
                else
                {
                    errmsg = GW.Localization.GetItem("Login-User-NotFound").Text;
                }

                if (errmsg == "")
                {
                    OperationStatus aux;
                    aux = await RepositorySet.User.SetPasswordRecoveryCode(
                        new SetPasswordRecoveryCode()
                        {
                            UserID = usermatch.UserID,
                            Code = code
                        });

                    if (!aux.Status)
                    {
                        errmsg = GW.Localization.GetItem("Execution-Error").Text + aux.Error.Message;
                    }

                }

            }
            else
            {
                errmsg = GW.Localization.GetItem("Execution-Error").Text + Context.ExecutionStatus.Error.Message;
            }

            if (errmsg != "")
            {
                ret.Status = false;
                ret.Error = new Exception(errmsg);              
            }
            else
            {
               
               ret.Returns = code;
               
            }

            return ret;

        }

        public async Task<OperationStatus> ChangeUserPassword(ChangeUserPassword model)
        {
            OperationStatus ret = new OperationStatus(true);

            string errmsg = "";

            UserModel usermatch = null;
            usermatch = await RepositorySet.User.GetByEmail(model.Email);

            if (Context.ExecutionStatus.Status)
            {

                if (usermatch != null)
                {
                    if (usermatch.IsActive == 0)
                    {
                        errmsg = GW.Localization.GetItem("Login-Inactive-Account").Text;
                    }
                    else
                    {
                        if (usermatch.PasswordRecoveryCode != null)
                        {
                            if (usermatch.PasswordRecoveryCode != model.Code)
                            {
                                errmsg = GW.Localization.GetItem("User-Invalid-Password-Code").Text;

                            }
                        }
                        else
                        {

                            errmsg = GW.Localization.GetItem("User-Invalid-Password-Code").Text;
                        }

                    }

                }
                else
                {
                    errmsg = GW.Localization.GetItem("Login-User-NotFound").Text;
                }

                if (errmsg == "")
                {
                    OperationStatus aux;
                    string pwd = MD5.BuildMD5(model.NewPassword);
                    pwd = MD5.BuildMD5(pwd + usermatch.Salt);

                    ChangeUserPassword change = new ChangeUserPassword();
                    change.NewPassword = pwd;
                    change.UserID = usermatch.UserID;

                    aux = await RepositorySet.User.ChangeUserPassword(change);

                    if (!aux.Status)
                    {
                        errmsg = GW.Localization.GetItem("Execution-Error").Text + aux.Error.Message;
                    }

                }

            }
            else
            {
                errmsg = GW.Localization.GetItem("Execution-Error").Text + Context.ExecutionStatus.Error.Message;
            }

            if (errmsg != "")
            {
                ret.Status = false;
                ret.Error = new Exception(errmsg);           
            }
         
            return ret;

        }

        public async Task<OperationStatus> ActiveUserAccount(ActiveUserAccount model)
        {
            OperationStatus ret = new OperationStatus(true);

            string errmsg = "";
            UserModel usermatch = null;
            usermatch = await RepositorySet.User.GetByEmail(model.Email);

            if (Context.ExecutionStatus.Status)
            {

                if (usermatch != null)
                {
                    if (usermatch.IsActive == 1)
                    {
                        errmsg = GW.Localization.GetItem("Account-Active").Text;
                    }
                    else
                    {
                        if (usermatch.PasswordRecoveryCode != null)
                        {
                            if (usermatch.PasswordRecoveryCode != model.Code)
                            {
                                errmsg = GW.Localization.GetItem("User-Invalid-Activation-Code").Text;
                            }
                        }
                        else
                        {

                            errmsg = GW.Localization.GetItem("User-Invalid-Activation-Code").Text;
                        }

                    }

                }
                else
                {
                    errmsg = GW.Localization.GetItem("Login-User-NotFound").Text;
                }

                if (errmsg == "")
                {
                    OperationStatus aux;

                    aux = await RepositorySet.User.ActiveUserAccount(model);

                    if (!aux.Status)
                    {
                        errmsg = GW.Localization.GetItem("Execution-Error").Text + aux.Error.Message;
                    }

                }

            }
            else
            {
                errmsg = GW.Localization.GetItem("Execution-Error").Text + Context.ExecutionStatus.Error.Message;
            }

            if (errmsg != "")
            {
                ret.Status = false;
                ret.Error = new Exception(errmsg);              
            }
          
            return ret;

        }

        public async Task<OperationStatus> ChangeUserProfileImage(ChangeUserImage model)
        {
            OperationStatus ret = new OperationStatus(true);

            string errmsg = "";

            if (model.FileName == "")
            {
                errmsg = GW.Localization.GetItem("User-No-Image").Text;
            }
            else
            {
                UserModel usermatch = null;
                usermatch = await RepositorySet.User.Read(new UserParam() { pUserID=model.UserID });

                if (Context.ExecutionStatus.Status)
                {

                    if (usermatch != null)
                    {
                        if (usermatch.IsActive == 0)
                        {
                            errmsg = GW.Localization.GetItem("Login-Inactive-Account").Text;
                        }

                    }
                    else
                    {
                        errmsg = GW.Localization.GetItem("Login-User-NotFound").Text;
                    }

                    if (errmsg == "")
                    {
                        OperationStatus aux;
                        aux = await RepositorySet.User.ChangeUserProfileImage(model);

                        if (!aux.Status)
                        {
                            errmsg = GW.Localization.GetItem("Execution-Error").Text + aux.Error.Message;
                        }

                    }

                }
                else
                {
                    errmsg = GW.Localization.GetItem("Execution-Error").Text + Context.ExecutionStatus.Error.Message;
                }
            }

            if (errmsg != "")
            {
                ret.Status = false;
                ret.Error = new Exception(errmsg);          
            }         

            return ret;

        }

        public async Task<OperationStatus> UpdateLoginFailCounter(UpdateUserLoginFailCounter model)
        {
            OperationStatus ret = new OperationStatus(true);

            ret = await RepositorySet.User.UpdateLoginFailCounter(model);

            return ret;

        }

        public async Task<OperationStatus> ChangeState(UserChangeState model)
        {
            OperationStatus ret = new OperationStatus(true);

            ret = await RepositorySet.User.ChangeState(model);    

            return ret;

        }

        public async Task<OperationStatus> SetDateLogout(Int64 userid)
        {
            OperationStatus ret = new OperationStatus(true);

            SessionLogParam param = new SessionLogParam();
          
            param.pUserID = userid;
            ret = await RepositorySet.SessionLog.SetDateLogout(param);

  
            return ret;

        }

        public async Task<OperationStatus> AddRoleToUser(Int64 userid, Int64 roleid, bool gocommit)
        {
            OperationStatus ret = new OperationStatus(true);

            List<UserRolesModel> list;
            UserRolesParam param = new UserRolesParam();
            param.pUserID = userid;
            param.pRoleID = roleid;

            list = await RepositorySet.UserRoles.Search(param);

            if (list != null)
            {
                if (list.Count > 0)
                {
                    ret.Status = false;
                    ret.Error = new Exception(GW.Localization.GetItem("User-Role-Exists").Text);
                }
            }

            if (ret.Status)
            {
                UserRolesModel obj = new UserRolesModel();
                obj.UserRoleID = Utilities.GenerateId();
                obj.RoleID = roleid;
                obj.UserID = userid;

                 await RepositorySet.UserRoles.Create(obj);

                if (ret.Status)
                {
                    ret.Returns = obj;
                }

            }

            return ret;
        }

        public async Task<OperationStatus> RemoveRoleFromUser(Int64 userid, Int64 roleid, bool gocommit)
        {
            OperationStatus ret = new OperationStatus(true);

            List<UserRolesModel> list;
            UserRolesParam param = new UserRolesParam();
            param.pUserID = userid;
            param.pRoleID = roleid;

            list = await RepositorySet.UserRoles.Search(param);

            if (list != null)
            {
                if (list.Count == 0)
                {
                    ret.Status = false;
                    ret.Error = new Exception(GW.Localization.GetItem("User-Role-No-Exists").Text);
                }
            }
            else
            {
                ret.Status = false;
                ret.Error = new Exception(GW.Localization.GetItem("User-Role-No-Exists").Text);
            }

            if (ret.Status)
            {
                UserRolesModel obj = list[0];

                await RepositorySet.UserRoles.Delete(obj);

                if (ret.Status)
                {
                    ret.Returns = obj;
                }
            }        

            return ret;
        }


        public async Task<OperationStatus> AddInstanceToUser(Int64 userid, Int64 instanceid, bool gocommit)
        {
            OperationStatus ret = new OperationStatus(true);

            List<UserInstancesModel> list;
            UserInstancesParam param = new UserInstancesParam();
            param.pUserID = userid;
            param.pInstanceID = instanceid;

            list = await RepositorySet.UserInstances.Search(param);

            if (list != null)
            {
                if (list.Count > 0)
                {
                    ret.Status = false;
                    ret.Error = new Exception(GW.Localization.GetItem("User-Instance-Exists").Text);
                }
            }

            if (ret.Status)
            {
                UserInstancesModel obj = new UserInstancesModel();
                obj.UserInstanceID = Utilities.GenerateId();
                obj.InstanceID = instanceid;
                obj.UserID = userid;

                 await RepositorySet.UserInstances.Create(obj);

                if (ret.Status)
                {
                    ret.Returns = obj;
                }

            }

            return ret;
        }


        public async Task<OperationStatus> RemoveInstanceFromUser(Int64 userid, Int64 instanceid, bool gocommit)
        {
            OperationStatus ret = new OperationStatus(true);

            List<UserInstancesModel> list;
            UserInstancesParam param = new UserInstancesParam();
            param.pUserID = userid;
            param.pInstanceID = instanceid;

            list = await RepositorySet.UserInstances.Search(param);

            if (list != null)
            {
                if (list.Count == 0)
                {
                    ret.Status = false;
                    ret.Error = new Exception(GW.Localization.GetItem("User-Instance-No-Exists").Text);
                }
            }
            else
            {
                ret.Status = false;
                ret.Error = new Exception(GW.Localization.GetItem("User-Instance-No-Exists").Text);
            }

            if (ret.Status)
            {
                UserInstancesModel obj = list[0];

                await RepositorySet.UserInstances.Delete(obj);

                if (ret.Status)
                {
                    ret.Returns = obj;
                }
            }         

            return ret;
        }

    }
}
