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
            this.init();

            RoleModel obj = new RoleModel();

            obj.RoleID = 3;
            obj.RoleName = "SimpleUser";
            obj.CreateDate = DateTime.Now;
            obj.IsActive = 1;

            RoleModel newobj = await this.Domain.Role.Set(obj, this.SysDefaultUser); 

            this.finalize();

            this.Perform_ShouldBeTrue();

        }

        [TestMethod]
        public async Task T01_01_2_Insert_New_Role_InvalidName()
        {
            this.init();

            RoleModel obj = new RoleModel();

            obj.RoleID = 3;
            obj.RoleName = "";
            obj.CreateDate = DateTime.Now;
            obj.IsActive = 1;

            RoleModel newobj = await  this.Domain.Role.Set(obj, this.SysDefaultUser);

            this.finalize();

            this.Perform_ShouldBeFalse();

        }

        [TestMethod]
        public async Task T01_01_3_Insert_New_Role_Success()
        {
            this.init();

            RoleModel obj = new RoleModel();

            obj.RoleID = 4;
            obj.RoleName = "Operador";
            obj.CreateDate = DateTime.Now;
            obj.IsActive = 1;

            RoleModel newobj = await this.Domain.Role.Set(obj, this.SysDefaultUser);

            this.finalize();

            this.Perform_ShouldBeTrue();

        }

        [TestMethod]
        public async Task T01_02_Get_Role()
        {
            this.init();

            RoleModel result = null;

            result = await this.Domain.Role.Get( new RoleParam() { pRoleID = 3 }); 
            
            result.ShouldNotBeNull<RoleModel>();

            this.finalize();

        }

        [TestMethod]
        public async Task T01_03_List_Role()
        {
            this.init();

            List<RoleList> result = null;

            result = await this.Domain.Role.List(new RoleParam() { });

            result.ShouldNotBeNull<List<RoleList>>();

            this.finalize();

        }

        [TestMethod]
        public async Task T01_04_Search_RoleByName()
        {
            this.init();

            List<RoleSearchResult> result = null;

            result = 
                await this.Domain.Role.Search(new RoleParam() { pRoleName= "SimpleUser" });

            result.ShouldNotBeNull<List<RoleSearchResult>>();

            this.finalize();

        }

        [TestMethod]
        public async Task T01_05_Delete_Role()
        {
            this.init();

            RoleModel obj = new RoleModel() { RoleID = 3 };

            RoleModel newobj =
                await this.Domain.Role.Delete(obj, SysDefaultUser);

            this.Perform_ShouldBeTrue();

            this.finalize();

        }

    }
}