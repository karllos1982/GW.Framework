using Test;
using GW.Membership.Models;
using Shouldly;

namespace GW.Membership.Test
{
    [TestClass]
    public class T01_RoleTests : BaseTest
    {
        [TestMethod]
        public async Task T01_01_1_Insert_New_Role_Success()
        {
            Resources res = new Resources();

            RoleEntry obj = new RoleEntry();

            obj.RoleID = 3;
            obj.RoleName = "SimpleUser";
            obj.CreateDate = DateTime.Now;
            obj.IsActive = true;

            RoleEntry newobj = await res.Domain.Role.Set(obj, this.SysDefaultUser);

            status = res.Context.ExecutionStatus;

            res.finalize();

            res.Perform_ShouldBeTrue(status);

        }

        [TestMethod]
        public async Task T01_01_2_Insert_New_Role_InvalidName()
        {
            Resources res = new Resources();

            RoleEntry obj = new RoleEntry();

            obj.RoleID = 3;
            obj.RoleName = "";
            obj.CreateDate = DateTime.Now;
            obj.IsActive = true;

            RoleEntry newobj = await res.Domain.Role.Set(obj, this.SysDefaultUser);

            status = res.Context.ExecutionStatus;

            res.finalize();

            res.Perform_ShouldBeFalse(status );

        }

        [TestMethod]
        public async Task T01_01_3_Insert_New_Role_Success()
        {
            Resources res = new Resources();

            RoleEntry obj = new RoleEntry();

            obj.RoleID = 4;
            obj.RoleName = "Operador";
            obj.CreateDate = DateTime.Now;
            obj.IsActive = true;

            RoleEntry newobj = await res.Domain.Role.Set(obj, this.SysDefaultUser);

            status = res.Context.ExecutionStatus;

            res.finalize();

            res.Perform_ShouldBeTrue(status);

        }

        [TestMethod]
        public async Task T01_02_Get_Role()
        {
            Resources res = new Resources();

            RoleResult result = null;

            result = await res.Domain.Role.Get( new RoleParam() { pRoleID = 3 }); 
            
            result.ShouldNotBeNull<RoleResult>();

            res.finalize();

        }

        [TestMethod]
        public async Task T01_03_List_Role()
        {
            Resources res = new Resources();

            List<RoleList> result = null;

            result = await res.Domain.Role.List(new RoleParam() { });

            result.ShouldNotBeNull<List<RoleList>>();

            res.finalize();

        }

        [TestMethod]
        public async Task T01_04_Search_RoleByName()
        {
            Resources res = new Resources();

            List<RoleResult> result = null;

            result = 
                await res.Domain.Role.Search(new RoleParam() { pRoleName= "SimpleUser" });

            result.ShouldNotBeNull<List<RoleResult>>();

            res.finalize();

        }

        [TestMethod]
        public async Task T01_05_Delete_Role()
        {
            Resources res = new Resources();

            RoleEntry obj = new RoleEntry() { RoleID = 3 };

            RoleEntry newobj =
                await res.Domain.Role.Delete(obj, SysDefaultUser);

            status = res.Context.ExecutionStatus;

            res.Perform_ShouldBeTrue(status);

            res.finalize();

        }

    }
}