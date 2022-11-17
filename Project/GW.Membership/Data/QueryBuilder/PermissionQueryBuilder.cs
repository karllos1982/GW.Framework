using GW.Helpers;
using System.Collections.Generic;

namespace GW.Membership.Data
{
    public class PermissionQueryBuilder : QueryBuilder
    {
        public PermissionQueryBuilder()
        {
            Initialize();
        }

        public override void Initialize()
        {
            Keys = new List<string>();
            ExcludeFields = new List<string>();

            Keys.Add("PermissionID");
       
        }

        public override string QueryForGet(object param)
        {
            string ret = @"Select * from sysPermission                
                where PermissionID=@pPermissionID";

            return ret;
        }

        public override string QueryForList(object param)
        {
            string ret = @"select p.PermissionID, p.ObjectPermissionID, o.ObjectName            
             from sysPermission p
             inner join sysObjectPermission o on o.ObjectPermissionID = p.ObjectPermissionID
             left join sysUser u on u.UserID = p.UserID          
             left join sysRole r on r.RoleID = p.RoleID 
             where 1=1
             and (@pObjectPermissionID=0 or p.ObjectPermissionID=@pObjectPermissionID) 
             and ((@pRoleID=0 or p.RoleID=@pRoleID)
             or (@pUserID=0 or p.UserID=@pUserID))
             order by o.ObjectName
             ";

            return ret;
        }

        public override string QueryForSearch(object param)
        {

            string ret = @"select p.PermissionID, p.ObjectPermissionID, o.ObjectName , o.ObjectCode,
             r.RoleID, r.RoleName, u.UserID, u.UserName, p.ReadStatus, p.SaveStatus, p.DeleteStatus, p.TypeGrant
             from sysPermission p
             inner join sysObjectPermission o on o.ObjectPermissionID = p.ObjectPermissionID
             left join sysUser u on u.UserID = p.UserID          
             left join sysRole r on r.RoleID = p.RoleID 
             where 1=1
             and (@pObjectPermissionID=0 or p.ObjectPermissionID=@pObjectPermissionID)
             and (@pRoleID=0 or p.RoleID=@pRoleID)
             and (@pUserID=0 or p.UserID=@pUserID)
             order by o.ObjectName , r.RoleID, u.UserID
             ";

            return ret;

        }

        public string QueryForGetPermissionsByRoleUser(object param)
        {

            string ret = @"select p.PermissionID, p.ObjectPermissionID, o.ObjectName , o.ObjectCode,
             r.RoleID, r.RoleName, u.UserID, u.UserName, p.ReadStatus, p.SaveStatus, p.DeleteStatus, p.TypeGrant
             from sysPermission p
             inner join sysObjectPermission o on o.ObjectPermissionID = p.ObjectPermissionID
             left join sysUser u on u.UserID = p.UserID          
             left join sysRole r on r.RoleID = p.RoleID 
             where 1=1             
             and ((@pRoleID=0 or p.RoleID=@pRoleID) or (@pUserID=0 or p.UserID=@pUserID))             
             order by o.ObjectName , r.RoleID, u.UserID
             ";

            return ret;

        }

    }
}
