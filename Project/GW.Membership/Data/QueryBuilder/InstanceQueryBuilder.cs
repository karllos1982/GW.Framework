using GW.Helpers;
using System.Collections.Generic;

namespace GW.Membership.Data
{
    public class InstanceQueryBuilder : QueryBuilder
    {
        public InstanceQueryBuilder()
        {
            Initialize();
        }

        public override void Initialize()
        {
            Keys = new List<string>();
            ExcludeFields = new List<string>();

            Keys.Add("InstanceID");
        }

        public override string QueryForGet(object param)
        {
            string ret = "Select * from sysInstance where InstanceID=@pInstanceID";

            return ret;
        }

        public override string QueryForList(object param)
        {
            string ret = @"select InstanceID, InstanceName             
             from sysInstance 
             where 1=1 
             and (@pInstanceName='' or InstanceName=@pInstanceName)
             and (@pInstanceTypeName='' or InstanceTypeName=@pInstanceTypeName)
             and (@pInstanceID=0 or InstanceID=@pInstanceID)      
             ";

            return ret;
        }

        public override string QueryForSearch(object param)
        {
            string ret = @"select *             
             from sysInstance 
             where 1=1 
             and (@pInstanceName='' or InstanceName=@pInstanceName)
             and (@pInstanceTypeName='' or InstanceTypeName=@pInstanceTypeName)
             and (@pInstanceID=0 or InstanceID=@pInstanceID)             
             ";

            return ret;

        }
    }
}
