USE MushroomDWH
go

/*---load----*/

insert into edw.dim_specimen(
mushroom_name,
mushroom_genus,
stage_name,
specimen_key,
entry_time
)
select a.mushroom_name,a.mushroom_genus,a.stage_name,a.specimen_key,a.entry_time
from staging.dim_specimen as a

/* ---- dim date ---- */

go 
declare @StartDate DATETIME
declare @EndDate DATETIME
set @StartDate = '2021-03-07'
set @EndDate = DATEADD(YEAR,20,@StartDate)

while @StartDate<@EndDate
begin
insert into edw.dim_date(
year,
season,
month,
month_name,
day_of_month,
week,
day_of_week
)
select DATEPART(year,@StartDate) as year,
(case when DATEPART(month,@StartDate) in (12,1,2)  then 'Winter'
      when DATEPART(month,@StartDate) in (3,4,5) then 'Spring'  
      when DATEPART(month,@StartDate) in (6,7,8) then 'summer'  
	                                             else 'autumn' end) as season,
DATEPART(month,@StartDate) as month,
DATENAME(month,@StartDate) as month_name,
DATEPART(day,@StartDate) as day_of_month,
(DATEPART(day,@StartDate) -1) / 7 +1  as week,
(case when
DATEPART(day,@StartDate) % 7 =0 then 7  
else 
DATEPART(day,@StartDate) % 7
end
)as day_of_week

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
declare @incrementMinute as datetime2
select @timeStamp1 =  planted_date from staging.fact_cultivation
select @planted_date =  planted_date from staging.fact_cultivation
select @incrementMinute =  planted_date from staging.fact_cultivation
set @incrementMinute=DATEADD(MINUTE,259200,@incrementMinute)
while @timeStamp1 < @incrementMinute
begin
insert into edw.dim_age(
minute
)
select DATEDIFF(MINUTE, @planted_date, @timeStamp1) AS TARGET
SET @timeStamp1 = DATEADD(MINUTE,1,@timeStamp1)
end


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
left join 
staging.dim_specimen as b 
on a.specimen = b.specimen_key and a.stage_name = b.stage_name
where a.entry_time >= b.entry_time 


