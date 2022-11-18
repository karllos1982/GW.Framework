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

        public UserModel Get(UserParam param)
        {
            UserModel ret = null;

            ret = RepositorySet.User.Read(param); 
            
            return ret;
        }

        public List<UserList> List(UserParam param)
        {
            List<UserList> ret = null;

            ret = RepositorySet.User.List(param);           

            return ret;
        }

        public List<UserSearchResult> Search(UserParam param)
        {
            List<UserSearchResult> ret = null;

            ret = RepositorySet.User.Search(param);

            return ret;
        }

        public OperationStatus Set(UserModel model, object userid)
        {
            OperationStatus ret = new OperationStatus(true);
            OPERATIONLOGENUM operation = OPERATIONLOGENUM.INSERT;

            ret = EntryValidation(model);

            if (ret.Status)
            {

                UserModel old 
                    = RepositorySet.User.Read(new UserParam() { pUserID = model.UserID });

                if (old == null)
                {
                    ret = InsertValidation(model);

                    if (ret.Status)
                    {
                        model.CreateDate = DateTime.Now;
                        ret = RepositorySet.User.Create(model);
                    }
                }
                else
                {
                    model.CreateDate = old.CreateDate;
                    operation = OPERATIONLOGENUM.UPDATE;

                    ret = UpdateValidation(model);

                    if (ret.Status)
                    {
                        ret = RepositorySet.User.Update(model);
                    }

                }

                if (ret.Status && userid != null)
                {
                    RepositorySet.User.Context
                        .RegisterDataLog(userid.ToString(), operation, "SYSUSER",
                        model.UserID.ToString(), old, model);

                    ret.Returns = model;
                }

            }     

            return ret;
        }

        public void FillChields(ref UserModel obj)
        {
            UserRolesParam param = new UserRolesParam();
            param.pUserID = obj.UserID;

            List<UserRolesModel> roles
                = RepositorySet.UserRoles.List(param);

            if (roles != null)
            {
                if (roles.Count > 0)
                {
                    obj.RoleList = roles;
                    obj.RoleID = roles[0].RoleID;

                    RoleModel r = RepositorySet.Role
                        .Read(new RoleParam() { pRoleID = obj.RoleID });
                    obj.Role = r;
                }
            }

            //

            UserInstancesParam param2 = new UserInstancesParam();
            param2.pUserID = obj.UserID;

            List<UserInstancesModel> instances
                = RepositorySet.UserInstances.List(param2);

            if (instances != null)
            {
                if (instances.Count > 0)
                {
                    obj.InstanceList = instances;
                    obj.InstanceID = instances[0].InstanceID;

                    InstanceModel i = RepositorySet.Instance
                        .Read(new InstanceParam() { pInstanceID = obj.InstanceID });
                    obj.Instance = i;
                }
            }
        }

        public OperationStatus Delete(UserModel model, object userid)
        {
            OperationStatus ret = new OperationStatus(true);

            UserModel old = this.Get(new UserParam() { pUserID = model.UserID });

            if (old != null)
            {
                ret = DeleteValidation(model);

                if (ret.Status)
                {
                    if (ret.Status && old.RoleList != null)
                    {
                        foreach (UserRolesModel u in old.RoleList)
                        {
                            ret = RepositorySet.UserRoles.Delete(u);
                            if (!ret.Status)
                            {
                                break;
                            }
                        }
                    }

                    if (ret.Status && old.InstanceList != null)
                    {
                        foreach (UserInstancesModel u in old.InstanceList)
                        {
                            ret = RepositorySet.UserInstances.Delete(u);
                            if (!ret.Status)
                            {
                                break;
                            }
                        }
                    }

                    if (ret.Status)
                    {
                        ret = RepositorySet.User.Delete(old);
                    }
                    else
                    {
                        ret.Status = false;
                        ret.Error = new System.Exception(GW.Localization.GetItem("User-Error-Exclude-Childs").Text);
                    }
                }
            }
            else
            {
                ret.Status = false;
                ret.Error = new System.Exception(GW.Localization.GetItem("Record-NotFound").Text);

            }

            return ret;
        }


        public OperationStatus EntryValidation(UserModel obj)
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

            return ret;
        }           
             
        public OperationStatus InsertValidation(UserModel obj)
        {
            OperationStatus ret = new OperationStatus(true);
            UserParam param = new UserParam()
            {
                pEmail = obj.Email,
            };

            List<UserList> list
                = RepositorySet.User.List(param);

            if (list != null)
            {
                if (list.Count > 0)
                {
                    ret.Status = false;
                    ret.Error = new Exception(GW.Localization.GetItem("Email-Exists").Text);
                }
            }

            Context.ExecutionStatus = ret;

            return ret;
        }
            
        public OperationStatus UpdateValidation(UserModel obj)
        {
            OperationStatus ret = new OperationStatus(true);
            UserParam param = new UserParam() { pEmail = obj.Email };

            List<UserList> list
                = RepositorySet.User.List(param);

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

            return ret;

        }

        public OperationStatus DeleteValidation(UserModel obj)
        {
            return new OperationStatus(true); 
        }

        public UserModel GetByEmail(string email)
        {
            UserModel ret = null;

            ret = RepositorySet.User.GetByEmail(email);

            if (ret != null)
            {
                if (Context.ExecutionStatus.Status)
                {
                    FillChields(ref ret);
                }
            }       

            return ret;
        }

        public OperationStatus UpdateUserLogin(UpdateUserLogin model)
        {
            OperationStatus ret = new OperationStatus(true);

            UserModel obj = null;

            obj = RepositorySet.User.Read(new UserParam() { pUserID = model.UserID });

            if (obj != null)
            {
                ret = RepositorySet.User.UpdateUserLogin(model);
            }
            else
            {
                ret.Status = false;
                ret.Error = new Exception(GW.Localization.GetItem("Login-User-NotFound").Text);
            }

            return ret;

        }

        public OperationStatus SetPasswordRecoveryCode(ChangeUserPassword model)
        {
            OperationStatus ret = new OperationStatus(true);

            string errmsg = "";
            string code = "";

            UserModel usermatch = null;
            usermatch = RepositorySet.User.GetByEmail(model.Email);

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
                    aux = RepositorySet.User.SetPasswordRecoveryCode(
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

        public OperationStatus ChangeUserPassword(ChangeUserPassword model)
        {
            OperationStatus ret = new OperationStatus(true);

            string errmsg = "";

            UserModel usermatch = null;
            usermatch = RepositorySet.User.GetByEmail(model.Email);

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

                    aux = RepositorySet.User.ChangeUserPassword(change);

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

        public OperationStatus ActiveUserAccount(ActiveUserAccount model)
        {
            OperationStatus ret = new OperationStatus(true);

            string errmsg = "";
            UserModel usermatch = null;
            usermatch = RepositorySet.User.GetByEmail(model.Email);

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

                    aux = RepositorySet.User.ActiveUserAccount(model);

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

        public OperationStatus ChangeUserProfileImage(ChangeUserImage model)
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
                usermatch = RepositorySet.User.Read(new UserParam() { pUserID=model.UserID });

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
                        aux = RepositorySet.User.ChangeUserProfileImage(model);

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

        public OperationStatus UpdateLoginFailCounter(UpdateUserLoginFailCounter model)
        {
            OperationStatus ret = new OperationStatus(true);

            ret = RepositorySet.User.UpdateLoginFailCounter(model);

            return ret;

        }

        public OperationStatus ChangeState(UserChangeState model)
        {
            OperationStatus ret = new OperationStatus(true);

            ret = RepositorySet.User.ChangeState(model);    

            return ret;

        }

        public OperationStatus SetDateLogout(Int64 userid)
        {
            OperationStatus ret = new OperationStatus(true);

            SessionLogParam param = new SessionLogParam();
          
            param.pUserID = userid;
            ret = RepositorySet.SessionLog.SetDateLogout(param);

  
            return ret;

        }

        public OperationStatus AddRoleToUser(Int64 userid, Int64 roleid, bool gocommit)
        {
            OperationStatus ret = new OperationStatus(true);

            List<UserRolesModel> list;
            UserRolesParam param = new UserRolesParam();
            param.pUserID = userid;
            param.pRoleID = roleid;

            list = RepositorySet.UserRoles.Search(param);

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

                ret = RepositorySet.UserRoles.Create(obj);

                if (ret.Status)
                {
                    ret.Returns = obj;
                }

            }

            return ret;
        }

        public OperationStatus RemoveRoleFromUser(Int64 userid, Int64 roleid, bool gocommit)
        {
            OperationStatus ret = new OperationStatus(true);

            List<UserRolesModel> list;
            UserRolesParam param = new UserRolesParam();
            param.pUserID = userid;
            param.pRoleID = roleid;

            list = RepositorySet.UserRoles.Search(param);

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

                ret = RepositorySet.UserRoles.Delete(obj);

                if (ret.Status)
                {
                    ret.Returns = obj;
                }
            }        

            return ret;
        }


        public OperationStatus AddInstanceToUser(Int64 userid, Int64 instanceid, bool gocommit)
        {
            OperationStatus ret = new OperationStatus(true);

            List<UserInstancesModel> list;
            UserInstancesParam param = new UserInstancesParam();
            param.pUserID = userid;
            param.pInstanceID = instanceid;

            list = RepositorySet.UserInstances.Search(param);

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

                ret = RepositorySet.UserInstances.Create(obj);

                if (ret.Status)
                {
                    ret.Returns = obj;
                }

            }

            return ret;
        }


        public OperationStatus RemoveInstanceFromUser(Int64 userid, Int64 instanceid, bool gocommit)
        {
            OperationStatus ret = new OperationStatus(true);

            List<UserInstancesModel> list;
            UserInstancesParam param = new UserInstancesParam();
            param.pUserID = userid;
            param.pInstanceID = instanceid;

            list = RepositorySet.UserInstances.Search(param);

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

                ret = RepositorySet.UserInstances.Delete(obj);

                if (ret.Status)
                {
                    ret.Returns = obj;
                }
            }         

            return ret;
        }

    }
}
