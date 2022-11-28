using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW.Membership.Models
{
    public class SessionLogParam
    {

        public SessionLogParam()
        {
            pSessionID = 0;
            pUserID = 0;
            pEmail = "";
            SearchByDate = false; 
        }

        public Int64 pSessionID { get; set; }

        public Int64 pUserID { get; set; }

        public string pEmail { get; set; }

        public DateTime pDate_Start { get; set; }

        public DateTime pData_End { get; set; }

        public bool SearchByDate { get; set; }

    }

    public class SessionLogEntry
    {
        public Int64 SessionID { get; set; }

        public Int64 UserID { get; set; }  

        public DateTime Date { get; set; }

        public string IP { get; set; }

        public string BrowserName { get; set; }

        public DateTime ? DateLogout { get; set; }

    }

    public class SessionLogList
    {
        public Int64 SessionID { get; set; }

        public Int64 UserID { get; set; }

        public string UserName { get; set; }

        public DateTime Date { get; set; }

    }

    public class SessionLogResult
    {
        public Int64 SessionID { get; set; }

        public Int64 UserID { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public DateTime Date { get; set; }

        public string IP { get; set; }

        public string BrowserName { get; set; }

        public DateTime? DateLogout { get; set; }

    }



}
