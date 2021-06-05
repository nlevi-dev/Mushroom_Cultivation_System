USE MushroomDWH
go

/*---add new data into dim_specimen----*/

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
INSERT into [update_log].[edw_logs]([Table], [LastLoadDate])
VALUES  ('dim_specimen',convert(char(8),GETDATE(),112))
------------
				
/*--- add new data into dim fact table ---*/
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
where not exists (select 1 from (edw.fact_cultivation as i 
inner join edw.dim_specimen as k on i.SPE_ID =k.spe_ID
inner join edw.dim_date as j on i.ED_ID = j.date_ID
inner join edw.dim_time as l on i.ET_ID = l.time_ID
)
where a.specimen = k.specimen_key
and datepart(year,a.entry_time) = j.year
and datepart(month,a.entry_time) = j.month
and datepart(year,a.entry_time) = j.year
and datepart(day,a.entry_time) = j.day_of_month
and datepart(hour,a.entry_time) = l.hour
and datepart(minute,a.entry_time) = l.minute
)

-----note changes into log table------
insert into [update_log].[edw_logs]([Table], [LastLoadDate]) 
VALUES ('fact_cultivation', convert(char(8),GETDATE(),112))
------------
