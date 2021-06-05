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
	[entry_time] datetime2(3) NOT NULL
)
GO

CREATE TABLE [staging].[fact_cultivation]
(
	[planted_date] datetime2(3) NOT NULL,
	[entry_time] datetime2(3) NOT NULL,
	[air_temperature] real NOT NULL,
	[air_humidity] real NOT NULL,
	[air_co2] real NOT NULL,
	[light_level] real NOT NULL,
	[specimen] int NOT NULL,
	[stage_name] nvarchar(32) NULL
)
GO

