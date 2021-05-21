USE [MushroomDWH]
GO

/*нн-нн------extraction--------*/


insert into staging.dim_specimen(
specimen_key,
mushroom_name,
mushroom_genus,
stage_name,
entry_time
)
select a.specimen_key,b.mushroom_name,b.mushroom_genus,d.stage_name 
from MushroomPP.dbo._specimen as a
inner join MushroomPP.dbo._mushroom_type as b on a.type_key=b.type_key
inner join MushroomPP.dbo._status_entry as c on a.specimen_key=c.specimen_key
inner join MushroomPP.dbo._mushroom_stage as d on c.stage_key=d.stage_key


insert into staging.fact_cultivation(
planted_date,
entry_time,
air_temperature,
air_humidity,
air_co2,
light_level,
specimen
)
select
a.planted_date,
b.entry_time,
b.air_temperature,
b.air_humidity,
b.air_co2,
b.light_level,
a.specimen_key
from MushroomPP.dbo._specimen as a
inner join MushroomPP.dbo._sensor_entry as b on a.specimen_key=b.specimen_key
