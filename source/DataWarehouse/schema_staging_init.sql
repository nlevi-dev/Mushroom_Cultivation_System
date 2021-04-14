USE MushroomDWH
GO

CREATE SCHEMA [staging]
GO

/* Create Tables */

CREATE TABLE staging.[bridge_entry]
(
	[bridge_key] int NOT NULL
)
GO

CREATE TABLE staging.[dim_mushroom]
(
	[type_key] int NOT NULL,
	[mushroom_name] nvarchar(32) NOT NULL,
	[mushroom_genus] nvarchar(32) NOT NULL
)
GO

CREATE TABLE staging.[dim_sensor_entry]
(
	[entry_key] int NOT NULL,
	[entry_time] datetime2 NOT NULL,
	[air_temperature] real NOT NULL,
	[air_humidity] real NOT NULL,
	[air_co2] real NOT NULL,
	[desired_air_temperature] real NULL,
	[desired_air_humidity] real NULL,
	[desired_air_co2] real NULL,
	[ambient_air_temperature] real NOT NULL,
	[ambient_air_humidity] real NOT NULL,
	[ambient_air_co2] real NOT NULL,
	[specimen_key] int NOT NULL
)
GO

CREATE TABLE staging.[dim_status_entry]
(
	[entry_key] int NOT NULL,
	[entry_time] datetime2 NOT NULL,
	[mushroom_stage] nvarchar(32) NOT NULL,
	[specimen_key] int NOT NULL
)
GO

CREATE TABLE staging.[fact_specimen]
(
	[specimen_key] int NOT NULL,
	[active] bit NOT NULL,
	[planted_date] date NOT NULL,
	[discraded_date] date NOT NULL,
	[type_key] int NOT NULL,
	[bridge_key] int NOT NULL
)
GO

/* Create Primary Keys, Indexes, Uniques, Checks */

ALTER TABLE staging.[bridge_entry] 
 ADD CONSTRAINT [PK_bridge_entry]
	PRIMARY KEY CLUSTERED ([bridge_key] ASC)
GO

ALTER TABLE staging.[dim_mushroom] 
 ADD CONSTRAINT [PK_dim_mushroom]
	PRIMARY KEY CLUSTERED ([type_key] ASC)
GO

ALTER TABLE staging.[dim_sensor_entry] 
 ADD CONSTRAINT [PK_dim_sensor_entry]
	PRIMARY KEY CLUSTERED ([entry_key] ASC)
GO

ALTER TABLE staging.[dim_status_entry] 
 ADD CONSTRAINT [PK_dim_status_entry]
	PRIMARY KEY CLUSTERED ([entry_key] ASC)
GO

ALTER TABLE staging.[fact_specimen] 
 ADD CONSTRAINT [PK_fact_specimen]
	PRIMARY KEY CLUSTERED ([specimen_key] ASC)
GO

/* Create Foreign Key Constraints */

ALTER TABLE staging.[dim_sensor_entry] ADD CONSTRAINT [FK_dim_sensor_entry_bridge_entry]
	FOREIGN KEY ([specimen_key]) REFERENCES staging.[bridge_entry] ([bridge_key]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE staging.[dim_status_entry] ADD CONSTRAINT [FK_dim_status_entry_bridge_entry]
	FOREIGN KEY ([specimen_key]) REFERENCES staging.[bridge_entry] ([bridge_key]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE staging.[fact_specimen] ADD CONSTRAINT [FK_fact_specimen_bridge_entry]
	FOREIGN KEY ([bridge_key]) REFERENCES staging.[bridge_entry] ([bridge_key]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE staging.[fact_specimen] ADD CONSTRAINT [FK_fact_specimen_dim_mushroom]
	FOREIGN KEY ([type_key]) REFERENCES staging.[dim_mushroom] ([type_key]) ON DELETE No Action ON UPDATE No Action
GO