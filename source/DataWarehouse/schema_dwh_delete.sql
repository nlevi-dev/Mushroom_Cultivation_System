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
IF OBJECT_ID('dwh.bridge_entry') IS NOT NULL
	TRUNCATE TABLE dwh.bridge_entry
IF OBJECT_ID('dwh.bridge_entry') IS NOT NULL
	DROP TABLE dwh.bridge_entry
IF OBJECT_ID('dwh.dim_mushroom') IS NOT NULL
	TRUNCATE TABLE dwh.dim_mushroom
IF OBJECT_ID('dwh.dim_mushroom') IS NOT NULL
	DROP TABLE dwh.dim_mushroom
DROP SCHEMA [dwh]
GO