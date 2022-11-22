using System;
using GW.Core.Data;
using GW.Core.Data.SQLServer;
using GW.Core.Manager;
using GW.Membership.Domain.Interfaces;
using GW.Membership.Domain;
using Test;
using GW.Membership.Models;
using Shouldly;

namespace GW.Membership.Test
{
    [TestClass]
    public class T02_UserTests: BaseTest
    {
       
        [TestMethod]
        public void T02_01_Get_User()
        {
            this.init();

            UserModel result = null;

            result = this.Domain.UserUnit.Get(1001);

            result.ShouldNotBeNull<UserModel>();

            this.finalize();

        }

        [TestMethod]
        public void T02_02_List_User()
        {
            this.init();

            List<UserList> result = null;

            result = this.Domain.UserUnit.List(new UserParam() { });

            result.ShouldNotBeNull<List<UserList>>();

            this.finalize();

        }

        [TestMethod]
        public void T02_03_Search_UserByEmail()
        {
            this.init();

            List<UserSearchResult> result = null;

            result = this.Domain.UserUnit.Search(new UserParam() { pEmail = "deleted.user@sys.com" });

            result.ShouldNotBeNull<List<UserSearchResult>>();

            this.finalize();

        }

        [TestMethod]
        public void T02_04_Search_UserByRole()
        {
            this.init();

            List<UserSearchResult> result = null;

            result = this.Domain.UserUnit.Search(new UserParam() { pRoleID=1 });

            result.ShouldNotBeNull<List<UserSearchResult>>();

            this.finalize();

        }

        [TestMethod]
        public void T02_05_Search_UserByInstance()
        {
            this.init();

            List<UserSearchResult> result = null;

            result = this.Domain.UserUnit.Search(new UserParam() { pInstanceID = 1 });

            result.ShouldNotBeNull<List<UserSearchResult>>();

            this.finalize();

        }

        [TestMethod]
        public void T02_06_01_AddRoleToUser_Success()
        {
            this.init();            

            status = this.Domain.UserUnit.AddRoleToUser(1003, 4, true);

            this.Perform_ShouldBeTrue();

            this.finalize();

        }

        [TestMethod]
        public void T02_06_02_AddRoleToUser_Fail()
        {
            this.init();

            status = this.Domain.UserUnit.AddRoleToUser(1003, 4, true);

            this.Perform_ShouldBeFalse();

            this.finalize();

        }


        [TestMethod]
        public void T02_06_03_RemoveRole_FromUser()
        {
            this.init();

            status = this.Domain.UserUnit.RemoveRoleFromUser(1003, 4, true);

            this.Perform_ShouldBeTrue();

            this.finalize();

        }

        [TestMethod]
        public void T02_07_01_AddInstanceToUser_Success()
        {
            this.init();

            status = this.Domain.UserUnit.AddInstanceToUser(1003, 2, true);

            this.Perform_ShouldBeTrue();

            this.finalize();

        }

        [TestMethod]
        public void T02_07_02_AddRoleToUser_Fail()
        {
            this.init();

            status = this.Domain.UserUnit.AddInstanceToUser(1003, 2, true);

            this.Perform_ShouldBeFalse();

            this.finalize();

        }


        [TestMethod]
        public void T02_07_03_RemoveRole_FromUser()
        {
            this.init();

            status = this.Domain.UserUnit.RemoveInstanceFromUser(1003, 2, true);

            this.Perform_ShouldBeTrue();

            this.finalize();

        }


        [TestMethod]
        public void T02_08_Delete_User()
        {
            this.init();

            UserModel obj = new UserModel() { UserID = 1002 };

            status = this.Domain.UserUnit.Delete(obj, SysDefaultUser);

            this.Perform_ShouldBeTrue();

            this.finalize();

        }

     
     

    }
}