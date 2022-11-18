using GW.Core;
using GW.Membership.Contracts.Domain;
using GW.Membership.Contracts.Data;
using GW.Common;
using GW.Helpers;
using GW.Membership.Models;

namespace GW.Membership.Domain
{
    public class MembershipManager : IMembershipManager
    {
        public MembershipManager(IContext context, IMembershipRepositorySet repositorySet)
        {
            Context = context;
            InitializeDomains(context, repositorySet); 
        }

        public void InitializeDomains(IContext context, IRepositorySet repositorySet)
        {
            DataLog = new DataLogDomain(context, (IMembershipRepositorySet)repositorySet);
            Instance = new InstanceDomain(context, (IMembershipRepositorySet)repositorySet);
            ObjectPermission = new ObjectPermissionDomain(context, (IMembershipRepositorySet)repositorySet);
            Permission = new PermissionDomain(context, (IMembershipRepositorySet)repositorySet);
            Role = new RoleDomain(context, (IMembershipRepositorySet)repositorySet);
            SessionLog = new SessionLogDomain(context, (IMembershipRepositorySet)repositorySet);
            User = new UserDomain(context, (IMembershipRepositorySet)repositorySet);

        }

        public IContext Context { get; set; }

        public IDataLogDomain DataLog { get; set; }

        public IInstanceDomain Instance { get; set; }

        public IObjectPermissionDomain ObjectPermission { get; set; }

        public IPermissionDomain Permission { get; set; }

        public IRoleDomain Role { get; set; }

        public ISessionLogDomain SessionLog { get; set; }

        public IUserDomain User { get; set; }


        // 


        public OperationStatus Login(UserLogin model)
        {
            OperationStatus ret = new OperationStatus(false);


            string errmsg = "";
            bool invalidpassword = false;
            Int32 activestatus = 1;
            int trys = 0;

            UserModel usermatch = null;
            usermatch = User.GetByEmail(model.Email);

            if (Context.ExecutionStatus.Status)
            {
                if (usermatch != null)
                {
                    if (usermatch.IsLocked == 0)
                    {
                        if (usermatch.IsActive == 1)
                        {

                            if (usermatch.Password == MD5.BuildMD5(model.Password + usermatch.Salt))
                            {

                            }
                            else
                            {
                                trys = 5 - usermatch.LoginFailCounter;
                                errmsg = GW.Localization.GetItem("Login-Invalid-Password").Text.Replace("{trys}", trys.ToString());
                                invalidpassword = true;

                                if (usermatch.PasswordRecoveryCode != null)
                                {
                                    if (usermatch.PasswordRecoveryCode.Length > 0)
                                    {
                                        if (MD5.BuildMD5(usermatch.PasswordRecoveryCode) == model.Password)
                                        {
                                            errmsg = "";
                                            invalidpassword = false;
                                        }
                                    }
                                }

                                if (usermatch.LoginFailCounter == 5)
                                {
                                    activestatus = 0;
                                    errmsg = GW.Localization.GetItem("Login-Attempts").Text;
                                }

                            }

                        }
                        else
                        {
                            errmsg = GW.Localization.GetItem("Login-Inactive-Account").Text;

                        }
                    }
                    else
                    {
                        errmsg =
                            GW.Localization.GetItem("Login-Locked-Account").Text;
                    }
                }
                else
                {
                    errmsg =
                         GW.Localization.GetItem("Login-User-NotFound").Text;
                }

                if (errmsg == "")
                {
                    usermatch.Permissions
                        = this.GetUserPermissions(usermatch.RoleID, usermatch.UserID);

                    ret.Returns = usermatch;
                }

                if (invalidpassword)
                {
                    User.UpdateLoginFailCounter(new UpdateUserLoginFailCounter()
                    { UserID = usermatch.UserID.ToString(), ActiveStatus = activestatus, Reset = false });
                }

            }
            else
            {
                errmsg = "Erro de execução: " + Context.ExecutionStatus.Error.Message;
            }

            if (errmsg != "")
            {
                ret.Status = false;
                ret.Error = new Exception(errmsg);

                Context.ExecutionStatus = ret; 
            }       

            return ret;
        }

        public List<UserPermissions> GetUserPermissions(Int64 roleid, Int64 userid)
        {
            List<UserPermissions> ret = new List<UserPermissions>();

            List<PermissionSearchResult> list
                = Permission.GetPermissionsByRoleUser(roleid, userid);

            foreach (PermissionSearchResult item in list)
            {
                ret.Add(new UserPermissions()
                {
                    PermissionID = item.PermissionID,
                    ObjectPermissionID = item.ObjectPermissionID,
                    ObjectCode = item.ObjectCode,
                    ReadStatus = item.ReadStatus,
                    SaveStatus = item.SaveStatus,
                    DeleteStatus = item.DeleteStatus,
                    TypeGrant = item.TypeGrant
                });
            }

            return ret;
        }

