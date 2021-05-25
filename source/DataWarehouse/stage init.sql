
USE [MushroomDWH]
GO

CREATE SCHEMA [staging]
GO

/* Create Tables */

CREATE TABLE [staging].[dim_specimen]
(
	[specimen_key] int NOT NULL,
	[mushroom_name] nvarchar(32) NULL,
	[mushroom_genus] nvarchar(32) NULL,
	[stage_name] nvarchar(32) NOT NULL,
	[entry_time] datetime2 NOT NULL
)
GO

CREATE TABLE [staging].[fact_cultivation]
(
	[planted_date] datetime2(3) NULL,
	[entry_time] datetime2(3) NOT NULL,
	[air_temperature] real NOT NULL,
	[air_humidity] real NOT NULL,
	[air_co2] real NOT NULL,
	[light_level] real NOT NULL,
	[specimen] int NOT NULL,
	[stage_name] nvarchar(32) NOT NULL
)
GO

/* Create Primary Keys, Indexes, Uniques, Checks */

ALTER TABLE [staging].[dim_specimen] 
    ADD CONSTRAINT [PK_dim_specimen] 
	PRIMARY KEY ([specimen_key],[entry_time])
GO

ALTER TABLE [staging].[fact_cultivation] 
 ADD CONSTRAINT [PK_fact_cultivation]
	PRIMARY KEY CLUSTERED ([entry_time] ASC,[specimen] ASC)
GO

/* Create Foreign Key Constraints */

ALTER TABLE [staging].[fact_cultivation] ADD CONSTRAINT [FK_fact_cultivation_dim_specimen]
	FOREIGN KEY ([specimen]) REFERENCES [staging].[dim_specimen] ([specimen_key]) ON DELETE No Action ON UPDATE No Action
GO

