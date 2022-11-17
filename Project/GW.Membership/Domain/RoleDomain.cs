using GW.Common;
using GW.Core;
using GW.Membership.Contracts;
using GW.Membership.Contracts.Domain;
using GW.Membership.Models;
using GW.Membership.Contracts.Data;
using GW.Helpers;

namespace GW.Membership.Domain
{
    public class RoleDomain : IRoleDomain
    {
        public RoleDomain(IContext context, IMembershipRepositorySet repositorySet)
        {
            Context = context;
            RepositorySet = repositorySet;  
        }

        public IContext Context { get; set; }

        public IMembershipRepositorySet RepositorySet { get; set; }

        public RoleModel Get(RoleParam param)
        {
            RoleModel ret = null;

            ret = RepositorySet.Role.Read(param); 
            
            return ret;
        }

        public List<RoleList> List(RoleParam param)
        {
            List<RoleList> ret = null;

            ret = RepositorySet.Role.List(param);           

            return ret;
        }

        public List<RoleSearchResult> Search(RoleParam param)
        {
            List<RoleSearchResult> ret = null;

            ret = RepositorySet.Role.Search(param);

            return ret;
        }

        public OperationStatus Set(RoleModel model, object userid)
        {
            OperationStatus ret = new OperationStatus(true);
            OPERATIONLOGENUM operation = OPERATIONLOGENUM.INSERT;

            ret = EntryValidation(model);

            if (ret.Status)
            {

                RoleModel old 
                    = RepositorySet.Role.Read(new RoleParam() { pRoleID = model.RoleID });

                if (old == null)
                {
                    ret = InsertValidation(model);

                    if (ret.Status)
                    {
                        model.CreateDate = DateTime.Now;
                        ret = RepositorySet.Role.Create(model);
                    }
                }
                else
                {
                    model.CreateDate = old.CreateDate;
                    operation = OPERATIONLOGENUM.UPDATE;

                    ret = UpdateValidation(model);

                    if (ret.Status)
                    {
                        ret = RepositorySet.Role.Update(model);
                    }

                }

                if (ret.Status && userid != null)
                {
                    RepositorySet.Role.Context
                        .RegisterDataLog(userid.ToString(), operation, "SYSROLE",
                        model.RoleID.ToString(), old, model);

                    ret.Returns = model;
                }

            }     

            return ret;
        }

        public void FillChields(ref RoleModel obj)
        {
            
        }

        public OperationStatus Delete(RoleModel model, object userid)
        {
            OperationStatus ret = new OperationStatus(true);

            RoleModel old 
                = RepositorySet.Role.Read(new RoleParam() { pRoleID = model.RoleID });

            if (old != null)
            {
                ret = DeleteValidation(model);

                if (ret.Status)
                {
                    ret = RepositorySet.Role.Delete(model);
                }
            }
            else
            {
                ret.Status = false;
                ret.Error = new System.Exception(GW.Localization.GetItem("Record-NotFound").Text);

            }           

            return ret;
        }


        public OperationStatus EntryValidation(RoleModel obj)
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
             
        public OperationStatus InsertValidation(RoleModel obj)
        {
            OperationStatus ret = new OperationStatus(true);
            RoleParam param = new RoleParam()
            {
                pRoleName = obj.RoleName
            };

            List<RoleList> list
                = RepositorySet.Role.List(param);

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
            
        public OperationStatus UpdateValidation(RoleModel obj)
        {
            OperationStatus ret = new OperationStatus(true);
            RoleParam param = new RoleParam() { pRoleName = obj.RoleName };
            List<RoleList> list
                = RepositorySet.Role.List(param);

            if (list != null)
            {
                if (list.Count > 0)
                {
                    if (list[0].RoleID != obj.RoleID)
                    {
                        ret.Status = false;
                        ret.Error = new Exception(GW.Localization.GetItem("Validation-Unique-Value").Text);
                    }
                }
            }

            Context.ExecutionStatus = ret;

            return ret;

        }

        public OperationStatus DeleteValidation(RoleModel obj)
        {
            return new OperationStatus(true); 
        }


    }
}
