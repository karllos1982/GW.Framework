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

        private string lang = "";

        public MembershipManager(IContext context, IMembershipRepositorySet repositorySet)
        {
            Context = context;
            InitializeDomains(context, repositorySet);

            lang = Context.Settings.LocalizationLanguage;
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


        public async Task<UserResult> Login(UserLogin model)
        {
            UserResult ret = null;

            string errmsg = "";
            bool invalidpassword = false;
            Int32 activestatus = 1;
            int trys = 0;

            UserResult usermatch = null;
            usermatch = await User.GetByEmail(model.Email);

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
                                
                                errmsg = string.Format(GW.Localization.GetItem("Login-Invalid-Password", lang).Text, trys.ToString());

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
                                    errmsg = GW.Localization.GetItem("Login-Attempts",lang).Text;
                                }

                            }

                        }
                        else
                        {
                            errmsg = GW.Localization.GetItem("Login-Inactive-Account",lang).Text;

                        }
                    }
                    else
                    {
                        errmsg =
                            GW.Localization.GetItem("Login-Locked-Account", lang).Text;
                    }
                }
                else
                {
                    errmsg =
                         GW.Localization.GetItem("Login-User-NotFound", lang).Text;
                }

                if (errmsg == "")
                {
                    usermatch.Permissions
                        = await this.GetUserPermissions(usermatch.Roles[0].RoleID, usermatch.UserID);

                    ret = usermatch;
                }

                if (invalidpassword)
                {
                   await User.UpdateLoginFailCounter(new UpdateUserLoginFailCounter()
                    { UserID = usermatch.UserID.ToString(), ActiveStatus = activestatus, Reset = false });
                }

            }
            
            if (errmsg != "")
            {
                Context.ExecutionStatus.Status = false;
                Context.ExecutionStatus.Error = new Exception(errmsg);             
            }       

            return ret;
        }

        public async Task<List<UserPermissions>> GetUserPermissions(Int64 roleid, Int64 userid)
        {
            List<UserPermissions> ret = new List<UserPermissions>();

            List<PermissionResult> list
                = await Permission.GetPermissionsByRoleUser(roleid, userid);

            foreach (PermissionResult item in list)
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

        public async Task<PERMISSION_STATE_ENUM> CheckPermission(List<UserPermissions> permissions,
          string objectcode, PERMISSION_CHECK_ENUM type)
        {
            PERMISSION_STATE_ENUM ret = PERMISSION_STATE_ENUM.NONE;

            ret = GW.Helpers.Utilities.CheckPermission(permissions, objectcode, type);

            return ret;
        }

        public async Task<PermissionsState> GetPermissionsState(List<UserPermissions> permissions,
         string objectcode, bool allownone)
        {
            PermissionsState ret = new PermissionsState(false, false, false);

            ret = GW.Helpers.Utilities.GetPermissionsState(permissions, objectcode, allownone);

            return ret;
        }

        public async Task RegisterLoginState(UserLogin model, UpdateUserLogin stateinfo)
        {

            await User.UpdateUserLogin(stateinfo);

            SessionLogEntry session = new SessionLogEntry();
            session.SessionID = 0;
            session.UserID = stateinfo.UserID;
            session.Date = DateTime.Now;
            session.IP = model.ClientIP;
            session.BrowserName = model.ClienteBrowserName;
            session.DateLogout = null;

            await SessionLog.Set(session, stateinfo.UserID);            

        }


        public async Task Logout(Int64 userid)
        {
            await User.SetDateLogout(userid);
        }

        public async Task<UserEntry> CreateNewUser(NewUser data, bool gocommit, object userid)
        {
            UserEntry ret = null;

            Context.ExecutionStatus =  PrimaryValidation.Execute(data, new List<string>());

            if (Context.ExecutionStatus.Status)
            {
                UserResult old = new UserResult();
                UserEntry obj ;

                old = await this.User.GetByEmail(data.Email);

                string pwd = MD5.BuildMD5(data.Password);
                string slt = "GWSLT";

                if (old == null)
                {
                    obj = new UserEntry();
                    obj.UserID = 0;
                    obj.UserName = data.UserName;
                    obj.ApplicationID = 0;                    
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
                    
                    ret = await this.User.Set(obj, userid);

                    if (Context.ExecutionStatus.Status)
                    {
                        var aux 
                            = await this.User.AddRoleToUser(ret.UserID, data.RoleID); 
                    }
                   
                }
                else
                {
                    Context.ExecutionStatus.Status = false;
                    Context.ExecutionStatus.Error 
                        = new Exception(GW.Localization.GetItem("User-Exists", lang).Text + data.Email);
                   
                }
            }
          
            return ret;
        }


        public async Task<OperationStatus> GetTemporaryPassword(ChangeUserPassword model)
        {
            OperationStatus ret = new OperationStatus(false);

            ret = await User.SetPasswordRecoveryCode(model);

            return ret;
        }

        public async Task<OperationStatus> GetActiveAccountCode(ActiveUserAccount model)
        {
            OperationStatus ret = new OperationStatus(false);

            ret = await User.SetPasswordRecoveryCode(new ChangeUserPassword()
                    { Email = model.Email, ToActivate = true });

            return ret;
        }

        public async Task<OperationStatus> GetChangePasswordCode(ChangeUserPassword model)
        {
            OperationStatus ret = new OperationStatus(false);

            ret = await User.SetPasswordRecoveryCode(model);

            return ret;
        }

        public async Task<OperationStatus> ChangeUserProfileImage(ChangeUserImage model)
        {
            OperationStatus ret = new OperationStatus(false);

            ret = await User.ChangeUserProfileImage(model);

            return ret;
        }


    }
}
