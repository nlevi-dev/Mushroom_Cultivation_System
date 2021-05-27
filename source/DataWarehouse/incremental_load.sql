USE MushroomDWH
go


--log for updates
CREATE SCHEMA etl;

CREATE TABLE etl.[LogUpdate]
(
	[Table] nvarchar(50) NULL,
	[LastLoadDate] int NULL
) on [PRIMARY]



/*---load into dim_specimen----*/

insert into edw.dim_specimen(
mushroom_name,
mushroom_genus,
stage_name,
specimen_key,
entry_time
)
select mushroom_name,mushroom_genus,stage_name,specimen_key,entry_time
from staging.dim_specimen as a
where not exists (select 1 from edw.dim_specimen as b where b.specimen_key =a.specimen_key
and b.stage_name = a.stage_name)

--------note changes into log table-----------
INSERT into etl.LogUpdate([Table],LastLoadDate)
			VALUES  ('dim_specimen',convert(char(8),GETDATE(),112)
)
				

/*--- load into dim fact table ---*/
insert into edw.fact_cultivation(
air_temperature,
air_humidity,
air_co2,
light_level,
PD_ID,
PT_ID,
ED_ID,
ET_ID,
SPE_ID,
MUA_ID,
STA_ID,
planted_date,
planted_time,
entry_date,
entry_time,
specimen,
mushroom_age,
stage_age,
stage_name
)
select 
a.air_temperature,
a.air_humidity,
a.air_co2,
a.light_level,
e.date_ID as PD_ID,
f.time_ID as PT_ID,
c.date_ID as ED_ID,
d.time_ID as ET_ID,
b.spe_ID as SPE_ID,
g.age_ID as MUA_ID,
h.age_ID as STA_ID,
(select convert(date,a.planted_date)) as planted_date,
(select convert(time,a.planted_date)) as planted_time,
(select convert(date,a.entry_time)) as entry_date,
(select convert(time,a.entry_time)) as entry_time,
a.specimen,
g.minute as mushroom_age,
h.minute as stage_age,
a.stage_name
from  staging.fact_cultivation as a 
inner join 
edw.dim_specimen as b 
on a.specimen = b.specimen_key and a.stage_name = b.stage_name
left join 
edw.dim_date as c
on DATEPART(month,a.entry_time)
= c.month
and DATEPART(year,a.entry_time)
= c.year
and DATEPART(day,a.entry_time)
= c.day_of_month
inner join 
edw.dim_time as d
on datepart(hour,a.entry_time) = d.hour and 
datepart(minute,a.entry_time) = d.minute 
inner join 
edw.dim_date as e
on datepart(day,a.planted_date) = e.day_of_month and 
datepart(month,a.planted_date) = e.month and 
datepart(year,a.planted_date) = e.year
inner join 
edw.dim_time as f
on datepart(hour,a.planted_date) = f.hour and 
datepart(minute,a.planted_date) = f.minute 
left join
edw.dim_age as g
on DATEDIFF(MINUTE,a.planted_date,a.entry_time) = g.minute
left join
edw.dim_age as h
on DATEDIFF(MINUTE,b.entry_time,a.entry_time) = h.minute
where not exists (select 1 from edw.fact_cultivation as b where b.specimen =a.specimen
and convert(time,a.entry_time) = b.entry_time
and convert(date,a.entry_time) = b.entry_date
)



-----add changes into log table------
insert into etl.LogUpdate ([Table], LastLoadDate) 
VALUES ('fact_cultivation', convert(char(8),GETDATE(),112))
------------






