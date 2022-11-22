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

        public async Task<ObjectPermissionModel> FillChields(ObjectPermissionModel obj)
        {
            return obj;
        }

        public async Task<ObjectPermissionModel> Get(ObjectPermissionParam param)
        {
            ObjectPermissionModel ret = null;

            ret = await RepositorySet.ObjectPermission.Read(param); 
            
            return ret;
        }

        public async Task<List<ObjectPermissionList>> List(ObjectPermissionParam param)
        {
            List<ObjectPermissionList> ret = null;

            ret = await RepositorySet.ObjectPermission.List(param);           

            return ret;
        }

        public async Task<List<ObjectPermissionSearchResult>> Search(ObjectPermissionParam param)
        {
            List<ObjectPermissionSearchResult> ret = null;

            ret = await RepositorySet.ObjectPermission.Search(param);

            return ret;
        }

        public async Task EntryValidation(ObjectPermissionModel obj)
        {
            OperationStatus ret = null;

            ret = PrimaryValidation.Execute(obj, new List<string>());

            if (!ret.Status)
            {
                ret.Error = new Exception(GW.Localization.GetItem("Validation-Error").Text);
            }

            Context.ExecutionStatus = ret;
         
        }

        public async Task InsertValidation(ObjectPermissionModel obj)
        {
            OperationStatus ret = new OperationStatus(true);
            ObjectPermissionParam param = new ObjectPermissionParam()
            {
                pObjectCode = obj.ObjectCode
            };

            List<ObjectPermissionList> list
                = await RepositorySet.ObjectPermission.List(param);

            if (list != null)
            {
                if (list.Count > 0)
                {
                    ret.Status = false;
                    ret.Error = new Exception(GW.Localization.GetItem("Validation-Unique-Value").Text);
                }
            }

            Context.ExecutionStatus = ret;
           
        }

        public async Task UpdateValidation(ObjectPermissionModel obj)
        {
            OperationStatus ret = new OperationStatus(true);
            ObjectPermissionParam param = new ObjectPermissionParam() { pObjectCode = obj.ObjectCode };
           
            List<ObjectPermissionList> list
                = await RepositorySet.ObjectPermission.List(param);

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

        }

        public async Task DeleteValidation(ObjectPermissionModel obj)
        {
            Context.ExecutionStatus = new OperationStatus(true);
        }

        public async Task<ObjectPermissionModel> Set(ObjectPermissionModel model, object userid)
        {
            ObjectPermissionModel ret = null;
            OPERATIONLOGENUM operation = OPERATIONLOGENUM.INSERT;

           await EntryValidation(model);

            if (Context.ExecutionStatus.Status)
            {

                ObjectPermissionModel old 
                    = await RepositorySet.ObjectPermission
                        .Read(new ObjectPermissionParam() { pObjectPermissionID = model.ObjectPermissionID });

                if (old == null)
                {
                   await InsertValidation(model);

                    if (Context.ExecutionStatus.Status)
                    {
                        if (model.ObjectPermissionID == 0) { model.ObjectPermissionID = GW.Helpers.Utilities.GenerateId(); }
                        await RepositorySet.ObjectPermission.Create(model);
                    }
                }
                else
                {                    
                    operation = OPERATIONLOGENUM.UPDATE;

                   await UpdateValidation(model);

                    if (Context.ExecutionStatus.Status)
                    {
                         await RepositorySet.ObjectPermission.Update(model);
                    }

                }

                if (Context.ExecutionStatus.Status && userid != null)
                {
                    RepositorySet.ObjectPermission.Context
                        .RegisterDataLog(userid.ToString(), operation, "SYSOBJECTPERMISSION",
                        model.ObjectPermissionID.ToString(), old, model);

                    ret = model;
                }

            }     

            return ret;
        }
      
        public async Task<ObjectPermissionModel> Delete(ObjectPermissionModel model, object userid)
        {
            ObjectPermissionModel ret = null;

            ObjectPermissionModel old 
                = await RepositorySet.ObjectPermission
                    .Read(new ObjectPermissionParam() { pObjectPermissionID = model.ObjectPermissionID });

            if (old != null)
            {
                await DeleteValidation(model);

                if (Context.ExecutionStatus.Status)
                {
                     await RepositorySet.ObjectPermission.Delete(model);
                    ret = model;
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
