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
    public class T05_SessionTests: BaseTest
    {

        private const string EMAIL_DEFAULT = "usertest@gw.com.br";
       
        [TestMethod]
        public void T05_01_List_Session()
        {
            this.init();

            List<SessionList> result = null;

            result = this.Domain.SessionUnit.List(new SessionParam() { });

            result.ShouldNotBeNull<List<SessionList>>();

            this.finalize();

        }

        [TestMethod]
        public void T05_02_Search_Session()
        {
            this.init();

            List<SessionSearchResult> result = null;
            SessionParam param = new SessionParam();

            param.pEmail = EMAIL_DEFAULT;
            param.pDate_Start = DateTime.Now.AddDays(-1);
            param.pData_End = DateTime.Now.AddDays(1);
            
            result = this.Domain.SessionUnit.Search(param);

            result.ShouldNotBeNull<List<SessionSearchResult>>();

            this.finalize();

        }
    
         

    }
}