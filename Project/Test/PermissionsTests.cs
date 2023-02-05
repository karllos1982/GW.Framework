
namespace GW.Membership.Test
{
    [TestClass]
    public class T03_PermissionTests: BaseTest
    {
       
        private ObjectPermissionEntry CreateNewObjectPermission(Int64 id, string name, string codigo)
        {
            ObjectPermissionEntry ret = new ObjectPermissionEntry();

            ret.ObjectPermissionID = id;
            ret.ObjectName = name;
            ret.ObjectCode = codigo;

            return ret;
        }

        [TestMethod]
        public async Task T03_01_1_InsertObjectPermissions_Success()
        {
            Resources res = new Resources();

            ObjectPermissionEntry obj;

            obj = CreateNewObjectPermission(1001, "Table.Sys.Basic", "SYSTEST");
            ObjectPermissionEntry newobj = await res.Domain.ObjectPermission.Set(obj, SysDefaultUser);

            obj = CreateNewObjectPermission(1002, "Table.Sys2.Basic2", "SYSTEST2");                       
            newobj = await res.Domain.ObjectPermission.Set(obj, SysDefaultUser);

            status = res.Context.ExecutionStatus;

            res.finalize();

            res.Perform_ShouldBeTrue(status);

        }

        [TestMethod]
        public async Task T03_01_2_InsertObjectPermissions_Fail()
        {
            Resources res = new Resources();

            ObjectPermissionEntry obj;

            obj = CreateNewObjectPermission(999, "", "");
            ObjectPermissionEntry newobj = await res.Domain.ObjectPermission.Set(obj, SysDefaultUser);          

            status = res.Context.ExecutionStatus;

            res.finalize();

            res.Perform_ShouldBeFalse(status);

        }


        [TestMethod]
        public async Task T03_02_1_List_ObjectPermissions()
        {
            Resources res = new Resources();

            List<ObjectPermissionList> result = null;

            result = await res.Domain.ObjectPermission.List(new ObjectPermissionParam() { });
         
            res.finalize();

            result.ShouldNotBeNull<List<ObjectPermissionList>>();

        }

        [TestMethod]
        public async Task T03_02_2_Get_ObjectPermissions()
        {
            Resources res = new Resources();

            ObjectPermissionResult result = null;

            result = await res.Domain.ObjectPermission.Get(
                new ObjectPermissionParam() { pObjectPermissionID = 1001 });
           
            res.finalize();

            result.ObjectCode.ShouldBeEquivalentTo("SYSTEST");


        }
        [TestMethod]
        public async Task T03_03_Search_ObjectPermissions()
        {
            Resources res = new Resources();

            List<ObjectPermissionResult> result = null;

            result = await res.Domain.ObjectPermission.Search(new ObjectPermissionParam() { });
            
            res.finalize();

            result.ShouldNotBeNull<List<ObjectPermissionResult>>();

        }


        [TestMethod]
        public async Task T03_04_Insert_PermissionsForRole()
        {
            Resources res = new Resources();

            PermissionEntry obj;

            obj = new PermissionEntry()
            {
                PermissionID = 1001,
                ObjectPermissionID = 1001,
                UserID = null,
                RoleID = 2,
                ReadStatus = 1,
                SaveStatus = -1,
                DeleteStatus = -1,
                TypeGrant = "N"
            };

            PermissionEntry newobj = await res.Domain.Permission.Set(obj, SysDefaultUser);

            status = res.Context.ExecutionStatus;

            res.finalize();

            res.Perform_ShouldBeTrue(status);

        }

        [TestMethod]
        public async Task T03_05_Insert_PermissionsForUser()
        {
            Resources res = new Resources();

            PermissionEntry obj;

            obj = new PermissionEntry()
            {
                PermissionID = 1002,
                ObjectPermissionID = 1001,
                UserID = 1003,
                RoleID = null,
                ReadStatus = 1,
                SaveStatus = 1, 
                DeleteStatus = 1,
                TypeGrant = "N"
            };

            PermissionEntry newobj = 
                await  res.Domain.Permission.Set(obj, SysDefaultUser);

            status = res.Context.ExecutionStatus;

            res.finalize();

            res.Perform_ShouldBeTrue(status);

        }

        [TestMethod]
        public async Task T03_06_Insert_PermissionsForUser()
        {
            Resources res = new Resources();

            PermissionEntry obj;

            obj = new PermissionEntry()
            {
                PermissionID = 1003,
                ObjectPermissionID = 1001,
                UserID = 1001,
                RoleID = null,
                ReadStatus = 1,
                SaveStatus = -1,
                DeleteStatus = -1,
                TypeGrant = "N"
            };

            PermissionEntry newobj = 
                await res.Domain.Permission.Set(obj, SysDefaultUser);

            status = res.Context.ExecutionStatus;

            res.finalize();

            res.Perform_ShouldBeTrue(status);

        }

        [TestMethod]
        public async Task T03_07_List_Permissions()
        {
            Resources res = new Resources();

            List<PermissionList> result = null;

            result = 
                await res.Domain.Permission.List(new PermissionParam() { });
         
            res.finalize();

            result.ShouldNotBeNull<List<PermissionList>>();

        }

        [TestMethod]
        public async Task T03_08_Search_Permissions()
        {
            Resources res = new Resources();

            List<PermissionResult> result = null;

            result = await res.Domain.Permission.Search(new PermissionParam() { });
         
            res.finalize();

            result.ShouldNotBeNull<List<PermissionResult>>();

        }

        [TestMethod]
        public async Task T03_09_Search_PermissionsByUser()
        {
            Resources res = new Resources();

            List<PermissionResult> result = null;

            result = await res.Domain.Permission.GetPermissionsByRoleUser(1,1001 );
         
            res.finalize();

            result.Count.ShouldBeEquivalentTo(9);

        }


    }
}