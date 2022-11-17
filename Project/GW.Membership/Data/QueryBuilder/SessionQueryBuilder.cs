using GW.Helpers;
using GW.Membership.Models;
using System.Collections.Generic;

namespace GW.Membership.Data
{
    public class SessionQueryBuilder : QueryBuilder
    {
        public SessionQueryBuilder()
        {
            Initialize();
        }

        public override void Initialize()
        {
            Keys = new List<string>();
            ExcludeFields = new List<string>();

            Keys.Add("SessionID");

            ExcludeFields.Add("UserName");
            ExcludeFields.Add("Email");
        }

        public override string QueryForGet(object param)
        {
            string ret = @"Select u.UserName, u.Email, s.* from sysSession
                inner join sysUser u on s.UserID = u.UserID   
                where SessionID=@pSessionID";

            return ret;
        }

        public override string QueryForList(object param)
        {
            string ret = @"select 
             s.SessionID, s.UserID, u.UserName, Date, IP, BrowserName
             from sysSession s
             inner join sysUser u on s.UserID = u.UserID               
             ";

            return ret;
        }

        public override string QueryForSearch(object param)
        {
            bool gobydate = ((SessionLogParam)param).SearchByDate;

            string ret = @"select u.UserName, u.Email, s.*             
             from sysSession s
             inner join sysUser u on s.UserID = u.UserID  
             where 1=1 
             and (@pUserID=0 or s.UserID=@pUserID)
             and (@pEmail='' or u.Email=@pEmail)
             ";

            if (gobydate)
            {
                ret = ret + " and (s.Date between @pDate_Start and @pData_End )";
            }

            ret = ret + " order by s.Date desc ";
             

            return ret;

        }

        public string QueryForSetDateLogout()
        {
            string ret = @"update sysSession set DateLogout = GetDate() 
                    where SessionID = (select top 1 SessionID 
                    from sysSession where UserID=@pUserID order by Date desc)
             ";

            return ret;
        }
    }
}
