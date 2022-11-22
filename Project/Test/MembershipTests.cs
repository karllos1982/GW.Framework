
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
            this.init();

            NewUser nuser = new NewUser();

            nuser.UserName = "UserTest";
            nuser.Password = "654321";
            nuser.Email = "usertest@gw.com.br";
            nuser.RoleID = 1;
            nuser.InstanceID = 1;
            UserModel user = await this.Domain.CreateNewUser(nuser, false, 1001);

            this.Perform_ShouldBeTrue();

            this.finalize();
                        

        }

        [TestMethod]
        public async Task T04_01_2_Create_New_User_Email_Repetido()
        {
            this.init();

            NewUser nuser = new NewUser();

            nuser.UserName = "UserTest";
            nuser.Password = "654321";
            nuser.Email = "usertest@gw.com.br";
            nuser.RoleID = 1;
            nuser.InstanceID = 1;
            UserModel user = await this.Domain.CreateNewUser(nuser, false, 1001);

            this.finalize();

            this.Perform_ShouldBeFalse();

        }

        [TestMethod]
        public async Task T04_02_Active_User_Account()
        {
            this.init();
          
            // get code
            ChangeUserPassword changemodel = new ChangeUserPassword();
            changemodel.Email =  EMAIL_DEFAULT;
            changemodel.ToActivate = true;
            status = await this.Domain.User.SetPasswordRecoveryCode(changemodel);
            
            this.init();

            // change password
            if (status.Status)
            {               
                ActiveUserAccount activemodel = new ActiveUserAccount();
                activemodel.Email = EMAIL_DEFAULT;
                activemodel.Code = status.Returns.ToString();

                status = await this.Domain.User.ActiveUserAccount(activemodel);
                this.Perform_ShouldBeTrue();

            }
            else
            {
                this.Perform_ShouldBeTrue();
            }

        }

        [TestMethod]
        public async Task T04_03_1_Login_Success()
        {
            this.init();

            UserLogin model = new UserLogin();

            model.Email = EMAIL_DEFAULT;
            model.Password = GW.Helpers.MD5.BuildMD5(PASSWORD_DEFAULT);
            model.ClientIP = "127.0.0.1";
            model.ClienteBrowserName = "Test";
            model.AuthToken = Guid.NewGuid().ToString();
            model.AuthTokenExpires = DateTime.Now.AddHours(8);

            UserModel user = await this.Domain.Login(model);
           
            this.Perform_ShouldBeTrue();

            this.finalize();

        }

        [TestMethod]
        public async Task T04_04_2_Login_InvalidPassword()
        {
            this.init();

            UserLogin model = new UserLogin();

            model.Email = EMAIL_DEFAULT;
            model.Password = GW.Helpers.MD5.BuildMD5("pwdinvalid");
            model.ClientIP = "127.0.0.1";
            model.ClienteBrowserName = "Test";
            model.AuthToken = Guid.NewGuid().ToString();
            model.AuthTokenExpires = DateTime.Now.AddHours(8);

            UserModel user = await this.Domain.Login(model);

            this.Perform_ShouldBeFalse();

            this.finalize();

            
        }


        [TestMethod]
        public async Task T04_05_1_CheckPermissions_When_Allowed()
        {
            this.init();
            
            List<UserPermissions> userPermissions = new List<UserPermissions>();

            userPermissions = await this.Domain.GetUserPermissions(2, 1003);

            userPermissions.ShouldNotBeNull<List<UserPermissions>>();

            PERMISSION_STATE_ENUM state 
                = await this.Domain.CheckPermission(userPermissions, "SYSTEST", PERMISSION_CHECK_ENUM.READ);

            state.ShouldBeEquivalentTo(PERMISSION_STATE_ENUM.ALLOWED);

            this.finalize();
           
        }

        [TestMethod]
        public async Task T04_05_2_CheckPermissions_When_Allowed()
        {
            this.init();

            List<UserPermissions> userPermissions = new List<UserPermissions>();

            userPermissions = await this.Domain.GetUserPermissions(2, 1003);

            userPermissions.ShouldNotBeNull<List<UserPermissions>>();

            PermissionsState state
                = await this.Domain.GetPermissionsState(userPermissions, "SYSTEST",true);

            state.AllowRead.ShouldBeEquivalentTo(true);
            state.AllowSave.ShouldBeEquivalentTo(true);
            state.AllowDelete.ShouldBeEquivalentTo(true);

            this.finalize();

        }

        [TestMethod]
        public async Task T04_05_3_GetListPermissions()
        {
            this.init();

            List<UserPermissions> userPermissions = new List<UserPermissions>();
            userPermissions = await this.Domain.GetUserPermissions(2, 1003);

            userPermissions = (from UserPermissions p in userPermissions
                               where p.ObjectCode == "SYSTEST"
                    select p).ToList();

            userPermissions.ShouldNotBeNull<List<UserPermissions>>();

            this.finalize();

        }

        [TestMethod]
        public async Task T04_06_CheckPermissions_When_Denied()
        {
            this.init();

            List<UserPermissions> userPermissions = new List<UserPermissions>();

            userPermissions = await this.Domain.GetUserPermissions(1, 1001);

            userPermissions.ShouldNotBeNull<List<UserPermissions>>();

            PERMISSION_STATE_ENUM state
                = await this.Domain.CheckPermission(userPermissions, "SYSUSER", PERMISSION_CHECK_ENUM.SAVE);

            state.ShouldBeEquivalentTo(PERMISSION_STATE_ENUM.DENIED);

            this.finalize();

        }


        [TestMethod]
        public async Task T04_07_Register_Login_State()
        {
            this.init();

            UserLogin model = new UserLogin();         
            model.ClientIP = "127.0.0.1";
            model.ClienteBrowserName = "Test";
         
            UpdateUserLogin login = new UpdateUserLogin();
            login.UserID = 1003;

            await this.Domain.RegisterLoginState(model, login);            

        }


        [TestMethod]
        public async Task T04_08_Logout()
        {
            this.init();

            status = await this.Domain.User.SetDateLogout(USERID_DEFAULT);

            this.Perform_ShouldBeTrue();

            this.finalize();

        }

        [TestMethod]
        public async Task T04_09_Get_Temporary_Password()
        {
            this.init();

            ChangeUserPassword model = new ChangeUserPassword();
            model.Email = EMAIL_DEFAULT;

            status = await this.Domain.GetTemporaryPassword(model);

            this.Perform_ShouldBeTrue();

        }

        [TestMethod]
        public async Task T04_10_Change_User_State()
        {
            this.init();

            UserChangeState model = new UserChangeState();
            model.UserID = USERID_DEFAULT;
            model.LockedValue = 0;
            model.ActiveValue = 1;

            status = await this.Domain.User.ChangeState(model);

            this.Perform_ShouldBeTrue();

            this.finalize();
        }
 

        [TestMethod]
        public async Task T04_11_Change_User_Password()
        {
            this.init();
          
            // get code
            ChangeUserPassword changemodel = new ChangeUserPassword();
            changemodel.Email = EMAIL_DEFAULT;
            changemodel.NewPassword = "NEWPWD"; 

            status = await this.Domain.User.SetPasswordRecoveryCode(changemodel);
                        
            this.init();

            // change password
            if (status.Status)
            {            
                changemodel.Code = status.Returns.ToString();

                status = await this.Domain.User.ChangeUserPassword(changemodel);
                this.Perform_ShouldBeTrue();

            }
            else
            {
                this.Perform_ShouldBeFalse();
            }

        }


        [TestMethod]
        public async Task T04_12_Change_User_Profile_Image()
        {
            this.init();

            ChangeUserImage model = new ChangeUserImage();
            model.UserID = USERID_DEFAULT;
            model.FileName = "image_user.png"; 

            status = await this.Domain.ChangeUserProfileImage(model);

            this.Perform_ShouldBeTrue();

        }
   

    }
}