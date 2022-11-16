using System;
using System.Collections;
using System.Collections.Generic;


namespace GW.Common
{

    public class DefaultGetParam
    {
        public DefaultGetParam(string idvalue)
        {
            id = idvalue.Trim();
        }

        public string id { get; set; }
    }

    public class InnerException
    {
        public string Key { get; set; }

        public string Description { get; set; }

        public InnerException(string key, string description)
        {
            this.Key = key;
            this.Description = description;
        }
    }

    public class OperationStatus
    {
        public bool Status; //indica o status do resultado da operação (true, false)
        public object Returns;//um objeto de retorno
        public Exception Error; // indica o erro (se ocorrer) ao realizar a operação
        public List<InnerException> InnerExceptions = new List<InnerException>(); //lista de erros na operação de validação

        public OperationStatus()
        {
            Status = false;
            Error = null;
            Returns = null;
        }

        public OperationStatus(bool status)
        {
            Status = status;
            Error = null;
            Returns = null;
        }

        public void Reset()
        {
            Status = true;
            InnerExceptions = null;
            Error = null;
            Returns = null;
        }

        public void AddInnerException(string key, string description)
        {
            InnerExceptions.Add(new InnerException(key, description));
            Status = false;
        }

        public ArrayList InnerToArrayList()
        {
            ArrayList ret = new ArrayList();

            foreach (InnerException ie in this.InnerExceptions)
            {
                ret.Add(ie.Description);
            }
            return ret;
        }

        public string InnerToString()
        {
            string ret = "";

            foreach (InnerException ie in this.InnerExceptions)
            {
                if (ret == "")
                {
                    ret = ie.Description;
                }
                else
                {
                    ret = ret + ", " + ie.Description;
                }


            }
            return ret;
        }
    }

    public class RequestStatus
    {
        public RequestStatus()
        {
            this._status = "success";
            this._message = "";
            this._returns = null;
        }

        public RequestStatus(string status, string message)
        {
            this._status = status;
            this._message = message;
            this._returns = null;
        }

        private string _status;
        public string status
        {
            get { return _status; }
            set { _status = value; }
        }

        private string _message;
        public string message
        {
            get { return _message; }
            set { _message = value; }
        }

        private object _returns;
        public object returns
        {
            get { return _returns; }
            set { _returns = value; }
        }

        public bool IsSuccess()
        {
            bool ret = false;

            if (status == "success")
            {
                ret = true;
            }

            return ret;
        }

        public void SetFailStatus(string msg)
        {
            status = "fail";
            message = msg;
        }

        public void SetValidationErrsList(OperationStatus ret)
        {
            this.returns = ret.InnerExceptions;
        }

    }
   
    public enum RECORDSTATEENUM
    {
        NONE = 0,
        ADD = 1,
        EDITED = 2,
        DELETED = 3
    }

    public enum OPERATIONLOGENUM
    {
        INSERT = 1,
        UPDATE = 2,
        DELETE = 3
    }

    public class UserStatus
    {

    }

    public class AuthToken
    {
        public string TokenValue { get; set; }

        public DateTime ExpiresDate { get; set; }
    }


    public class AuthParams
    {
        public string ApplicationID { get; set; }

        public string ClientID { get; set; }

        public string SecretKey { get; set; }

        public string DeviceName { get; set; }

        public string IP { get; set; }

        public string UserIdentity { get; set; }

        public string UserPassword { get; set; }

    }

    public class SessionParams
    {
        public string SessionID { get; set; }

        public string AccessToken { get; set; }

    }

    public class DataLogObject
    {
        public Int64 DataLogID { get; set; }

        public Int64 UserID { get; set; }

        public DateTime Date { get; set; }

        public string Operation { get; set; }

        public string TableName { get; set; }

        public Int64 ID { get; set; }

        public string LogOldData { get; set; }

        public string LogCurrentData { get; set; }
    }

    public class SourceConfig
    {
        public string SourceName { get; set; }

        public string  SourceValue { get; set; }

    }

    public enum PERMISSION_CHECK_ENUM
    {
        READ = 1,
        SAVE = 2,
        DELETE = 3

    }

    public enum PERMISSION_STATE_ENUM
    {
        NONE = 0,
        ALLOWED = 1,
        DENIED = 2

    }

    public interface IUserPermissions
    {
       

    }

    public class UserPermissions : IUserPermissions
    {
        public Int64 PermissionID { get; set; }

        public Int64 ObjectPermissionID { get; set; }

        public string ObjectCode { get; set; }

        public int ReadStatus { get; set; }

        public int SaveStatus { get; set; }

        public int DeleteStatus { get; set; }

        public string TypeGrant { get; set; }

    }

    public class PermissionsState
    {
        public bool AllowRead { get; set; }

        public bool AllowSave { get; set; }

        public bool AllowDelete { get; set; }

        public PermissionsState(bool allowread, bool allowsave, bool allowdelete)
        {
            AllowRead = allowread;
            AllowSave = allowsave;  
            AllowDelete = allowdelete;  
        }

    }

}
