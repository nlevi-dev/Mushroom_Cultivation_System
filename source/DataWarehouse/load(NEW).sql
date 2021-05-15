 USE MushroomDWH
go

/*---load----*/

insert into edw.dim_specimen(
mushroom_name,
mushroom_genus,
stage_name,
specimen_key
)
select mushroom_name,mushroom_genus,stage_name,specimen_key
from staging.dim_specimen


/* ---- dim date ---- */

go 
declare @StartDate DATETIME
declare @EndDate DATETIME
set @StartDate = '2021-06-04'
set @EndDate = DATEADD(YEAR,20,@StartDate)

while @StartDate<@EndDate
begin
insert into edw.dim_date(
year,
season,
month,
month_name,
week,
day
)
select DATEPART(year,@StartDate) as year,
(case when DATEPART(month,@StartDate) in (12,1,2)  then 'Winter'
      when DATEPART(month,@StartDate) in (3,4,5) then 'Spring'  
      when DATEPART(month,@StartDate) in (6,7,8) then 'summer'  
	                                             else 'autumn' end) as season,
DATEPART(month,@StartDate) as month,
DATENAME(month,@StartDate) as month_name,
(DATEPART(day,@StartDate) -1) / 7 +1  as week,
(case when
DATEPART(day,@StartDate) % 7 =0 then 7  
else 
DATEPART(day,@StartDate) % 7
end
)as day

set @StartDate = DATEADD(dd,1,@StartDate)

end
;


/*--- dim time ---*/

declare @timeStamp as int
declare @startOfDay as datetime
set @timeStamp = 0 
set @startOfDay = '2021-05-04 00:00:00'
while @timeStamp < 1440
begin
insert into edw.dim_time(
time_of_day,
hour,
minute
)
select
  CONVERT(nvarchar(100), DATEPART(hour,@startOfDay), 0)
+ ':'
+ CONVERT(nvarchar(100), DATEPART(minute,@startOfDay), 0) as time_of_day,
DATEPART(hour,@startOfDay) as hour,
DATEPART(minute,@startOfDay) as minute;
set @startOfDay=DATEADD(minute,1,@startOfDay);
set @timeStamp=@timeStamp+1
end 




/*--- dim age ---*/

declare @timeStamp1 as datetime2
declare @planted_date as datetime2
select @timeStamp1 =  planted_date from staging.fact_cultivation
select @planted_date =  planted_date from staging.fact_cultivation
while @timeStamp1 < DATEADD(MINUTE,525949,@timeStamp1)
insert into edw.dim_age(
minute
)
select DATEDIFF(MINUTE, @planted_date, @timeStamp1 );



/*--- dim fact table ---*/

declare @planted_time_all as datetime2
declare @entry_time_all as datetime2
select @planted_time_all = planted_date from MushroomDWH.staging.fact_cultivation
select @entry_time_all = entry_time from MushroomDWH.staging.fact_cultivation
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
stage_age
)
select air_temperature,
air_humidity,
air_co2,
light_level,
(select convert(date,@planted_time_all)) as planted_date,
(select convert(time,@planted_time_all)) as planted_time,
(select convert(date,@entry_time_all)) as entry_date,
(select convert(time,@entry_time_all)) as entry_time,
specimen,
(select DATEDIFF(MINUTE,planted_date,GETDATE())) as mushroom_age,
(select DATEDIFF(MINUTE,c.entry_time,GETDATE())) as stage_age
from  staging.dim_specimen as a inner join staging.fact_cultivation as b
on a.specimen_key = b.specimen
left join MushroomPP.dbo._status_entry as c
on a.specimen_key = c.specimen_key


