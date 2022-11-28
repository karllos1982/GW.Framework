using System;
using System.Collections.Generic;
using GW.Helpers;
using GW.Common;

namespace GW.Membership.Models
{
    public class PermissionParam
    {
        public Int64 pPermissionID { get; set; }

        public Int64 pUserID { get; set; }

        public Int64 pRoleID { get; set; }

        public Int64 pObjectPermissionID { get; set; }

        public PermissionParam()
        {
            pPermissionID = 0;
            pUserID = 0;
            pRoleID = 0;
            pObjectPermissionID = 0;
        }

    }

    public class PermissionEntry
    {
        public Int64 PermissionID { get; set; }

        [PrimaryValidationConfig("ObjectPermissionID", "Object Permission ID", FieldType.NUMERIC, false, 0)]
        public Int64 ObjectPermissionID { get; set; }

        [PrimaryValidationConfig("RoleID", "Role ID", FieldType.NUMERIC, true, 0)]
        public Int64? RoleID { get; set; }

        [PrimaryValidationConfig("UserID", "User ID", FieldType.NUMERIC, true, 0)]
        public Int64? UserID { get; set; }

        [PrimaryValidationConfig("ReadStatus", "Read Status", FieldType.NUMERIC, false, 0)]
        public int ReadStatus { get; set; }       

        [PrimaryValidationConfig("SaveStatus", "Save Status", FieldType.NUMERIC, false, 0)]
        public int SaveStatus { get; set; }

        [PrimaryValidationConfig("DeleteStatus", "Delete Status", FieldType.NUMERIC, false, 0)]
        public int DeleteStatus { get; set; }

        [PrimaryValidationConfig("TypeGrant", "Type Grant", FieldType.TEXT, false,1 )]
        public string TypeGrant { get; set; }
    }

    public class PermissionList
    {
        public Int64 PermissionID { get; set; }

        public Int64 ObjectPermissionID { get; set; }

        public string ObjectName { get; set; }        


    }

    public class PermissionResult
    {
        public Int64 PermissionID { get; set; }

        public Int64 ObjectPermissionID { get; set; }

        public string ObjectName { get; set; }

        public string ObjectCode { get; set; }

        public Int64 RoleID { get; set; }

        public string RoleName { get; set; }

        public Int64 UserID { get; set; }

        public string UserName { get; set; }

        public int ReadStatus { get; set; }

        public int SaveStatus { get; set; }        

        public int DeleteStatus { get; set; }

        public string TypeGrant { get; set; }

    }
  

}
