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

        public async Task<InstanceModel> FillChields(InstanceModel obj)
        {
            return obj;
        }

        public async Task<InstanceModel> Get(InstanceParam param)
        {
            InstanceModel ret = null;

            ret = await RepositorySet.Instance.Read(param);

            return ret;
        }

        public async Task<List<InstanceList>> List(InstanceParam param)
        {
            List<InstanceList> ret = null;

            ret = await RepositorySet.Instance.List(param);

            return ret;
        }

        public async Task<List<InstanceSearchResult>> Search(InstanceParam param)
        {
            List<InstanceSearchResult> ret = null;

            ret = await RepositorySet.Instance.Search(param);

            return ret;
        }

        public async Task EntryValidation(InstanceModel obj)
        {
            OperationStatus ret = null;

            ret = PrimaryValidation.Execute(obj, new List<string>());

            if (!ret.Status)
            {
                ret.Error = new Exception(GW.Localization.GetItem("Validation-Error").Text);
            }

            Context.ExecutionStatus = ret;

        }

        public async Task InsertValidation(InstanceModel obj)
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
                    ret.Error = new Exception(GW.Localization.GetItem("Validation-Unique-Value").Text);
                }
            }

            Context.ExecutionStatus = ret;

        }

        public async Task UpdateValidation(InstanceModel obj)
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
                        ret.Error = new Exception(GW.Localization.GetItem("Validation-Unique-Value").Text);
                    }
                }
            }

            Context.ExecutionStatus = ret;

        }

        public async Task DeleteValidation(InstanceModel obj)
        {
            Context.ExecutionStatus = new OperationStatus(true);
        }

        public async Task<InstanceModel> Set(InstanceModel model, object userid)
        {
            InstanceModel ret = null;
            OPERATIONLOGENUM operation = OPERATIONLOGENUM.INSERT;

            await EntryValidation(model);

            if (Context.ExecutionStatus.Status)
            {

                InstanceModel old
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

        public async Task<InstanceModel> Delete(InstanceModel model, object userid)
        {
            InstanceModel ret = null;

            InstanceModel old
                = await RepositorySet.Instance.Read(new InstanceParam() { pInstanceID = model.InstanceID });

            if (old != null)
            {
                await DeleteValidation(model);

                if (Context.ExecutionStatus.Status)
                {
                    await RepositorySet.Instance.Delete(model);
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
