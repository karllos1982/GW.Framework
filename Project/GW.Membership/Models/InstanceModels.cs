using System;
using System.Collections.Generic;
using GW.Helpers;
using GW.Common;

namespace GW.Membership.Models
{

    public class InstanceParam
    {
        public InstanceParam()
        {        
            pInstanceID = 0;
            pInstanceName = "";
            pInstanceTypeName = "";
        }


        public Int64 pInstanceID { get; set; }
        
        public string pInstanceTypeName { get; set; }

        public string pInstanceName { get; set; }

        
    }

    public class InstanceEntry
    {
        [PrimaryValidationConfig("InstanceID", "Instance ID", FieldType.NUMERIC, false, 0)]
        public Int64 InstanceID { get; set; }

        [PrimaryValidationConfig("InstanceTypeName", "Instance Type Name", FieldType.TEXT , false, 50)]
        public string InstanceTypeName { get; set; }

        [PrimaryValidationConfig("InstanceName", "Instance Name", FieldType.TEXT, false, 100)]
        public string InstanceName { get; set; }

        public int IsActive { get; set; }

        public DateTime CreateDate { get; set; }

    }


    public class InstanceList
    {        
        public Int64 InstanceID { get; set; }
     
        public string InstanceName { get; set; }

    }

    public class InstanceResult
    {
        
        public Int64 InstanceID { get; set; }
        
        public string InstanceTypeName { get; set; }
       
        public string InstanceName { get; set; }

        public int IsActive { get; set; }

        public DateTime CreateDate { get; set; }

    }

}
