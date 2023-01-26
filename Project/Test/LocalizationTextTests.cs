
namespace GW.Membership.Test
{
    [TestClass]
    public class T07_LocalizationTextTests : BaseTest
    {
        public T07_LocalizationTextTests()
        {
            //this.init();
            // this.Context.End(false,true);

        }

        [TestMethod]
        public async Task T07_01_1_Insert_New_LocalizationText_Success()
        {
            Resources res = new Resources();

            LocalizationTextEntry obj = new LocalizationTextEntry();

            obj.LocalizationTextID = 999;
            obj.Language = "en-us";
            obj.Name = "New-Localization-Name";
            obj.Code= "999";    
            obj.Text= "New-Localization-Text";

            LocalizationTextEntry Entry = await res.Domain.LocalizationText.Set(obj, this.SysDefaultUser);

            status = res.Context.ExecutionStatus;

            res.finalize();

            res.Perform_ShouldBeTrue(status);
        }

        [TestMethod]
        public async Task T07_01_2_Insert_New_LocalizationText_InvalidName()
        {
            Resources res = new Resources();

            LocalizationTextEntry obj = new LocalizationTextEntry();

            obj.LocalizationTextID = 1000;
            obj.Language = "en-us";
            obj.Name = "New-Localization-Name";
            obj.Code = "1000";
            obj.Text = "New-Localization-Text";

            LocalizationTextEntry Entry = await res.Domain.LocalizationText.Set(obj, this.SysDefaultUser);

            status = res.Context.ExecutionStatus;

            res.finalize();

            res.Perform_ShouldBeFalse(status);
        }

        [TestMethod]
        public async Task T07_02_Get_LocalizationText()
        {
            Resources res = new Resources();

            LocalizationTextResult result = null;

            result = await res.Domain.LocalizationText
                .Get(new LocalizationTextParam() { pLocalizationTextID = 999 });

            res.finalize();

            result.ShouldNotBeNull<LocalizationTextResult>();

        }

        [TestMethod]
        public async Task T07_03_List_LocalizationText()
        {
            Resources res = new Resources();

            List<LocalizationTextList> result = null;

            result = await res.Domain.LocalizationText.List(new LocalizationTextParam() { });

            res.finalize();

            result.ShouldNotBeNull<List<LocalizationTextList>>();

        }

        [TestMethod]
        public async Task T07_04_Search_LocalizationTextByLanguage()
        {
            Resources res = new Resources();

            List<LocalizationTextResult> result = null;

            result = await res.Domain.LocalizationText
                .Search(new LocalizationTextParam() { pLanguage = "en-us" });

            res.finalize();

            result.ShouldNotBeNull<List<LocalizationTextResult>>();

        }


    }
}


