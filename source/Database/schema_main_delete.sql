USE MushroomPP
GO

IF OBJECT_ID('dbo._sensor_entry') IS NOT NULL
	TRUNCATE TABLE dbo._sensor_entry
IF OBJECT_ID('dbo._sensor_entry') IS NOT NULL
	DROP TABLE dbo._sensor_entry
IF OBJECT_ID('dbo._status_entry') IS NOT NULL
	TRUNCATE TABLE dbo._status_entry
IF OBJECT_ID('dbo._status_entry') IS NOT NULL
	DROP TABLE dbo._status_entry
IF OBJECT_ID('dbo._specimen') IS NOT NULL
	TRUNCATE TABLE dbo._specimen
IF OBJECT_ID('dbo._specimen') IS NOT NULL
	DROP TABLE dbo._specimen
IF OBJECT_ID('dbo._mushroom_type') IS NOT NULL
	TRUNCATE TABLE dbo._mushroom_type
IF OBJECT_ID('dbo._mushroom_type') IS NOT NULL
	DROP TABLE dbo._mushroom_type
IF OBJECT_ID('dbo._mushroom_stage') IS NOT NULL
	TRUNCATE TABLE dbo._mushroom_stage
IF OBJECT_ID('dbo._mushroom_stage') IS NOT NULL
	DROP TABLE dbo._mushroom_stage
IF OBJECT_ID('dbo._hardware') IS NOT NULL
	TRUNCATE TABLE dbo._hardware
IF OBJECT_ID('dbo._hardware') IS NOT NULL
	DROP TABLE dbo._hardware
IF OBJECT_ID('dbo._user') IS NOT NULL
	TRUNCATE TABLE dbo._user
IF OBJECT_ID('dbo._user') IS NOT NULL
	DROP TABLE dbo._user
GO