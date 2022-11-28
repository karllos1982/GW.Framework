using GW.Common;
using GW.Core;
using GW.Membership.Contracts;
using GW.Membership.Contracts.Domain;
using GW.Membership.Models;
using GW.Membership.Contracts.Data;
using GW.Helpers;

namespace GW.Membership.Domain
{
    public class DataLogDomain : IDataLogDomain
    {
        private string lang = "";

        public DataLogDomain(IContext context, IMembershipRepositorySet repositorySet)
        {
            Context = context;
            RepositorySet = repositorySet;
            lang = Context.Settings.LocalizationLanguage;
        }

        public IContext Context { get; set; }

        public IMembershipRepositorySet RepositorySet { get; set; }

        public async Task<DataLogResult> FillChields(DataLogResult obj)
        {
            return obj;
        }

        public async Task<DataLogResult> Get(DataLogParam param)
        {
            DataLogResult ret = null;

            ret = await RepositorySet.DataLog.Read(param); 
            
            return ret;
        }

        public async Task<List<DataLogList>> List(DataLogParam param)
        {
            List<DataLogList> ret = null;

            ret = await RepositorySet.DataLog.List(param);           

            return ret;
        }

        public async Task<List<DataLogResult>> Search(DataLogParam param)
        {
            List<DataLogResult> ret = null;

            ret = await RepositorySet.DataLog.Search(param);

            return ret;
        }

        public async Task EntryValidation(DataLogEntry obj)
        {
            OperationStatus ret = null;

            ret = PrimaryValidation.Execute(obj, new List<string>());

            if (!ret.Status)
            {
                ret.Error 
                    = new Exception(GW.Localization.GetItem("Validation-Error",lang).Text);
            }

            Context.ExecutionStatus = ret;
            
        }

        public async Task InsertValidation(DataLogEntry obj)
        {
            Context.ExecutionStatus = new OperationStatus(true); 
        }

        public async Task UpdateValidation(DataLogEntry obj)
        {
            Context.ExecutionStatus = new OperationStatus(true);

        }

        public async Task DeleteValidation(DataLogEntry obj)
        {
            Context.ExecutionStatus = new OperationStatus(true);
        }

        public async Task<DataLogEntry> Set(DataLogEntry model, object userid)
        {
            DataLogEntry ret = null;

            OPERATIONLOGENUM operation = OPERATIONLOGENUM.INSERT;

            await EntryValidation(model);

            if (Context.ExecutionStatus.Status)
            {

                DataLogResult old 
                    = await RepositorySet.DataLog.Read(new DataLogParam() { pDataLogID = model.DataLogID });

                if (old == null)
                {
                    await InsertValidation(model); 
                    if (Context.ExecutionStatus.Status)
                    {           
                        if (model.DataLogID ==0 ) { model.DataLogID = GW.Helpers.Utilities.GenerateId(); }
                        await RepositorySet.DataLog.Create(model);
                    }
                }
                else
                {                    
                    operation = OPERATIONLOGENUM.UPDATE;

                     await UpdateValidation(model);

                    if (Context.ExecutionStatus.Status)
                    {
                         await RepositorySet.DataLog.Update(model);
                    }

                }

                if (Context.ExecutionStatus.Status && userid != null)
                {
                   await  RepositorySet.DataLog.Context
                        .RegisterDataLogAsync(userid.ToString(), operation, "SYSDATALOG",
                        model.DataLogID.ToString(), old, model);

                    ret = model;
                }

            }
            
            return ret;
        }

      

        public async Task<DataLogEntry> Delete(DataLogEntry model, object userid)
        {
            DataLogEntry ret = null;

            DataLogResult old 
                = await RepositorySet.DataLog.Read(new DataLogParam() { pDataLogID = model.DataLogID });

            if (old != null)
            {
               await DeleteValidation(model);

                if (Context.ExecutionStatus.Status)
                {
                   await RepositorySet.DataLog.Delete(model);

                    if (Context.ExecutionStatus.Status && userid != null)
                    {
                        await RepositorySet.User.Context
                            .RegisterDataLogAsync(userid.ToString(), OPERATIONLOGENUM.DELETE, "SYSDATALOG",
                            model.DataLogID.ToString(), old, model);

                        ret = model;
                    }
                }
            }
            else
            {
                Context.ExecutionStatus.Status = false;
                Context.ExecutionStatus.Error 
                    = new System.Exception(GW.Localization.GetItem("Record-NotFound",lang).Text);

            }
           
            return ret;
        }


      

        public async Task<List<DataLogTimelineModel>> GetTimeLine(Int64 recordID)
        {
            List<DataLogTimelineModel> ret = null;

            ret = await RepositorySet.DataLog.GetDataLogTimeline(recordID);    

            return ret;
        }

        public async Task<List<TabelasValueModel>> GetTableList()
        {
            List<TabelasValueModel> ret = null;

            ret =await  RepositorySet.DataLog.GetTableList();


            return ret;
        }

    }
}
