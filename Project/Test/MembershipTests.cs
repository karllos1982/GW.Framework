
namespace GW.Membership.Test
{
    [TestClass]
    public class T04_MembershipTests : BaseTest
    {

        private const string EMAIL_DEFAULT = "usertest@gw.com.br";
        private const string PASSWORD_DEFAULT = "654321";
        private const Int64 USERID_DEFAULT = 1001;

        [TestMethod]
        public async Task T04_01_1_Create_New_User_Success()
        {
            Resources res = new Resources();

            NewUser nuser = new NewUser();

            nuser.UserName = "UserTest";
            nuser.Password = "654321";
            nuser.Email = "usertest@gw.com.br";
            nuser.RoleID = 1;
            nuser.InstanceID = 1;
            nuser.DefaultLanguage = "en-us"; 
            UserEntry user = await res.Domain.CreateNewUser(nuser, false, 1001);

            status = res.Context.ExecutionStatus;

            res.finalize();

            res.Perform_ShouldBeTrue(status);


        }

        [TestMethod]
        public async Task T04_01_2_Create_New_User_Email_Repetido()
        {
            Resources res = new Resources();

            NewUser nuser = new NewUser();

            nuser.UserName = "UserTest";
            nuser.Password = "654321";
            nuser.Email = "usertest@gw.com.br";
            nuser.RoleID = 1;
            nuser.InstanceID = 1;
            nuser.DefaultLanguage = "en-us";
            UserEntry user = await res.Domain.CreateNewUser(nuser, false, 1001);

            status = res.Context.ExecutionStatus;

            res.finalize();

            res.Perform_ShouldBeFalse(status);

        }

        [TestMethod]
        public async Task T04_01_3_Create_New_User_Role_Instance_Null()
        {
            Resources res = new Resources();

            NewUser nuser = new NewUser();

            nuser.UserName = "UserTest2";
            nuser.Password = "654321";
            nuser.Email = "usertest2@gw.com.br";
            nuser.RoleID = 0;
            nuser.InstanceID = 0;
            nuser.DefaultLanguage = "en-us";
            UserEntry user = await res.Domain.CreateNewUser(nuser, false, 1001);

            status = res.Context.ExecutionStatus;

            res.finalize();

            res.Perform_ShouldBeFalse(status);

        }

        [TestMethod]
        public async Task T04_02_Active_User_Account()
        {
            Resources res = new Resources();
          
            // get code
            ChangeUserPassword changeEntry = new ChangeUserPassword();
            changeEntry.Email =  EMAIL_DEFAULT;
            changeEntry.ToActivate = true;
            status = await res.Domain.User.SetPasswordRecoveryCode(changeEntry);
            
        
            // change password
            if (status.Status)
            {               
                ActiveUserAccount activeEntry = new ActiveUserAccount();
                activeEntry.Email = EMAIL_DEFAULT;
                activeEntry.Code = status.Returns.ToString();

                status = await res.Domain.User.ActiveUserAccount(activeEntry);
                
            }

            res.finalize();
            res.Perform_ShouldBeTrue(status);

        }

        [TestMethod]
        public async Task T04_03_1_Login_Success()
        {
            Resources res = new Resources();

            UserLogin Entry = new UserLogin();

            Entry.Email = EMAIL_DEFAULT;
            Entry.Password = GW.Helpers.MD5.BuildMD5(PASSWORD_DEFAULT);
            Entry.ClientIP = "127.0.0.1";
            Entry.ClienteBrowserName = "Test";
            Entry.AuthToken = Guid.NewGuid().ToString();
            Entry.AuthTokenExpires = DateTime.Now.AddHours(8);

            UserResult user = await res.Domain.Login(Entry);

            status = res.Context.ExecutionStatus;

            res.finalize();

            res.Perform_ShouldBeTrue(status);

        }

        [TestMethod]
        public async Task T04_04_2_Login_InvalidPassword()
        {
            Resources res = new Resources();

            UserLogin Entry = new UserLogin();

            Entry.Email = EMAIL_DEFAULT;
            Entry.Password = GW.Helpers.MD5.BuildMD5("pwdinvalid");
            Entry.ClientIP = "127.0.0.1";
            Entry.ClienteBrowserName = "Test";
            Entry.AuthToken = Guid.NewGuid().ToString();
            Entry.AuthTokenExpires = DateTime.Now.AddHours(8);

            UserResult user = await res.Domain.Login(Entry);

            status = res.Context.ExecutionStatus;

            res.finalize();

            res.Perform_ShouldBeFalse(status );

        }

        [TestMethod]
        public async Task T04_04_2_Login_UserNotExists()
        {
            Resources res = new Resources();

            UserLogin Entry = new UserLogin();

            Entry.Email = "userinvalid@sys.com";
            Entry.Password = GW.Helpers.MD5.BuildMD5("pwdinvalid");
            Entry.ClientIP = "127.0.0.1";
            Entry.ClienteBrowserName = "Test";
            Entry.AuthToken = Guid.NewGuid().ToString();
            Entry.AuthTokenExpires = DateTime.Now.AddHours(8);

            UserResult user = await res.Domain.Login(Entry);

            status = res.Context.ExecutionStatus;

            res.finalize();

            res.Perform_ShouldBeFalse(status);

        }

