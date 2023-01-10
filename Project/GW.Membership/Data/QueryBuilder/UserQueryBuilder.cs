using GW.Helpers;
using System.Collections.Generic;
using System.Collections.Generic;

namespace GW.Membership.Data
{
    public class UserQueryBuilder : QueryBuilder
    {
        public UserQueryBuilder()
        {
            Initialize(); 
        }

        public override void Initialize()
        {
            Keys = new List<string>();
            ExcludeFields = new List<string>(); 

            Keys.Add("UserID");
            ExcludeFields.Add("Avatar");           
                  

        }

        public string QueryForGetByEmail()
        {
            string ret = @"Select 
                    UserID,UserName,ApplicationID,Email,Password,Salt,CreateDate,IsActive,IsLocked,DefaultLanguage,LastLoginDate,
                    LastLoginIP,LoginCounter,LoginFailCounter,AuthCode,AuthCodeExpires,PasswordRecoveryCode,ProfileImage,AuthUserID 
                    from sysUser where Email=@pEmail";

            return ret;

        }

        public string QueryForUpdateUserLogin()
        {
            string ret;

            ret = @"update sysUser set LastLoginDate=@LastLoginDate,
                  LoginCounter = LoginCounter + 1, PasswordRecoveryCode=null, 
                  AuthCode=@AuthToken, AuthCodeExpires=@AuthTokenExpires
                  where UserID=@UserID";

            return ret;
        }

        public string QueryForSetPasswordRecoveryCode()
        {
            string ret;

            ret = @"update sysUser set PasswordRecoveryCode=@Code               
                  where UserID=@UserID";

            return ret;
        }

        public string QueryForChangeUserPassword()
        {
            string ret;

            ret = @"update sysUser set Password=@NewPassword
                  where UserID=@UserID";


            return ret;
        }

        public string QueryForActiveAccount()
        {
            string ret;

            ret = @"update sysUser set IsActive=1, PasswordRecoveryCode=null , LoginFailCounter = 0 
                  where Email=@Email";


            return ret;
        }

        public string QueryForChangeUserProfileImage()
        {
            string ret;

            ret = @"update sysUser set ProfileImage=@FileName 
                  where UserID=@UserID";


            return ret;
        }

        public string QueryForSetLoginFailCounter(bool isreset)
        {
            string ret;

            if (!isreset)
            {
                ret = @"update sysUser set LoginFailCounter = LoginFailCounter + 1, IsActive=@ActiveStatus
                  where UserID=@UserID";
            }
            else
            {
                ret = @"update sysUser set LoginFailCounter = 0, IsActive=1
                  where UserID=@UserID";
            }


            return ret;
        }

        public string QueryForChangeUserState()
        {
            string ret = "";

            ret = @"update sysUser set IsActive=@ActiveValue,IsLocked=@LockedValue  
                 where UserID=@UserID";


            return ret;
        }


        public override string QueryForGet(object param)
        {
            string ret = @"Select 
                    UserID,ApplicationID,UserName,Email,Password,Salt,CreateDate,IsActive,IsLocked,DefaultLanguage,LastLoginDate,
                    LastLoginIP,LoginCounter,LoginFailCounter,AuthCode,AuthCodeExpires,PasswordRecoveryCode,ProfileImage,AuthUserID                     
                    from sysUser where userid=@pUserID";

            return ret;
            
        }

        public override string QueryForList(object param)
        {
            string ret = @"select 
                UserID,UserName,Email from sysUser u
                where 1=1 
                and (@pUserID=0 or u.UserID=@pUserID)
                and (@pEmail='' or u.Email=@pEmail)
                and (@pUserName='' or u.UserName=@pUserName)
                and (@pRoleID=0 or (u.UserID in (select r.UserID from sysUserRoles r where r.RoleID=@pRoleID)))
                and (@pInstanceID=0 or (u.UserID in (select r.UserID from sysUserInstances r where r.InstanceID=@pInstanceID)))"
                ;

            return ret;
        }

        public override string QueryForSearch(object param)
        {
            string ret = @"Select 
                UserID,ApplicationID,UserName,Email,Password,Salt,CreateDate,IsActive,IsLocked,DefaultLanguage,LastLoginDate,
                LastLoginIP,LoginCounter,LoginFailCounter,AuthCode,AuthCodeExpires,PasswordRecoveryCode,ProfileImage,AuthUserID 
                from sysUser u
                where 1=1 
                and (@pUserID=0 or u.UserID=@pUserID)
                and (@pEmail='' or u.Email=@pEmail)
                and (@pUserName='' or u.UserName=@pUserName)
                and (@pRoleID=0 or (u.UserID in (select r.UserID from sysUserRoles r where r.RoleID=@pRoleID)))
                and (@pInstanceID=0 or (u.UserID in (select r.UserID from sysUserInstances r where r.InstanceID=@pInstanceID)))
                ";

            return ret;

        }


    }
}
