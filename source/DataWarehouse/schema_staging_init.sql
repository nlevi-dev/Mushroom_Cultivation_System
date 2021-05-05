USE [MushroomDWH]
GO

CREATE SCHEMA [staging]
GO

/* Create Tables */

CREATE TABLE [staging].[dim_age]
(
	[age_key] int IDENTITY (1, 1) NOT NULL,
	[minute] int NOT NULL
)
GO

CREATE TABLE [staging].[dim_date]
(
	[date_key] int IDENTITY (1, 1) NOT NULL,
	[year] int NOT NULL,
	[season] nvarchar(6) NOT NULL,
	[month] int NOT NULL,
	[month_name] nvarchar(9) NOT NULL,
	[week] int NOT NULL,
	[day] int NOT NULL
)
GO

CREATE TABLE [staging].[dim_specimen]
(
	[specimen_key] int IDENTITY (1, 1) NOT NULL,
	[mushroom_name] nvarchar(32) NULL,
	[mushroom_genus] nvarchar(32) NULL,
	[stage_name] nvarchar(32) NULL,
	[business_key] int NOT NULL
)
GO

CREATE TABLE [staging].[dim_time]
(
	[time_key] int IDENTITY (1, 1) NOT NULL,
	[time_of_day] nvarchar(16) NOT NULL,
	[hour] int NOT NULL,
	[minute] int NOT NULL
)
GO

CREATE TABLE [staging].[fact_cultivation]
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
	[business_key] int NOT NULL,
	[old_entry_time] datetime2(3) NOT NULL,
	[old_planted_date] datetime2(3) NULL
)
GO

/* Create Primary Keys, Indexes, Uniques, Checks */

ALTER TABLE [staging].[dim_age] 
 ADD CONSTRAINT [PK_dim_age]
	PRIMARY KEY CLUSTERED ([age_key] ASC)
GO

ALTER TABLE [staging].[dim_date] 
 ADD CONSTRAINT [PK_dim_date]
	PRIMARY KEY CLUSTERED ([date_key] ASC)
GO

ALTER TABLE [staging].[dim_specimen] 
 ADD CONSTRAINT [PK_dim_specimen]
	PRIMARY KEY CLUSTERED ([specimen_key] ASC)
GO

ALTER TABLE [staging].[dim_time] 
 ADD CONSTRAINT [PK_dim_time]
	PRIMARY KEY CLUSTERED ([time_key] ASC)
GO

ALTER TABLE [staging].[fact_cultivation] 
 ADD CONSTRAINT [PK_fact_cultivation]
	PRIMARY KEY CLUSTERED ([planted_date] ASC,[planted_time] ASC,[entry_date] ASC,[entry_time] ASC,[specimen] ASC,[mushroom_age] ASC,[stage_age] ASC)
GO

/* Create Foreign Key Constraints */

ALTER TABLE [staging].[fact_cultivation] ADD CONSTRAINT [FK_fact_cultivation_dim_mushroom_age]
	FOREIGN KEY ([mushroom_age]) REFERENCES [staging].[dim_age] ([age_key]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [staging].[fact_cultivation] ADD CONSTRAINT [FK_fact_cultivation_dim_specimen]
	FOREIGN KEY ([specimen]) REFERENCES [staging].[dim_specimen] ([specimen_key]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [staging].[fact_cultivation] ADD CONSTRAINT [FK_fact_cultivation_dim_stage_age]
	FOREIGN KEY ([stage_age]) REFERENCES [staging].[dim_age] ([age_key]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [staging].[fact_cultivation] ADD CONSTRAINT [FK_fact_cultivation_entry_date]
	FOREIGN KEY ([entry_date]) REFERENCES [staging].[dim_date] ([date_key]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [staging].[fact_cultivation] ADD CONSTRAINT [FK_fact_cultivation_entry_time]
	FOREIGN KEY ([entry_time]) REFERENCES [staging].[dim_time] ([time_key]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [staging].[fact_cultivation] ADD CONSTRAINT [FK_fact_cultivation_planted_date]
	FOREIGN KEY ([planted_date]) REFERENCES [staging].[dim_date] ([date_key]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [staging].[fact_cultivation] ADD CONSTRAINT [FK_fact_cultivation_planted_time]
	FOREIGN KEY ([planted_time]) REFERENCES [staging].[dim_time] ([time_key]) ON DELETE No Action ON UPDATE No Action
GO
