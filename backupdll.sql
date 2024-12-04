-- DROP SCHEMA dbo;

CREATE SCHEMA dbo;
-- master.dbo.Homes definition

-- Drop table

-- DROP TABLE master.dbo.Homes;

CREATE TABLE master.dbo.Homes (
	HomeId int IDENTITY(1,1) NOT NULL,
	Address nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Latitude float NOT NULL,
	Longitude float NOT NULL,
	MaxMembers int NOT NULL,
	HomeName nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK_Homes PRIMARY KEY (HomeId)
);


-- master.dbo.Notification definition

-- Drop table

-- DROP TABLE master.dbo.Notification;

CREATE TABLE master.dbo.Notification (
	NotificationId int IDENTITY(1,1) NOT NULL,
	EventName nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	HardwareId int NOT NULL,
	CreatedAt nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK_Notification PRIMARY KEY (NotificationId)
);


-- master.dbo.Users definition

-- Drop table

-- DROP TABLE master.dbo.Users;

CREATE TABLE master.dbo.Users (
	UserId int IDENTITY(1,1) NOT NULL,
	FirstName nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	LastName nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Email nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ImageUrl nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Password nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Role] int NOT NULL,
	RegisterDate datetime2 NOT NULL,
	CONSTRAINT PK_Users PRIMARY KEY (UserId)
);


-- master.dbo.[__EFMigrationsHistory] definition

-- Drop table

-- DROP TABLE master.dbo.[__EFMigrationsHistory];

