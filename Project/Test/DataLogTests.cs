

using System.Data;
using System.Data.SqlClient;

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

            DataTable dt = ((SqlConnection)((DapperContext)res.Domain.Context).Connection[0]).GetSchema(); 

            res.finalize();

            result.ShouldNotBeNull<List<DataLogList>>();

        }

        [TestMethod]
        public async Task T06_02_Search_DataLog()
        {
             Resources res = new Resources(); 

            List<DataLogResult> result = null;
            DataLogParam param = new DataLogParam();

            param.pEmail = EMAIL_DEFAULT;
            param.pDate_Start = DateTime.Now.AddDays(-1);
            param.pData_End = DateTime.Now.AddDays(1);

            result = await res.Domain.DataLog.Search(param);
            
            res.finalize();

            result.ShouldNotBeNull<List<DataLogResult>>();

        }

        [TestMethod]
        public async Task T06_03_Get_DataLog()
        {
            Resources res = new Resources();

            DataLogResult result = null;
            DataLogParam param = new DataLogParam();

            param.pDataLogID = -1; 

            result = await res.Domain.DataLog.Get(param);

            res.finalize();

            result.ShouldBeNull<DataLogResult>();

        }

    }
}