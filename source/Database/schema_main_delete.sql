USE MushroomPP
GO

IF OBJECT_ID('main._sensor_entry') IS NOT NULL
	TRUNCATE TABLE main._sensor_entry
IF OBJECT_ID('main._sensor_entry') IS NOT NULL
	DROP TABLE main._sensor_entry
IF OBJECT_ID('main._status_entry') IS NOT NULL
	TRUNCATE TABLE main._status_entry
IF OBJECT_ID('main._status_entry') IS NOT NULL
	DROP TABLE main._status_entry
IF OBJECT_ID('main._specimen') IS NOT NULL
	TRUNCATE TABLE main._specimen
IF OBJECT_ID('main._specimen') IS NOT NULL
	DROP TABLE main._specimen
IF OBJECT_ID('main._mushroom_type') IS NOT NULL
	TRUNCATE TABLE main._mushroom_type
IF OBJECT_ID('main._mushroom_type') IS NOT NULL
	DROP TABLE main._mushroom_type
IF OBJECT_ID('main._mushroom_stage') IS NOT NULL
	TRUNCATE TABLE main._mushroom_stage
IF OBJECT_ID('main._mushroom_stage') IS NOT NULL
	DROP TABLE main._mushroom_stage
IF OBJECT_ID('main._hardware') IS NOT NULL
	TRUNCATE TABLE main._hardware
IF OBJECT_ID('main._hardware') IS NOT NULL
	DROP TABLE main._hardware
IF OBJECT_ID('main._user') IS NOT NULL
	TRUNCATE TABLE main._user
IF OBJECT_ID('main._user') IS NOT NULL
	DROP TABLE main._user
DROP SCHEMA [main]
GO