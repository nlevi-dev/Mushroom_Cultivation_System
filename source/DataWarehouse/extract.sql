USE MushroomDWH
GO

alter table staging.dim_mushroom alter column mushroom_genus nvarchar(32) null 


insert into staging.dim_mushroom
(
type_key,
mushroom_name

)
select type_key,mushroom_name from MushroomPP.dbo._mushroom_type;


insert into staging.dim_sensor_entry
(
entry_key,
entry_time,
air_temperature,
air_humidity,
air_co2,
desired_air_temperature,
desired_air_humidity,
desired_air_co2,
ambient_air_temperature,
ambient_air_humidity,
ambient_air_co2,
specimen_key
)select entry_key,
entry_time,
air_temperature,
air_humidity,
air_co2,
desired_air_temperature,
desired_air_humidity,
desired_air_co2,
ambient_air_temperature,
ambient_air_humidity,
ambient_air_co2,
specimen_key from MushroomPP.dbo._sensor_entry;



insert into staging.dim_status_entry
(
entry_key,
entry_time,
mushroom_stage
)
select entry_key,entry_time,stage_name from MushroomPP.dbo._status_entry as a
inner join MushroomPP.dbo._mushroom_stage as b on a.stage_key=b.stage_key
 

insert into staging.bridge_entry
(bridge_key)
select entry_key from staging.dim_status_entry;



insert into staging.fact_specimen
(
specimen_key,
active,
planted_date,
discraded_date,
type_key,
bridge_key
)
select a.specimen_key,(Case when discraded_date is null then 1 else 0 end),planted_date,discraded_date,type_key,entry_key
from MushroomPP.dbo._specimen as a inner join MushroomPP.dbo._status_entry as b on a.specimen_key=b.specimen_key;
 

