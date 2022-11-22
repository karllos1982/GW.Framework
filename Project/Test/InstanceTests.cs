
namespace GW.Membership.Test
{
    [TestClass]
    public class T00_InstanceTests : BaseTest
    {
        public T00_InstanceTests()
        {
            //this.init();
           // this.Context.End(false,true);

        }

        [TestMethod]
        public async Task T00_01_1_Insert_New_Instance_Success()
        {
            this.init();

            InstanceModel obj = new InstanceModel();

            obj.InstanceID = 2;
            obj.InstanceName = "System";
            obj.InstanceTypeName = "Default Instances"; 
            obj.CreateDate = DateTime.Now;
            obj.IsActive = 1;

            InstanceModel model = await this.Domain.Instance.Set(obj, this.SysDefaultUser);

            this.finalize();

            this.Perform_ShouldBeTrue();

        }

        [TestMethod]
        public async Task T00_01_2_Insert_New_Instance_InvalidName()
        {
            this.init();

            InstanceModel obj = new InstanceModel();

            obj.InstanceID = 0;
            obj.InstanceName = "System";
            obj.InstanceTypeName = "Default Instances";
            obj.CreateDate = DateTime.Now;
            obj.IsActive = 1;

            InstanceModel model = await this.Domain.Instance.Set(obj, this.SysDefaultUser);

            this.finalize();

            this.Perform_ShouldBeFalse();

        }

        [TestMethod]
        public async Task T00_02_Get_Instance()
        {
            this.init();

            InstanceModel result = null;

            result = await this.Domain.Instance.Get(new InstanceParam() { pInstanceID=2});

            result.ShouldNotBeNull<InstanceModel>();

            this.finalize();

        }

        [TestMethod]
        public async Task T00_03_List_Instance()
        {
            this.init();

            List<InstanceList> result = null;

            result = await this.Domain.Instance.List(new InstanceParam() { });

            result.ShouldNotBeNull<List<InstanceList>>();

            this.finalize();

        }

        [TestMethod]
        public async Task T00_04_Search_InstanceByName()
        {
            this.init();

            List<InstanceSearchResult> result = null;

            result = await this.Domain.Instance.Search(new InstanceParam() { pInstanceName = "System" });

            result.ShouldNotBeNull<List<InstanceSearchResult>>();

            this.finalize();

        }

      

    }
}