using GW.Common;
using GW.Core;
using GW.Membership.Contracts;
using GW.Membership.Contracts.Domain;
using GW.Membership.Models;
using GW.Membership.Contracts.Data;
using GW.Helpers;
using Microsoft.SqlServer.Server;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection;

namespace GW.Membership.Domain
{
    public class LocalizationTextDomain : ILocalizationTextDomain
    {

        public LocalizationTextDomain(IContext context, IMembershipRepositorySet repositorySet)
        {
            Context = context;
            RepositorySet = repositorySet;

        }

        public IContext Context { get; set; }

        public IMembershipRepositorySet RepositorySet { get; set; }

        public async Task<LocalizationTextResult> FillChields(LocalizationTextResult obj)
        {
            return obj;
        }

        public async Task<LocalizationTextResult> Get(LocalizationTextParam param)
        {
            LocalizationTextResult ret = null;

            ret = await RepositorySet.LocalizationText.Read(param);

            return ret;
        }

        public async Task<List<LocalizationTextList>> List(LocalizationTextParam param)
        {
            List<LocalizationTextList> ret = null;

            ret = await RepositorySet.LocalizationText.List(param);

            return ret;
        }

        public async Task<List<LocalizationTextResult>> Search(LocalizationTextParam param)
        {
            List<LocalizationTextResult> ret = null;

            ret = await RepositorySet.LocalizationText.Search(param);

            return ret;
        }

        public async Task EntryValidation(LocalizationTextEntry obj)
        {
            OperationStatus ret = null;

            ret = PrimaryValidation.Execute(obj, new List<string>(), Context.LocalizationLanguage);

            if (!ret.Status)
            {
                ret.Error
                    = new Exception(GW.LocalizationText.Get("Validation-Error", Context.LocalizationLanguage).Text);
            }

            Context.ExecutionStatus = ret;

        }

        public async Task InsertValidation(LocalizationTextEntry obj)
        {
            OperationStatus ret = new OperationStatus(true);
            LocalizationTextParam param = null;
            List<LocalizationTextList> list = null;

            // verificar por code

            param = new LocalizationTextParam() { pCode = obj.Code };
            list  = await RepositorySet.LocalizationText.List(param);

            if (list != null)
            {
                if (list.Count > 0 )
                {
                    ret.Status = false;
                    string msg
                        = string.Format(GW.LocalizationText.Get("Validation-Unique-Value",
                        Context.LocalizationLanguage).Text, "Code");
                    ret.Error = new Exception(msg);
                    ret.AddInnerException("Code", msg);
                }
            }

            // verificar por name na mesma linguagem

            param = new LocalizationTextParam() { pName = obj.Name };
            list = await RepositorySet.LocalizationText.List(param);

            if (list != null)
            {
                if (list.Count > 0 && list[0].Language == obj.Language)
                {
                    ret.Status = false;
                    string msg
                        = string.Format(GW.LocalizationText.Get("Validation-Unique-Value",
                        Context.LocalizationLanguage).Text, "Name");
                    ret.Error = new Exception(msg);
                    ret.AddInnerException("Name", msg);
                }
            }


            Context.ExecutionStatus = ret;

        }

        public async Task UpdateValidation(LocalizationTextEntry obj)
        {
            OperationStatus ret = new OperationStatus(true);
            LocalizationTextParam param = null;
            List<LocalizationTextList> list = null;

            // verificar por code

            param = new LocalizationTextParam() { pCode = obj.Code };
            list = await RepositorySet.LocalizationText.List(param);

            if (list != null)
            {
                if (list.Count > 0)
                {
                    if (list[0].LocalizationTextID != obj.LocalizationTextID)
                    {
                        ret.Status = false;
                        string msg
                            = string.Format(GW.LocalizationText.Get("Validation-Unique-Value", 
                            Context.LocalizationLanguage).Text, "Code");
                        ret.Error = new Exception(msg);
                        ret.AddInnerException("Code", msg);
                    }
                }
            }


            // verificar por name na mesma linguagem

            param = new LocalizationTextParam() { pName = obj.Name };
            list = await RepositorySet.LocalizationText.List(param);

            if (list != null)
            {
                if (list.Count > 0 && list[0].LocalizationTextID != obj.LocalizationTextID 
                    && list[0].Language == obj.Language)
                {
                    ret.Status = false;
                    string msg
                        = string.Format(GW.LocalizationText.Get("Validation-Unique-Value",
                        Context.LocalizationLanguage).Text, "Name");
                    ret.Error = new Exception(msg);
                    ret.AddInnerException("Name", msg);
                }
            }

            Context.ExecutionStatus = ret;

        }

