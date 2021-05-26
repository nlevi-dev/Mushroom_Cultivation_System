USE MushroomDWH
go

/*---load----*/

insert into edw.dim_specimen(
mushroom_name,
mushroom_genus,
stage_name,
specimen_key
)
select a.mushroom_name,a.mushroom_genus,a.stage_name,a.specimen_key
from staging.dim_specimen as a
WHERE NOT EXISTS (SELECT stage_name, specimen_key FROM edw.dim_specimen)

/*--- dim fact table ---*/

insert into edw.fact_cultivation(
air_temperature,
air_humidity,
air_co2,
light_level,
planted_date,
planted_time,
entry_date,
entry_time,
specimen,
mushroom_age,
stage_age,
stage_name
)
select a.air_temperature,
a.air_humidity,
a.air_co2,
a.light_level,
(select convert(date,a.planted_date)) as planted_date,
(select convert(time,a.planted_date)) as planted_time,
(select convert(date,a.entry_time)) as entry_date,
(select convert(time,a.entry_time)) as entry_time,
a.specimen,
(select DATEDIFF(MINUTE,a.planted_date,a.entry_time)) as mushroom_age,
(select DATEDIFF(MINUTE,b.entry_time,a.entry_time)) as stage_age,
a.stage_name
from  staging.fact_cultivation as a 
left join staging.dim_specimen as b on a.specimen = b.specimen_key and a.stage_name = b.stage_name
where a.entry_time > b.entry_time 
and NOT EXISTS (SELECT specimen_key, entry_time FROM edw.fact_cultivation)