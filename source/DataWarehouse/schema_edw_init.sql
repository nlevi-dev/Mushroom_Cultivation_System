USE [MushroomDWH]
GO

CREATE SCHEMA [edw]
GO

/* Create Tables */

CREATE TABLE [edw].[dim_age]
(
	[age_key] int NOT NULL,
	[minute] int NOT NULL
)
GO

CREATE TABLE [edw].[dim_date]
(
	[date_key] int NOT NULL,
	[year] int NOT NULL,
	[season] nvarchar(6) NOT NULL,
	[month] int NOT NULL,
	[month_name] nvarchar(9) NOT NULL,
	[week] int NOT NULL,
	[day] int NOT NULL
)
GO

CREATE TABLE [edw].[dim_specimen]
(
	[specimen_key] int NOT NULL,
	[mushroom_name] nvarchar(32) NOT NULL,
	[mushroom_genus] nvarchar(32) NOT NULL,
	[stage_name] nvarchar(32) NOT NULL,
	[business_key] int NOT NULL
)
GO

CREATE TABLE [edw].[dim_time]
(
	[time_key] int NOT NULL,
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
	[planted_date] int NOT NULL,
	[planted_time] int NOT NULL,
	[entry_date] int NOT NULL,
	[entry_time] int NOT NULL,
	[specimen] int NOT NULL,
	[mushroom_age] int NOT NULL,
	[stage_age] int NOT NULL,
	[business_key] int NOT NULL
)
GO

/* Create Primary Keys, Indexes, Uniques, Checks */

ALTER TABLE [edw].[dim_age] 
 ADD CONSTRAINT [PK_dim_age]
	PRIMARY KEY CLUSTERED ([age_key] ASC)
GO

ALTER TABLE [edw].[dim_date] 
 ADD CONSTRAINT [PK_dim_date]
	PRIMARY KEY CLUSTERED ([date_key] ASC)
GO

ALTER TABLE [edw].[dim_specimen] 
 ADD CONSTRAINT [PK_dim_specimen]
	PRIMARY KEY CLUSTERED ([specimen_key] ASC)
GO

ALTER TABLE [edw].[dim_time] 
 ADD CONSTRAINT [PK_dim_time]
	PRIMARY KEY CLUSTERED ([time_key] ASC)
GO

ALTER TABLE [edw].[fact_cultivation] 
 ADD CONSTRAINT [PK_fact_cultivation]
	PRIMARY KEY CLUSTERED ([planted_date] ASC,[planted_time] ASC,[entry_date] ASC,[entry_time] ASC,[specimen] ASC,[mushroom_age] ASC,[stage_age] ASC)
GO

/* Create Foreign Key Constraints */

ALTER TABLE [edw].[fact_cultivation] ADD CONSTRAINT [FK_fact_cultivation_dim_mushroom_age]
	FOREIGN KEY ([mushroom_age]) REFERENCES [edw].[dim_age] ([age_key]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [edw].[fact_cultivation] ADD CONSTRAINT [FK_fact_cultivation_dim_specimen]
	FOREIGN KEY ([specimen]) REFERENCES [edw].[dim_specimen] ([specimen_key]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [edw].[fact_cultivation] ADD CONSTRAINT [FK_fact_cultivation_dim_stage_age]
	FOREIGN KEY ([stage_age]) REFERENCES [edw].[dim_age] ([age_key]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [edw].[fact_cultivation] ADD CONSTRAINT [FK_fact_cultivation_entry_date]
	FOREIGN KEY ([entry_date]) REFERENCES [edw].[dim_date] ([date_key]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [edw].[fact_cultivation] ADD CONSTRAINT [FK_fact_cultivation_entry_time]
	FOREIGN KEY ([entry_time]) REFERENCES [edw].[dim_time] ([time_key]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [edw].[fact_cultivation] ADD CONSTRAINT [FK_fact_cultivation_planted_date]
	FOREIGN KEY ([planted_date]) REFERENCES [edw].[dim_date] ([date_key]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [edw].[fact_cultivation] ADD CONSTRAINT [FK_fact_cultivation_planted_time]
	FOREIGN KEY ([planted_time]) REFERENCES [edw].[dim_time] ([time_key]) ON DELETE No Action ON UPDATE No Action
GO
