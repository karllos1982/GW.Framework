using GW.Common;
using GW.Core;
using GW.Membership.Contracts;
using GW.Membership.Contracts.Domain;
using GW.Membership.Models;
using GW.Membership.Contracts.Data;
using GW.Helpers;

namespace GW.Membership.Domain
{
    public class SessionLogDomain : ISessionLogDomain
    {
        
        public SessionLogDomain(IContext context, IMembershipRepositorySet repositorySet)
        {
            Context = context;
            RepositorySet = repositorySet;
         
        }

        public IContext Context { get; set; }

        public IMembershipRepositorySet RepositorySet { get; set; }

        public async Task<SessionLogResult> FillChields(SessionLogResult obj)
        {
            return obj;
        }

        public async Task<SessionLogResult> Get(SessionLogParam param)
        {
            SessionLogResult ret = null;

            ret = await RepositorySet.SessionLog.Read(param); 
            
            return ret;
        }

        public async Task<List<SessionLogList>> List(SessionLogParam param)
        {
            List<SessionLogList> ret = null;

            ret = await RepositorySet.SessionLog.List(param);           

            return ret;
        }

        public async Task<List<SessionLogResult>> Search(SessionLogParam param)
        {
            List<SessionLogResult> ret = null;

            ret = await  RepositorySet.SessionLog.Search(param);

            return ret;
        }

        public async Task EntryValidation(SessionLogEntry obj)
        {
            OperationStatus ret = null;

            ret = PrimaryValidation.Execute(obj, new List<string>(), Context.LocalizationLanguage);

            if (!ret.Status)
            {
                ret.Error 
                    = new Exception(GW.Localization.GetItem("Validation-Error", Context.LocalizationLanguage).Text);
            }

            Context.ExecutionStatus = ret;
        
        }

        public async Task InsertValidation(SessionLogEntry obj)
        {
            Context.ExecutionStatus = new OperationStatus(true);
        }

        public async Task UpdateValidation(SessionLogEntry obj)
        {
            Context.ExecutionStatus = new OperationStatus(true);

        }

        public async Task DeleteValidation(SessionLogEntry obj)
        {
            Context.ExecutionStatus = new OperationStatus(true);
        }

        public async Task<SessionLogEntry> Set(SessionLogEntry model, object userid)
        {
            SessionLogEntry ret = null;
            OPERATIONLOGENUM operation = OPERATIONLOGENUM.INSERT;

             await EntryValidation(model);

            if (Context.ExecutionStatus.Status)
            {

                SessionLogResult old 
                    = await RepositorySet.SessionLog.Read(new SessionLogParam() 
                            { pSessionID = model.SessionID });

                if (old == null)
                {
                    await InsertValidation(model);

                    if (Context.ExecutionStatus.Status)
                    {
                        if (model.SessionID == 0) { model.SessionID = GW.Helpers.Utilities.GenerateId(); }
                        await RepositorySet.SessionLog.Create(model);
                    }
                }
                else
                {                    
                    operation = OPERATIONLOGENUM.UPDATE;

                   await UpdateValidation(model);

                    if (Context.ExecutionStatus.Status)
                    {
                       await RepositorySet.SessionLog.Update(model);
                    }

                }

                if (Context.ExecutionStatus.Status && userid != null)
                {
                   await  RepositorySet.SessionLog.Context
                        .RegisterDataLogAsync(userid.ToString(), operation, "SYSSESSIONLOG",
                        model.SessionID.ToString(), old, model);

                    ret= model;
                }

            }     

            return ret;
        }
      
        public async Task<SessionLogEntry> Delete(SessionLogEntry model, object userid)
        {
            SessionLogEntry ret = null;

            SessionLogResult old 
                = await RepositorySet.SessionLog.Read(new SessionLogParam() 
                    { pSessionID = model.SessionID });

            if (old != null)
            {
                await DeleteValidation(model);

                if (Context.ExecutionStatus.Status)
                {
                    await RepositorySet.SessionLog.Delete(model);

                    if (Context.ExecutionStatus.Status && userid != null)
                    {
                        await RepositorySet.User.Context
                            .RegisterDataLogAsync(userid.ToString(), OPERATIONLOGENUM.DELETE, "SYSSESSIONLOG",
                            model.SessionID.ToString(), old, model);

                        ret = model;
                    }
                }
            }
            else
            {
                Context.ExecutionStatus.Status = false;
                Context.ExecutionStatus.Error 
                    = new System.Exception(GW.Localization.GetItem("Record-NotFound",Context.LocalizationLanguage).Text);

            }           

            return ret;
        }

        

    }
}
