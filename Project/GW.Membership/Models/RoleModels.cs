using System;
using GW.Helpers;

namespace GW.Membership.Models
{
    public class RoleParam
    {

        public RoleParam()
        {
            pRoleID = 0;
            pRoleName = ""; 
        }

        public Int64 pRoleID { get; set; }

        public string pRoleName { get; set; }

    }

    public class RoleEntry
    {
        [PrimaryValidationConfig("RoleID", "Role ID", FieldType.NUMERIC, false, 0)]
        public long RoleID { get; set; }

        [PrimaryValidationConfig("RoleName", "Role Name", FieldType.TEXT, false, 50)]
        public string RoleName { get; set; }

        public DateTime CreateDate { get; set; }

        public int IsActive { get; set; }
      
    }

    public class RoleList
    {
        
        public long RoleID { get; set; }
        
        public string RoleName { get; set; }

        public string sRoleID { get; set; }
    }

    public class RoleResult
    {
        
        public long RoleID { get; set; }
        
        public string RoleName { get; set; }

        public DateTime CreateDate { get; set; }

        public int IsActive { get; set; }
        

    }


}
