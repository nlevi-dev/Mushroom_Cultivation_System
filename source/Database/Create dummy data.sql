USE MushroomPP
GO

insert into dbo._user
(username,password_hashed,permission_key) values
(
'Admin',
'123456',
1
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
1,
21.588134765625,
10,
392,
10,
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
21.588134765625,
10,
392,
10,
1,
1,
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
21.588134765625,
10,
392,
10,
2,
1,
1
)



insert into dbo._status_entry(entry_time,stage_key,specimen_key)values('2021-03-08 13:05:13.000',1,1)
insert into dbo._status_entry(entry_time,stage_key,specimen_key)values('2021-03-12 13:05:13.000',2,1)
insert into dbo._status_entry(entry_time,stage_key,specimen_key)values('2021-03-20 13:05:13.000',3,1)
insert into dbo._status_entry(entry_time,stage_key,specimen_key)values('2021-03-28 13:05:13.000',4,1)
insert into dbo._status_entry(entry_time,stage_key,specimen_key)values('2021-04-04 13:05:13.000',5,1)
insert into dbo._status_entry(entry_time,stage_key,specimen_key)values('2021-04-10 13:05:13.000',6,1)


insert into dbo._status_entry(entry_time,stage_key,specimen_key)values('2021-03-08 13:04:13.000',1,2)
insert into dbo._status_entry(entry_time,stage_key,specimen_key)values('2021-03-15 13:05:13.000',2,2)
insert into dbo._status_entry(entry_time,stage_key,specimen_key)values('2021-03-23 13:05:13.000',3,2)
insert into dbo._status_entry(entry_time,stage_key,specimen_key)values('2021-03-28 13:05:13.000',4,2)
insert into dbo._status_entry(entry_time,stage_key,specimen_key)values('2021-04-02 13:05:13.000',5,2)
insert into dbo._status_entry(entry_time,stage_key,specimen_key)values('2021-04-07 13:05:13.000',6,2)

/*update [001] set [specimen_key] = 2 where [specimen_key] is null*/

insert into dbo._sensor_entry(entry_time,air_temperature,air_humidity,air_co2,light_level,desired_air_temperature,
desired_air_humidity,desired_air_co2,desired_light_level,specimen_key)
select 
[Date Time, GMT +0100],[RH, %],[CO2, ppm],[CO2, ppm],[CO2, ppm],[RH, %],[CO2, ppm],[CO2, ppm],[CO2, ppm],(select 1 as specimen_key)
from dbo.[001] 

insert into dbo._sensor_entry(entry_time,air_temperature,air_humidity,air_co2,light_level,desired_air_temperature,
desired_air_humidity,desired_air_co2,desired_light_level,specimen_key)
select 
[Date Time, GMT +0100],[RH, %],[CO2, ppm],[CO2, ppm],[CO2, ppm],[RH, %],[CO2, ppm],[CO2, ppm],[CO2, ppm],(select 2 as specimen_key)
from dbo.[001] 

