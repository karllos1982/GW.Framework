using System;
using Test;
using GW.Membership.Models;
using Shouldly;
using GW.Core.Helpers;

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
            this.init();

            ObjectPermissionModel obj;

            obj = CreateNewObjectPermission(1001, "Table.Sys.Basic", "SYSTEST");
            ObjectPermissionModel newobj = await this.Domain.ObjectPermission.Set(obj, SysDefaultUser);

            obj = CreateNewObjectPermission(1002, "Table.Sys2.Basic2", "SYSTEST2");                       
            newobj = await this.Domain.ObjectPermission.Set(obj, SysDefaultUser);

            this.Perform_ShouldBeTrue();

            this.finalize();

        }

        [TestMethod]
        public async Task T03_02_List_ObjectPermissions()
        {
            this.init();

            List<ObjectPermissionList> result = null;

            result = await this.Domain.ObjectPermission.List(new ObjectPermissionParam() { });

            result.ShouldNotBeNull<List<ObjectPermissionList>>();

            this.finalize();

        }

        [TestMethod]
        public async Task T03_02_2_Get_ObjectPermissions()
        {
            this.init();

            ObjectPermissionModel result = null;

            result = await this.Domain.ObjectPermission.Get(
                new ObjectPermissionParam() { pObjectPermissionID = 1001 });

            result.ObjectCode.ShouldBeEquivalentTo("SYSTEST");

            this.finalize();


        }
        [TestMethod]
        public async Task T03_03_Search_ObjectPermissions()
        {
            this.init();

            List<ObjectPermissionSearchResult> result = null;

            result = await this.Domain.ObjectPermission.Search(new ObjectPermissionParam() { });

            result.ShouldNotBeNull<List<ObjectPermissionSearchResult>>();

            this.finalize();

        }


        [TestMethod]
        public async Task T03_04_Insert_PermissionsForRole()
        {
            this.init();

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

            PermissionModel newobj = await this.Domain.Permission.Set(obj, SysDefaultUser);

            this.Perform_ShouldBeTrue();

            this.finalize();

        }

        [TestMethod]
        public async Task T03_05_Insert_PermissionsForUser()
        {
            this.init();

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
                await  this.Domain.Permission.Set(obj, SysDefaultUser);

            this.Perform_ShouldBeTrue();

            this.finalize();

        }

        [TestMethod]
        public async Task T03_06_Insert_PermissionsForUser()
        {
            this.init();

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
                await this.Domain.Permission.Set(obj, SysDefaultUser);

            this.Perform_ShouldBeTrue();

            this.finalize();

        }

        [TestMethod]
        public async Task T03_07_List_Permissions()
        {
            this.init();

            List<PermissionList> result = null;

            result = 
                await this.Domain.Permission.List(new PermissionParam() { });

            result.ShouldNotBeNull<List<PermissionList>>();

            this.finalize();

        }

        [TestMethod]
        public async Task T03_08_Search_Permissions()
        {
            this.init();

            List<PermissionSearchResult> result = null;

            result = await this.Domain.Permission.Search(new PermissionParam() { });

            result.ShouldNotBeNull<List<PermissionSearchResult>>();

            this.finalize();

        }

        [TestMethod]
        public async Task T03_09_Search_PermissionsByUser()
        {
            this.init();

            List<PermissionSearchResult> result = null;

            result = await this.Domain.Permission.GetPermissionsByRoleUser(1,1001 );

            result.Count.ShouldBeEquivalentTo(8);

            this.finalize();

        }


    }
}