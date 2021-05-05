USE [MushroomDWH]
GO

IF OBJECT_ID('staging.fact_cultivation') IS NOT NULL
	TRUNCATE TABLE staging.fact_cultivation;
IF OBJECT_ID('staging.fact_cultivation') IS NOT NULL
	DROP TABLE staging.fact_cultivation;
IF OBJECT_ID('staging.dim_time') IS NOT NULL
	TRUNCATE TABLE staging.dim_time;
IF OBJECT_ID('staging.dim_time') IS NOT NULL
	DROP TABLE staging.dim_time;
IF OBJECT_ID('staging.dim_date') IS NOT NULL
	TRUNCATE TABLE staging.dim_date;
IF OBJECT_ID('staging.dim_date') IS NOT NULL
	DROP TABLE staging.dim_date;
IF OBJECT_ID('staging.dim_age') IS NOT NULL
	TRUNCATE TABLE staging.dim_age;
IF OBJECT_ID('staging.dim_age') IS NOT NULL
	DROP TABLE staging.dim_age;
IF OBJECT_ID('staging.dim_specimen') IS NOT NULL
	TRUNCATE TABLE staging.dim_specimen;
IF OBJECT_ID('staging.dim_specimen') IS NOT NULL
	DROP TABLE staging.dim_specimen;	
GO
DROP SCHEMA staging;
GO