        [TestMethod]
        public async Task T04_05_1_CheckPermissions_When_Allowed()
        {
            Resources res = new Resources();
            
            List<UserPermissions> userPermissions = new List<UserPermissions>();

            userPermissions = await res.Domain.GetUserPermissions(2, 1003);

            userPermissions.ShouldNotBeNull<List<UserPermissions>>();

            PERMISSION_STATE_ENUM state 
                = await res.Domain.CheckPermission(userPermissions, "SYSTEST", PERMISSION_CHECK_ENUM.READ);

            res.finalize();

            state.ShouldBeEquivalentTo(PERMISSION_STATE_ENUM.ALLOWED);
                       
        }

        [TestMethod]
        public async Task T04_05_2_CheckPermissions_When_Allowed()
        {
            Resources res = new Resources();

            List<UserPermissions> userPermissions = new List<UserPermissions>();

            userPermissions = await res.Domain.GetUserPermissions(2, 1003);

            userPermissions.ShouldNotBeNull<List<UserPermissions>>();

            PermissionsState state
                = await res.Domain.GetPermissionsState(userPermissions, "SYSTEST",true);

            res.finalize();

            state.AllowRead.ShouldBeEquivalentTo(true);
            state.AllowSave.ShouldBeEquivalentTo(true);
            state.AllowDelete.ShouldBeEquivalentTo(true);

            

        }

        [TestMethod]
        public async Task T04_05_3_GetListPermissions()
        {
            Resources res = new Resources();

            List<UserPermissions> userPermissions = new List<UserPermissions>();
            userPermissions = await res.Domain.GetUserPermissions(2, 1003);

            userPermissions = (from UserPermissions p in userPermissions
                               where p.ObjectCode == "SYSTEST"
                    select p).ToList();
            
            res.finalize();

            userPermissions.ShouldNotBeNull<List<UserPermissions>>();

        }

        [TestMethod]
        public async Task T04_06_CheckPermissions_When_Denied()
        {
            Resources res = new Resources();

            List<UserPermissions> userPermissions = new List<UserPermissions>();

            userPermissions = await res.Domain.GetUserPermissions(1, 1001);

            userPermissions.ShouldNotBeNull<List<UserPermissions>>();

            PERMISSION_STATE_ENUM state
                = await res.Domain.CheckPermission(userPermissions, "SYSTEST", PERMISSION_CHECK_ENUM.SAVE);
           
            res.finalize();

            state.ShouldBeEquivalentTo(PERMISSION_STATE_ENUM.DENIED);
        }


        [TestMethod]
        public async Task T04_07_Register_Login_State()
        {
            Resources res = new Resources();

            UserLogin Entry = new UserLogin();         
            Entry.ClientIP = "127.0.0.1";
            Entry.ClienteBrowserName = "Test";
            
            UpdateUserLogin login = new UpdateUserLogin();
            login.UserID = 1003;
            login.LastLoginDate = DateTime.UtcNow;
            login.AuthToken = Guid.NewGuid().ToString();
            login.AuthTokenExpires = DateTime.Now.AddHours(10); 
            
            await res.Domain.RegisterLoginState(Entry, login);

            status = res.Context.ExecutionStatus;

            res.finalize();

            res.Perform_ShouldBeTrue(status);                        

        }


        [TestMethod]
        public async Task T04_08_Logout()
        {
            Resources res = new Resources();

            status = await res.Domain.User.SetDateLogout(USERID_DEFAULT);
        
            res.finalize();

            res.Perform_ShouldBeTrue(status);

        }

        [TestMethod]
        public async Task T04_09_Get_Temporary_Password()
        {
            Resources res = new Resources();

            ChangeUserPassword Entry = new ChangeUserPassword();
            Entry.Email = EMAIL_DEFAULT;

            status = await res.Domain.GetTemporaryPassword(Entry);

            res.finalize();

            res.Perform_ShouldBeTrue(status);

        }

        [TestMethod]
        public async Task T04_10_Change_User_State()
        {
            Resources res = new Resources();

            UserChangeState Entry = new UserChangeState();
            Entry.UserID = USERID_DEFAULT;
            Entry.LockedValue = false;
            Entry.ActiveValue = true ;

            status = await res.Domain.User.ChangeState(Entry);

            res.finalize();

            res.Perform_ShouldBeTrue(status);

          
        }
 

        [TestMethod]
        public async Task T04_11_Change_User_Password()
        {
            Resources res = new Resources();
          
            // get code
            ChangeUserPassword changeEntry = new ChangeUserPassword();
            changeEntry.Email = EMAIL_DEFAULT;
            changeEntry.NewPassword = "NEWPWD"; 

            status = await res.Domain.User.SetPasswordRecoveryCode(changeEntry);
                         
            // change password
            if (status.Status)
            {            
                changeEntry.Code = status.Returns.ToString();

                status = await res.Domain.User.ChangeUserPassword(changeEntry);
                res.finalize();
                res.Perform_ShouldBeTrue(status);

            }
            else
            {

                res.finalize( );
                res.Perform_ShouldBeFalse(status);
            }
            

        }


        [TestMethod]
        public async Task T04_12_Change_User_Profile_Image()
        {
            Resources res = new Resources();

            ChangeUserImage Entry = new ChangeUserImage();
            Entry.UserID = USERID_DEFAULT;
            Entry.FileName = "image_user.png"; 

            status = await res.Domain.ChangeUserProfileImage(Entry);
            
            res.finalize();

            res.Perform_ShouldBeTrue(status);

        }
   

    }
}