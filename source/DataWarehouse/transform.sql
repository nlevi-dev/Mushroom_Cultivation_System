USE [MushroomDWH]
GO

/* find all stages for all fact entries */

UPDATE staging.fact_cultivation SET stage_name = d.stage_name
from MushroomPP.dbo._sensor_entry as b
left join MushroomPP.dbo._specimen as a on a.specimen_key=b.specimen_key
inner join MushroomPP.dbo._status_entry as c on a.specimen_key=c.specimen_key
inner join MushroomPP.dbo._mushroom_stage as d on c.stage_key=d.stage_key
inner join (
select
b.specimen_key,
b.entry_time,
max(c.entry_time) as stage_time
from MushroomPP.dbo._sensor_entry as b
left join MushroomPP.dbo._specimen as a on a.specimen_key=b.specimen_key
inner join MushroomPP.dbo._status_entry as c on a.specimen_key=c.specimen_key
where b.entry_time >= c.entry_time
group by b.specimen_key, b.entry_time
)
as h on h.entry_time=b.entry_time and h.stage_time=c.entry_time and h.specimen_key=b.specimen_key
WHERE staging.fact_cultivation.entry_time = b.entry_time AND staging.fact_cultivation.specimen = b.specimen_key

/* Add PK for dim_specimen */

ALTER TABLE [staging].[dim_specimen] 
    ADD CONSTRAINT [PK_dim_specimen] 
	PRIMARY KEY CLUSTERED ([specimen_key] ASC,[stage_name] ASC)
GO

/* Add FK for fact_cultivation */

ALTER TABLE [staging].[fact_cultivation] ADD CONSTRAINT [FK_fact_cultivation_dim_specimen]
	FOREIGN KEY ([specimen], [stage_name]) REFERENCES [staging].[dim_specimen] ([specimen_key], [stage_name]) ON DELETE No Action ON UPDATE No Action
GO
