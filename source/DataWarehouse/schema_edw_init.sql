USE [MushroomDWH]
GO

CREATE SCHEMA [edw]
GO

/* Create Tables */

CREATE TABLE [edw].[dim_age]
(
	[age_ID] int IDENTITY (1, 1) NOT NULL,
	[minute] int NOT NULL
)
GO

CREATE TABLE [edw].[dim_date]
(
	[date_ID] int IDENTITY (1, 1) NOT NULL,
	[year] int NOT NULL,
	[season] nvarchar(6) NOT NULL,
	[month] int NOT NULL,
	[month_name] nvarchar(9) NOT NULL,
	[day_of_month] int NOT NULL,
	[week] int NOT NULL,
	[day_of_week] int NOT NULL
)
GO

CREATE TABLE [edw].[dim_specimen]
(
	[spe_ID] int IDENTITY (1, 1) NOT NULL,
	[mushroom_name] nvarchar(32) NOT NULL,
	[mushroom_genus] nvarchar(32) NOT NULL,
	[stage_name] nvarchar(32) NOT NULL,
	[specimen_key] int NOT NULL,
	[entry_time] datetime2(3) NOT NULL
)
GO

CREATE TABLE [edw].[dim_time]
(
	[time_ID] int IDENTITY (1, 1) NOT NULL,
	[time_of_day] nvarchar(16) NOT NULL,
	[hour] int NOT NULL,
	[minute] int NOT NULL
)
GO

CREATE TABLE [edw].[fact_cultivation]
(
	[air_temperature] real NOT NULL,
	[air_humidity] real NOT NULL,
	[air_co2] real NOT NULL,
	[light_level] real NOT NULL,
	PD_ID INT NULL,
	PT_ID INT NULL,
	ED_ID INT NULL,
	ET_ID INT NULL,
	SPE_ID INT NULL,
	MUA_ID INT NULL,
	STA_ID INT NULL,
	[planted_date] date NOT NULL,
	[planted_time] time NOT NULL,
	[entry_date] date NOT NULL,
	[entry_time] time NOT NULL,
	[specimen] int NOT NULL,
	[mushroom_age] int NOT NULL,
	[stage_age] int NOT NULL,
	[stage_name] nvarchar(32) NOT NULL
)
GO

/* Create Primary Keys, Indexes, Uniques, Checks */

ALTER TABLE [edw].[dim_age] 
 ADD CONSTRAINT [PK_dim_age]
	PRIMARY KEY CLUSTERED ([age_ID] ASC)
GO

ALTER TABLE [edw].[dim_date] 
 ADD CONSTRAINT [PK_dim_date]
	PRIMARY KEY CLUSTERED ([date_ID] ASC)
GO

ALTER TABLE [edw].[dim_specimen] 
 ADD CONSTRAINT [PK_dim_specimen]
	PRIMARY KEY CLUSTERED ([spe_ID] ASC)
GO

ALTER TABLE [edw].[dim_time] 
 ADD CONSTRAINT [PK_dim_time]
	PRIMARY KEY CLUSTERED ([time_ID] ASC)
GO