        public PERMISSION_STATE_ENUM CheckPermission(List<UserPermissions> permissions,
          string objectcode, PERMISSION_CHECK_ENUM type)
        {
            PERMISSION_STATE_ENUM ret = PERMISSION_STATE_ENUM.NONE;

            ret = GW.Helpers.Utilities.CheckPermission(permissions, objectcode, type);

            return ret;
        }

        public PermissionsState GetPermissionsState(List<UserPermissions> permissions,
         string objectcode, bool allownone)
        {
            PermissionsState ret = new PermissionsState(false, false, false);

            ret = GW.Helpers.Utilities.GetPermissionsState(permissions, objectcode, allownone);

            return ret;
        }

        public void RegisterLoginState(UserLogin model, UpdateUserLogin stateinfo)
        {

            User.UpdateUserLogin(stateinfo);

            SessionLogModel session = new SessionLogModel();
            session.SessionID = 0;
            session.UserID = stateinfo.UserID;
            session.Date = DateTime.Now;
            session.IP = model.ClientIP;
            session.BrowserName = model.ClienteBrowserName;
            session.DateLogout = null;

            SessionLog.Set(session, stateinfo.UserID);            

        }


        public void Logout(Int64 userid)
        {
            User.SetDateLogout(userid);
        }

        public OperationStatus CreateNewUser(NewUser data, bool gocommit, object userid)
        {
            OperationStatus ret = new OperationStatus(true);

            ret = PrimaryValidation.Execute(data, new List<string>());

            if (!ret.Status)
            {
                ret.Error = new Exception(GW.Localization.GetItem("Validation-Error").Text);
            }
            else
            {
                UserModel obj = new UserModel();

                obj = this.User.GetByEmail(data.Email);

                string pwd = MD5.BuildMD5(data.Password);
                string slt = "GWSLT";

                if (obj == null)
                {
                    obj = new UserModel();
                    obj.UserID = 0;
                    obj.UserName = data.UserName;
                    obj.ApplicationID = 0;
                    obj.RoleID = data.RoleID;
                    obj.Email = data.Email;
                    obj.Password = MD5.BuildMD5(pwd + slt);
                    obj.Salt = slt;
                    obj.CreateDate = DateTime.Now;
                    obj.IsActive = 0;
                    obj.IsLocked = 0;
                    obj.LastLoginDate = DateTime.Now;
                    obj.LastLoginIP = null;
                    obj.LoginCounter = 0;
                    obj.LoginFailCounter = 0;
                    obj.AuthCode = null;
                    obj.AuthCodeExpires = DateTime.Now;
                    obj.PasswordRecoveryCode = null;
                    obj.ProfileImage = null;
                    obj.AuthUserID = null;

                    obj.RoleList = new List<UserRolesModel>();
                    obj.RoleList
                            .Add(new UserRolesModel()
                            {
                                RecordState = RECORDSTATEENUM.ADD,
                                RoleID = data.RoleID
                            }
                            );

                    obj.InstanceList = new List<UserInstancesModel>();
                    obj.InstanceList
                            .Add(new UserInstancesModel()
                            {
                                RecordState = RECORDSTATEENUM.ADD,
                                InstanceID = data.InstanceID
                            }
                            );

                    ret = this.User.Set(obj, userid);
                   
                }
                else
                {
                    ret.Status = false;
                    ret.Error = new Exception(GW.Localization.GetItem("User-Exists").Text + data.Email);
                   
                }
            }

            Context.ExecutionStatus = ret;
            return ret;
        }


        public OperationStatus GetTemporaryPassword(ChangeUserPassword model)
        {
            OperationStatus ret = new OperationStatus(false);

            ret = User.SetPasswordRecoveryCode(model);

            return ret;
        }

        public OperationStatus GetActiveAccountCode(ActiveUserAccount model)
        {
            OperationStatus ret = new OperationStatus(false);

            ret = User.SetPasswordRecoveryCode(new ChangeUserPassword() { Email = model.Email, ToActivate = true });

            return ret;
        }

        public OperationStatus GetChangePasswordCode(ChangeUserPassword model)
        {
            OperationStatus ret = new OperationStatus(false);

            ret = User.SetPasswordRecoveryCode(model);

            return ret;
        }

        public OperationStatus ChangeUserProfileImage(ChangeUserImage model)
        {
            OperationStatus ret = new OperationStatus(false);

            ret = User.ChangeUserProfileImage(model);

            return ret;
        }


    }
}
