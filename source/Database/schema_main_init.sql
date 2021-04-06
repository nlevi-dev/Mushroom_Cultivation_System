USE MushroomPP
GO

/* Create Tables */

CREATE TABLE dbo.[_permission_level]
(
	[permission_key] int IDENTITY (1, 1) NOT NULL,
	[permission_type] nvarchar(16) NOT NULL
)
GO

CREATE TABLE dbo.[_hardware]
(
	[hardware_key] int IDENTITY (1, 1) NOT NULL,
	[hardware_id] nvarchar(16) NOT NULL,
	[desired_air_temperature] real NULL,
	[desired_air_humidity] real NULL,
	[desired_air_co2] real NULL,
	[user_key] int NOT NULL
)
GO

CREATE TABLE dbo.[_mushroom_stage]
(
	[stage_key] int IDENTITY (1, 1) NOT NULL,
	[stage_name] nvarchar(32) NOT NULL
)
GO

CREATE TABLE dbo.[_mushroom_type]
(
	[type_key] int IDENTITY (1, 1) NOT NULL,
	[mushroom_name] nvarchar(32) NOT NULL
)
GO

CREATE TABLE dbo.[_sensor_entry]
(
	[entry_key] int IDENTITY (1, 1) NOT NULL,
	[entry_time] datetime2(3) NOT NULL,
	[air_temperature] real NOT NULL,
	[air_humidity] real NOT NULL,
	[air_co2] real NOT NULL,
	[desired_air_temperature] real NULL,
	[desired_air_humidity] real NULL,
	[desired_air_co2] real NULL,
	[ambient_air_temperature] real NOT NULL,
	[ambient_air_humidity] real NOT NULL,
	[ambient_air_co2] real NOT NULL,
	[specimen_key] int NULL
)
GO

CREATE TABLE dbo.[_specimen]
(
	[specimen_key] int IDENTITY (1, 1) NOT NULL,
	[planted_date] datetime2(3) NOT NULL,
	[discraded_date] datetime2(3) NULL,
	[specimen_name] nvarchar(32) NOT NULL,
	[specimen_description] nvarchar(256) NOT NULL,
	[desired_air_temperature] real NULL,
	[desired_air_humidity] real NULL,
	[desired_air_co2] real NULL,
	[type_key] int NOT NULL,
	[hardware_key] int NULL
)
GO

CREATE TABLE dbo.[_status_entry]
(
	[entry_key] int IDENTITY (1, 1) NOT NULL,
	[entry_time] datetime2(3) NOT NULL,
	[stage_key] int NOT NULL,
	[specimen_key] int NOT NULL
)
GO

CREATE TABLE dbo.[_user]
(
	[user_key] int IDENTITY (1, 1) NOT NULL,
	[username] nvarchar(32) NOT NULL,
	[password_hashed] nvarchar(64) NOT NULL,
	[permission_key] int NOT NULL
)
GO

/* Create Primary Keys, Indexes, Uniques, Checks */

ALTER TABLE dbo.[_hardware] 
 ADD CONSTRAINT [PK_hardware]
	PRIMARY KEY CLUSTERED ([hardware_key] ASC)
GO

ALTER TABLE dbo.[_hardware] 
 ADD CONSTRAINT [hardware_id] UNIQUE NONCLUSTERED ([hardware_id] ASC)
GO

ALTER TABLE dbo.[_mushroom_stage] 
 ADD CONSTRAINT [PK_mushroom_stage]
	PRIMARY KEY CLUSTERED ([stage_key] ASC)
GO

ALTER TABLE dbo.[_mushroom_stage] 
 ADD CONSTRAINT [mushroom_stage] UNIQUE NONCLUSTERED ([stage_name] ASC)
GO

ALTER TABLE dbo.[_mushroom_type] 
 ADD CONSTRAINT [PK_mushroom_type]
	PRIMARY KEY CLUSTERED ([type_key] ASC)