        public async Task DeleteValidation(LocalizationTextEntry obj)
        {
            Context.ExecutionStatus = new OperationStatus(true);
        }

        public async Task<LocalizationTextEntry> Set(LocalizationTextEntry model, object userid)
        {
            LocalizationTextEntry ret = null;
            OPERATIONLOGENUM operation = OPERATIONLOGENUM.INSERT;

            await EntryValidation(model);

            if (Context.ExecutionStatus.Status)
            {

                LocalizationTextResult old
                    = await RepositorySet.LocalizationText.Read(new LocalizationTextParam() { pLocalizationTextID = model.LocalizationTextID });

                if (old == null)
                {
                    await InsertValidation(model);

                    if (Context.ExecutionStatus.Status)
                    {                       
                        if (model.LocalizationTextID == 0) { model.LocalizationTextID = GW.Helpers.Utilities.GenerateId(); }
                        await RepositorySet.LocalizationText.Create(model);
                    }
                }
                else
                {                   
                    operation = OPERATIONLOGENUM.UPDATE;

                    await UpdateValidation(model);

                    if (Context.ExecutionStatus.Status)
                    {
                        await RepositorySet.LocalizationText.Update(model);
                    }

                }

                if (Context.ExecutionStatus.Status && userid != null)
                {
                    await RepositorySet.LocalizationText.Context
                        .RegisterDataLogAsync(userid.ToString(), operation, "SYSLOCALIZATIONTEXT",
                        model.LocalizationTextID.ToString(), old, model);

                    ret = model;
                }

            }

            return ret;
        }

        public async Task<LocalizationTextEntry> Delete(LocalizationTextEntry model, object userid)
        {
            LocalizationTextEntry ret = null;

            LocalizationTextResult old
                = await RepositorySet.LocalizationText.Read(new LocalizationTextParam() { pLocalizationTextID = model.LocalizationTextID });

            if (old != null)
            {
                await DeleteValidation(model);

                if (Context.ExecutionStatus.Status)
                {
                    await RepositorySet.LocalizationText.Delete(model);

                    if (Context.ExecutionStatus.Status && userid != null)
                    {
                        await RepositorySet.User.Context
                            .RegisterDataLogAsync(userid.ToString(), OPERATIONLOGENUM.DELETE, "SYSLOCALIZATIONTEXT",
                            model.LocalizationTextID.ToString(), old, model);

                        ret = model;
                    }

                }
            }
            else
            {
                Context.ExecutionStatus.Status = false;
                Context.ExecutionStatus.Error
                    = new System.Exception(GW.LocalizationText.Get("Record-NotFound", Context.LocalizationLanguage).Text);

            }

            return ret;
        }

        public async Task<List<LocalizationTextList>> GetListOfLanguages()
        {
            List<LocalizationTextList> ret = null;

            ret = await RepositorySet.LocalizationText.GetListOfLanguages();

            return ret;
        }


    }


    public class LocalizationServiceBase
    {

        public void FillTexts(List<LocalizationTextResult> textLists,
                string language)
        {
            Type t = this.GetType();
            PropertyInfo[] prop = t.GetProperties();

            string text = "";
            string auxname = "";

            foreach (PropertyInfo p in prop)
            {

                auxname = p.Name;
                auxname = auxname.Replace("_", "-");

                text = GetText(textLists, auxname, language);

                p.SetValue(this, text, null);

            }

        }

        private string GetText(List<LocalizationTextResult> textLists,
                string name, string lang)
        {
            string ret = "";

            var aux = textLists
                .Where(t => t.Name == name && t.Language == lang)
                .FirstOrDefault();

            if (aux != null)
            {
                ret = aux.Text;
            }

            return ret;

        }


        public string SearchButtonLabel { get; set; }

        public string SearchingLabel { get; set; }

        public string InsertingLoadingLabel { get; set; }

        public string SearchResultLabel { get; set; }

        public string DetailsLabel { get; set; }

        public string NoRecordsFound { get; set; }

        public string LoadingPage { get; set; }

        public string LoadingData { get; set; }

        public string ErrorOnExecuteSearch { get; set; }

        public string ErrorOnReturnData { get; set; }

        public string ErrorOnCreateNewRecord { get; set; }

        public string AfterSaveAnswering { get; set; }

        public string NoticeLabel { get; set; }

        public string SuccessLabel { get; set; }

    }

}
