-- # GW.FRAMEWORK v4.0 PROJECT
-- Scripts Create Tables for Membership Module (GW.Membership.dll)


-- GO
	
	drop table sysDataLog
	drop table sysSession	
	drop table sysPermission
	drop table sysObjectPermission
	drop table sysUserRoles
	drop table sysUserInstances
	drop table sysInstance
	drop table sysRole
	drop table sysUser

/****** Object:  Table [dbo].[sysRole]    Script Date: 21/10/2022 19:40:13 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[sysRole](
	[RoleID] [bigint] NOT NULL,
	[RoleName] [varchar](30) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[IsActive] [tinyint] NOT NULL,
 CONSTRAINT [pk_sysRole] PRIMARY KEY CLUSTERED 
(
	[RoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

-- 

CREATE TABLE [dbo].[sysObjectPermission](
	[ObjectPermissionID] [bigint] NOT NULL,
	[ObjectName] [varchar](100) NOT NULL,
	[ObjectCode] [varchar](25) NOT NULL,
 CONSTRAINT [PK_sysObjectPermission] PRIMARY KEY CLUSTERED 
(
	[ObjectPermissionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

-- 

CREATE TABLE [dbo].[sysInstance](
	[InstanceID] [bigint] NOT NULL,
	[InstanceTypeName] [varchar](50) NOT NULL,
	[InstanceName] [varchar](100) NOT NULL,
	[IsActive] [tinyint] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_sysInstance] PRIMARY KEY CLUSTERED 
(
	[InstanceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

-- 


CREATE TABLE [dbo].[sysUser](
	[UserID] [bigint] NOT NULL,
	[ApplicationID] [bigint] NOT NULL,	
	[UserName] [varchar](50) NOT NULL,	
	[Email] [varchar](100) NOT NULL,
	[Password] [varchar](100) NOT NULL,
	[Salt] [varchar](10) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[IsActive] [tinyint] NOT NULL,
	[IsLocked] [tinyint] NOT NULL,
	[DefaultLanguage] [varchar](5) NOT NULL,
	[LastLoginDate] [datetime] NULL,
	[LastLoginIP] [varchar](30) NULL,
	[LoginCounter] [int] NULL,
	[LoginFailCounter] [int] NULL,
	[Avatar] [image] NULL,
	[AuthCode] [varchar](max) NULL,
	[AuthCodeExpires] [datetime] NULL,
	[PasswordRecoveryCode] [varchar](45) NULL,
	[ProfileImage] [varchar](255) NULL,
	[AuthUserID] [varchar](50) NULL,
 CONSTRAINT [pk_sysUser] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO



-- 

CREATE TABLE [dbo].[sysUserInstances](
	[UserInstanceID] [bigint] NOT NULL,
	[UserID] [bigint] NOT NULL,
	[InstanceID] [bigint] NOT NULL,
 CONSTRAINT [PK_sysUserInstances] PRIMARY KEY CLUSTERED 
(
	[UserInstanceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[sysUserInstances]  WITH NOCHECK ADD  CONSTRAINT [fk_sysUserInstances_instance] FOREIGN KEY([InstanceID])
REFERENCES [dbo].[sysInstance] ([InstanceID])
GO

ALTER TABLE [dbo].[sysUserInstances] CHECK CONSTRAINT [fk_sysUserInstances_instance]
GO

ALTER TABLE [dbo].[sysUserInstances]  WITH NOCHECK ADD  CONSTRAINT [fk_sysUserInstances_user] FOREIGN KEY([UserID])
REFERENCES [dbo].[sysUser] ([UserID])
GO

ALTER TABLE [dbo].[sysUserInstances] CHECK CONSTRAINT [fk_sysUserInstances_user]
GO


-- 

CREATE TABLE [dbo].[sysUserRoles](
	[UserRoleID] [bigint] NOT NULL,
	[UserID] [bigint] NOT NULL,
	[RoleID] [bigint] NOT NULL,
 CONSTRAINT [PK_sysUserRoles] PRIMARY KEY CLUSTERED 
(
	[UserRoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[sysUserRoles]  WITH NOCHECK ADD  CONSTRAINT [fk_sysUserRoles_role] FOREIGN KEY([RoleID])
REFERENCES [dbo].[sysRole] ([RoleID])
GO

ALTER TABLE [dbo].[sysUserRoles] CHECK CONSTRAINT [fk_sysUserRoles_role]
GO

ALTER TABLE [dbo].[sysUserRoles]  WITH NOCHECK ADD  CONSTRAINT [fk_sysUserRoles_user] FOREIGN KEY([UserID])
REFERENCES [dbo].[sysUser] ([UserID])
GO

ALTER TABLE [dbo].[sysUserRoles] CHECK CONSTRAINT [fk_sysUserRoles_user]
GO


-- 


CREATE TABLE [dbo].[sysPermission](
	[PermissionID] [bigint] NOT NULL,
	[ObjectPermissionID] [bigint] NOT NULL,
	[RoleID] [bigint] NULL,
	[UserID] [bigint] NULL,
	[ReadStatus] [int] NOT NULL,
	[SaveStatus] [int] NOT NULL,
	[DeleteStatus] [int] NOT NULL,
	[TypeGrant] [varchar](1) NULL,
 CONSTRAINT [PK_sysPermission] PRIMARY KEY CLUSTERED 
(
	[PermissionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[sysPermission]  WITH NOCHECK ADD  CONSTRAINT [fk_sysPermission_object] FOREIGN KEY([ObjectPermissionID])
REFERENCES [dbo].[sysObjectPermission] ([ObjectPermissionID])
GO

ALTER TABLE [dbo].[sysPermission] CHECK CONSTRAINT [fk_sysPermission_object]
GO

ALTER TABLE [dbo].[sysPermission]  WITH NOCHECK ADD  CONSTRAINT [fk_sysPermission_role] FOREIGN KEY([RoleID])
REFERENCES [dbo].[sysRole] ([RoleID])
GO

ALTER TABLE [dbo].[sysPermission] CHECK CONSTRAINT [fk_sysPermission_role]
GO

ALTER TABLE [dbo].[sysPermission]  WITH NOCHECK ADD  CONSTRAINT [fk_sysPermission_user] FOREIGN KEY([UserID])
REFERENCES [dbo].[sysUser] ([UserID])
GO

ALTER TABLE [dbo].[sysPermission] CHECK CONSTRAINT [fk_sysPermission_user]
GO


--


CREATE TABLE [dbo].[sysSession](
	[SessionID] [bigint] NOT NULL,
	[UserID] [bigint] NOT NULL,
	[Date] [datetime] NOT NULL,
	[IP] [varchar](25) NULL,
	[BrowserName] [varchar](100) NULL,
	[DateLogout] [datetime] NULL,
 CONSTRAINT [pk_sysSesssion] PRIMARY KEY CLUSTERED 
(
	[SessionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


-- 


CREATE TABLE [dbo].[sysDataLog](
	[DataLogID] [bigint] NOT NULL,
	[UserID] [bigint] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Operation] [varchar](1) NOT NULL,
	[TableName] [varchar](50) NOT NULL,
	[ID] [bigint] NULL,
	[LogOldData] [varchar](max) NULL,
	[LogCurrentData] [varchar](max) NULL,
 CONSTRAINT [pk_sysDataLog] PRIMARY KEY CLUSTERED 
(
	[DataLogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[sysDataLog]  WITH CHECK ADD  CONSTRAINT [fk_sysDataLog_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[sysUser] ([UserID])
GO

ALTER TABLE [dbo].[sysDataLog] CHECK CONSTRAINT [fk_sysDataLog_User]
GO

