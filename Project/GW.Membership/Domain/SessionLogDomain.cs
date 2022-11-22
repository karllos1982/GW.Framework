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

        public async Task<SessionLogModel> FillChields(SessionLogModel obj)
        {
            return obj;
        }

        public async Task<SessionLogModel> Get(SessionLogParam param)
        {
            SessionLogModel ret = null;

            ret = await RepositorySet.SessionLog.Read(param); 
            
            return ret;
        }

        public async Task<List<SessionLogList>> List(SessionLogParam param)
        {
            List<SessionLogList> ret = null;

            ret = await RepositorySet.SessionLog.List(param);           

            return ret;
        }

        public async Task<List<SessionLogSearchResult>> Search(SessionLogParam param)
        {
            List<SessionLogSearchResult> ret = null;

            ret = await  RepositorySet.SessionLog.Search(param);

            return ret;
        }

        public async Task EntryValidation(SessionLogModel obj)
        {
            OperationStatus ret = null;

            ret = PrimaryValidation.Execute(obj, new List<string>());

            if (!ret.Status)
            {
                ret.Error = new Exception(GW.Localization.GetItem("Validation-Error").Text);
            }

            Context.ExecutionStatus = ret;
        
        }

        public async Task InsertValidation(SessionLogModel obj)
        {
            Context.ExecutionStatus = new OperationStatus(true);
        }

        public async Task UpdateValidation(SessionLogModel obj)
        {
            Context.ExecutionStatus = new OperationStatus(true);

        }

        public async Task DeleteValidation(SessionLogModel obj)
        {
            Context.ExecutionStatus = new OperationStatus(true);
        }

        public async Task<SessionLogModel> Set(SessionLogModel model, object userid)
        {
            SessionLogModel ret = null;
            OPERATIONLOGENUM operation = OPERATIONLOGENUM.INSERT;

             await EntryValidation(model);

            if (Context.ExecutionStatus.Status)
            {

                SessionLogModel old 
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
                    RepositorySet.SessionLog.Context
                        .RegisterDataLog(userid.ToString(), operation, "SYSSESSIONLOG",
                        model.SessionID.ToString(), old, model);

                    ret= model;
                }

            }     

            return ret;
        }
      
        public async Task<SessionLogModel> Delete(SessionLogModel model, object userid)
        {
            SessionLogModel ret = null;

            SessionLogModel old 
                = await RepositorySet.SessionLog.Read(new SessionLogParam() 
                    { pSessionID = model.SessionID });

            if (old != null)
            {
                await DeleteValidation(model);

                if (Context.ExecutionStatus.Status)
                {
                    await RepositorySet.SessionLog.Delete(model);
                }
            }
            else
            {
                Context.ExecutionStatus.Status = false;
                Context.ExecutionStatus.Error = new System.Exception(GW.Localization.GetItem("Record-NotFound").Text);

            }           

            return ret;
        }

        

    }
}
