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

        public SessionLogModel Get(SessionLogParam param)
        {
            SessionLogModel ret = null;

            ret = RepositorySet.SessionLog.Read(param); 
            
            return ret;
        }

        public List<SessionLogList> List(SessionLogParam param)
        {
            List<SessionLogList> ret = null;

            ret = RepositorySet.SessionLog.List(param);           

            return ret;
        }

        public List<SessionLogSearchResult> Search(SessionLogParam param)
        {
            List<SessionLogSearchResult> ret = null;

            ret = RepositorySet.SessionLog.Search(param);

            return ret;
        }

        public OperationStatus Set(SessionLogModel model, object userid)
        {
            OperationStatus ret = new OperationStatus(true);
            OPERATIONLOGENUM operation = OPERATIONLOGENUM.INSERT;

            ret = EntryValidation(model);

            if (ret.Status)
            {

                SessionLogModel old 
                    = RepositorySet.SessionLog.Read(new SessionLogParam() 
                    { pSessionID = model.SessionID });

                if (old == null)
                {
                    ret = InsertValidation(model);

                    if (ret.Status)
                    {                        
                        ret = RepositorySet.SessionLog.Create(model);
                    }
                }
                else
                {                    
                    operation = OPERATIONLOGENUM.UPDATE;

                    ret = UpdateValidation(model);

                    if (ret.Status)
                    {
                        ret = RepositorySet.SessionLog.Update(model);
                    }

                }

                if (ret.Status && userid != null)
                {
                    RepositorySet.SessionLog.Context
                        .RegisterDataLog(userid.ToString(), operation, "SYSSESSIONLOG",
                        model.SessionID.ToString(), old, model);

                    ret.Returns = model;
                }

            }     

            return ret;
        }

        public void FillChields(ref SessionLogModel obj)
        {
            
        }

        public OperationStatus Delete(SessionLogModel model, object userid)
        {
            OperationStatus ret = new OperationStatus(true);

            SessionLogModel old 
                = RepositorySet.SessionLog.Read(new SessionLogParam() 
                { pSessionID = model.SessionID });

            if (old != null)
            {
                ret = DeleteValidation(model);

                if (ret.Status)
                {
                    ret = RepositorySet.SessionLog.Delete(model);
                }
            }
            else
            {
                ret.Status = false;
                ret.Error = new System.Exception(GW.Localization.GetItem("Record-NotFound").Text);

            }           

            return ret;
        }


        public OperationStatus EntryValidation(SessionLogModel obj)
        {
            OperationStatus ret = null;

            ret = PrimaryValidation.Execute(obj, new List<string>());

            if (!ret.Status)
            {
                ret.Error = new Exception(GW.Localization.GetItem("Validation-Error").Text);
            }

            Context.ExecutionStatus = ret; 

            return ret;
        }           
             
        public OperationStatus InsertValidation(SessionLogModel obj)
        {
            OperationStatus ret = new OperationStatus(true);
            
            Context.ExecutionStatus = ret;

            return ret;
        }
            
        public OperationStatus UpdateValidation(SessionLogModel obj)
        {
            OperationStatus ret = new OperationStatus(true);
        
            Context.ExecutionStatus = ret;

            return ret;

        }

        public OperationStatus DeleteValidation(SessionLogModel obj)
        {
            return new OperationStatus(true); 
        }


    }
}
