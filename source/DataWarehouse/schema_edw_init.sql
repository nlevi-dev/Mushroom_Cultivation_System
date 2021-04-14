/* Create Tables */
USE MushroomDWH
GO


CREATE SCHEMA [edw]
GO

CREATE TABLE edw.[bridge_entry]
(
    B_ID int IDENTITY(1,1) not null,
	[bridge_key] int NOT NULL
)
GO

CREATE TABLE edw.[dim_mushroom]
(
    M_ID int IDENTITY(1,1) not null,
	[type_key] int NOT NULL,
	[mushroom_name] nvarchar(32) NOT NULL,
	[mushroom_genus] nvarchar(32) NOT NULL
)


CREATE TABLE edw.[dim_sensor_entry]
(
    Sensor_ID int IDENTITY(1,1) not null,
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


CREATE TABLE edw.[dim_status_entry]
(
    Status_ID int IDENTITY(1,1) not null,
	[entry_key] int NOT NULL,
	[entry_time] datetime2 NOT NULL,
	[mushroom_stage] nvarchar(32) NOT NULL,
	[specimen_key] int NOT NULL
)








CREATE TABLE edw.[fact_specimen]
(
    B_ID int NOT NULL,
    M_ID int NOT NULL,
    Sensor_ID int NOT NULL,
    Status_ID int NOT NULL,
    Specimen_ID int IDENTITY(1,1) NOT NULL,
	[specimen_key] int NOT NULL,
	[active] bit NOT NULL,
	[planted_date] date NOT NULL,
	[discraded_date] date NOT NULL,
	[type_key] int NOT NULL,
	[bridge_key] int NOT NULL
)
GO

USE MushroomDWH
GO 
ALTER TABLE  edw.[bridge_entry] ADD CONSTRAINT PK_DimBridgeKey PRIMARY KEY (B_ID);
ALTER TABLE  edw.[dim_mushroom] ADD CONSTRAINT PK_DimMushroomKey PRIMARY KEY (M_ID);
ALTER TABLE  edw.[dim_sensor_entry] ADD CONSTRAINT PK_DimSensor_Entry_Key PRIMARY KEY (Sensor_ID);
ALTER TABLE  edw.[dim_status_entry] ADD CONSTRAINT PK_DimStatus_Entry_Key PRIMARY KEY (Status_ID);


