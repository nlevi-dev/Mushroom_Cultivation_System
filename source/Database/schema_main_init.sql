USE MushroomPP
GO

/* Create Tables */

CREATE TABLE [_hardware]
(
	[hardware_key] int IDENTITY (1, 1) NOT NULL,
	[hardware_id] nvarchar(16) NOT NULL,
	[desired_air_temperature] real NULL,
	[desired_air_humidity] real NULL,
	[desired_air_co2] real NULL,
	[desired_light_level] real NULL,
	[user_key] int NOT NULL
);

CREATE TABLE [_mushroom_stage]
(
	[stage_key] int IDENTITY (1, 1) NOT NULL,
	[stage_name] nvarchar(32) NOT NULL
);

CREATE TABLE [_mushroom_type]
(
	[type_key] int IDENTITY (1, 1) NOT NULL,
	[mushroom_name] nvarchar(32) NOT NULL,
	[mushroom_genus] nvarchar(32) NOT NULL
);

CREATE TABLE [_permission_level]
(
	[permission_key] int IDENTITY (1, 1) NOT NULL,
	[permission_type] nvarchar(16) NOT NULL
);

CREATE TABLE [_sensor_entry]
(
	[entry_key] int IDENTITY (1, 1) NOT NULL,
	[entry_time] datetime2(3) NOT NULL DEFAULT GETDATE(),
	[air_temperature] real NOT NULL,
	[air_humidity] real NOT NULL,
	[air_co2] real NOT NULL,
	[light_level] real NOT NULL,
	[desired_air_temperature] real NULL,
	[desired_air_humidity] real NULL,
	[desired_air_co2] real NULL,
	[desired_light_level] real NULL,
	[specimen_key] int NULL
);

CREATE TABLE [_specimen]
(
	[specimen_key] int IDENTITY (1, 1) NOT NULL,
	[planted_date] datetime2(3) NOT NULL DEFAULT GETDATE(),
	[discarded_date] datetime2(3) NULL,
	[specimen_name] nvarchar(32) NOT NULL,
	[specimen_description] nvarchar(256) NOT NULL,
	[desired_air_temperature] real NULL,
	[desired_air_humidity] real NULL,
	[desired_air_co2] real NULL,
	[desired_light_level] real NULL,
	[type_key] int NOT NULL,
	[hardware_key] int NULL,
	[user_key] int NULL
);

CREATE TABLE [_status_entry]
(
	[entry_key] int IDENTITY (1, 1) NOT NULL,
	[entry_time] datetime2(3) NOT NULL DEFAULT GETDATE(),
	[stage_key] int NOT NULL,
	[specimen_key] int NOT NULL
);

CREATE TABLE [_user]
(
	[user_key] int IDENTITY (1, 1) NOT NULL,
	[username] nvarchar(32) NOT NULL,
	[password_hashed] nvarchar(64) NOT NULL,
	[permission_key] int NOT NULL DEFAULT 1
);

/* Create Primary Keys, Indexes, Uniques, Checks */

ALTER TABLE [_hardware] 
 ADD CONSTRAINT [PK_hardware]
	PRIMARY KEY CLUSTERED ([hardware_key] ASC);

ALTER TABLE [_hardware] 
 ADD CONSTRAINT [hardware_id] UNIQUE NONCLUSTERED ([hardware_id] ASC);

ALTER TABLE [_mushroom_stage] 
 ADD CONSTRAINT [PK_mushroom_stage]
	PRIMARY KEY CLUSTERED ([stage_key] ASC);

ALTER TABLE [_mushroom_stage] 
 ADD CONSTRAINT [mushroom_stage] UNIQUE NONCLUSTERED ([stage_name] ASC);

ALTER TABLE [_mushroom_type] 
 ADD CONSTRAINT [PK_mushroom_type]
	PRIMARY KEY CLUSTERED ([type_key] ASC);