GO

ALTER TABLE dbo.[_mushroom_type] 
 ADD CONSTRAINT [mushroom_type] UNIQUE NONCLUSTERED ([mushroom_name] ASC)
GO

ALTER TABLE dbo.[_sensor_entry] 
 ADD CONSTRAINT [PK_sensor_entry]
	PRIMARY KEY CLUSTERED ([entry_key] ASC)
GO

ALTER TABLE dbo.[_specimen] 
 ADD CONSTRAINT [PK_specimen]
	PRIMARY KEY CLUSTERED ([specimen_key] ASC)
GO

ALTER TABLE dbo.[_status_entry] 
 ADD CONSTRAINT [PK_status_entry]
	PRIMARY KEY CLUSTERED ([entry_key] ASC)
GO

ALTER TABLE dbo.[_user] 
 ADD CONSTRAINT [PK_user]
	PRIMARY KEY CLUSTERED ([user_key] ASC)
GO

ALTER TABLE dbo.[_user] 
 ADD CONSTRAINT [username] UNIQUE NONCLUSTERED ([username] ASC)
GO

ALTER TABLE [_permission_level] 
 ADD CONSTRAINT [PK_permission_level]
	PRIMARY KEY CLUSTERED ([permission_key] ASC)
GO

/* Create Foreign Key Constraints */

ALTER TABLE dbo.[_hardware] ADD CONSTRAINT [FK_hardware_user]
	FOREIGN KEY ([user_key]) REFERENCES dbo.[_user] ([user_key]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE dbo.[_sensor_entry] ADD CONSTRAINT [FK_sensor_entry_specimen]
	FOREIGN KEY ([specimen_key]) REFERENCES dbo.[_specimen] ([specimen_key]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE dbo.[_specimen] ADD CONSTRAINT [FK_specimen_hardware]
	FOREIGN KEY ([hardware_key]) REFERENCES dbo.[_hardware] ([hardware_key]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE dbo.[_specimen] ADD CONSTRAINT [FK_specimen_mushroom_type]
	FOREIGN KEY ([type_key]) REFERENCES dbo.[_mushroom_type] ([type_key]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE dbo.[_status_entry] ADD CONSTRAINT [FK_status_entry_mushroom_stage]
	FOREIGN KEY ([stage_key]) REFERENCES dbo.[_mushroom_stage] ([stage_key]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE dbo.[_status_entry] ADD CONSTRAINT [FK_status_entry_specimen]
	FOREIGN KEY ([specimen_key]) REFERENCES dbo.[_specimen] ([specimen_key]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE dbo.[_user] ADD CONSTRAINT [FK_user_permission_level]
	FOREIGN KEY ([permission_key]) REFERENCES dbo.[_permission_level] ([permission_key]) ON DELETE No Action ON UPDATE No Action
GO

/* Defaults */

ALTER TABLE dbo.[_user] ADD CONSTRAINT [DEFAULT_user_permission_level]
	DEFAULT 1 FOR [permission_key] 
GO

/* Triggers */

CREATE TRIGGER dbo._user_delete_cascade ON dbo._user
INSTEAD OF DELETE AS
BEGIN
 SET NOCOUNT ON;
 DELETE FROM dbo._hardware WHERE user_key IN (SELECT user_key FROM DELETED)
 DELETE FROM dbo._user WHERE user_key IN (SELECT user_key FROM DELETED)
END
GO

CREATE TRIGGER dbo._hardware_delete_cascade ON dbo._hardware
INSTEAD OF DELETE AS
BEGIN
 SET NOCOUNT ON;
 UPDATE dbo._specimen SET hardware_key = NULL WHERE hardware_key IN (SELECT hardware_key FROM DELETED)
 DELETE FROM dbo._hardware WHERE hardware_key IN (SELECT hardware_key FROM DELETED)
END
GO

/*specimens can only be discarded, not deleted, thus it doesnt need triggers*/
