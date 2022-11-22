
namespace GW.Membership.Test
{
    [TestClass]
    public class T02_UserTests: BaseTest
    {
       
        [TestMethod]
        public async Task T02_01_Get_User()
        {
            this.init();

            UserModel result = null;

            result = await this.Domain.User.Get(new UserParam() { pUserID = 1001 });

            result.ShouldNotBeNull<UserModel>();

            this.finalize();

        }

        [TestMethod]
        public async Task T02_02_List_User()
        {
            this.init();

            List<UserList> result = null;

            result = await this.Domain.User.List(new UserParam() { });

            result.ShouldNotBeNull<List<UserList>>();

            this.finalize();

        }

        [TestMethod]
        public async Task T02_03_Search_UserByEmail()
        {
            this.init();

            List<UserSearchResult> result = null;

            result = await  this.Domain.User.Search(
                new UserParam() { pEmail = "deleted.user@sys.com" });

            result.ShouldNotBeNull<List<UserSearchResult>>();

            this.finalize();

        }

        [TestMethod]
        public async Task T02_04_Search_UserByRole()
        {
            this.init();

            List<UserSearchResult> result = null;

            result = await this.Domain.User.Search(new UserParam() { pRoleID=1 });

            result.ShouldNotBeNull<List<UserSearchResult>>();

            this.finalize();

        }

        [TestMethod]
        public async Task T02_05_Search_UserByInstance()
        {
            this.init();

            List<UserSearchResult> result = null;

            result = await this.Domain.User.Search(new UserParam() { pInstanceID = 1 });

            result.ShouldNotBeNull<List<UserSearchResult>>();

            this.finalize();

        }

        [TestMethod]
        public async Task T02_06_01_AddRoleToUser_Success()
        {
            this.init();            

            status = await this.Domain.User.AddRoleToUser(1003, 4, true);

            this.Perform_ShouldBeTrue();

            this.finalize();

        }

        [TestMethod]
        public async Task T02_06_02_AddRoleToUser_Fail()
        {
            this.init();

            status = await this.Domain.User.AddRoleToUser(1003, 4, true);

            this.Perform_ShouldBeFalse();

            this.finalize();

        }


        [TestMethod]
        public async Task T02_06_03_RemoveRole_FromUser()
        {
            this.init();

            status = await this.Domain.User.RemoveRoleFromUser(1003, 4, true);

            this.Perform_ShouldBeTrue();

            this.finalize();

        }

        [TestMethod]
        public async Task T02_07_01_AddInstanceToUser_Success()
        {
            this.init();

            status = await this.Domain.User.AddInstanceToUser(1003, 2, true);

            this.Perform_ShouldBeTrue();

            this.finalize();

        }

        [TestMethod]
        public async Task T02_07_02_AddRoleToUser_Fail()
        {
            this.init();

            status = await this.Domain.User.AddInstanceToUser(1003, 2, true);

            this.Perform_ShouldBeFalse();

            this.finalize();

        }


        [TestMethod]
        public async Task T02_07_03_RemoveRole_FromUser()
        {
            this.init();

            status = await this.Domain.User.RemoveInstanceFromUser(1003, 2, true);

            this.Perform_ShouldBeTrue();

            this.finalize();

        }


        [TestMethod]
        public async Task T02_08_Delete_User()
        {
            this.init();

            UserModel obj = new UserModel() { UserID = 1002 };

            UserModel newobj = await this.Domain.User.Delete(obj, SysDefaultUser);

            this.Perform_ShouldBeTrue();

            this.finalize();

        }

     
     

    }
}