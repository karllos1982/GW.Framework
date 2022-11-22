
namespace GW.Membership.Test
{
    [TestClass]
    public class T05_SessionTests: BaseTest
    {

        private const string EMAIL_DEFAULT = "usertest@gw.com.br";
       
        [TestMethod]
        public async Task T05_01_List_Session()
        {
            this.init();

            List<SessionLogList> result = null;

            result = await this.Domain.SessionLog.List(new SessionLogParam() { });

            result.ShouldNotBeNull<List<SessionLogList>>();

            this.finalize();

        }

        [TestMethod]
        public async Task T05_02_Search_Session()
        {
            this.init();

            List<SessionLogSearchResult> result = null;
            SessionLogParam param = new SessionLogParam();

            param.pEmail = EMAIL_DEFAULT;
            param.pDate_Start = DateTime.Now.AddDays(-1);
            param.pData_End = DateTime.Now.AddDays(1);
            
            result = await this.Domain.SessionLog.Search(param);

            result.ShouldNotBeNull<List<SessionLogSearchResult>>();

            this.finalize();

        }
    
         

    }
}