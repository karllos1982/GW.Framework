
namespace GW.Membership.Test
{
    [TestClass]
    public class T03_PermissionTests: BaseTest
    {
       
        private ObjectPermissionModel CreateNewObjectPermission(Int64 id, string name, string codigo)
        {
            ObjectPermissionModel ret = new ObjectPermissionModel();

            ret.ObjectPermissionID = id;
            ret.ObjectName = name;
            ret.ObjectCode = codigo;

            return ret;
        }

        [TestMethod]
        public async Task T03_01_InsertObjectPermissions()
        {
            Resources res = new Resources();

            ObjectPermissionModel obj;

            obj = CreateNewObjectPermission(1001, "Table.Sys.Basic", "SYSTEST");
            ObjectPermissionModel newobj = await res.Domain.ObjectPermission.Set(obj, SysDefaultUser);

            obj = CreateNewObjectPermission(1002, "Table.Sys2.Basic2", "SYSTEST2");                       
            newobj = await res.Domain.ObjectPermission.Set(obj, SysDefaultUser);
            
            res.finalize();

            res.Perform_ShouldBeTrue();

        }

        [TestMethod]
        public async Task T03_02_List_ObjectPermissions()
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

            ObjectPermissionModel result = null;

            result = await res.Domain.ObjectPermission.Get(
                new ObjectPermissionParam() { pObjectPermissionID = 1001 });
           
            res.finalize();

            result.ObjectCode.ShouldBeEquivalentTo("SYSTEST");


        }
        [TestMethod]
        public async Task T03_03_Search_ObjectPermissions()
        {
            Resources res = new Resources();

            List<ObjectPermissionSearchResult> result = null;

            result = await res.Domain.ObjectPermission.Search(new ObjectPermissionParam() { });
            
            res.finalize();

            result.ShouldNotBeNull<List<ObjectPermissionSearchResult>>();

        }


        [TestMethod]
        public async Task T03_04_Insert_PermissionsForRole()
        {
            Resources res = new Resources();

            PermissionModel obj;

            obj = new PermissionModel()
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

            PermissionModel newobj = await res.Domain.Permission.Set(obj, SysDefaultUser);
            
            res.finalize();

            res.Perform_ShouldBeTrue();

        }

        [TestMethod]
        public async Task T03_05_Insert_PermissionsForUser()
        {
            Resources res = new Resources();

            PermissionModel obj;

            obj = new PermissionModel()
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

            PermissionModel newobj = 
                await  res.Domain.Permission.Set(obj, SysDefaultUser);
        
            res.finalize();

            res.Perform_ShouldBeTrue();

        }

        [TestMethod]
        public async Task T03_06_Insert_PermissionsForUser()
        {
            Resources res = new Resources();

            PermissionModel obj;

            obj = new PermissionModel()
            {
                PermissionID = 1003,
                ObjectPermissionID = 10001,
                UserID = 1001,
                RoleID = null,
                ReadStatus = 1,
                SaveStatus = -1,
                DeleteStatus = -1,
                TypeGrant = "N"
            };

            PermissionModel newobj = 
                await res.Domain.Permission.Set(obj, SysDefaultUser);
          
            res.finalize();

            res.Perform_ShouldBeTrue();

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

            List<PermissionSearchResult> result = null;

            result = await res.Domain.Permission.Search(new PermissionParam() { });
         
            res.finalize();

            result.ShouldNotBeNull<List<PermissionSearchResult>>();

        }

        [TestMethod]
        public async Task T03_09_Search_PermissionsByUser()
        {
            Resources res = new Resources();

            List<PermissionSearchResult> result = null;

            result = await res.Domain.Permission.GetPermissionsByRoleUser(1,1001 );
         
            res.finalize();

            result.Count.ShouldBeEquivalentTo(8);

        }


    }
}