CREATE TABLE master.dbo.[__EFMigrationsHistory] (
	MigrationId nvarchar(150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ProductVersion nvarchar(32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK___EFMigrationsHistory PRIMARY KEY (MigrationId)
);


-- master.dbo.spt_fallback_db definition

-- Drop table

-- DROP TABLE master.dbo.spt_fallback_db;

CREATE TABLE master.dbo.spt_fallback_db (
	xserver_name varchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	xdttm_ins datetime NOT NULL,
	xdttm_last_ins_upd datetime NOT NULL,
	xfallback_dbid smallint NULL,
	name varchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	dbid smallint NOT NULL,
	status smallint NOT NULL,
	version smallint NOT NULL
);


-- master.dbo.spt_fallback_dev definition

-- Drop table

-- DROP TABLE master.dbo.spt_fallback_dev;

CREATE TABLE master.dbo.spt_fallback_dev (
	xserver_name varchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	xdttm_ins datetime NOT NULL,
	xdttm_last_ins_upd datetime NOT NULL,
	xfallback_low int NULL,
	xfallback_drive char(2) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	low int NOT NULL,
	high int NOT NULL,
	status smallint NOT NULL,
	name varchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	phyname varchar(127) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
);


-- master.dbo.spt_fallback_usg definition

-- Drop table

-- DROP TABLE master.dbo.spt_fallback_usg;

CREATE TABLE master.dbo.spt_fallback_usg (
	xserver_name varchar(30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	xdttm_ins datetime NOT NULL,
	xdttm_last_ins_upd datetime NOT NULL,
	xfallback_vstart int NULL,
	dbid smallint NOT NULL,
	segmap int NOT NULL,
	lstart int NOT NULL,
	sizepg int NOT NULL,
	vstart int NOT NULL
);


-- master.dbo.spt_monitor definition

-- Drop table

-- DROP TABLE master.dbo.spt_monitor;

CREATE TABLE master.dbo.spt_monitor (
	lastrun datetime NOT NULL,
	cpu_busy int NOT NULL,
	io_busy int NOT NULL,
	idle int NOT NULL,
	pack_received int NOT NULL,
	pack_sent int NOT NULL,
	connections int NOT NULL,
	pack_errors int NOT NULL,
	total_read int NOT NULL,
	total_write int NOT NULL,
	total_errors int NOT NULL
);


-- master.dbo.Companies definition

-- Drop table

-- DROP TABLE master.dbo.Companies;

CREATE TABLE master.dbo.Companies (
	CompanyId int IDENTITY(1,1) NOT NULL,
	Name nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	LogotypeUrl nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Rut int NOT NULL,
	CompanyOwnerId int NOT NULL,
	DeviceValidator nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT PK_Companies PRIMARY KEY (CompanyId),
	CONSTRAINT FK_Companies_Users_CompanyOwnerId FOREIGN KEY (CompanyOwnerId) REFERENCES master.dbo.Users(UserId) ON DELETE CASCADE
);
 CREATE  UNIQUE NONCLUSTERED INDEX IX_Companies_CompanyOwnerId ON dbo.Companies (  CompanyOwnerId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- master.dbo.Devices definition

-- Drop table

-- DROP TABLE master.dbo.Devices;

CREATE TABLE master.dbo.Devices (
	DeviceId int IDENTITY(1,1) NOT NULL,
	Name nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	DeviceType int NOT NULL,
	Model nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Description nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ImageUrl nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CompanyId int NOT NULL,
	IsInterior bit NULL,
	IsExterior bit NULL,
	CanDetectMotion bit NULL,
	CanDetectPerson bit NULL,
	IsTurnedOn bit NULL,
	CONSTRAINT PK_Devices PRIMARY KEY (DeviceId),
	CONSTRAINT FK_Devices_Companies_CompanyId FOREIGN KEY (CompanyId) REFERENCES master.dbo.Companies(CompanyId) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_Devices_CompanyId ON dbo.Devices (  CompanyId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- master.dbo.HomeMember definition

-- Drop table

-- DROP TABLE master.dbo.HomeMember;

CREATE TABLE master.dbo.HomeMember (
	HomeId int NOT NULL,
	UserId int NOT NULL,
	CanAddDevice bit NOT NULL,
	CanListDevices bit NOT NULL,
	CanReceiveNotifications bit NOT NULL,
	CONSTRAINT PK_HomeMember PRIMARY KEY (HomeId,UserId),
	CONSTRAINT FK_HomeMember_Homes_HomeId FOREIGN KEY (HomeId) REFERENCES master.dbo.Homes(HomeId) ON DELETE CASCADE,
	CONSTRAINT FK_HomeMember_Users_UserId FOREIGN KEY (UserId) REFERENCES master.dbo.Users(UserId) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_HomeMember_UserId ON dbo.HomeMember (  UserId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- master.dbo.HomeMemberNotification definition

-- Drop table

-- DROP TABLE master.dbo.HomeMemberNotification;

CREATE TABLE master.dbo.HomeMemberNotification (
	NotificationId int NOT NULL,
	HomeId int NOT NULL,
	UserId int NOT NULL,
	[Read] bit NOT NULL,
	CONSTRAINT PK_HomeMemberNotification PRIMARY KEY (HomeId,UserId,NotificationId),
	CONSTRAINT FK_HomeMemberNotification_HomeMember_HomeId_UserId FOREIGN KEY (HomeId,UserId) REFERENCES master.dbo.HomeMember(HomeId,UserId) ON DELETE CASCADE,
	CONSTRAINT FK_HomeMemberNotification_Notification_NotificationId FOREIGN KEY (NotificationId) REFERENCES master.dbo.Notification(NotificationId) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_HomeMemberNotification_NotificationId ON dbo.HomeMemberNotification (  NotificationId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- master.dbo.Room definition

-- Drop table

-- DROP TABLE master.dbo.Room;

CREATE TABLE master.dbo.Room (
	RoomId int IDENTITY(1,1) NOT NULL,
	RoomName nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	HomeId int NOT NULL,
	CONSTRAINT PK_Room PRIMARY KEY (RoomId),
	CONSTRAINT FK_Room_Homes_HomeId FOREIGN KEY (HomeId) REFERENCES master.dbo.Homes(HomeId) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_Room_HomeId ON dbo.Room (  HomeId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- master.dbo.Sessions definition

-- Drop table

-- DROP TABLE master.dbo.Sessions;

CREATE TABLE master.dbo.Sessions (
	Token nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	UserId int NOT NULL,
	Expires datetime2 NOT NULL,
	CONSTRAINT PK_Sessions PRIMARY KEY (Token),
	CONSTRAINT FK_Sessions_Users_UserId FOREIGN KEY (UserId) REFERENCES master.dbo.Users(UserId) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_Sessions_UserId ON dbo.Sessions (  UserId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- master.dbo.HomeDevices definition

-- Drop table

-- DROP TABLE master.dbo.HomeDevices;

CREATE TABLE master.dbo.HomeDevices (
	HardwareId int IDENTITY(1,1) NOT NULL,
	HomeId int NOT NULL,
	RoomId int NULL,
	DeviceId int NOT NULL,
	IsConnected bit NOT NULL,
	IsOpen bit NULL,
	DeviceName nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK_HomeDevices PRIMARY KEY (HardwareId),
	CONSTRAINT FK_HomeDevices_Devices_DeviceId FOREIGN KEY (DeviceId) REFERENCES master.dbo.Devices(DeviceId) ON DELETE CASCADE,
	CONSTRAINT FK_HomeDevices_Homes_HomeId FOREIGN KEY (HomeId) REFERENCES master.dbo.Homes(HomeId) ON DELETE CASCADE,
	CONSTRAINT FK_HomeDevices_Room_RoomId FOREIGN KEY (RoomId) REFERENCES master.dbo.Room(RoomId)
);
 CREATE NONCLUSTERED INDEX IX_HomeDevices_DeviceId ON dbo.HomeDevices (  DeviceId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_HomeDevices_HomeId ON dbo.HomeDevices (  HomeId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_HomeDevices_RoomId ON dbo.HomeDevices (  RoomId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- dbo.spt_values source

create view spt_values as
select name collate database_default as name,
	number,
	type collate database_default as type,
	low, high, status
from sys.spt_values;