
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
            Resources res = new Resources(); 

            InstanceEntry obj = new InstanceEntry();

            obj.InstanceID = 2;
            obj.InstanceName = "System";
            obj.InstanceTypeName = "Default Instances"; 
            obj.CreateDate = DateTime.Now;
            obj.IsActive = 1;

            InstanceEntry Entry = await res.Domain.Instance.Set(obj, this.SysDefaultUser);

            status = res.Context.ExecutionStatus;

            res.finalize();

            res.Perform_ShouldBeTrue(status);
        }

        [TestMethod]
        public async Task T00_01_2_Insert_New_Instance_InvalidName()
        {
            Resources res = new Resources();

            InstanceEntry obj = new InstanceEntry();

            obj.InstanceID = 0;
            obj.InstanceName = "System";
            obj.InstanceTypeName = "Default Instances";
            obj.CreateDate = DateTime.Now;
            obj.IsActive = 1;

            InstanceEntry Entry = await res.Domain.Instance.Set(obj, this.SysDefaultUser);

            status = res.Context.ExecutionStatus;

            res.finalize();

            res.Perform_ShouldBeFalse(status );
        }

        [TestMethod]
        public async Task T00_02_Get_Instance()
        {
            Resources res = new Resources();

            InstanceResult result = null;

            result = await res.Domain.Instance.Get(new InstanceParam() { pInstanceID=2});
           
            res.finalize();

            result.ShouldNotBeNull<InstanceResult>();

        }

        [TestMethod]
        public async Task T00_03_List_Instance()
        {
            Resources res = new Resources();

            List<InstanceList> result = null;

            result = await res.Domain.Instance.List(new InstanceParam() { });
          
            res.finalize();

            result.ShouldNotBeNull<List<InstanceList>>();

        }

        [TestMethod]
        public async Task T00_04_Search_InstanceByName()
        {
            Resources res = new Resources();

            List<InstanceResult> result = null;

            result = await res.Domain.Instance.Search(new InstanceParam() { pInstanceName = "System" });
           
            res.finalize();

            result.ShouldNotBeNull<List<InstanceResult>>();

        }
              

    }
}