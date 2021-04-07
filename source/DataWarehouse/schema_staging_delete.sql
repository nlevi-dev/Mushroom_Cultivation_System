USE MushroomPP
GO

IF OBJECT_ID('staging.dim_sensor_entry') IS NOT NULL
	TRUNCATE TABLE staging.dim_sensor_entry
IF OBJECT_ID('staging.dim_sensor_entry') IS NOT NULL
	DROP TABLE staging.dim_sensor_entry
IF OBJECT_ID('staging.dim_status_entry') IS NOT NULL
	TRUNCATE TABLE staging.dim_status_entry
IF OBJECT_ID('staging.dim_status_entry') IS NOT NULL
	DROP TABLE staging.dim_status_entry
IF OBJECT_ID('staging.fact_specimen') IS NOT NULL
	TRUNCATE TABLE staging.fact_specimen
IF OBJECT_ID('staging.fact_specimen') IS NOT NULL
	DROP TABLE staging.fact_specimen
IF OBJECT_ID('staging.bridge_entry') IS NOT NULL
	TRUNCATE TABLE staging.bridge_entry
IF OBJECT_ID('staging.bridge_entry') IS NOT NULL
	DROP TABLE staging.bridge_entry
IF OBJECT_ID('staging.dim_mushroom') IS NOT NULL
	TRUNCATE TABLE staging.dim_mushroom
IF OBJECT_ID('staging.dim_mushroom') IS NOT NULL
	DROP TABLE staging.dim_mushroom
DROP SCHEMA [staging]
GO