ALTER TABLE [_permission_level] 
 ADD CONSTRAINT [PK_permission_level]
	PRIMARY KEY CLUSTERED ([permission_key] ASC);

ALTER TABLE [_sensor_entry] 
 ADD CONSTRAINT [PK_sensor_entry]
	PRIMARY KEY CLUSTERED ([entry_key] ASC);

ALTER TABLE [_specimen] 
 ADD CONSTRAINT [PK_specimen]
	PRIMARY KEY CLUSTERED ([specimen_key] ASC);

ALTER TABLE [_status_entry] 
 ADD CONSTRAINT [PK_status_entry]
	PRIMARY KEY CLUSTERED ([entry_key] ASC);

ALTER TABLE [_user] 
 ADD CONSTRAINT [PK_user]
	PRIMARY KEY CLUSTERED ([user_key] ASC);

ALTER TABLE [_user] 
 ADD CONSTRAINT [username] UNIQUE NONCLUSTERED ([username] ASC);

/* Create Foreign Key Constraints */

ALTER TABLE [_hardware] ADD CONSTRAINT [FK_hardware_user]
	FOREIGN KEY ([user_key]) REFERENCES [_user] ([user_key]) ON DELETE No Action ON UPDATE No Action;

ALTER TABLE [_sensor_entry] ADD CONSTRAINT [FK_sensor_entry_specimen]
	FOREIGN KEY ([specimen_key]) REFERENCES [_specimen] ([specimen_key]) ON DELETE Cascade ON UPDATE No Action;

ALTER TABLE [_specimen] ADD CONSTRAINT [FK_specimen_hardware]
	FOREIGN KEY ([hardware_key]) REFERENCES [_hardware] ([hardware_key]) ON DELETE No Action ON UPDATE No Action;

ALTER TABLE [_specimen] ADD CONSTRAINT [FK_specimen_mushroom_type]
	FOREIGN KEY ([type_key]) REFERENCES [_mushroom_type] ([type_key]) ON DELETE No Action ON UPDATE No Action;

ALTER TABLE [_specimen] ADD CONSTRAINT [FK_specimen_user]
	FOREIGN KEY ([user_key]) REFERENCES [_user] ([user_key]) ON DELETE No Action ON UPDATE No Action;

ALTER TABLE [_status_entry] ADD CONSTRAINT [FK_status_entry_mushroom_stage]
	FOREIGN KEY ([stage_key]) REFERENCES [_mushroom_stage] ([stage_key]) ON DELETE No Action ON UPDATE No Action;

ALTER TABLE [_status_entry] ADD CONSTRAINT [FK_status_entry_specimen]
	FOREIGN KEY ([specimen_key]) REFERENCES [_specimen] ([specimen_key]) ON DELETE Cascade ON UPDATE No Action;

ALTER TABLE [_user] ADD CONSTRAINT [FK_user_permission_level]
	FOREIGN KEY ([permission_key]) REFERENCES [_permission_level] ([permission_key]) ON DELETE Set Default ON UPDATE No Action;

GO

/* Triggers */

CREATE TRIGGER dbo._user_delete_cascade ON dbo._user
INSTEAD OF DELETE AS
BEGIN
 SET NOCOUNT ON;
 UPDATE dbo._specimen SET user_key = NULL WHERE user_key IN (SELECT user_key FROM DELETED);
 DELETE FROM dbo._hardware WHERE user_key IN (SELECT user_key FROM DELETED);
 DELETE FROM dbo._user WHERE user_key IN (SELECT user_key FROM DELETED);
END
GO

CREATE TRIGGER dbo._hardware_delete_cascade ON dbo._hardware
INSTEAD OF DELETE AS
BEGIN
 SET NOCOUNT ON;
 UPDATE dbo._specimen SET hardware_key = NULL WHERE hardware_key IN (SELECT hardware_key FROM DELETED);
 DELETE FROM dbo._hardware WHERE hardware_key IN (SELECT hardware_key FROM DELETED);
END
GO