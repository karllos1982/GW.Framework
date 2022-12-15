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
        private string lang = "";

        public RoleDomain(IContext context, IMembershipRepositorySet repositorySet)
        {
            Context = context;
            RepositorySet = repositorySet;
            lang = Context.Settings.LocalizationLanguage;
        }

        public IContext Context { get; set; }

        public IMembershipRepositorySet RepositorySet { get; set; }

        public async Task<RoleResult> FillChields(RoleResult obj)
        {
            return obj;
        }

        public async Task<RoleResult> Get(RoleParam param)
        {
            RoleResult ret = null;

            ret = await RepositorySet.Role.Read(param); 
            
            return ret;
        }

        public async Task<List<RoleList>> List(RoleParam param)
        {
            List<RoleList> ret = null;

            ret = await RepositorySet.Role.List(param);           

            return ret;
        }

        public async Task<List<RoleResult>> Search(RoleParam param)
        {
            List<RoleResult> ret = null;

            ret = await RepositorySet.Role.Search(param);

            return ret;
        }
        public async Task EntryValidation(RoleEntry obj)
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

        public async Task InsertValidation(RoleEntry obj)
        {
            OperationStatus ret = new OperationStatus(true);
            RoleParam param = new RoleParam()
            {
                pRoleName = obj.RoleName
            };

            List<RoleList> list
                = await RepositorySet.Role.List(param);

            if (list != null)
            {
                if (list.Count > 0)
                {
                    ret.Status = false;
                    string msg 
                        = string.Format(GW.Localization.GetItem("Validation-Unique-Value",lang).Text, "Role Name");
                    ret.Error = new Exception(msg);
                    ret.AddInnerException("RoleName", msg);
                }
            }

            Context.ExecutionStatus = ret;
          
        }

        public async Task UpdateValidation(RoleEntry obj)
        {
            OperationStatus ret = new OperationStatus(true);
            RoleParam param = new RoleParam() { pRoleName = obj.RoleName };

            List<RoleList> list
                = await RepositorySet.Role.List(param);

            if (list != null)
            {
                if (list.Count > 0)
                {
                    if (list[0].RoleID != obj.RoleID)
                    {
                        ret.Status = false;
                        string msg 
                            = string.Format(GW.Localization.GetItem("Validation-Unique-Value",lang).Text, "Role Name");
                        ret.Error = new Exception(msg);
                        ret.AddInnerException("RoleName", msg);
                    }
                }
            }

            Context.ExecutionStatus = ret;          

        }

        public async Task DeleteValidation(RoleEntry obj)
        {
            Context.ExecutionStatus = new OperationStatus(true);
        }

        public async Task<RoleEntry> Set(RoleEntry model, object userid)
        {
            RoleEntry ret = null;
            OPERATIONLOGENUM operation = OPERATIONLOGENUM.INSERT;

            await EntryValidation(model);

            if (Context.ExecutionStatus.Status)
            {

                RoleResult old 
                    = await RepositorySet.Role.Read(new RoleParam() { pRoleID = model.RoleID });

                if (old == null)
                {
                    await InsertValidation(model);

                    if (Context.ExecutionStatus.Status)
                    {
                        model.CreateDate = DateTime.Now;
                        if (model.RoleID == 0) { model.RoleID = GW.Helpers.Utilities.GenerateId(); }
                        await RepositorySet.Role.Create(model);
                    }
                }
                else
                {
                    model.CreateDate = old.CreateDate;
                    operation = OPERATIONLOGENUM.UPDATE;

                    await UpdateValidation(model);

                    if (Context.ExecutionStatus.Status)
                    {
                        await RepositorySet.Role.Update(model);
                    }

                }

                if (Context.ExecutionStatus.Status && userid != null)
                {
                   await  RepositorySet.Role.Context
                        .RegisterDataLogAsync(userid.ToString(), operation, "SYSROLE",
                        model.RoleID.ToString(), old, model);

                    ret = model;
                }

            }     

            return ret;
        }
      
        public async Task<RoleEntry> Delete(RoleEntry model, object userid)
        {
            RoleEntry ret = null;

            RoleResult old 
                = await RepositorySet.Role.Read(new RoleParam() { pRoleID = model.RoleID });

            if (old != null)
            {
                await DeleteValidation(model);

                if (Context.ExecutionStatus.Status)
                {
                   await RepositorySet.Role.Delete(model);
                    if (Context.ExecutionStatus.Status && userid != null)
                    {
                        await RepositorySet.User.Context
                            .RegisterDataLogAsync(userid.ToString(), OPERATIONLOGENUM.DELETE, "SYSROLE",
                            model.RoleID.ToString(), old, model);

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
