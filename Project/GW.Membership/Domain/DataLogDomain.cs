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
        public DataLogDomain(IContext context, IMembershipRepositorySet repositorySet)
        {
            Context = context;
            RepositorySet = repositorySet;  
        }

        public IContext Context { get; set; }

        public IMembershipRepositorySet RepositorySet { get; set; }

        public DataLogModel Get(DataLogParam param)
        {
            DataLogModel ret = null;

            ret = RepositorySet.DataLog.Read(param); 
            
            return ret;
        }

        public List<DataLogList> List(DataLogParam param)
        {
            List<DataLogList> ret = null;

            ret = RepositorySet.DataLog.List(param);           

            return ret;
        }

        public List<DataLogSearchResult> Search(DataLogParam param)
        {
            List<DataLogSearchResult> ret = null;

            ret = RepositorySet.DataLog.Search(param);

            return ret;
        }

        public OperationStatus Set(DataLogModel model, object userid)
        {
            OperationStatus ret = new OperationStatus(true);
            OPERATIONLOGENUM operation = OPERATIONLOGENUM.INSERT;

            ret = EntryValidation(model);

            if (ret.Status)
            {

                DataLogModel old 
                    = RepositorySet.DataLog.Read(new DataLogParam() { pDataLogID = model.DataLogID });

                if (old == null)
                {
                    ret = InsertValidation(model);

                    if (ret.Status)
                    {                        
                        ret = RepositorySet.DataLog.Create(model);
                    }
                }
                else
                {                    
                    operation = OPERATIONLOGENUM.UPDATE;

                    ret = UpdateValidation(model);

                    if (ret.Status)
                    {
                        ret = RepositorySet.DataLog.Update(model);
                    }

                }

                if (ret.Status && userid != null)
                {
                    RepositorySet.DataLog.Context
                        .RegisterDataLog(userid.ToString(), operation, "SYSDATALOG",
                        model.DataLogID.ToString(), old, model);

                    ret.Returns = model;
                }

            }     

            return ret;
        }

        public void FillChields(ref DataLogModel obj)
        {
            
        }

        public OperationStatus Delete(DataLogModel model, object userid)
        {
            OperationStatus ret = new OperationStatus(true);

            DataLogModel old 
                = RepositorySet.DataLog.Read(new DataLogParam() { pDataLogID = model.DataLogID });

            if (old != null)
            {
                ret = DeleteValidation(model);

                if (ret.Status)
                {
                    ret = RepositorySet.DataLog.Delete(model);
                }
            }
            else
            {
                ret.Status = false;
                ret.Error = new System.Exception(GW.Localization.GetItem("Record-NotFound").Text);

            }           

            return ret;
        }


        public OperationStatus EntryValidation(DataLogModel obj)
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
             
        public OperationStatus InsertValidation(DataLogModel obj)
        {
            OperationStatus ret = new OperationStatus(true);
            

            return ret;
        }
            
        public OperationStatus UpdateValidation(DataLogModel obj)
        {
            OperationStatus ret = new OperationStatus(true);
           

            return ret;

        }

        public OperationStatus DeleteValidation(DataLogModel obj)
        {
            return new OperationStatus(true); 
        }


    }
}
