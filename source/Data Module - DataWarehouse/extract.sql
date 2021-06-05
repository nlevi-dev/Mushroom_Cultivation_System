USE [MushroomDWH]
GO

/*нн-нн------wipe staging--------*/

TRUNCATE TABLE staging.fact_cultivation;
TRUNCATE TABLE staging.dim_specimen;
GO

/*нн-нн------extraction--------*/
INSERT INTO staging.dim_specimen(
specimen_key,
mushroom_name,
mushroom_genus,
stage_name,
entry_time
)
SELECT a.specimen_key,b.mushroom_name,b.mushroom_genus,d.stage_name, c.entry_time
FROM MushroomPP.dbo._specimen AS a
INNER JOIN MushroomPP.dbo._mushroom_type AS b ON a.type_key=b.type_key
INNER JOIN MushroomPP.dbo._status_entry AS c ON a.specimen_key=c.specimen_key
INNER JOIN MushroomPP.dbo._mushroom_stage AS d ON c.stage_key=d.stage_key


INSERT INTO staging.fact_cultivation(
planted_date,
entry_time,
air_temperature,
light_level,
air_humidity,
air_co2,
specimen
)
SELECT
a.planted_date,
b.entry_time,
b.air_temperature,
b.light_level,
b.air_humidity,
b.air_co2,
a.specimen_key
FROM MushroomPP.dbo._sensor_entry AS b
LEFT JOIN MushroomPP.dbo._specimen AS a ON a.specimen_key=b.specimen_key
