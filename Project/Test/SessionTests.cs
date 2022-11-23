
namespace GW.Membership.Test
{
    [TestClass]
    public class T05_SessionTests: BaseTest
    {

        private const string EMAIL_DEFAULT = "usertest@gw.com.br";
       
        [TestMethod]
        public async Task T05_01_List_Session()
        {
            Resources res = new Resources();

            List<SessionLogList> result = null;

            result = await res.Domain.SessionLog.List(new SessionLogParam() { });
            
            res.finalize();

            result.ShouldNotBeNull<List<SessionLogList>>();

        }

        [TestMethod]
        public async Task T05_02_Search_Session()
        {
            Resources res = new Resources();

            List<SessionLogSearchResult> result = null;
            SessionLogParam param = new SessionLogParam();

            param.pEmail = EMAIL_DEFAULT;
            param.pDate_Start = DateTime.Now.AddDays(-1);
            param.pData_End = DateTime.Now.AddDays(1);
            
            result = await res.Domain.SessionLog.Search(param);
            
            res.finalize();

            result.ShouldNotBeNull<List<SessionLogSearchResult>>();

        }
    
         

    }
}