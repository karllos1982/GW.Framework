using GW.Common;
using GW.Core;
using GW.Membership.Contracts;
using GW.Membership.Contracts.Domain;
using GW.Membership.Models;
using GW.Membership.Contracts.Data;

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
            throw new NotImplementedException();
        }

        public List<InstanceList> List(InstanceParam param)
        {
            throw new NotImplementedException();
        }

        public List<InstanceSearchResult> Search(InstanceParam param)
        {
            throw new NotImplementedException();
        }

        public OperationStatus Set(InstanceModel model, object userid)
        {
            throw new NotImplementedException();
        }

        public void FillChields(ref InstanceModel obj)
        {
            throw new NotImplementedException();
        }

        public OperationStatus Delete(InstanceModel model, object userid)
        {
            throw new NotImplementedException();
        }

        public OperationStatus DeleteValidation(InstanceModel obj)
        {
            throw new NotImplementedException();
        }

        public OperationStatus EntryValidation(InstanceModel obj)
        {
            throw new NotImplementedException();
        }
             
        public OperationStatus InsertValidation(InstanceModel obj)
        {
            throw new NotImplementedException();
        }
            
        public OperationStatus UpdateValidation(InstanceModel obj)
        {
            throw new NotImplementedException();
        }

    }
}
