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

      
        public async Task<PermissionResult> FillChields(PermissionResult obj)
        {
            return obj;
        }

        public async Task<PermissionResult> Get(PermissionParam param)
        {
            PermissionResult ret = null;

            ret = await RepositorySet.Permission.Read(param); 
            
            return ret;
        }

        public async Task<List<PermissionList>> List(PermissionParam param)
        {
            List<PermissionList> ret = null;

            ret = await RepositorySet.Permission.List(param);           

            return ret;
        }

        public async Task<List<PermissionResult>> Search(PermissionParam param)
        {
            List<PermissionResult> ret = null;

            ret = await RepositorySet.Permission.Search(param);

            return ret;
        }

        public async Task EntryValidation(PermissionEntry obj)
        {
            OperationStatus ret = null;

            ret = PrimaryValidation.Execute(obj, new List<string>(), Context.LocalizationLanguage);

            if (!ret.Status)
            {
                ret.Error 
                    = new Exception(GW.LocalizationText.Get("Validation-Error",Context.LocalizationLanguage).Text);
            }

            Context.ExecutionStatus = ret;           
        }

        public async Task InsertValidation(PermissionEntry obj)
        {
            Context.ExecutionStatus = new OperationStatus(true);
        }

        public async Task UpdateValidation(PermissionEntry obj)
        {
            Context.ExecutionStatus = new OperationStatus(true);

        }

        public async Task DeleteValidation(PermissionEntry obj)
        {
            Context.ExecutionStatus = new OperationStatus(true);
        }

        public async Task<PermissionEntry> Set(PermissionEntry model, object userid)
        {
            PermissionEntry ret = null;
            OPERATIONLOGENUM operation = OPERATIONLOGENUM.INSERT;

            await EntryValidation(model);

            if (Context.ExecutionStatus.Status)
            {

                PermissionResult old 
                    = await RepositorySet.Permission.Read(new PermissionParam() { pPermissionID = model.PermissionID });

                if (old == null)
                {
                    await InsertValidation(model);

                    if (Context.ExecutionStatus.Status)
                    {
                        if (model.PermissionID  == 0) { model.PermissionID = GW.Helpers.Utilities.GenerateId(); }

                        if (model.RoleID != null) { model.TypeGrant = "R"; }
                        if (model.UserID != null) { model.TypeGrant = "U"; }
                        if (model.RoleID != null && model.UserID != null) { model.TypeGrant = "U"; }

                        await RepositorySet.Permission.Create(model);
                    }
                }
                else
                {                    
                    operation = OPERATIONLOGENUM.UPDATE;

                    await UpdateValidation(model);

                    if (Context.ExecutionStatus.Status)
                    {
                        await RepositorySet.Permission.Update(model);
                    }

                }

                if (Context.ExecutionStatus.Status && userid != null)
                {
                   await  RepositorySet.Permission.Context
                        .RegisterDataLogAsync(userid.ToString(), operation, "SYSPERMISSION",
                        model.PermissionID.ToString(), old, model);

                    ret = model;
                }

            }     

            return ret;
        }
     

        public async Task<PermissionEntry> Delete(PermissionEntry model, object userid)
        {
            PermissionEntry ret = null;

            PermissionResult old 
                = await RepositorySet.Permission.Read(new PermissionParam() { pPermissionID = model.PermissionID });

            if (old != null)
            {
                await DeleteValidation(model);

                if (Context.ExecutionStatus.Status)
                {
                    await RepositorySet.Permission.Delete(model);

                    if (Context.ExecutionStatus.Status && userid != null)
                    {
                        await RepositorySet.User.Context
                            .RegisterDataLogAsync(userid.ToString(), OPERATIONLOGENUM.DELETE, "SYSPERMISSION",
                            model.PermissionID.ToString(), old, model);

                        ret = model;
                    }
                }
            }
            else
            {
                Context.ExecutionStatus.Status = false;
                Context.ExecutionStatus.Error
                    = new System.Exception(GW.LocalizationText.Get("Record-NotFound",Context.LocalizationLanguage).Text);

            }           

            return ret;
        }
      
        public async Task<List<PermissionResult>> GetPermissionsByRoleUser(Int64 roleid, Int64 userid)
        {
            List<PermissionResult> ret = null;

            PermissionParam param = new PermissionParam()
            {
                pRoleID = roleid,
                pUserID = userid
            };

            ret = await RepositorySet.Permission.GetPermissionsByRoleUser(param);
            

            return ret;
        }

    }
}
