using GW.Helpers;
using System.Collections.Generic;
using GW.Membership.Models;

namespace GW.Membership.Data
{
    public class DataLogQueryBuilder : QueryBuilder
    {
        public DataLogQueryBuilder()
        {
            Initialize();
        }

        public override void Initialize()
        {
            Keys = new List<string>();
            ExcludeFields = new List<string>();

            Keys.Add("DataLogID");

            ExcludeFields.Add("Email");
            ExcludeFields.Add("OperationText");
        }

        public override string QueryForGet(object param)
        {
            string ret = @"Select u.Email, l.* from sysDataLog l
                inner join sysUser u on l.UserID=u.UserID
                where DataLogID=@pDataLogID";

            return ret;
        }

        public override string QueryForList(object param)
        {
            string ret = @"select *             
             from sysDataLog s
             ";

            return ret;
        }

        public override string QueryForSearch(object param)
        {
            bool gobydate = ((DataLogParam)param).SearchByDate;

            string ret = @"select s.*, u.Email             
             from sysDataLog s
             inner join sysUser u on s.UserID = u.UserID  
             where 1=1 
             and (@pUserID=0 or s.UserID=@pUserID)  
             and (@pEmail='' or u.Email=@pEmail) 
             and (@pOperation='0' or Operation=@pOperation)
             and (@pTableName='0' or TableName=@pTableName) 
             and (@pID=0 or s.ID=@pID)              
             ";

            if (gobydate)
            {
                ret = ret + " and (s.Date between @pDate_Start and @pData_End )";
            }

            ret = ret + " order by s.Date desc ";
            

            return ret;

        }

        public string QueryForGetTimeLine()
        {
            string ret =
                    @"Select 
                     a.DataLogID, a.Operation , a.Date,
                    u.email as UserEmail                  
                    from sysDataLog a 
                    inner join sysUser u on a.UserID = u.UserID
                    where a.ID=@pID
                    order by a.Date";

            return ret;

        }

        public string QueryForGetTableList()
        {
            string ret =
                    @"select distinct TableName 'Value', TableName 'Text' from sysDataLog order by TableName ";

            return ret;

        }

    }
}
