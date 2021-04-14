--Populate EDW Tables
--Populate Dim Mushroom
USE MushroomDWH
GO

insert into edw.dim_mushroom(type_key,mushroom_name,mushroom_genus)

select type_key,mushroom_name,mushroom_genus from staging.dim_mushroom

--Populate  Bridge Entry
USE MushroomDWH
GO

insert into edw.bridge_entry(bridge_key)

select bridge_key from staging.bridge_entry

--Populate EDW Sensor Entry
USE MushroomDWH
GO

insert into edw.dim_sensor_entry(entry_key,entry_time, air_temperature, air_humidity, air_co2, desired_air_temperature, desired_air_humidity, desired_air_co2, ambient_air_temperature, ambient_air_humidity, ambient_air_co2, )

select type_key,mushroom_name,mushroom_genus from staging.dim_mushroom
