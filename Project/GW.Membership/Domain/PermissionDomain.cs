using GW.Common;
using GW.Core;
using GW.Membership.Contracts;
using GW.Membership.Contracts.Domain;
using GW.Membership.Models;
using GW.Membership.Contracts.Data;
using GW.Helpers;

namespace GW.Membership.Domain
{
    public class PermissionDomain : IPermissionDomain
    {
        public PermissionDomain(IContext context, IMembershipRepositorySet repositorySet)
        {
            Context = context;
            RepositorySet = repositorySet;  
        }

        public IContext Context { get; set; }

        public IMembershipRepositorySet RepositorySet { get; set; }

        public PermissionModel Get(PermissionParam param)
        {
            PermissionModel ret = null;

            ret = RepositorySet.Permission.Read(param); 
            
            return ret;
        }

        public List<PermissionList> List(PermissionParam param)
        {
            List<PermissionList> ret = null;

            ret = RepositorySet.Permission.List(param);           

            return ret;
        }

        public List<PermissionSearchResult> Search(PermissionParam param)
        {
            List<PermissionSearchResult> ret = null;

            ret = RepositorySet.Permission.Search(param);

            return ret;
        }

        public OperationStatus Set(PermissionModel model, object userid)
        {
            OperationStatus ret = new OperationStatus(true);
            OPERATIONLOGENUM operation = OPERATIONLOGENUM.INSERT;

            ret = EntryValidation(model);

            if (ret.Status)
            {

                PermissionModel old 
                    = RepositorySet.Permission.Read(new PermissionParam() { pPermissionID = model.PermissionID });

                if (old == null)
                {
                    ret = InsertValidation(model);

                    if (ret.Status)
                    {                       
                        ret = RepositorySet.Permission.Create(model);
                    }
                }
                else
                {                    
                    operation = OPERATIONLOGENUM.UPDATE;

                    ret = UpdateValidation(model);

                    if (ret.Status)
                    {
                        ret = RepositorySet.Permission.Update(model);
                    }

                }

                if (ret.Status && userid != null)
                {
                    RepositorySet.Permission.Context
                        .RegisterDataLog(userid.ToString(), operation, "SYSPERMISSION",
                        model.PermissionID.ToString(), old, model);

                    ret.Returns = model;
                }

            }     

            return ret;
        }

        public void FillChields(ref PermissionModel obj)
        {
            
        }

        public OperationStatus Delete(PermissionModel model, object userid)
        {
            OperationStatus ret = new OperationStatus(true);

            PermissionModel old 
                = RepositorySet.Permission.Read(new PermissionParam() { pPermissionID = model.PermissionID });

            if (old != null)
            {
                ret = DeleteValidation(model);

                if (ret.Status)
                {
                    ret = RepositorySet.Permission.Delete(model);
                }
            }
            else
            {
                ret.Status = false;
                ret.Error = new System.Exception(GW.Localization.GetItem("Record-NotFound").Text);

            }           

            return ret;
        }


        public OperationStatus EntryValidation(PermissionModel obj)
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
             
        public OperationStatus InsertValidation(PermissionModel obj)
        {
            OperationStatus ret = new OperationStatus(true);
           
            Context.ExecutionStatus = ret;

            return ret;
        }
            
        public OperationStatus UpdateValidation(PermissionModel obj)
        {
            OperationStatus ret = new OperationStatus(true);
           
            Context.ExecutionStatus = ret;

            return ret;

        }

        public OperationStatus DeleteValidation(PermissionModel obj)
        {
            return new OperationStatus(true); 
        }


    }
}
