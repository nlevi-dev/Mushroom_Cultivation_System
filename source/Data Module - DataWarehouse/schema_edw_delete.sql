USE [MushroomDWH]
GO

IF OBJECT_ID('edw.fact_cultivation') IS NOT NULL
	TRUNCATE TABLE edw.fact_cultivation;
IF OBJECT_ID('edw.fact_cultivation') IS NOT NULL
	DROP TABLE edw.fact_cultivation;
IF OBJECT_ID('edw.dim_time') IS NOT NULL
	TRUNCATE TABLE edw.dim_time;
IF OBJECT_ID('edw.dim_time') IS NOT NULL
	DROP TABLE edw.dim_time;
IF OBJECT_ID('edw.dim_date') IS NOT NULL
	TRUNCATE TABLE edw.dim_date;
IF OBJECT_ID('edw.dim_date') IS NOT NULL
	DROP TABLE edw.dim_date;
IF OBJECT_ID('edw.dim_age') IS NOT NULL
	TRUNCATE TABLE edw.dim_age;
IF OBJECT_ID('edw.dim_age') IS NOT NULL
	DROP TABLE edw.dim_age;
IF OBJECT_ID('edw.dim_specimen') IS NOT NULL
	TRUNCATE TABLE edw.dim_specimen;
IF OBJECT_ID('edw.dim_specimen') IS NOT NULL
	DROP TABLE edw.dim_specimen;
IF OBJECT_ID('update_log.edw_logs') IS NOT NULL
	TRUNCATE TABLE update_log.edw_logs;
IF OBJECT_ID('update_log.edw_logs') IS NOT NULL
	DROP TABLE update_log.edw_logs;
GO
DROP SCHEMA edw;
DROP SCHEMA update_log;
GO