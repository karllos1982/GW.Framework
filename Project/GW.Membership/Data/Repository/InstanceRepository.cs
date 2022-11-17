using GW.Common;
using GW.Core;
using GW.Membership.Contracts;
using GW.Membership.Contracts.Data;
using GW.Membership.Models;

namespace GW.Membership.Data
{
    public class InstanceRepository : IInstanceRepository        
    {
        IDapperContext _ctx;

        public InstanceRepository(IContext context)
        {
            Context = Context;
            _ctx = (IDapperContext)context;
        }

        private InstanceQueryBuilder query = new InstanceQueryBuilder();

        public IContext Context { get; set; }

        public OperationStatus Create(InstanceModel model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForCreate("sysInstance", model,model);
            ret = _ctx.Execute(sql, model);

            return ret;
        }

        public InstanceModel Read(InstanceParam param)
        {
            InstanceModel ret = null;
            
            string sql = query.QueryForGet(null);

            ret = _ctx.ExecuteQueryFirst<InstanceModel>(sql, param); 
                 
            return ret;
        }

        public OperationStatus Update(InstanceModel model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForUpdate("sysInstance", model, model);
            ret = _ctx.Execute(sql, model);

            return ret;
        }

        public OperationStatus Delete(InstanceModel model)
        {
            OperationStatus ret = new OperationStatus(true);

            string sql = query.QueryForDelete("sysInstance", model, model);
            ret = _ctx.Execute(sql, model);

            return ret;
        }

        public List<InstanceList> List(InstanceParam param)
        {
            List<InstanceList> ret = null;

            ret = _ctx.ExecuteQueryToList<InstanceList>(query.QueryForList(null),
                 param); 

            return ret;
        }
             
        public List<InstanceSearchResult> Search(InstanceParam param)
        {
            List<InstanceSearchResult> ret = null;

            ret = _ctx.ExecuteQueryToList<InstanceSearchResult>(query.QueryForSearch(null),
                 param);

            return ret;
        }

        
    }

}
