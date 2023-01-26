using System;
using System.Collections.Generic;
using GW.Helpers;
using GW.Common;

namespace GW.Membership.Models
{

    public class UserLogin
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string TargetRole { get; set; }

        public bool KeepConnection { get; set; }

        public string SessionTimeOut { get; set; }

        public string ClientIP { get; set; }

        public string ClienteBrowserName { get; set; }

        public string AuthToken { get; set; }

        public DateTime AuthTokenExpires { get; set; }

    }

    public class UserAuthenticated
    {
        public string UserID { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string RoleName { get; set; }

        public string InstanceName { get; set; }

        public string HomeURL { get; set; }

        public string ProfileImageURL { get; set; }

        public string Token { get; set; }

        public DateTime Expires { get; set; }

        public string Status { get; set; }

        public List<UserPermissions> Permissions { get; set; }

        public string LocalizationLanguage { get; set; }

    }
   

    public class UserParam
    {
        public UserParam()
        {
            pUserID = 0;
            pRoleID = 0;
            pInstanceID = 0;
            pUserName = "";
            pEmail = ""; 
        }

        public Int64 pUserID { get; set; }

        public string pEmail { get; set; }

        public string pUserName { get; set; }

        public Int64 pRoleID { get; set; }

        public Int64 pInstanceID { get; set; }

    }

    public class NewUser
    {

        [PrimaryValidationConfig("UserName", "User Name", FieldType.USERNAME, false, 50)]
        public string UserName { get; set; }

        [PrimaryValidationConfig("Email", "E-mail", FieldType.EMAIL, false, 100)]
        public string Email { get; set; }

        [PrimaryValidationConfig("RoleID", "Role ID", FieldType.NUMERIC, false, 0)]
        public Int64 RoleID { get; set; }

        [PrimaryValidationConfig("InstanceID", "LocalizationText ID", FieldType.NUMERIC, false, 0)]
        public Int64 InstanceID { get; set; }

        [PrimaryValidationConfig("DefaultLanguage", "Default Language", FieldType.TEXT, false, 5)]
        public string DefaultLanguage { get; set; }

        [PrimaryValidationConfig("Password", "Password", FieldType.TEXT, false, 8)]
        public string Password { get; set; }

    }

    public class EmailConfirmation
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Code { get; set; }
    }

    public class UserEntry
    {
        [PrimaryValidationConfig("UserID", "User ID", FieldType.NUMERIC, false, 0)]
        public Int64 UserID { get; set; }

        public Int64 ApplicationID { get; set; }

        [PrimaryValidationConfig("UserName", "User Name", FieldType.USERNAME, false, 50)]
        public string UserName { get; set; }

        [PrimaryValidationConfig("Email", "E-mail", FieldType.EMAIL, false, 100)]
        public string Email { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }                    

        public DateTime CreateDate { get; set; }

        public int IsActive { get; set; }

        public int IsLocked { get; set; }

        public string DefaultLanguage { get; set; }

        public DateTime LastLoginDate { get; set; }

        public string LastLoginIP { get; set; }

        public Int32 LoginCounter { get; set; }

        public Int32 LoginFailCounter { get; set; }

        public string Avatar { get; set; }

        public string AuthCode { get; set; }

        public DateTime AuthCodeExpires { get; set; }

        public string PasswordRecoveryCode { get; set; }

        public string ProfileImage { get; set; }

        public string AuthUserID { get; set; }

    }

    public class UserList
    {
        public Int64 UserID { get; set; }

        public string UserName { get; set; }       

        public string Email { get; set; }

    }

    public class UserResult
    {
        
        public Int64 UserID { get; set; }

        public Int64 ApplicationID { get; set; }
        
        public string UserName { get; set; }
        
        public string Email { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

        public DateTime CreateDate { get; set; }

        public int IsActive { get; set; }

        public int IsLocked { get; set; }

        public string DefaultLanguage { get; set; }

        public DateTime LastLoginDate { get; set; }

        public string LastLoginIP { get; set; }

        public Int32 LoginCounter { get; set; }

        public Int32 LoginFailCounter { get; set; }

        public string Avatar { get; set; }

        public string AuthCode { get; set; }

        public DateTime AuthCodeExpires { get; set; }

        public string PasswordRecoveryCode { get; set; }

        public string ProfileImage { get; set; }

        public string AuthUserID { get; set; }

        public string ProfileImageURL { get; set; }

        //
      
        public List<UserRolesResult> Roles { get; set; }

        public List<UserInstancesResult> Instances { get; set; }

        //

        public List<UserPermissions> Permissions { get; set; }

    }


    public class UpdateUserLogin
    {
        public Int64 UserID { get; set; }

        public DateTime LastLoginDate { get; set; }

        public string AuthToken { get; set; }

        public DateTime AuthTokenExpires { get; set; }

    }
   

    public class ChangeUserPassword
    {
        public Int64 UserID { get; set; }

        public string Email { get; set; }

        public string NewPassword { get; set; }

        public string Code { get; set; }

        public bool ToActivate { get; set; }
    }

    public class SetPasswordRecoveryCode
    {
        public Int64 UserID { get; set; }

        public string Code { get; set; }
    }

    public class ActiveUserAccount
    {
        public string Email { get; set; }

        public string Code { get; set; }
    }

    public class ChangeUserImage
    {
        public Int64 UserID { get; set; }

        public string FileName { get; set; }
    }

    public class UpdateUserLoginFailCounter
    {
        public string UserID { get; set; }

        public bool Reset { get; set; }

        public Int32 ActiveStatus { get; set; }

    }

    public class UserChangeState
    {
        public Int64 UserID { get; set; }

        public Int32 ActiveValue { get; set; }

        public Int32 LockedValue { get; set; }
    }

    public class RegisterSession
    {
        public Int64 SessionID { get; set; }

        public Int64 UserID { get; set; }

        public DateTime Date { get; set; }

        public string IP { get; set; }

        public string BrowserName { get; set; }
    }


   


}
