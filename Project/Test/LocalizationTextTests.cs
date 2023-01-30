
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
        public async Task T07_01_2_Insert_New_LocalizationText_InvalidCode()
        {
            Resources res = new Resources();

            LocalizationTextEntry obj = new LocalizationTextEntry();

            obj.LocalizationTextID = 0;
            obj.Language = "en-us";
            obj.Name = "New-Localization-Name";
            obj.Code = "999";
            obj.Text = "New-Localization-Text";

            LocalizationTextEntry Entry = await res.Domain.LocalizationText.Set(obj, this.SysDefaultUser);

            status = res.Context.ExecutionStatus;

            res.finalize();

            res.Perform_ShouldBeFalse(status);
        }

        [TestMethod]
        public async Task T07_01_3_Insert_New_LocalizationText_InvalidName()
        {
            Resources res = new Resources();

            LocalizationTextEntry obj = new LocalizationTextEntry();

            obj.LocalizationTextID = 0;
            obj.Language = "en-us";
            obj.Name = "Execution-Error";
            obj.Code = "9999";
            obj.Text = "New Execution error";

            LocalizationTextEntry Entry = await res.Domain.LocalizationText.Set(obj, this.SysDefaultUser);

            status = res.Context.ExecutionStatus;

            res.finalize();

            res.Perform_ShouldBeFalse(status);
        }
       

        [TestMethod]
        public async Task T07_01_4_Edit_New_LocalizationText_Success()
        {
            Resources res = new Resources();

            LocalizationTextEntry obj = new LocalizationTextEntry();

            obj.LocalizationTextID = 2;
            obj.Language = "en-us";
            obj.Name = "Validation-Error";
            obj.Code = "1002";
            obj.Text = "Data validation error. [ed]";

            LocalizationTextEntry Entry = await res.Domain.LocalizationText.Set(obj, this.SysDefaultUser);

            status = res.Context.ExecutionStatus;

            res.finalize();

            res.Perform_ShouldBeTrue(status);
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

        [TestMethod]
        public async Task T07_05_Delete_LocalizationText()
        {
            Resources res = new Resources();

            LocalizationTextEntry obj = new LocalizationTextEntry();
            obj.LocalizationTextID = 999;

            LocalizationTextEntry deleteobj 
                = await res.Domain.LocalizationText.Delete(obj, this.SysDefaultUser);

            res.finalize();

            deleteobj.ShouldNotBeNull<LocalizationTextEntry>();

        }

        [TestMethod]
        public async Task T07_06_GetListOfLanguage()
        {
            Resources res = new Resources();

            List<LocalizationTextList> result = null;

            result = await res.Domain.LocalizationText.GetListOfLanguages();

            res.finalize();

            result.ShouldNotBeNull<List<LocalizationTextList>>();

        }

    }
}


