
namespace GW.Membership.Test
{
    [TestClass]
    public class T02_UserTests: BaseTest
    {
       
        [TestMethod]
        public async Task T02_01_Get_User()
        {
            Resources res = new Resources();

            UserResult result = null;

            result = await res.Domain.User.Get(new UserParam() { pUserID = 1001 });
            
            res.finalize();

            result.ShouldNotBeNull<UserResult>();

        }

        [TestMethod]
        public async Task T02_02_List_User()
        {
            Resources res = new Resources();

            List<UserList> result = null;

            result = await res.Domain.User.List(new UserParam() { });
            
            res.finalize();

            result.ShouldNotBeNull<List<UserList>>();

        }

        [TestMethod]
        public async Task T02_03_Search_UserByEmail()
        {
            Resources res = new Resources();

            List<UserResult> result = null;

            result = await  res.Domain.User.Search(
                new UserParam() { pEmail = "deleted.user@sys.com" });
            
            res.finalize();

            result.ShouldNotBeNull<List<UserResult>>();

        }

        [TestMethod]
        public async Task T02_04_Search_UserByRole()
        {
            Resources res = new Resources();

            List<UserResult> result = null;

            result = await res.Domain.User.Search(new UserParam() { pRoleID=1 });
            
            res.finalize();

            result.ShouldNotBeNull<List<UserResult>>();

        }

        [TestMethod]
        public async Task T02_05_Search_UserByInstance()
        {
            Resources res = new Resources();

            List<UserResult> result = null;

            result = await res.Domain.User.Search(new UserParam() { pInstanceID = 1 });
           
            res.finalize();

            result.ShouldNotBeNull<List<UserResult>>();

        }

        [TestMethod]
        public async Task T02_06_01_AddRoleToUser_Success()
        {
            Resources res = new Resources();            

            var obj = await res.Domain.User.AddRoleToUser(1003, 4);

            status = res.Context.ExecutionStatus;

            res.finalize();

            res.Perform_ShouldBeTrue(status);

        }

        [TestMethod]
        public async Task T02_06_02_AddRoleToUser_Fail()
        {
            Resources res = new Resources();

            var obj = await res.Domain.User.AddRoleToUser(1003, 4);

            status = res.Context.ExecutionStatus;

            res.finalize();

            res.Perform_ShouldBeFalse(status );

        }


        [TestMethod]
        public async Task T02_06_03_RemoveRole_FromUser()
        {
            Resources res = new Resources();

            var obj = await res.Domain.User.RemoveRoleFromUser(1003, 4);

            status = res.Context.ExecutionStatus;

            res.finalize();

            res.Perform_ShouldBeTrue(status);

        }

        [TestMethod]
        public async Task T02_06_04_AlterRole()
        {
            Resources res = new Resources();

             await res.Domain.User.AlterRole(3, 2); 

            status = res.Context.ExecutionStatus;

            res.finalize();

            res.Perform_ShouldBeTrue(status);

        }

        [TestMethod]
        public async Task T02_07_01_AddInstanceToUser_Success()
        {
            Resources res = new Resources();

            var obj = await res.Domain.User.AddInstanceToUser(1003, 2);

            status = res.Context.ExecutionStatus;

            res.Perform_ShouldBeTrue(status);

            res.finalize();

        }

        [TestMethod]
        public async Task T02_07_02_AddInstanceToUser_Fail()
        {
            Resources res = new Resources();

            var obj = await res.Domain.User.AddInstanceToUser(1003, 2);

            status = res.Context.ExecutionStatus;

            res.finalize();

            res.Perform_ShouldBeFalse(status );

        }


        [TestMethod]
        public async Task T02_07_03_RemoveInstance_FromUser()
        {
            Resources res = new Resources();

            var obj = await res.Domain.User.RemoveInstanceFromUser(1003, 2);

            status = res.Context.ExecutionStatus;

            res.finalize();

            res.Perform_ShouldBeTrue(status);

        }

        [TestMethod]
        public async Task T02_07_04_AlterInstance()
        {
            Resources res = new Resources();

            await res.Domain.User.AlterInstance(3, 2);

            status = res.Context.ExecutionStatus;

            res.finalize();

            res.Perform_ShouldBeTrue(status);

        }


        [TestMethod]
        public async Task T02_08_Delete_User()
        {
            Resources res = new Resources();

            UserEntry obj = new UserEntry() { UserID = 1002 };

            UserEntry newobj = await res.Domain.User.Delete(obj, SysDefaultUser);

            status = res.Context.ExecutionStatus;

            res.finalize();

            res.Perform_ShouldBeTrue(status);

        }             
     

    }
}