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
IF OBJECT_ID('staging.bridge_specimen') IS NOT NULL
	TRUNCATE TABLE staging.bridge_specimen
IF OBJECT_ID('staging.bridge_specimen') IS NOT NULL
	DROP TABLE staging.bridge_specimen
IF OBJECT_ID('staging.dim_mushroom_type') IS NOT NULL
	TRUNCATE TABLE staging.dim_mushroom_type
IF OBJECT_ID('staging.dim_mushroom_type') IS NOT NULL
	DROP TABLE staging.dim_mushroom_type
IF OBJECT_ID('staging.dim_mushroom_stage') IS NOT NULL
	TRUNCATE TABLE staging.dim_mushroom_stage
IF OBJECT_ID('staging.dim_mushroom_stage') IS NOT NULL
	DROP TABLE staging.dim_mushroom_stage
IF OBJECT_ID('staging.dim_hardware') IS NOT NULL
	TRUNCATE TABLE staging.dim_hardware
IF OBJECT_ID('staging.dim_hardware') IS NOT NULL
	DROP TABLE staging.dim_hardware
IF OBJECT_ID('staging.dim_user') IS NOT NULL
	TRUNCATE TABLE staging.dim_user
IF OBJECT_ID('staging.dim_user') IS NOT NULL
	DROP TABLE staging.dim_user
DROP SCHEMA [staging]
GO