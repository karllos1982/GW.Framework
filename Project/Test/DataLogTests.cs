

namespace GW.Membership.Test
{
    [TestClass]
    public class T06_DataLogTests: BaseTest
    {

        private const string EMAIL_DEFAULT = "usertest@gw.com.br";

        [TestMethod]
        public async Task T06_01_List_DataLog()
        {
             Resources res = new Resources(); 

            List<DataLogList> result = null;

            result = await res.Domain.DataLog.List(new DataLogParam() { });
           
            res.finalize();

            result.ShouldNotBeNull<List<DataLogList>>();

        }

        [TestMethod]
        public async Task T06_02_Search_DataLog()
        {
             Resources res = new Resources(); 

            List<DataLogSearchResult> result = null;
            DataLogParam param = new DataLogParam();

            param.pEmail = EMAIL_DEFAULT;
            param.pDate_Start = DateTime.Now.AddDays(-1);
            param.pData_End = DateTime.Now.AddDays(1);

            result = await res.Domain.DataLog.Search(param);
            
            res.finalize();

            result.ShouldNotBeNull<List<DataLogSearchResult>>();

        }


    }
}