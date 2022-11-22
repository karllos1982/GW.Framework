

namespace GW.Membership.Test
{
    [TestClass]
    public class T06_DataLogTests: BaseTest
    {

        private const string EMAIL_DEFAULT = "usertest@gw.com.br";

        [TestMethod]
        public async Task T06_01_List_DataLog()
        {
            this.init();

            List<DataLogList> result = null;

            result = await this.Domain.DataLog.List(new DataLogParam() { });

            result.ShouldNotBeNull<List<DataLogList>>();

            this.finalize();

        }

        [TestMethod]
        public async Task T06_02_Search_DataLog()
        {
            this.init();

            List<DataLogSearchResult> result = null;
            DataLogParam param = new DataLogParam();

            param.pEmail = EMAIL_DEFAULT;
            param.pDate_Start = DateTime.Now.AddDays(-1);
            param.pData_End = DateTime.Now.AddDays(1);

            result = await this.Domain.DataLog.Search(param);

            result.ShouldNotBeNull<List<DataLogSearchResult>>();

            this.finalize();

        }


    }
}