using GW.Helpers;
using System.Collections.Generic;
using GW.Membership.Models;
using System.ComponentModel.Design;

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
            
        }

        public override string QueryForGet(object param)
        {
            string ret = "";

            SelectBuilder.Clear();
            SelectBuilder.AddTable("sysDataLog", "l", true, "UserID", "", JOINTYPE.NONE, null);
            SelectBuilder.AddTable("sysUser", "u", false, "DataLogID", "UserID", JOINTYPE.INNER, "l");
            SelectBuilder.AddField("u", "Email", "", true,null,null, ORDERBYTYPE.NONE);
            SelectBuilder.AddField("l", "DataLogID", "@pDataLogID",false,"0",null, ORDERBYTYPE.NONE);
            
            ret = SelectBuilder.BuildQuery();

            return ret;
        }

        public override string QueryForList(object param)
        {
            string ret = ""; 

            SelectBuilder.Clear();
            SelectBuilder.AddTable("sysDataLog", "l", true, "", "", JOINTYPE.NONE, null);

            ret = SelectBuilder.BuildQuery();

            return ret;
        }

        public override string QueryForSearch(object param)
        {
            bool gobydate = ((DataLogParam)param).SearchByDate;

            string ret = ""; 
                                     

            SelectBuilder.Clear();
            SelectBuilder.AddTable("sysDataLog", "l", true, "UserID", "", JOINTYPE.NONE, null);
            SelectBuilder.AddTable("sysUser", "u", false, "DataLogID", "UserID", JOINTYPE.INNER, "l");
            SelectBuilder.AddField("u", "Email", "@pEmail", true, "''", null, ORDERBYTYPE.NONE);

            SelectBuilder.AddField("l", "UserID", "@pUserID", false,"0",null, ORDERBYTYPE.NONE);
            SelectBuilder.AddField("l", "Operation", "@pOperation",false,"'0'",null, ORDERBYTYPE.NONE);
            SelectBuilder.AddField("l", "TableName", "@pTableName",false,"'0'",null, ORDERBYTYPE.NONE);
            SelectBuilder.AddField("l", "ID", "@pID", false, "0", null, ORDERBYTYPE.NONE);
            SelectBuilder.AddField("l", "Date", "", false, null, null, ORDERBYTYPE.DESC);
           
            if (gobydate)
            {
                ret = SelectBuilder.BuildQuery(" and (l.Date between @pDate_Start and @pData_End )");               
            }
            else
            {
                ret = SelectBuilder.BuildQuery(); 
            }
                                   

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
