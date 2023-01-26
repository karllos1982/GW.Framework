using GW.Helpers;
using System.Collections.Generic;

namespace GW.Membership.Data
{
    public class ObjectPermissionQueryBuilder : QueryBuilder
    {
        public ObjectPermissionQueryBuilder()
        {
            Initialize();
        }

        public override void Initialize()
        {
            Keys = new List<string>();
            ExcludeFields = new List<string>();

            Keys.Add("ObjectPermissionID");
            
            
        }

        public override string QueryForGet(object param)
        {
            string ret = @"Select * from sysObjectPermission                
                where ObjectPermissionID=@pObjectPermissionID";

            return ret;
        }

        public override string QueryForList(object param)
        {
            string ret = @"select *             
             from sysObjectPermission s                           
             where 1=1 
             and (@pObjectPermissionID=0 or s.ObjectPermissionID=@pObjectPermissionID)             
             and (@pObjectName='' or s.ObjectName like '%' + @pObjectName + '%')  
             and (@pObjectCode='' or s.ObjectCode=@pObjectCode)
             order by ObjectName
             ";

            return ret;
        }

        public override string QueryForSearch(object param)
        {

            string ret = @"select *             
             from sysObjectPermission s              
             where 1=1 
             and (@pObjectPermissionID=0 or s.ObjectPermissionID=@pObjectPermissionID)
             and (@pObjectName='' or s.ObjectName=@pObjectName)
             and (@pObjectCode='' or s.ObjectCode=@pObjectCode)
             order by ObjectName
             ";           

            return ret;

        }

       
    }
}
