USE [MushroomDWH]
GO

CREATE SCHEMA [update_log]
GO

CREATE SCHEMA [edw]
GO

/* Create Tables */

CREATE TABLE [update_log].[edw_logs]
(
	[Table] nvarchar(50) NULL,
	[LastLoadDate] int NULL
)
GO

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
	PD_ID INT NOT NULL,
	PT_ID INT NOT NULL,
	ED_ID INT NOT NULL,
	ET_ID INT NOT NULL,
	SPE_ID INT NOT NULL,
	MUA_ID INT NOT NULL,
	STA_ID INT NOT NULL,
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

ALTER TABLE [edw].[fact_cultivation] 
 ADD CONSTRAINT [PK_fact_cultivation]
	PRIMARY KEY CLUSTERED ([PD_ID] ASC,[PT_ID] ASC,[ED_ID] ASC,[ET_ID] ASC,[SPE_ID] ASC,[MUA_ID] ASC,[STA_ID] ASC)
GO

/* Create Foreign Key Constraints */

ALTER TABLE [edw].[fact_cultivation] 
ALTER COLUMN MUA_ID INT NOT NULL

ALTER TABLE [edw].[fact_cultivation] 
ALTER COLUMN STA_ID INT NOT NULL

ALTER TABLE [edw].[fact_cultivation] 
ALTER COLUMN ED_ID INT NOT NULL

ALTER TABLE [edw].[fact_cultivation] 
ALTER COLUMN ET_ID INT NOT NULL

ALTER TABLE [edw].[fact_cultivation] 
ALTER COLUMN PD_ID INT NOT NULL

ALTER TABLE [edw].[fact_cultivation] 
ALTER COLUMN PT_ID INT NOT NULL

ALTER TABLE [edw].[fact_cultivation] 
ALTER COLUMN SPE_ID INT NOT NULL

ALTER TABLE [edw].[fact_cultivation] ADD CONSTRAINT [FK_fact_cultivation_dim_mushroom_age]
	FOREIGN KEY ([MUA_ID]) REFERENCES [edw].[dim_age] ([age_ID]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [edw].[fact_cultivation] ADD CONSTRAINT [FK_fact_cultivation_dim_specimen]
	FOREIGN KEY ([SPE_ID]) REFERENCES [edw].[dim_specimen] ([spe_ID]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [edw].[fact_cultivation] ADD CONSTRAINT [FK_fact_cultivation_dim_stage_age]
	FOREIGN KEY ([STA_ID]) REFERENCES [edw].[dim_age] ([age_ID]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [edw].[fact_cultivation] ADD CONSTRAINT [FK_fact_cultivation_entry_date]
	FOREIGN KEY ([ED_ID]) REFERENCES [edw].[dim_date] ([date_ID]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [edw].[fact_cultivation] ADD CONSTRAINT [FK_fact_cultivation_entry_time]
	FOREIGN KEY ([ET_ID]) REFERENCES [edw].[dim_time] ([time_ID]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [edw].[fact_cultivation] ADD CONSTRAINT [FK_fact_cultivation_planted_date]
	FOREIGN KEY ([PD_ID]) REFERENCES [edw].[dim_date] ([date_ID]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [edw].[fact_cultivation] ADD CONSTRAINT [FK_fact_cultivation_planted_time]
	FOREIGN KEY ([PT_ID]) REFERENCES [edw].[dim_time] ([time_ID]) ON DELETE No Action ON UPDATE No Action
GO
