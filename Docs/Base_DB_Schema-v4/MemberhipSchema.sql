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


-- 

CREATE TABLE [dbo].[sysLocalizationText](
	[LocalizationTextID] [bigint] NOT NULL,
	[Language] [varchar](5) NOT NULL,
	[Code] [varchar](10) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Text] [varchar](255) NOT NULL,
 CONSTRAINT [PK_sysLocalizationText] PRIMARY KEY CLUSTERED 
(
	[LocalizationTextID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


-- initialing table sysLocalizationText

insert into sysLocalizationText Values(1,      'en-us',   '1',     'Execution-Error',    'Execution error')
insert into sysLocalizationText Values(2,      'en-us',      '2',     'Validation-Error',    'Data validation error.')
insert into sysLocalizationText Values(3,      'en-us',      '3',     'Record-NotFound',    'The requested record was not found.')
insert into sysLocalizationText Values(4,      'en-us',      '4',     'Login-Invalid-Password',    'Invalid password. You still have 0 attempts before the account is deactivated.')
insert into sysLocalizationText Values(5,      'en-us',      '5',     'Login-Attempts',    'You have already used access attempts and the account has been disabled. Request activation.')
insert into sysLocalizationText Values(6,      'en-us',      '6',     'Login-Inactive-Account',    'The account associated with the User is not active. Request account activation.')
insert into sysLocalizationText Values(7,      'en-us',     '7',     'Login-Locked-Account',    'The account associated with the User is locked out. Contact your system administrator.')
insert into sysLocalizationText Values(8,      'en-us',      '8',     'Login-User-NotFound',    'User not found.')
insert into sysLocalizationText Values(9,      'en-us',      '9',     'User-Exists',    'There is already a user with the email')
insert into sysLocalizationText Values(10,      'en-us',      '10',     'Email-Exists',    'The email you entered already exists.')
insert into sysLocalizationText Values(11,      'en-us',      '11',     'Profile-NotBe-Null',    'The Profile cannot be null.')
insert into sysLocalizationText Values(12,      'en-us',      '12',     'User-Error-Exclude-Childs',    'There was an error deleting child items (Roles).')
insert into sysLocalizationText Values(13,      'en-us',      '13',     'User-Invalid-Password-Code',    'The password exchange authorization code is invalid.')
insert into sysLocalizationText Values(14,      'en-us',      '14',     'Account-Active',    'The account associated with the User is already active.')
insert into sysLocalizationText Values(15,      'en-us',      '15',     'User-Invalid-Activation-Code',    'The activation authorization code is invalid.')
insert into sysLocalizationText Values(16,      'en-us',      '16',     'User-No-Image',    'Send the image file.')
insert into sysLocalizationText Values(17,      'en-us',     '17',     'User-Role-Exists',    'This Role is already associated with the user.')
insert into sysLocalizationText Values(18,      'en-us',      '18',     'User-Role-No-Exists',    'This Role does not belong to the user.')
insert into sysLocalizationText Values(19,      'en-us',      '19',     'Http-Unauthorized',    'Unauthorized access')
insert into sysLocalizationText Values(20,      'en-us',      '20',     'Http-NotFound',    'The resource could not be found')
insert into sysLocalizationText Values(21,      'en-us',      '21',     'Http-Forbidden',    'User profile without access permission.')
insert into sysLocalizationText Values(22,      'en-us',      '22',     'Http-500Error',    'An error occurred in the processing of the request.')
insert into sysLocalizationText Values(23,      'en-us',      '23',     'Http-ServiceUnavailable',    'The requested service is unavailable.')
insert into sysLocalizationText Values(24,      'en-us',      '24',     'API-Unexpected-Exception',    'Unexpected error not identified GetInnerExceptions@f2]')
insert into sysLocalizationText Values(25,      'en-us',      '25',     'ShortDayName-1',    'Sun')
insert into sysLocalizationText Values(26,      'en-us',      '26',     'ShortDayName-2',    'Mon')
insert into sysLocalizationText Values(27,      'en-us',      '27',     'ShortDayName-3',    'Tue')
insert into sysLocalizationText Values(28,      'en-us',      '28',     'ShortDayName-4',    'Wed')
insert into sysLocalizationText Values(29,      'en-us',     '29',     'ShortDayName-5',    'Thu')
insert into sysLocalizationText Values(30,      'en-us',      '30',     'ShortDayName-6',    'Fri')
insert into sysLocalizationText Values(31,      'en-us',     '31',     'ShortDayName-7',    'Sat'  )
insert into sysLocalizationText Values(32,      'en-us',      '32',     'MonthName-1',    'JANUARY')
insert into sysLocalizationText Values(33,      'en-us',      '33',     'MonthName-2',    'FEBRUARY')
insert into sysLocalizationText Values(34,      'en-us',      '34',     'MonthName-3',    'MARCH')
insert into sysLocalizationText Values(35,      'en-us',      '35',     'MonthName-4',    'APRIL')
insert into sysLocalizationText Values(36,      'en-us',      '36',     'MonthName-5',    'MAY')
insert into sysLocalizationText Values(37,      'en-us',      '37',     'MonthName-6',    'JUNE')
insert into sysLocalizationText Values(38,      'en-us',      '38',     'MonthName-7',    'JULY')
insert into sysLocalizationText Values(39,      'en-us',      '39',     'MonthName-8',    'AUGUST')
insert into sysLocalizationText Values(40,      'en-us',      '40',     'MonthName-9',    'SEPTEMBER')
insert into sysLocalizationText Values(41,      'en-us',      '41',     'MonthName-10',    'OCTOBER')
insert into sysLocalizationText Values(42,      'en-us',      '42',     'MonthName-11',    'NOVEMBER')
insert into sysLocalizationText Values(43,      'en-us',      '43',     'MonthName-12',    'DECEMBER')
insert into sysLocalizationText Values(44,      'en-us',      '44',     'Validation-NotNull',    'cannot be null.')
insert into sysLocalizationText Values(45,      'en-us',      '45',     'Validation-Max-Characters',    'The {0} field cannot have more than 1 characters.')
insert into sysLocalizationText Values(46,      'en-us',      '46',     'Validation-Invalid-Field',    'The {0} field is invalid.')
insert into sysLocalizationText Values(47,      'en-us',      '47',     'Validation-Invalid-UserName',    'The {0} field is invalid. Do not use special characters or spaces.')
insert into sysLocalizationText Values(48,      'en-us',      '48',     'Validation-Unique-Value',    'The {0} field is invalid. Value must be unique.')
insert into sysLocalizationText Values(49,      'en-us',      '49',     'User-Instance-Exists',    'This Instance is already associated with the user.')
insert into sysLocalizationText Values(50,      'en-us',      '50',     'User-Instance-No-Exists',    'This Instance does not belong to the user.')
insert into sysLocalizationText Values(51,      'pt-br',      '1',     'Execution-Error',    'Erro de execução ')
insert into sysLocalizationText Values(52,      'pt-br',      '2',     'Validation-Error',    'Erro na validação de dados.')
insert into sysLocalizationText Values(53,      'pt-br',      '3',     'Record-NotFound',    'O registro solicitado não foi encontrado.')
insert into sysLocalizationText Values(54,      'pt-br',      '4',     'Login-Invalid-Password',    'Senha inválida. Você ainda tem {0} tentativas antes da conta ser desativada.')
insert into sysLocalizationText Values(55,      'pt-br',      '5',     'Login-Attempts',    'Você já utilizou as tentativas de acesso e a conta foi desativada. Solicite a ativação.')
insert into sysLocalizationText Values(56,      'pt-br',      '6',     'Login-Inactive-Account',    'A conta associada ao Usuário não está ativa. Solicite a ativação da conta.')
insert into sysLocalizationText Values(57,      'pt-br',      '7',     'Login-Locked-Account',    'A conta associada ao Usuário está bloqueada. Contate o administrador do sistema.')
insert into sysLocalizationText Values(58,      'pt-br',      '8',     'Login-User-NotFound',    'Usuário não encontrado.')
insert into sysLocalizationText Values(59,      'pt-br',      '9',     'User-Exists',    'Já existe um usuário com o e-mail ')
insert into sysLocalizationText Values(60,      'pt-br',      '10',     'Email-Exists',    'O e-mail informado já existe.')
insert into sysLocalizationText Values(61,      'pt-br',      '11',     'Profile-NotBe-Null',    'O Perfil não pode ser nulo.')
insert into sysLocalizationText Values(62,      'pt-br',      '12',     'User-Error-Exclude-Childs',    'Houve um erro ao excluir os itens filhos (Roles).')
insert into sysLocalizationText Values(63,      'pt-br',      '13',     'User-Invalid-Password-Code',    'O código de autorização de troca de senha é inválido.')
insert into sysLocalizationText Values(64,      'pt-br',      '14',     'Account-Active',    'A conta associada ao Usuário já está ativa.')
insert into sysLocalizationText Values(65,      'pt-br',      '15',     'User-Invalid-Activation-Code',    'O código de autorização de ativação é inválido.')
insert into sysLocalizationText Values(66,      'pt-br',      '16',     'User-No-Image',    'Envie o arquivo da imagem.')
insert into sysLocalizationText Values(67,      'pt-br',      '17',     'User-Role-Exists',    'Esta Role já está associada ao usuário.')
insert into sysLocalizationText Values(68,      'pt-br',      '18',     'User-Role-No-Exists',    'Esta Role não pertence ao usuário.')
insert into sysLocalizationText Values(69,      'pt-br',      '19',     'Http-Unauthorized',    'Acesso não autorizado')
insert into sysLocalizationText Values(70,      'pt-br',      '20',     'Http-NotFound',    'O recurso não foi encontrado')
insert into sysLocalizationText Values(71,     'pt-br',      '21',     'Http-Forbidden',    'Perfil do usuário sem permissão de acesso')
insert into sysLocalizationText Values(72,      'pt-br',      '22',     'Http-500Error',    'Ocorreu um erro no processamento da requisição.')
insert into sysLocalizationText Values(73,      'pt-br',      '23',     'Http-ServiceUnavailable',    'O serviço solicitado está indisponível.')
insert into sysLocalizationText Values(74,      'pt-br',      '24',     'API-Unexpected-Exception',    'Erro inesperado não identificado GetInnerExceptions@f2]')
insert into sysLocalizationText Values(75,      'pt-br',      '25',     'ShortDayName-1',    'Dom')
insert into sysLocalizationText Values(76,      'pt-br',      '26',     'ShortDayName-2',    'Seg')
insert into sysLocalizationText Values(77,      'pt-br',      '27',     'ShortDayName-3',    'Ter')
insert into sysLocalizationText Values(78,      'pt-br',      '28',     'ShortDayName-4',    'Qua')
insert into sysLocalizationText Values(79,      'pt-br',     '29',     'ShortDayName-5',    'Qui')
insert into sysLocalizationText Values(80,      'pt-br',      '30',     'ShortDayName-6',    'Sex')
insert into sysLocalizationText Values(81,      'pt-br',      '31',     'ShortDayName-7',    'Sáb')
insert into sysLocalizationText Values(82,      'pt-br',      '32',     'MonthName-1',    'JANEIRO')
insert into sysLocalizationText Values(83,      'pt-br',      '33',     'MonthName-2',    'FEVEREIRO')
insert into sysLocalizationText Values(84,      'pt-br',      '34',     'MonthName-3',    'MARÇO')
insert into sysLocalizationText Values(85,      'pt-br',      '35',     'MonthName-4',    'ABRIL')
insert into sysLocalizationText Values(86,      'pt-br',      '36',     'MonthName-5',    'MAIO')
insert into sysLocalizationText Values(87,      'pt-br',      '37',     'MonthName-6',    'JUNHO')
insert into sysLocalizationText Values(88,      'pt-br',      '38',     'MonthName-7',    'JULHO')
insert into sysLocalizationText Values(89,      'pt-br',      '39',     'MonthName-8',    'AGOSTO')
insert into sysLocalizationText Values(90,      'pt-br',      '40',     'MonthName-9',    'SETEMBRO')
insert into sysLocalizationText Values(91,      'pt-br',      '41',     'MonthName-10',    'OUTUBRO')
insert into sysLocalizationText Values(92,      'pt-br',      '42',     'MonthName-11',    'NOVEMBRO')
insert into sysLocalizationText Values(93,      'pt-br',      '43',     'MonthName-12',    'DEZEMBRO')
insert into sysLocalizationText Values(94,      'pt-br',      '44',     'Validation-NotNull',    'não pode ser nulo.')
insert into sysLocalizationText Values(95,      'pt-br',      '45',     'Validation-Max-Characters',    'O campo {0} não pode ter mais de maxlength caracteres.')
insert into sysLocalizationText Values(96,      'pt-br',      '46',     'Validation-Invalid-Field',    'O campo {0} é inválido.')
insert into sysLocalizationText Values(97,      'pt-br',      '47',     'Validation-Invalid-UserName',    'O campo {0} é inválido. Não use caracteres especiais ou espaços.')
insert into sysLocalizationText Values(98,      'pt-br',      '48',     'Validation-Unique-Value',    'O campo {0} é inválido. O valor informado deve ser único.')
insert into sysLocalizationText Values(99,      'pt-br',      '49',     'User-Instance-Exists',    'Esta Instância já está associada ao Usuário.')
insert into sysLocalizationText Values(100,      'pt-br',      '50',     'User-Instance-No-Exists',    'Esta Instância não pertence ao Usuário.')


