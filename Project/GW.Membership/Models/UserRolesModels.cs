using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using GW.Common; 

namespace GW.Membership.Models
{
    public class UserRolesParam
    {

        public UserRolesParam()
        {
            pUserRoleID = 0;
            pUserID = 0;
            pRoleID = 0; 
        }

        public Int64 pUserRoleID { get; set; }

        public Int64 pUserID { get; set; }

        public Int64 pRoleID { get; set; }
    }

    public class UserRolesModel
    {
        public Int64 UserRoleID { get; set; }

        public Int64 UserID { get; set; }

        public Int64 RoleID { get; set; }

        public string RoleName { get; set; }

        public RECORDSTATEENUM RecordState { get; set; }
    }

    public class UserRolesList
    {
        public Int64 UserRoleID { get; set; }

        public Int64 RoleID { get; set; }

        public string RoleName { get; set; }

    }

    public class UserRolesSearchResult
    {
        public Int64 UserRoleID { get; set; }

        public Int64 UserID { get; set; }

        public string UserName { get; set; }

        public Int64 RoleID { get; set; }

        public string RoleName { get; set; }
        

    }

}
