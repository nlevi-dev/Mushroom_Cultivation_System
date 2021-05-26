drop table etl.LogUpdate;

--create scheme --
use MushroomDWH
go
;

create schema etl;

--create log table

CREATE TABLE etl.[LogUpdate]
(
	[Table] nvarchar(50) NULL,
	[LastLoadDate] int NULL
) on [PRIMARY]


--log for updates
INSERT into etl.LogUpdate([Table],LastLoadDate)
			VALUES  ('DimSpecimen',20210413)
				

--valid from and valid to

insert into edw.dim_specimen(
mushroom_name,
mushroom_genus,
stage_name,
specimen_key
)
select a.mushroom_name,a.mushroom_genus,a.stage_name,a.specimen_key
from staging.dim_specimen as a














