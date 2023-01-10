using GW.Common;
using GW.Core;
using GW.Membership.Contracts;
using GW.Membership.Contracts.Domain;
using GW.Membership.Models;
using GW.Membership.Contracts.Data;
using GW.Helpers;
using Microsoft.SqlServer.Server;
using static System.Net.Mime.MediaTypeNames;

namespace GW.Membership.Domain
{
    public class InstanceDomain : IInstanceDomain
    {
        private string lang = ""; 

        public InstanceDomain(IContext context, IMembershipRepositorySet repositorySet)
        {
            Context = context;
            RepositorySet = repositorySet;
            lang = Context.LocalizationLanguage;
        }

        public IContext Context { get; set; }

        public IMembershipRepositorySet RepositorySet { get; set; }

        public async Task<InstanceResult> FillChields(InstanceResult obj)
        {
            return obj;
        }

        public async Task<InstanceResult> Get(InstanceParam param)
        {
            InstanceResult ret = null;

            ret = await RepositorySet.Instance.Read(param);

            return ret;
        }

        public async Task<List<InstanceList>> List(InstanceParam param)
        {
            List<InstanceList> ret = null;

            ret = await RepositorySet.Instance.List(param);

            return ret;
        }

        public async Task<List<InstanceResult>> Search(InstanceParam param)
        {
            List<InstanceResult> ret = null;

            ret = await RepositorySet.Instance.Search(param);

            return ret;
        }

        public async Task EntryValidation(InstanceEntry obj)
        {
            OperationStatus ret = null;

            ret = PrimaryValidation.Execute(obj, new List<string>(), lang);

            if (!ret.Status)
            {
                ret.Error 
                    = new Exception(GW.Localization.GetItem("Validation-Error", lang).Text);
            }

            Context.ExecutionStatus = ret;

        }

        public async Task InsertValidation(InstanceEntry obj)
        {
            OperationStatus ret = new OperationStatus(true);
            InstanceParam param = new InstanceParam()
            {
                pInstanceName = obj.InstanceName
            };

            List<InstanceList> list
                = await RepositorySet.Instance.List(param);

            if (list != null)
            {
                if (list.Count > 0)
                {
                    ret.Status = false;
                    string msg
                        = string.Format(GW.Localization.GetItem("Validation-Unique-Value", lang).Text, "Instance Name"); 
                    ret.Error = new Exception(msg);
                    ret.AddInnerException("InstanceName", msg);
                }
            }

            Context.ExecutionStatus = ret;

        }

        public async Task UpdateValidation(InstanceEntry obj)
        {
            OperationStatus ret = new OperationStatus(true);
            InstanceParam param = new InstanceParam() { pInstanceName = obj.InstanceName };
            List<InstanceList> list
                = await RepositorySet.Instance.List(param);

            if (list != null)
            {
                if (list.Count > 0)
                {
                    if (list[0].InstanceID != obj.InstanceID)
                    {
                        ret.Status = false;
                        string msg 
                            = string.Format(GW.Localization.GetItem("Validation-Unique-Value", lang).Text, "Instance Name");
                        ret.Error = new Exception(msg);
                        ret.AddInnerException("InstanceName", msg);
                    }
                }
            }

            Context.ExecutionStatus = ret;

        }

        public async Task DeleteValidation(InstanceEntry obj)
        {
            Context.ExecutionStatus = new OperationStatus(true);
        }

        public async Task<InstanceEntry> Set(InstanceEntry model, object userid)
        {
            InstanceEntry ret = null;
            OPERATIONLOGENUM operation = OPERATIONLOGENUM.INSERT;

            await EntryValidation(model);

            if (Context.ExecutionStatus.Status)
            {

                InstanceResult old
                    = await RepositorySet.Instance.Read(new InstanceParam() { pInstanceID = model.InstanceID });

                if (old == null)
                {
                    await InsertValidation(model);

                    if (Context.ExecutionStatus.Status)
                    {
                        model.CreateDate = DateTime.Now;
                        if (model.InstanceID == 0) { model.InstanceID = GW.Helpers.Utilities.GenerateId(); }
                        await RepositorySet.Instance.Create(model);
                    }
                }
                else
                {
                    model.CreateDate = old.CreateDate;
                    operation = OPERATIONLOGENUM.UPDATE;

                    await UpdateValidation(model);

                    if (Context.ExecutionStatus.Status)
                    {
                        await RepositorySet.Instance.Update(model);
                    }

                }

                if (Context.ExecutionStatus.Status && userid != null)
                {
                    await RepositorySet.Instance.Context
                        .RegisterDataLogAsync(userid.ToString(), operation, "SYSINSTANCE",
                        model.InstanceID.ToString(), old, model);

                    ret = model;
                }

            }

            return ret;
        }

        public async Task<InstanceEntry> Delete(InstanceEntry model, object userid)
        {
            InstanceEntry ret = null;

            InstanceResult old
                = await RepositorySet.Instance.Read(new InstanceParam() { pInstanceID = model.InstanceID });

            if (old != null)
            {
                await DeleteValidation(model);

                if (Context.ExecutionStatus.Status)
                {
                    await RepositorySet.Instance.Delete(model);

                    if (Context.ExecutionStatus.Status && userid != null)
                    {
                        await RepositorySet.User.Context
                            .RegisterDataLogAsync(userid.ToString(),  OPERATIONLOGENUM.DELETE, "SYSINSTANCE",
                            model.InstanceID.ToString(), old, model);

                        ret = model;
                    }

                }
            }
            else
            {
                Context.ExecutionStatus.Status = false;
                Context.ExecutionStatus.Error 
                    = new System.Exception(GW.Localization.GetItem("Record-NotFound", lang).Text);

            }

            return ret;
        }



    }
}
