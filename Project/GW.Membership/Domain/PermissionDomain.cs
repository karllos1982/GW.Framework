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

      
        public async Task<PermissionModel> FillChields(PermissionModel obj)
        {
            return obj;
        }

        public async Task<PermissionModel> Get(PermissionParam param)
        {
            PermissionModel ret = null;

            ret = await RepositorySet.Permission.Read(param); 
            
            return ret;
        }

        public async Task<List<PermissionList>> List(PermissionParam param)
        {
            List<PermissionList> ret = null;

            ret = await RepositorySet.Permission.List(param);           

            return ret;
        }

        public async Task<List<PermissionSearchResult>> Search(PermissionParam param)
        {
            List<PermissionSearchResult> ret = null;

            ret = await RepositorySet.Permission.Search(param);

            return ret;
        }

        public async Task EntryValidation(PermissionModel obj)
        {
            OperationStatus ret = null;

            ret = PrimaryValidation.Execute(obj, new List<string>());

            if (!ret.Status)
            {
                ret.Error = new Exception(GW.Localization.GetItem("Validation-Error").Text);
            }

            Context.ExecutionStatus = ret;           
        }

        public async Task InsertValidation(PermissionModel obj)
        {
            Context.ExecutionStatus = new OperationStatus(true);
        }

        public async Task UpdateValidation(PermissionModel obj)
        {
            Context.ExecutionStatus = new OperationStatus(true);

        }

        public async Task DeleteValidation(PermissionModel obj)
        {
            Context.ExecutionStatus = new OperationStatus(true);
        }

        public async Task<PermissionModel> Set(PermissionModel model, object userid)
        {
            PermissionModel ret = null;
            OPERATIONLOGENUM operation = OPERATIONLOGENUM.INSERT;

            await EntryValidation(model);

            if (Context.ExecutionStatus.Status)
            {

                PermissionModel old 
                    = await RepositorySet.Permission.Read(new PermissionParam() { pPermissionID = model.PermissionID });

                if (old == null)
                {
                    await InsertValidation(model);

                    if (Context.ExecutionStatus.Status)
                    {
                        if (model.PermissionID  == 0) { model.PermissionID = GW.Helpers.Utilities.GenerateId(); }
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
     

        public async Task<PermissionModel> Delete(PermissionModel model, object userid)
        {
            PermissionModel ret = null;

            PermissionModel old 
                = await RepositorySet.Permission.Read(new PermissionParam() { pPermissionID = model.PermissionID });

            if (old != null)
            {
                await DeleteValidation(model);

                if (Context.ExecutionStatus.Status)
                {
                    await RepositorySet.Permission.Delete(model);
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
      
        public async Task<List<PermissionSearchResult>> GetPermissionsByRoleUser(Int64 roleid, Int64 userid)
        {
            List<PermissionSearchResult> ret = null;

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
