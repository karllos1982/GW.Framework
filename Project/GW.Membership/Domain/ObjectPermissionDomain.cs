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

        private string lang = "";

        public ObjectPermissionDomain(IContext context, IMembershipRepositorySet repositorySet)
        {
            Context = context;
            RepositorySet = repositorySet;
            lang = Context.Settings.LocalizationLanguage;
        }

        public IContext Context { get; set; }

        public IMembershipRepositorySet RepositorySet { get; set; }

        public async Task<ObjectPermissionResult> FillChields(ObjectPermissionResult obj)
        {
            return obj;
        }

        public async Task<ObjectPermissionResult> Get(ObjectPermissionParam param)
        {
            ObjectPermissionResult ret = null;

            ret = await RepositorySet.ObjectPermission.Read(param); 
            
            return ret;
        }

        public async Task<List<ObjectPermissionList>> List(ObjectPermissionParam param)
        {
            List<ObjectPermissionList> ret = null;

            ret = await RepositorySet.ObjectPermission.List(param);           

            return ret;
        }

        public async Task<List<ObjectPermissionResult>> Search(ObjectPermissionParam param)
        {
            List<ObjectPermissionResult> ret = null;

            ret = await RepositorySet.ObjectPermission.Search(param);

            return ret;
        }

        public async Task EntryValidation(ObjectPermissionEntry obj)
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

        public async Task InsertValidation(ObjectPermissionEntry obj)
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
                    string msg 
                        = string.Format(GW.Localization.GetItem("Validation-Unique-Value",lang).Text, "Object Code");
                    ret.Error = new Exception(msg);
                  
                }
            }

            Context.ExecutionStatus = ret;
           
        }

        public async Task UpdateValidation(ObjectPermissionEntry obj)
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
                        string msg 
                            = string.Format(GW.Localization.GetItem("Validation-Unique-Value",lang).Text, "Object Code");
                        ret.Error = new Exception(msg);
                    }
                }
            }

            Context.ExecutionStatus = ret;    

        }

        public async Task DeleteValidation(ObjectPermissionEntry obj)
        {
            Context.ExecutionStatus = new OperationStatus(true);
        }

        public async Task<ObjectPermissionEntry> Set(ObjectPermissionEntry model, object userid)
        {
            ObjectPermissionEntry ret = null;
            OPERATIONLOGENUM operation = OPERATIONLOGENUM.INSERT;

           await EntryValidation(model);

            if (Context.ExecutionStatus.Status)
            {

                ObjectPermissionResult old 
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
                   await RepositorySet.ObjectPermission.Context
                        .RegisterDataLogAsync(userid.ToString(), operation, "SYSOBJECTPERMISSION",
                        model.ObjectPermissionID.ToString(), old, model);

                    ret = model;
                }

            }     

            return ret;
        }
      
        public async Task<ObjectPermissionEntry> Delete(ObjectPermissionEntry model, object userid)
        {
            ObjectPermissionEntry ret = null;

            ObjectPermissionResult old 
                = await RepositorySet.ObjectPermission
                    .Read(new ObjectPermissionParam() { pObjectPermissionID = model.ObjectPermissionID });

            if (old != null)
            {
                await DeleteValidation(model);

                if (Context.ExecutionStatus.Status)
                {
                     await RepositorySet.ObjectPermission.Delete(model);

                    if (Context.ExecutionStatus.Status && userid != null)
                    {
                        await RepositorySet.User.Context
                            .RegisterDataLogAsync(userid.ToString(), OPERATIONLOGENUM.DELETE, "SYSOBJECTPERMISSION",
                            model.ObjectPermissionID.ToString(), old, model);

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
     

    }
}
