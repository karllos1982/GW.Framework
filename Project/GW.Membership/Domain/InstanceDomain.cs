using GW.Common;
using GW.Core;
using GW.Membership.Contracts;
using GW.Membership.Contracts.Domain;
using GW.Membership.Models;
using GW.Membership.Contracts.Data;
using GW.Helpers;

namespace GW.Membership.Domain
{
    public class InstanceDomain : IInstanceDomain
    {
        public InstanceDomain(IContext context, IMembershipRepositorySet repositorySet)
        {
            Context = context;
            RepositorySet = repositorySet;  
        }

        public IContext Context { get; set; }

        public IMembershipRepositorySet RepositorySet { get; set; }

        public InstanceModel Get(InstanceParam param)
        {
            InstanceModel ret = null;

            ret = RepositorySet.Instance.Read(param); 
            
            return ret;
        }

        public List<InstanceList> List(InstanceParam param)
        {
            List<InstanceList> ret = null;

            ret = RepositorySet.Instance.List(param);           

            return ret;
        }

        public List<InstanceSearchResult> Search(InstanceParam param)
        {
            List<InstanceSearchResult> ret = null;

            ret = RepositorySet.Instance.Search(param);

            return ret;
        }

        public OperationStatus Set(InstanceModel model, object userid)
        {
            OperationStatus ret = new OperationStatus(true);
            OPERATIONLOGENUM operation = OPERATIONLOGENUM.INSERT;

            ret = EntryValidation(model);

            if (ret.Status)
            {

                InstanceModel old 
                    = RepositorySet.Instance.Read(new InstanceParam() { pInstanceID = model.InstanceID });

                if (old == null)
                {
                    ret = InsertValidation(model);

                    if (ret.Status)
                    {
                        model.CreateDate = DateTime.Now;
                        ret = RepositorySet.Instance.Create(model);
                    }
                }
                else
                {
                    model.CreateDate = old.CreateDate;
                    operation = OPERATIONLOGENUM.UPDATE;

                    ret = UpdateValidation(model);

                    if (ret.Status)
                    {
                        ret = RepositorySet.Instance.Update(model);
                    }

                }

                if (ret.Status && userid != null)
                {
                    //RepositorySet.Instance.Context.
                    //    .RegisterDataLog(userid.ToString(), operation, "SYSINSTANCE",
                    //  obj.InstanceID.ToString(), old, obj);

                    ret.Returns = model;
                }

            }     

            return ret;
        }

        public void FillChields(ref InstanceModel obj)
        {
            
        }

        public OperationStatus Delete(InstanceModel model, object userid)
        {
            OperationStatus ret = new OperationStatus(true);

            InstanceModel old 
                = RepositorySet.Instance.Read(new InstanceParam() { pInstanceID = model.InstanceID });

            if (old != null)
            {
                ret = DeleteValidation(model);

                if (ret.Status)
                {
                    ret = RepositorySet.Instance.Delete(model);
                }
            }
            else
            {
                ret.Status = false;
                ret.Error = new System.Exception(GW.Localization.GetItem("Record-NotFound").Text);

            }           

            return ret;
        }


        public OperationStatus EntryValidation(InstanceModel obj)
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
             
        public OperationStatus InsertValidation(InstanceModel obj)
        {
            OperationStatus ret = new OperationStatus(true);
            InstanceParam param = new InstanceParam()
            {
                pInstanceName = obj.InstanceName
            };

            List<InstanceList> list
                = RepositorySet.Instance.List(param);

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
            
        public OperationStatus UpdateValidation(InstanceModel obj)
        {
            OperationStatus ret = new OperationStatus(true);
            InstanceParam param = new InstanceParam() { pInstanceName = obj.InstanceName };
            List<InstanceList> list
                = RepositorySet.Instance.List(param);

            if (list != null)
            {
                if (list.Count > 0)
                {
                    if (list[0].InstanceID != obj.InstanceID)
                    {
                        ret.Status = false;
                        ret.Error = new Exception(GW.Localization.GetItem("Validation-Unique-Value").Text);
                    }
                }
            }

            Context.ExecutionStatus = ret;

            return ret;

        }

        public OperationStatus DeleteValidation(InstanceModel obj)
        {
            return new OperationStatus(true); 
        }


    }
}
