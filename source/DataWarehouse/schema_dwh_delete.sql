USE MushroomDWH
GO

IF OBJECT_ID('dbo.dim_sensor_entry') IS NOT NULL
	TRUNCATE TABLE dbo.dim_sensor_entry
IF OBJECT_ID('dbo.dim_sensor_entry') IS NOT NULL
	DROP TABLE dbo.dim_sensor_entry
IF OBJECT_ID('dbo.dim_status_entry') IS NOT NULL
	TRUNCATE TABLE dbo.dim_status_entry
IF OBJECT_ID('dbo.dim_status_entry') IS NOT NULL
	DROP TABLE dbo.dim_status_entry
IF OBJECT_ID('dbo.fact_specimen') IS NOT NULL
	TRUNCATE TABLE dbo.fact_specimen
IF OBJECT_ID('dbo.fact_specimen') IS NOT NULL
	DROP TABLE dbo.fact_specimen
IF OBJECT_ID('dbo.bridge_entry') IS NOT NULL
	TRUNCATE TABLE dbo.bridge_entry
IF OBJECT_ID('dbo.bridge_entry') IS NOT NULL
	DROP TABLE dbo.bridge_entry
IF OBJECT_ID('dbo.dim_mushroom') IS NOT NULL
	TRUNCATE TABLE dbo.dim_mushroom
IF OBJECT_ID('dbo.dim_mushroom') IS NOT NULL
	DROP TABLE dbo.dim_mushroom

GO