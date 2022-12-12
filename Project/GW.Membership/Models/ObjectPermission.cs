using System;
using System.Collections.Generic;
using GW.Helpers;
using GW.Common;

namespace GW.Membership.Models
{
    public class ObjectPermissionParam
    {
        public ObjectPermissionParam()
        {
            pObjectCode = "";
            pObjectName = "";
            pObjectPermissionID = 0; 
        }

        public Int64 pObjectPermissionID { get; set; }

        public string pObjectName { get; set; }

        public string pObjectCode{ get; set; }

    }

    public class ObjectPermissionEntry
    {
        
        public Int64 ObjectPermissionID { get; set; }

        [PrimaryValidationConfig("ObjectName", "Object Name", FieldType.TEXT, false, 50)]
        public string ObjectName { get; set; }

        [PrimaryValidationConfig("ObjectCode", "Object Code", FieldType.TEXT, false, 25)]
        public string ObjectCode { get; set; }

    }

    public class ObjectPermissionList
    {
        public Int64 ObjectPermissionID { get; set; }

        public string ObjectName { get; set; }

        public string ObjectCode { get; set; }

    }

    public class ObjectPermissionResult
    {
        public Int64 ObjectPermissionID { get; set; }

        public string ObjectName { get; set; }

        public string ObjectCode { get; set; }

    }

}
