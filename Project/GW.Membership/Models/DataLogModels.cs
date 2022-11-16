using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW.Membership.Models
{
    public class DataLogParam
    {
        public Int64 pDataLogID { get; set; }

        public Int64 pUserID { get; set; }
        
        public string pEmail { get; set; }

        public DateTime pDate_Start { get; set; }

        public DateTime pData_End { get; set; }

        public string pOperation { get; set; }

        public string pTableName { get; set; }

        public bool SearchByDate { get; set; }

        public Int64 pID { get; set; }

        public DataLogParam()
        {
            pDataLogID = 0;
            pUserID = 0;
            pID = 0;
            pEmail = "";
            pOperation = "0";
            pTableName = "0";
            SearchByDate = false;
        }

    }

    public class DataLogModel
    {
        public Int64 DataLogID { get; set; }

        public Int64 UserID { get; set; }

        public string Email { get; set; }

        public DateTime Date { get; set; }

        public string Operation { get; set; }

        public string OperationText
        {
            get
            {
                string aux = "";

                switch (Operation)
                {
                    case "I":
                        aux = "INSERT";
                        break;

                    case "U":
                        aux = "UPDATE";
                        break;

                    case "D":
                        aux = "DELETE";
                        break;

                }

                return aux;
            }

        }

        public string TableName { get; set; }

        public Int64 ID { get; set; }

        public string LogOldData { get; set; }

        public string LogCurrentData { get; set; }

    }

    public class DataLogTimelineModel
    {

        public Int64 LogID { get; set; }

        public string Operation { get; set; }

        public string OperationText
        {
            get
            {
                string aux = "";

                switch (Operation)
                {
                    case "I":
                        aux = "INSERT";
                        break;

                    case "U":
                        aux = "UPDATE";
                        break;

                    case "D":
                        aux = "DELETE";
                        break;

                }

                return aux;
            }

        }

        public string UserEmail { get; set; }

        public DateTime Date { get; set; }

    }

    public class TipoOperacaoValueModel
    {
        public string Value { get; set; }

        public string Text { get; set; }
    }

    public class TabelasValueModel
    {
        public string Value { get; set; }

        public string Text { get; set; }
    }

    public class DataLogItem
    {
        public string ItemName { get; set; }

        public string ItemValue { get; set; }

        public bool IsDifferent { get; set; }
    }

    public class DataLogList
    {
        public Int64 DataLogID { get; set; }

        public Int64 UserID { get; set; }

        public DateTime Date { get; set; }

        public string Operation { get; set; }

        public string TableName { get; set; }

        public Int64 ID { get; set; }
      
    }

    public class DataLogSearchResult
    {
        public Int64 DataLogID { get; set; }

        public Int64 UserID { get; set; }

        public string Email { get; set; }

        public DateTime Date { get; set; }

        public string Operation { get; set; }

        public string OperationText
        {
            get
            {
                string aux = "";

                switch (Operation)
                {
                    case "I":
                        aux = "INSERT";
                        break;

                    case "U":
                        aux = "UPDATE";
                        break;

                    case "D":
                        aux = "DELETE";
                        break;

                }

                return aux;
            }

        }

        public string TableName { get; set; }

        public Int64 ID { get; set; }

        public string LogOldData { get; set; }

        public string LogCurrentData { get; set; }
    }

}
