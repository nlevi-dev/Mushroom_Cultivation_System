USE MushroomPP
GO

insert into dbo._user
(username,password_hashed,permission_key,user_token) values
(
'Admin',
'123456',
1,
'Token1'
)

insert into dbo._hardware(
hardware_id,
desired_air_temperature,
desired_air_humidity,
desired_air_co2,
desired_light_level,
user_key
) values
(
'0004A30B002528D3',
71.7911682128906,
21.588134765625,
392,
392,
1
)

/*---  asume 2 specimens     ---*/
insert into dbo._specimen(
planted_date,
discarded_date,
specimen_name,
specimen_description,
desired_air_temperature,
desired_air_humidity,
desired_air_co2,
desired_light_level,
type_key,
hardware_key,
user_key
)
values 
(
'2021-03-08 13:05:13.000',
'2023-03-08 13:05:13.000',
'specimen_no_1',
'I have no idea',
21.7,
1.2,
1.2,
1.2,
1,
3,
1
)

insert into dbo._specimen(
planted_date,
discarded_date,
specimen_name,
specimen_description,
desired_air_temperature,
desired_air_humidity,
desired_air_co2,
desired_light_level,
type_key,
hardware_key,
user_key
)
values 
(
'2021-03-08 13:04:13.000',
'2024-03-08 13:04:13.000',
'specimen_no_2',
'I have no idea 2',
21.7,
1.2,
1.2,
1.2,
2,
3,
1
)

insert into dbo._status_entry(entry_time,stage_key,specimen_key)values('2021-03-08 13:05:13.000',1,7)
insert into dbo._status_entry(entry_time,stage_key,specimen_key)values('2021-03-12 13:05:13.000',2,7)
insert into dbo._status_entry(entry_time,stage_key,specimen_key)values('2021-03-20 13:05:13.000',3,7)
insert into dbo._status_entry(entry_time,stage_key,specimen_key)values('2021-03-28 13:05:13.000',4,7)
insert into dbo._status_entry(entry_time,stage_key,specimen_key)values('2021-04-04 13:05:13.000',5,7)
insert into dbo._status_entry(entry_time,stage_key,specimen_key)values('2021-04-10 13:05:13.000',6,7)
insert into dbo._status_entry(entry_time,stage_key,specimen_key)values('2021-03-08 13:04:13.000',1,8)
insert into dbo._status_entry(entry_time,stage_key,specimen_key)values('2021-03-15 13:05:13.000',2,8)
insert into dbo._status_entry(entry_time,stage_key,specimen_key)values('2021-03-23 13:05:13.000',3,8)
insert into dbo._status_entry(entry_time,stage_key,specimen_key)values('2021-03-28 13:05:13.000',4,8)
insert into dbo._status_entry(entry_time,stage_key,specimen_key)values('2021-04-02 13:05:13.000',5,8)
insert into dbo._status_entry(entry_time,stage_key,specimen_key)values('2021-04-07 13:05:13.000',6,8)

insert into dbo._sensor_entry
(
entry_time,
air_temperature,
air_humidity,
air_co2,
light_level,
desired_air_temperature,
desired_air_humidity,
desired_air_co2,
desired_light_level,
specimen_key)
select
[Date Time, GMT +0100],
[Temp, �F],
[RH, %],
[CO2, ppm],
[CO2, ppm],
[Temp, �F],
[RH, %],
[CO2, ppm],
[CO2, ppm],
(select 7 as specimen_key)
from dbo.[001]


insert into dbo._sensor_entry
(
entry_time,
air_temperature,
air_humidity,
air_co2,
light_level,
desired_air_temperature,
desired_air_humidity,
desired_air_co2,
desired_light_level,
specimen_key)
select
[Date Time, GMT +0100],
[Temp, �F],
[RH, %],
[CO2, ppm],
[CO2, ppm],
[Temp, �F],
[RH, %],
[CO2, ppm],
[CO2, ppm],
(select 8 as specimen_key)
from dbo.[001]
