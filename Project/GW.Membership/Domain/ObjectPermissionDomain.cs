using GW.Common;
using GW.Core;
using GW.Membership.Contracts;
using GW.Membership.Contracts.Domain;
using GW.Membership.Models;
using GW.Membership.Contracts.Data;
using GW.Helpers;

namespace GW.Membership.Domain
{
    public class ObjectPermissionDomain : IObjectPermissionDomain
    {
        public ObjectPermissionDomain(IContext context, IMembershipRepositorySet repositorySet)
        {
            Context = context;
            RepositorySet = repositorySet;  
        }

        public IContext Context { get; set; }

        public IMembershipRepositorySet RepositorySet { get; set; }

        public ObjectPermissionModel Get(ObjectPermissionParam param)
        {
            ObjectPermissionModel ret = null;

            ret = RepositorySet.ObjectPermission.Read(param); 
            
            return ret;
        }

        public List<ObjectPermissionList> List(ObjectPermissionParam param)
        {
            List<ObjectPermissionList> ret = null;

            ret = RepositorySet.ObjectPermission.List(param);           

            return ret;
        }

        public List<ObjectPermissionSearchResult> Search(ObjectPermissionParam param)
        {
            List<ObjectPermissionSearchResult> ret = null;

            ret = RepositorySet.ObjectPermission.Search(param);

            return ret;
        }

        public OperationStatus Set(ObjectPermissionModel model, object userid)
        {
            OperationStatus ret = new OperationStatus(true);
            OPERATIONLOGENUM operation = OPERATIONLOGENUM.INSERT;

            ret = EntryValidation(model);

            if (ret.Status)
            {

                ObjectPermissionModel old 
                    = RepositorySet.ObjectPermission.Read(new ObjectPermissionParam() { pObjectPermissionID = model.ObjectPermissionID });

                if (old == null)
                {
                    ret = InsertValidation(model);

                    if (ret.Status)
                    {                        
                        ret = RepositorySet.ObjectPermission.Create(model);
                    }
                }
                else
                {                    
                    operation = OPERATIONLOGENUM.UPDATE;

                    ret = UpdateValidation(model);

                    if (ret.Status)
                    {
                        ret = RepositorySet.ObjectPermission.Update(model);
                    }

                }

                if (ret.Status && userid != null)
                {
                    RepositorySet.ObjectPermission.Context
                        .RegisterDataLog(userid.ToString(), operation, "SYSOBJECTPERMISSION",
                        model.ObjectPermissionID.ToString(), old, model);

                    ret.Returns = model;
                }

            }     

            return ret;
        }

        public void FillChields(ref ObjectPermissionModel obj)
        {
            
        }

        public OperationStatus Delete(ObjectPermissionModel model, object userid)
        {
            OperationStatus ret = new OperationStatus(true);

            ObjectPermissionModel old 
                = RepositorySet.ObjectPermission.Read(new ObjectPermissionParam() { pObjectPermissionID = model.ObjectPermissionID });

            if (old != null)
            {
                ret = DeleteValidation(model);

                if (ret.Status)
                {
                    ret = RepositorySet.ObjectPermission.Delete(model);
                }
            }
            else
            {
                ret.Status = false;
                ret.Error = new System.Exception(GW.Localization.GetItem("Record-NotFound").Text);

            }           

            return ret;
        }


        public OperationStatus EntryValidation(ObjectPermissionModel obj)
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
             
        public OperationStatus InsertValidation(ObjectPermissionModel obj)
        {
            OperationStatus ret = new OperationStatus(true);
            ObjectPermissionParam param = new ObjectPermissionParam()
            {
                pObjectCode = obj.ObjectCode
            };

            List<ObjectPermissionList> list
                = RepositorySet.ObjectPermission.List(param);

            if (list != null)
            {
                if (list.Count > 0)
                {
                    ret.Status = false;
                    ret.Error = new Exception(GW.Localization.GetItem("Validation-Unique-Value").Text);
                }
            }

            Context.ExecutionStatus = ret;

            return ret;
        }
            
        public OperationStatus UpdateValidation(ObjectPermissionModel obj)
        {
            OperationStatus ret = new OperationStatus(true);
            ObjectPermissionParam param = new ObjectPermissionParam() { pObjectCode = obj.ObjectCode };
            List<ObjectPermissionList> list
                = RepositorySet.ObjectPermission.List(param);

            if (list != null)
            {
                if (list.Count > 0)
                {
                    if (list[0].ObjectPermissionID != obj.ObjectPermissionID)
                    {
                        ret.Status = false;
                        ret.Error = new Exception(GW.Localization.GetItem("Validation-Unique-Value").Text);
                    }
                }
            }

            Context.ExecutionStatus = ret;

            return ret;

        }

        public OperationStatus DeleteValidation(ObjectPermissionModel obj)
        {
            return new OperationStatus(true); 
        }


    }
}
