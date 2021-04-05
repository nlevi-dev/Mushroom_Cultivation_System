USE MushroomPP
GO

IF OBJECT_ID('dwh.dim_sensor_entry') IS NOT NULL
	TRUNCATE TABLE dwh.dim_sensor_entry
IF OBJECT_ID('dwh.dim_sensor_entry') IS NOT NULL
	DROP TABLE dwh.dim_sensor_entry
IF OBJECT_ID('dwh.dim_status_entry') IS NOT NULL
	TRUNCATE TABLE dwh.dim_status_entry
IF OBJECT_ID('dwh.dim_status_entry') IS NOT NULL
	DROP TABLE dwh.dim_status_entry
IF OBJECT_ID('dwh.fact_specimen') IS NOT NULL
	TRUNCATE TABLE dwh.fact_specimen
IF OBJECT_ID('dwh.fact_specimen') IS NOT NULL
	DROP TABLE dwh.fact_specimen
IF OBJECT_ID('dwh.bridge_specimen') IS NOT NULL
	TRUNCATE TABLE dwh.bridge_specimen
IF OBJECT_ID('dwh.bridge_specimen') IS NOT NULL
	DROP TABLE dwh.bridge_specimen
IF OBJECT_ID('dwh.dim_mushroom_type') IS NOT NULL
	TRUNCATE TABLE dwh.dim_mushroom_type
IF OBJECT_ID('dwh.dim_mushroom_type') IS NOT NULL
	DROP TABLE dwh.dim_mushroom_type
IF OBJECT_ID('dwh.dim_mushroom_stage') IS NOT NULL
	TRUNCATE TABLE dwh.dim_mushroom_stage
IF OBJECT_ID('dwh.dim_mushroom_stage') IS NOT NULL
	DROP TABLE dwh.dim_mushroom_stage
IF OBJECT_ID('dwh.dim_hardware') IS NOT NULL
	TRUNCATE TABLE dwh.dim_hardware
IF OBJECT_ID('dwh.dim_hardware') IS NOT NULL
	DROP TABLE dwh.dim_hardware
IF OBJECT_ID('dwh.dim_user') IS NOT NULL
	TRUNCATE TABLE dwh.dim_user
IF OBJECT_ID('dwh.dim_user') IS NOT NULL
	DROP TABLE dwh.dim_user
DROP SCHEMA [dwh]
GO