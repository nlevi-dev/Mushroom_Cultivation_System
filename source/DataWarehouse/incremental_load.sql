USE MushroomDWH
go


--log for updates		
CREATE SCHEMA etl;

CREATE TABLE etl.[LogUpdate]
(
	[Table] nvarchar(50) NULL,
	[LastLoadDate] int NULL
) on [PRIMARY]


/*---dim specimen----*/

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
		




/*--- dim fact table ---*/

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
d.date_ID as PD_ID,
e.time_ID as PT_ID,
c.date_ID as ED_ID,
f.time_ID as ET_ID,
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
left join 
edw.dim_specimen as b 
on a.specimen = b.specimen_key and a.stage_name = b.stage_name
left join 
edw.dim_date as c
on datepart(day,a.planted_date) = c.day_of_month and 
datepart(month,a.planted_date) = c.month and 
datepart(year,a.planted_date) = c.year
left join 
edw.dim_date as d 
on datepart(day,a.entry_time) = d.day_of_month and 
datepart(month,a.entry_time) = d.month and 
datepart(year,a.entry_time) = d.year
left join 
edw.dim_time as e
on datepart(hour,a.planted_date) = e.hour and 
datepart(minute,a.planted_date) = e.minute 
left join 
edw.dim_time as f
on datepart(hour,a.entry_time) = f.hour and 
datepart(minute,a.entry_time) = f.minute 
left join
edw.dim_age as g
on DATEDIFF(MINUTE,a.planted_date,a.entry_time) = g.minute
left join
edw.dim_age as h
on DATEDIFF(MINUTE,b.entry_time,a.entry_time) = h.minute
where concat(specimen_key, a.entry_time) not in (select concat(specimen_key, entry_time) from edw.fact_cultivation)

-----improvements below----
--where not exists (select * from edw.fact_cultivation as i where specimen = i.specimen 
--and entry_date = i.entry_date
--and entry_time = i.entry_time)


-----add changes into log table------
INSERT INTO etl.LogUpdate ([Table], LastLoadDate) 
VALUES ('fact_cultivation', convert(char(8),GETDATE(),112)
------------








