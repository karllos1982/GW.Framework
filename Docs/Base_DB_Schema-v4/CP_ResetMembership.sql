-- # GW.FRAMEWORK v4.0 PROJECT
-- Procedure for initialization of Membership tables (GW.Membership.dll)
-- Use for tests executions (runs this proc before execute tests)

-- drop procedure [dbo].[sp_ResetMembership]

CREATE PROCEDURE [dbo].[sp_ResetMembership]
AS
BEGIN
	
	
	delete from sysDataLog
	delete from sysSession	
	delete from sysPermission
	delete from sysObjectPermission
	delete from sysUserRoles
	delete from  sysUserInstances
	delete from sysRole
	delete from sysInstance
	delete from sysUser
	

	insert into [sysInstance] values (1,'Default','Default',1,GetDate())

	insert into [sysRole] values (1,'SuperAdmin',GetDate(),1)
	insert into [sysRole] values (2,'Admin',GetDate(),1)

	insert into [sysUser] values (1001,1,'master.admin','master.user@sys.com',
		'03273b3cc1a8e4c9811ac88ee757275f','GWSLT',GetDate(),
		1,0,'en-us',null,null,0,0,null,null,null,null,null,null)

	insert into [sysUserRoles] values (1,1001,1)
	insert into [sysUserInstances] values (1,1001,1)

	-- 

	insert into [sysUser] values (1002,1,'deleted.user','deleted.user@sys.com',
		'03273b3cc1a8e4c9811ac88ee757275f','GWSLT',GetDate(),
		1,0,'en-us',null,null,0,0,null,null,null,null,null,null)

	insert into [sysUserRoles] values (2,1002,1)
	insert into [sysUserInstances] values (2,1002,1)

	-- 

	insert into [sysUser] values (1003,1,'simple.user','simple.user@sys.com',
		'03273b3cc1a8e4c9811ac88ee757275f','GWSLT',GetDate(),
		1,0,'en-us',null,null,0,0,null,null,null,null,null,null)

	insert into [sysUserRoles] values (3,1003,2)
	insert into [sysUserInstances] values (3,1003,1)

	--

	insert into sysObjectPermission values (10001, 'Table.SysUser.Basic', 'SYSUSER')
	insert into sysObjectPermission values (10002, 'Table.SysRole.Basic', 'SYSROLE')
	insert into sysObjectPermission values (10003, 'Table.SysObjectPermission.Basic', 'SYSOBJECTPERMISSION')
	insert into sysObjectPermission values (10004, 'Table.SysPermission.Basic', 'SYSPERMISSION')
	insert into sysObjectPermission values (10005, 'Table.sysSession.Basic', 'SYSSESSION')
	insert into sysObjectPermission values (10006, 'Table.sysDataLog.Basic', 'SYSDATALOG')
	insert into sysObjectPermission values (10007, 'Table.SysInstance.Basic', 'SYSINSTANCE')

	insert into sysPermission values (10001, 10001, 1, null, 1,1,1,'R')
	insert into sysPermission values (10002, 10002, 1, null, 1,1,1,'R')
	insert into sysPermission values (10003, 10003, 1, null, 1,1,1,'R')
	insert into sysPermission values (10004, 10004, 1, null, 1,1,1,'R')
	insert into sysPermission values (10005, 10005, 1, null, 1,1,1,'R')
	insert into sysPermission values (10006, 10006, 1, null, 1,1,1,'R')
	insert into sysPermission values (10007, 10007, 1, null, 1,1,1,'R')

END



GO

