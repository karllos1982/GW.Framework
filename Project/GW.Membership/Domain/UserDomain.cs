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
        private string lang = "";

        public UserDomain(IContext context, IMembershipRepositorySet repositorySet)
        {
            Context = context;
            RepositorySet =repositorySet;
            lang = Context.Settings.LocalizationLanguage;
        }

        public IContext Context { get; set; }

        public IMembershipRepositorySet RepositorySet { get; set; }

        public async Task<UserResult> FillChields(UserResult obj)
        {
            UserRolesParam param = new UserRolesParam();
            param.pUserID = obj.UserID;

            List<UserRolesResult> roles
                = await  RepositorySet.UserRoles.Search(param);

            obj.Roles = roles;

            //

            UserInstancesParam param2 = new UserInstancesParam();
            param2.pUserID = obj.UserID;

            List<UserInstancesResult> instances
                = await RepositorySet.UserInstances.Search(param2);

            obj.Instances = instances; 

            return obj;
        }

        public async Task<UserResult> Get(UserParam param)
        {
            UserResult ret = null;

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

        public async Task<List<UserResult>> Search(UserParam param)
        {
            List<UserResult> ret = null;

            ret = await RepositorySet.User.Search(param);

            return ret;
        }

        public async Task EntryValidation(UserEntry obj)
        {
            OperationStatus ret = null;

            ret = PrimaryValidation.Execute(obj, new List<string>());        

            if (!ret.Status)
            {
                ret.Error 
                    = new Exception(GW.Localization.GetItem("Validation-Error", lang).Text);
            }

            Context.ExecutionStatus = ret;
            
        }

        public async Task InsertValidation(UserEntry obj)
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
                    string msg = GW.Localization.GetItem("Email-Exists", lang).Text;
                    ret.Error = new Exception(msg);
                    ret.AddInnerException("Email", msg);
                }
            }

            Context.ExecutionStatus = ret;
         
        }

        public async Task UpdateValidation(UserEntry obj)
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
                        string msg = GW.Localization.GetItem("Email-Exists", lang).Text;
                        ret.Error = new Exception(msg);
                        ret.AddInnerException("Email", msg);
                    }
                }
            }

            Context.ExecutionStatus = ret;      

        }

        public async Task DeleteValidation(UserEntry obj)
        {
            Context.ExecutionStatus = new OperationStatus(true);
        }

        public async Task<UserEntry> Set(UserEntry model, object userid)
        {
            UserEntry ret = null;
            OPERATIONLOGENUM operation = OPERATIONLOGENUM.INSERT;

           await EntryValidation(model);

            if (Context.ExecutionStatus.Status)
            {

                UserResult old 
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

     

        public async Task<UserEntry> Delete(UserEntry model, object userid)
        {
            UserEntry ret = null;

            UserResult old 
                = await this.Get(new UserParam() { pUserID = model.UserID });

            if (old != null)
            {
                await DeleteValidation(model);

                if (Context.ExecutionStatus.Status)
                {
                    if (Context.ExecutionStatus.Status && old.Roles != null)
                    {
                        foreach (UserRolesResult u in old.Roles)
                        {
                            await RepositorySet.UserRoles.Delete(
                                    new UserRolesEntry() { 
                                        UserRoleID = u.UserRoleID,  
                                        UserID  = u.UserID, 
                                        RoleID = u.RoleID
                                    } 
                                );

                            if (!Context.ExecutionStatus.Status)
                            {
                                break;
                            }
                        }
                    }

                    if (Context.ExecutionStatus.Status && old.Instances != null)
                    {
                        foreach (UserInstancesResult u in old.Instances)
                        {
                           await RepositorySet.UserInstances.Delete(
                                     new UserInstancesEntry()
                                     {
                                         UserInstanceID = u.UserInstanceID, 
                                         UserID = u.UserID, 
                                         InstanceID =   u.InstanceID

                                     }
                               );

                            if (!Context.ExecutionStatus.Status)
                            {
                                break;
                            }
                        }
                    }

                    if (Context.ExecutionStatus.Status)
                    {
                        await RepositorySet.User.Delete(model);

                        if (Context.ExecutionStatus.Status && userid != null)
                        {
                            await RepositorySet.User.Context
                                .RegisterDataLogAsync(userid.ToString(), OPERATIONLOGENUM.DELETE, "SYSUSER",
                                model.UserID.ToString(), old, model);

                            ret = model;
                        }

                    }                   
                }
            }
            else
            {
                Context.ExecutionStatus.Status = false;
                Context.ExecutionStatus.Error 
                    = new System.Exception(GW.Localization.GetItem("Record-NotFound", lang).Text);

            }

            return ret;
        }

    
        public async  Task<UserResult> GetByEmail(string email)
        {
            UserResult ret = null;

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

            UserResult obj = null;

            obj = await RepositorySet.User.Read(new UserParam() { pUserID = model.UserID });

            if (obj != null)
            {
                ret = await RepositorySet.User.UpdateUserLogin(model);
            }
            else
            {
                ret.Status = false;
                ret.Error 
                    = new Exception(GW.Localization.GetItem("Login-User-NotFound", lang).Text);
            }

            return ret;

        }

        public async Task<OperationStatus> SetPasswordRecoveryCode(ChangeUserPassword model)
        {
            OperationStatus ret = new OperationStatus(true);

            string errmsg = "";
            string code = "";

            UserResult usermatch = null;
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
                            errmsg = GW.Localization.GetItem("Login-Inactive-Account", lang).Text;

                        }
                    }
                    else
                    {
                        code = Utilities.GenerateCode(6);
                    }

                }
                else
                {
                    errmsg = GW.Localization.GetItem("Login-User-NotFound", lang).Text;
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
                        errmsg 
                            = GW.Localization.GetItem("Execution-Error", lang).Text + aux.Error.Message;
                    }

                }

            }
            else
            {
                errmsg
                    = GW.Localization.GetItem("Execution-Error", lang).Text + Context.ExecutionStatus.Error.Message;
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

            UserResult usermatch = null;
            usermatch = await RepositorySet.User.GetByEmail(model.Email);

            if (Context.ExecutionStatus.Status)
            {

                if (usermatch != null)
                {
                    if (usermatch.IsActive == 0)
                    {
                        errmsg = GW.Localization.GetItem("Login-Inactive-Account",lang).Text;
                    }
                    else
                    {
                        if (usermatch.PasswordRecoveryCode != null)
                        {
                            if (usermatch.PasswordRecoveryCode != model.Code)
                            {
                                errmsg 
                                    = GW.Localization.GetItem("User-Invalid-Password-Code",lang).Text;

                            }
                        }
                        else
                        {

                            errmsg
                                = GW.Localization.GetItem("User-Invalid-Password-Code",lang).Text;
                        }

                    }

                }
                else
                {
                    errmsg = GW.Localization.GetItem("Login-User-NotFound", lang).Text;
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
                        errmsg = GW.Localization.GetItem("Execution-Error", lang).Text + aux.Error.Message;
                    }

                }

            }
            else
            {
                errmsg
                    = GW.Localization.GetItem("Execution-Error", lang).Text + Context.ExecutionStatus.Error.Message;
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
            UserResult usermatch = null;
            usermatch = await RepositorySet.User.GetByEmail(model.Email);

            if (Context.ExecutionStatus.Status)
            {

                if (usermatch != null)
                {
                    if (usermatch.IsActive == 1)
                    {
                        errmsg = GW.Localization.GetItem("Account-Active", lang).Text;
                    }
                    else
                    {
                        if (usermatch.PasswordRecoveryCode != null)
                        {
                            if (usermatch.PasswordRecoveryCode != model.Code)
                            {
                                errmsg = GW.Localization.GetItem("User-Invalid-Activation-Code", lang).Text;
                            }
                        }
                        else
                        {

                            errmsg = GW.Localization.GetItem("User-Invalid-Activation-Code", lang).Text;
                        }

                    }

                }
                else
                {
                    errmsg = GW.Localization.GetItem("Login-User-NotFound", lang).Text;
                }

                if (errmsg == "")
                {
                    OperationStatus aux;

                    aux = await RepositorySet.User.ActiveUserAccount(model);

                    if (!aux.Status)
                    {
                        errmsg = GW.Localization.GetItem("Execution-Error", lang).Text + aux.Error.Message;
                    }

                }

            }
            else
            {
                errmsg 
                    = GW.Localization.GetItem("Execution-Error", lang).Text + Context.ExecutionStatus.Error.Message;
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
                errmsg = GW.Localization.GetItem("User-No-Image", lang).Text;
            }
            else
            {
                UserResult usermatch = null;
                usermatch = await RepositorySet.User.Read(new UserParam() { pUserID=model.UserID });

                if (Context.ExecutionStatus.Status)
                {

                    if (usermatch != null)
                    {
                        if (usermatch.IsActive == 0)
                        {
                            errmsg = GW.Localization.GetItem("Login-Inactive-Account", lang).Text;
                        }

                    }
                    else
                    {
                        errmsg = GW.Localization.GetItem("Login-User-NotFound", lang).Text;
                    }

                    if (errmsg == "")
                    {
                        OperationStatus aux;
                        aux = await RepositorySet.User.ChangeUserProfileImage(model);

                        if (!aux.Status)
                        {
                            errmsg = GW.Localization.GetItem("Execution-Error", lang).Text + aux.Error.Message;
                        }

                    }

                }
                else
                {
                    errmsg = GW.Localization.GetItem("Execution-Error", lang).Text + Context.ExecutionStatus.Error.Message;
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

        public async Task<UserRolesEntry> AddRoleToUser(Int64 userid, Int64 roleid)
        {
            UserRolesEntry ret = null;

            List<UserRolesResult> list;
            UserRolesParam param = new UserRolesParam();
            param.pUserID = userid;
            param.pRoleID = roleid;

            list = await RepositorySet.UserRoles.Search(param);

            if (list != null)
            {
                if (list.Count > 0)
                {
                    Context.ExecutionStatus.Status = false;
                    Context.ExecutionStatus.Error 
                        = new Exception(GW.Localization.GetItem("User-Role-Exists", lang).Text);
                }
            }

            if (Context.ExecutionStatus.Status)
            {
                UserRolesEntry obj = new UserRolesEntry();
                obj.UserRoleID = Utilities.GenerateId();
                obj.RoleID = roleid;
                obj.UserID = userid;

                 await RepositorySet.UserRoles.Create(obj);

                if (Context.ExecutionStatus.Status)
                {
                    ret= obj;
                }

            }
          
            return ret;
        }

        public async Task<UserRolesEntry> RemoveRoleFromUser(Int64 userid, Int64 roleid)
        {
            UserRolesEntry ret = null;

            List<UserRolesResult> list;
            UserRolesParam param = new UserRolesParam();
            param.pUserID = userid;
            param.pRoleID = roleid;

            list = await RepositorySet.UserRoles.Search(param);

            if (list != null)
            {
                if (list.Count == 0)
                {
                    Context.ExecutionStatus.Status = false;
                    Context.ExecutionStatus.Error
                        = new Exception(GW.Localization.GetItem("User-Role-No-Exists", lang).Text);
                }
            }
            else
            {
                Context.ExecutionStatus.Status = false;
                Context.ExecutionStatus.Error 
                    = new Exception(GW.Localization.GetItem("User-Role-No-Exists", lang).Text);
            }

            if (Context.ExecutionStatus.Status)
            {
                UserRolesResult obj = list[0];

                await RepositorySet.UserRoles.Delete(new UserRolesEntry()
                {
                    UserRoleID = obj.UserRoleID,
                    UserID = obj.UserID,
                    RoleID = obj.RoleID
                });
               

                if (Context.ExecutionStatus.Status)
                {
                    ret= new UserRolesEntry()
                        {
                            UserRoleID = obj.UserRoleID,
                            UserID = obj.UserID,
                            RoleID = obj.RoleID
                        };
                }
            }        

            return ret;
        }


        public async Task<UserInstancesEntry> AddInstanceToUser(Int64 userid, Int64 instanceid)
        {
            UserInstancesEntry ret = null;

            List<UserInstancesResult> list;
            UserInstancesParam param = new UserInstancesParam();
            param.pUserID = userid;
            param.pInstanceID = instanceid;

            list = await RepositorySet.UserInstances.Search(param);

            if (list != null)
            {
                if (list.Count > 0)
                {
                    Context.ExecutionStatus.Status = false;
                    Context.ExecutionStatus.Error 
                        = new Exception(GW.Localization.GetItem("User-Instance-Exists", lang).Text);
                }
            }

            if (Context.ExecutionStatus.Status)
            {
                UserInstancesEntry obj = new UserInstancesEntry();
                obj.UserInstanceID = Utilities.GenerateId();
                obj.InstanceID = instanceid;
                obj.UserID = userid;

                 await RepositorySet.UserInstances.Create(obj);

                if (Context.ExecutionStatus.Status)
                {
                    ret = obj;
                }

            }

            return ret;
        }


        public async Task<UserInstancesEntry> RemoveInstanceFromUser(Int64 userid, Int64 instanceid)
        {
            UserInstancesEntry ret = null; ;

            List<UserInstancesResult> list;
            UserInstancesParam param = new UserInstancesParam();
            param.pUserID = userid;
            param.pInstanceID = instanceid;

            list = await RepositorySet.UserInstances.Search(param);

            if (list != null)
            {
                if (list.Count == 0)
                {
                    Context.ExecutionStatus.Status = false;
                    Context.ExecutionStatus.Error
                        = new Exception(GW.Localization.GetItem("User-Instance-No-Exists", lang).Text);
                }
            }
            else
            {
                Context.ExecutionStatus.Status = false;
                Context.ExecutionStatus.Error 
                    = new Exception(GW.Localization.GetItem("User-Instance-No-Exists", lang).Text);
            }

            if (Context.ExecutionStatus.Status)
            {
                UserInstancesResult obj = list[0];

                await RepositorySet.UserInstances.Delete(new UserInstancesEntry()
                {
                    UserInstanceID = obj.UserInstanceID,
                    UserID = obj.UserID,
                    InstanceID = obj.InstanceID

                });

                if (Context.ExecutionStatus.Status)
                {
                    ret = new UserInstancesEntry()
                    {
                        UserInstanceID = obj.UserInstanceID,
                        UserID = obj.UserID,
                        InstanceID = obj.InstanceID

                    };
                }
            }         

            return ret;
        }

    }
}
