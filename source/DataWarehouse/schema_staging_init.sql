USE MushroomPP
GO

CREATE SCHEMA [staging]
GO

/* Create Tables */

CREATE TABLE staging.[bridge_specimen]
(
	[specimen_key] int NOT NULL
)
GO

CREATE TABLE staging.[dim_hardware]
(
	[hardware_key] int NOT NULL,
	[in_use] bit NOT NULL,
	[last_update] datetime NOT NULL,
	[current_air_temperature] real NOT NULL,
	[current_air_humidity] real NOT NULL,
	[current_air_co2] real NOT NULL,
	[desired_air_humidity] real NOT NULL,
	[user_key] int NOT NULL
)
GO

CREATE TABLE staging.[dim_mushroom_type]
(
	[type_key] int NOT NULL,
	[type_name] nvarchar(32) NOT NULL
)
GO

CREATE TABLE staging.[dim_sensor_entry]
(
	[entry_key] int NOT NULL,
	[entry_time] datetime NOT NULL,
	[air_temperature] real NOT NULL,
	[air_humidity] real NOT NULL,
	[air_co2] real NOT NULL,
	[desired_air_humidity] real NULL,
	[specimen_key] int NOT NULL
)
GO

CREATE TABLE staging.[dim_stage]
(
	[stage_key] int NOT NULL,
	[stage_name] nvarchar(32) NOT NULL
)
GO

CREATE TABLE staging.[dim_status_entry]
(
	[entry_key] int NOT NULL,
	[entry_time] datetime NOT NULL,
	[stage_key] int NOT NULL,
	[specimen_key] int NOT NULL
)
GO

CREATE TABLE staging.[dim_user]
(
	[user_key] int NOT NULL,
	[username] nvarchar(32) NOT NULL
)
GO

CREATE TABLE staging.[fact_specimen]
(
	[specimen_key] int NOT NULL,
	[active] bit NOT NULL,
	[planted_date] datetime NOT NULL,
	[discraded_date] datetime NOT NULL,
	[desired_air_humidity] real NOT NULL,
	[type_key] int NOT NULL,
	[hardware_key] int NULL
)
GO

/* Create Primary Keys, Indexes, Uniques, Checks */

ALTER TABLE staging.[bridge_specimen] 
 ADD CONSTRAINT [PK_bridge_specimen]
	PRIMARY KEY CLUSTERED ([specimen_key] ASC)
GO

ALTER TABLE staging.[dim_hardware] 
 ADD CONSTRAINT [PK_dim_hardware]
	PRIMARY KEY CLUSTERED ([hardware_key] ASC)
GO

ALTER TABLE staging.[dim_mushroom_type] 
 ADD CONSTRAINT [PK_dim_mushroom_type]
	PRIMARY KEY CLUSTERED ([type_key] ASC)
GO

ALTER TABLE staging.[dim_mushroom_type] 
 ADD CONSTRAINT [mushroom_type] UNIQUE NONCLUSTERED ([type_name] ASC)
GO

ALTER TABLE staging.[dim_sensor_entry] 
 ADD CONSTRAINT [PK_dim_sensor_entry]
	PRIMARY KEY CLUSTERED ([entry_key] ASC)
GO

ALTER TABLE staging.[dim_stage] 
 ADD CONSTRAINT [PK_dim_stage]
	PRIMARY KEY CLUSTERED ([stage_key] ASC)
GO

ALTER TABLE staging.[dim_stage] 
 ADD CONSTRAINT [mushroom_stage_name] UNIQUE NONCLUSTERED ([stage_name] ASC)
GO

ALTER TABLE staging.[dim_status_entry] 
 ADD CONSTRAINT [PK_dim_status_entry]
	PRIMARY KEY CLUSTERED ([entry_key] ASC)
GO

ALTER TABLE staging.[dim_user] 
 ADD CONSTRAINT [PK_dim_user]
	PRIMARY KEY CLUSTERED ([user_key] ASC)
GO

ALTER TABLE staging.[dim_user] 
 ADD CONSTRAINT [username] UNIQUE NONCLUSTERED ([username] ASC)
GO

ALTER TABLE staging.[fact_specimen] 
 ADD CONSTRAINT [PK_fact_specimen]
	PRIMARY KEY CLUSTERED ([specimen_key] ASC)
GO

/* Create Foreign Key Constraints */

ALTER TABLE staging.[dim_hardware] ADD CONSTRAINT [FK_dim_hardware_dim_user]
	FOREIGN KEY ([user_key]) REFERENCES staging.[dim_user] ([user_key]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE staging.[dim_sensor_entry] ADD CONSTRAINT [FK_dim_sensor_entry_bridge_specimen]
	FOREIGN KEY ([specimen_key]) REFERENCES staging.[bridge_specimen] ([specimen_key]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE staging.[dim_status_entry] ADD CONSTRAINT [FK_dim_status_entry_bridge_specimen]
	FOREIGN KEY ([specimen_key]) REFERENCES staging.[bridge_specimen] ([specimen_key]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE staging.[dim_status_entry] ADD CONSTRAINT [FK_dim_status_entry_dim_stage]
	FOREIGN KEY ([stage_key]) REFERENCES staging.[dim_stage] ([stage_key]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE staging.[fact_specimen] ADD CONSTRAINT [FK_fact_specimen_bridge_specimen]
	FOREIGN KEY ([specimen_key]) REFERENCES staging.[bridge_specimen] ([specimen_key]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE staging.[fact_specimen] ADD CONSTRAINT [FK_fact_specimen_dim_hardware]
	FOREIGN KEY ([hardware_key]) REFERENCES staging.[dim_hardware] ([hardware_key]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE staging.[fact_specimen] ADD CONSTRAINT [FK_fact_specimen_dim_mushroom_type]
	FOREIGN KEY ([type_key]) REFERENCES staging.[dim_mushroom_type] ([type_key]) ON DELETE No Action ON UPDATE No Action
GO
