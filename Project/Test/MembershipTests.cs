
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
            UserModel user = await res.Domain.CreateNewUser(nuser, false, 1001);
            
            res.finalize();

            res.Perform_ShouldBeTrue();


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
            UserModel user = await res.Domain.CreateNewUser(nuser, false, 1001);

            res.finalize();

            res.Perform_ShouldBeFalse();

        }

        [TestMethod]
        public async Task T04_02_Active_User_Account()
        {
            Resources res = new Resources();
          
            // get code
            ChangeUserPassword changemodel = new ChangeUserPassword();
            changemodel.Email =  EMAIL_DEFAULT;
            changemodel.ToActivate = true;
            status = await res.Domain.User.SetPasswordRecoveryCode(changemodel);
            
        
            // change password
            if (status.Status)
            {               
                ActiveUserAccount activemodel = new ActiveUserAccount();
                activemodel.Email = EMAIL_DEFAULT;
                activemodel.Code = status.Returns.ToString();

                status = await res.Domain.User.ActiveUserAccount(activemodel);
                
            }

            res.finalize();
            res.Perform_ShouldBeTrue();

        }

        [TestMethod]
        public async Task T04_03_1_Login_Success()
        {
            Resources res = new Resources();

            UserLogin model = new UserLogin();

            model.Email = EMAIL_DEFAULT;
            model.Password = GW.Helpers.MD5.BuildMD5(PASSWORD_DEFAULT);
            model.ClientIP = "127.0.0.1";
            model.ClienteBrowserName = "Test";
            model.AuthToken = Guid.NewGuid().ToString();
            model.AuthTokenExpires = DateTime.Now.AddHours(8);

            UserModel user = await res.Domain.Login(model);
                       
            res.finalize();

            res.Perform_ShouldBeTrue();

        }

        [TestMethod]
        public async Task T04_04_2_Login_InvalidPassword()
        {
            Resources res = new Resources();

            UserLogin model = new UserLogin();

            model.Email = EMAIL_DEFAULT;
            model.Password = GW.Helpers.MD5.BuildMD5("pwdinvalid");
            model.ClientIP = "127.0.0.1";
            model.ClienteBrowserName = "Test";
            model.AuthToken = Guid.NewGuid().ToString();
            model.AuthTokenExpires = DateTime.Now.AddHours(8);

            UserModel user = await res.Domain.Login(model);
           
            res.finalize();

            res.Perform_ShouldBeFalse();

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

            state.ShouldBeEquivalentTo(PERMISSION_STATE_ENUM.ALLOWED);

            res.finalize();
           
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

            state.AllowRead.ShouldBeEquivalentTo(true);
            state.AllowSave.ShouldBeEquivalentTo(true);
            state.AllowDelete.ShouldBeEquivalentTo(true);

            res.finalize();

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
                = await res.Domain.CheckPermission(userPermissions, "SYSUSER", PERMISSION_CHECK_ENUM.SAVE);
           
            res.finalize();

            state.ShouldBeEquivalentTo(PERMISSION_STATE_ENUM.DENIED);
        }


        [TestMethod]
        public async Task T04_07_Register_Login_State()
        {
            Resources res = new Resources();

            UserLogin model = new UserLogin();         
            model.ClientIP = "127.0.0.1";
            model.ClienteBrowserName = "Test";
         
            UpdateUserLogin login = new UpdateUserLogin();
            login.UserID = 1003;

            await res.Domain.RegisterLoginState(model, login);

            res.finalize();

            res.Perform_ShouldBeTrue();                        

        }


        [TestMethod]
        public async Task T04_08_Logout()
        {
            Resources res = new Resources();

            status = await res.Domain.User.SetDateLogout(USERID_DEFAULT);
        
            res.finalize();

            res.Perform_ShouldBeTrue();

        }

        [TestMethod]
        public async Task T04_09_Get_Temporary_Password()
        {
            Resources res = new Resources();

            ChangeUserPassword model = new ChangeUserPassword();
            model.Email = EMAIL_DEFAULT;

            status = await res.Domain.GetTemporaryPassword(model);

            res.finalize();

            res.Perform_ShouldBeTrue();

        }

        [TestMethod]
        public async Task T04_10_Change_User_State()
        {
            Resources res = new Resources();

            UserChangeState model = new UserChangeState();
            model.UserID = USERID_DEFAULT;
            model.LockedValue = 0;
            model.ActiveValue = 1;

            status = await res.Domain.User.ChangeState(model);

            res.finalize();

            res.Perform_ShouldBeTrue();

          
        }
 

        [TestMethod]
        public async Task T04_11_Change_User_Password()
        {
            Resources res = new Resources();
          
            // get code
            ChangeUserPassword changemodel = new ChangeUserPassword();
            changemodel.Email = EMAIL_DEFAULT;
            changemodel.NewPassword = "NEWPWD"; 

            status = await res.Domain.User.SetPasswordRecoveryCode(changemodel);
                         
            // change password
            if (status.Status)
            {            
                changemodel.Code = status.Returns.ToString();

                status = await res.Domain.User.ChangeUserPassword(changemodel);
                res.finalize();
                res.Perform_ShouldBeTrue();

            }
            else
            {
                res.finalize();
                res.Perform_ShouldBeFalse();
            }
            

        }


        [TestMethod]
        public async Task T04_12_Change_User_Profile_Image()
        {
            Resources res = new Resources();

            ChangeUserImage model = new ChangeUserImage();
            model.UserID = USERID_DEFAULT;
            model.FileName = "image_user.png"; 

            status = await res.Domain.ChangeUserProfileImage(model);
            
            res.finalize();

            res.Perform_ShouldBeTrue();

        }
   

    }
}