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

            ret = PrimaryValidation.Execute(obj, new List<string>(),Context.LocalizationLanguage);        

            if (!ret.Status)
            {
                ret.Error 
                    = new Exception(GW.LocalizationText.Get("Validation-Error", Context.LocalizationLanguage).Text);
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
                    string msg = GW.LocalizationText.Get("Email-Exists", Context.LocalizationLanguage).Text;
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
                        string msg = GW.LocalizationText.Get("Email-Exists", Context.LocalizationLanguage).Text;
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
                    = new System.Exception(GW.LocalizationText.Get("Record-NotFound", Context.LocalizationLanguage).Text);

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
                    = new Exception(GW.LocalizationText.Get("Login-User-NotFound", Context.LocalizationLanguage).Text);
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
                        if (usermatch.IsActive)
                        {
                            code = Utilities.GenerateCode(6);

                        }
                        else
                        {
                            errmsg = GW.LocalizationText.Get("Login-Inactive-Account", Context.LocalizationLanguage).Text;

                        }
                    }
                    else
                    {
                        code = Utilities.GenerateCode(6);
                    }

                }
                else
                {
                    errmsg = GW.LocalizationText.Get("Login-User-NotFound", Context.LocalizationLanguage).Text;
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
                            = GW.LocalizationText.Get("Execution-Error", Context.LocalizationLanguage).Text + aux.Error.Message;
                    }

                }

            }
            else
            {
                errmsg
                    = GW.LocalizationText.Get("Execution-Error", Context.LocalizationLanguage).Text + Context.ExecutionStatus.Error.Message;
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
                    if (!usermatch.IsActive)
                    {
                        errmsg = GW.LocalizationText.Get("Login-Inactive-Account",Context.LocalizationLanguage).Text;
                    }
                    else
                    {
                        if (usermatch.PasswordRecoveryCode != null)
                        {
                            if (usermatch.PasswordRecoveryCode != model.Code)
                            {
                                errmsg 
                                    = GW.LocalizationText.Get("User-Invalid-Password-Code",Context.LocalizationLanguage).Text;

                            }
                        }
                        else
                        {

                            errmsg
                                = GW.LocalizationText.Get("User-Invalid-Password-Code",Context.LocalizationLanguage).Text;
                        }

                    }

                }
                else
                {
                    errmsg = GW.LocalizationText.Get("Login-User-NotFound", Context.LocalizationLanguage).Text;
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
                        errmsg = GW.LocalizationText.Get("Execution-Error", Context.LocalizationLanguage).Text + aux.Error.Message;
                    }

                }

            }
            else
            {
                errmsg
                    = GW.LocalizationText.Get("Execution-Error", Context.LocalizationLanguage).Text + Context.ExecutionStatus.Error.Message;
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
                    if (usermatch.IsActive)
                    {
                        errmsg = GW.LocalizationText.Get("Account-Active", Context.LocalizationLanguage).Text;
                    }
                    else
                    {
                        if (usermatch.PasswordRecoveryCode != null)
                        {
                            if (usermatch.PasswordRecoveryCode != model.Code)
                            {
                                errmsg = GW.LocalizationText.Get("User-Invalid-Activation-Code", Context.LocalizationLanguage).Text;
                            }
                        }
                        else
                        {

                            errmsg = GW.LocalizationText.Get("User-Invalid-Activation-Code", Context.LocalizationLanguage).Text;
                        }

                    }

                }
                else
                {
                    errmsg = GW.LocalizationText.Get("Login-User-NotFound", Context.LocalizationLanguage).Text;
                }

                if (errmsg == "")
                {
                    OperationStatus aux;

                    aux = await RepositorySet.User.ActiveUserAccount(model);

                    if (!aux.Status)
                    {
                        errmsg = GW.LocalizationText.Get("Execution-Error", Context.LocalizationLanguage).Text + aux.Error.Message;
                    }

                }

            }
            else
            {
                errmsg 
                    = GW.LocalizationText.Get("Execution-Error", Context.LocalizationLanguage).Text + Context.ExecutionStatus.Error.Message;
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
                errmsg = GW.LocalizationText.Get("User-No-Image", Context.LocalizationLanguage).Text;
            }
            else
            {
                UserResult usermatch = null;
                usermatch = await RepositorySet.User.Read(new UserParam() { pUserID=model.UserID });

                if (Context.ExecutionStatus.Status)
                {

                    if (usermatch != null)
                    {
                        if (!usermatch.IsActive)
                        {
                            errmsg = GW.LocalizationText.Get("Login-Inactive-Account", Context.LocalizationLanguage).Text;
                        }

                    }
                    else
                    {
                        errmsg = GW.LocalizationText.Get("Login-User-NotFound", Context.LocalizationLanguage).Text;
                    }

                    if (errmsg == "")
                    {
                        OperationStatus aux;
                        aux = await RepositorySet.User.ChangeUserProfileImage(model);

                        if (!aux.Status)
                        {
                            errmsg = GW.LocalizationText.Get("Execution-Error", Context.LocalizationLanguage).Text + aux.Error.Message;
                        }

                    }

                }
                else
                {
                    errmsg = GW.LocalizationText.Get("Execution-Error", Context.LocalizationLanguage).Text + Context.ExecutionStatus.Error.Message;
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
                        = new Exception(GW.LocalizationText.Get("User-Role-Exists", Context.LocalizationLanguage).Text);
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
                        = new Exception(GW.LocalizationText.Get("User-Role-No-Exists", Context.LocalizationLanguage).Text);
                }
            }
            else
            {
                Context.ExecutionStatus.Status = false;
                Context.ExecutionStatus.Error 
                    = new Exception(GW.LocalizationText.Get("User-Role-No-Exists", Context.LocalizationLanguage).Text);
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
                        = new Exception(GW.LocalizationText.Get("User-LocalizationText-Exists", Context.LocalizationLanguage).Text);
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
                        = new Exception(GW.LocalizationText.Get("User-LocalizationText-No-Exists", Context.LocalizationLanguage).Text);
                }
            }
            else
            {
                Context.ExecutionStatus.Status = false;
                Context.ExecutionStatus.Error 
                    = new Exception(GW.LocalizationText.Get("User-LocalizationText-No-Exists", Context.LocalizationLanguage).Text);
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


        public async Task AlterRole(Int64 userroleid, Int64 newroleid)
        {
            
            Context.ExecutionStatus.InnerExceptions = new List<InnerException>();

            if (userroleid == 0)
            {
                Context.ExecutionStatus.Status = false;
                Context.ExecutionStatus.Error
                    = new Exception(GW.LocalizationText.Get("Validation-Error", Context.LocalizationLanguage).Text);
                Context.ExecutionStatus.AddInnerException("UserRoleID",
                    GW.LocalizationText.Get("Validation-NotNull", Context.LocalizationLanguage).Text);
            }

            if (newroleid == 0)
            {
                Context.ExecutionStatus.Status = false;
                Context.ExecutionStatus.Error
                    = new Exception(GW.LocalizationText.Get("Validation-Error", Context.LocalizationLanguage).Text);
                Context.ExecutionStatus.AddInnerException("RoleID",
                    GW.LocalizationText.Get("Validation-NotNull", Context.LocalizationLanguage).Text);
            }

            if (Context.ExecutionStatus.Status)
            {
                UserRolesParam obj = new UserRolesParam();
                obj.pUserRoleID = userroleid;
                obj.pRoleID = newroleid;

                await RepositorySet.UserRoles.AlterRole(obj); 
                
            }
            
        }

        public async Task AlterInstance(Int64 userinstanceid, Int64 newinstanceid)
        {

            Context.ExecutionStatus.InnerExceptions = new List<InnerException>();

            if (userinstanceid == 0)
            {
                Context.ExecutionStatus.Status = false;
                Context.ExecutionStatus.Error
                    = new Exception(GW.LocalizationText.Get("Validation-Error", Context.LocalizationLanguage).Text);
                Context.ExecutionStatus.AddInnerException("UserInstanceID",
                    GW.LocalizationText.Get("Validation-NotNull", Context.LocalizationLanguage).Text);
            }

            if (newinstanceid == 0)
            {
                Context.ExecutionStatus.Status = false;
                Context.ExecutionStatus.Error
                    = new Exception(GW.LocalizationText.Get("Validation-Error", Context.LocalizationLanguage).Text);
                Context.ExecutionStatus.AddInnerException("InstanceID",
                    GW.LocalizationText.Get("Validation-NotNull", Context.LocalizationLanguage).Text);
            }

            if (Context.ExecutionStatus.Status)
            {
                UserInstancesParam obj = new UserInstancesParam();
                obj.pUserInstanceID= userinstanceid;
                obj.pInstanceID = newinstanceid;

                await RepositorySet.UserInstances.AlterInstance(obj);

            }

        }

    }
}
