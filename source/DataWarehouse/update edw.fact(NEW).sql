/* update fact table add keys */

UPDATE [edw].[fact_cultivation] 
set SPE_ID = (
select spe_ID from edw.dim_specimen as a
where a.specimen_key= edw.fact_cultivation.specimen
)

UPDATE [edw].[fact_cultivation] 
set PD_ID = (
select date_ID from edw.dim_date as a
where a.year = year(edw.fact_cultivation.planted_date) and
a.month = month(edw.fact_cultivation.planted_date) and 
a.day= day(edw.fact_cultivation.planted_date)
)

UPDATE [edw].[fact_cultivation] 
set PT_ID = (
select time_ID from edw.dim_time as a
where a.hour = datepart(hour,edw.fact_cultivation.planted_date) and
a.minute = datepart(minute,edw.fact_cultivation.planted_date)
)

UPDATE [edw].[fact_cultivation] 
set ED_ID = (
select date_ID from edw.dim_date as a
where a.year = year(edw.fact_cultivation.entry_date) and
a.month = month(edw.fact_cultivation.entry_date) and 
a.day= day(edw.fact_cultivation.entry_date)
)

UPDATE [edw].[fact_cultivation] 
set ET_ID = (
select time_ID from edw.dim_time as a
where a.hour = datepart(hour,edw.fact_cultivation.entry_date) and
a.minute = datepart(minute,edw.fact_cultivation.entry_date)
)

UPDATE [edw].[fact_cultivation] 
set MUA_ID = (
select age_ID from edw.dim_age as a
where a.minute= edw.fact_cultivation.mushroom_age
)

UPDATE [edw].[fact_cultivation] 
set STA_ID = (
select age_ID from edw.dim_age as a
where a.minute= edw.fact_cultivation.stage_age
)


/* Create Foreign Key Constraints */

ALTER TABLE [edw].[fact_cultivation] 
ALTER COLUMN MUA_ID INT NOT NULL

ALTER TABLE [edw].[fact_cultivation] 
ALTER COLUMN STA_ID INT NOT NULL

ALTER TABLE [edw].[fact_cultivation] 
ALTER COLUMN ED_ID INT NOT NULL

ALTER TABLE [edw].[fact_cultivation] 
ALTER COLUMN ET_ID INT NOT NULL

ALTER TABLE [edw].[fact_cultivation] 
ALTER COLUMN PD_ID INT NOT NULL

ALTER TABLE [edw].[fact_cultivation] 
ALTER COLUMN PT_ID INT NOT NULL

ALTER TABLE [edw].[fact_cultivation] 
ALTER COLUMN SPE_ID INT NOT NULL

ALTER TABLE [edw].[fact_cultivation] ADD CONSTRAINT [FK_fact_cultivation_dim_mushroom_age]
	FOREIGN KEY ([MUA_ID]) REFERENCES [edw].[dim_age] ([age_ID]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [edw].[fact_cultivation] ADD CONSTRAINT [FK_fact_cultivation_dim_specimen]
	FOREIGN KEY ([SPE_ID]) REFERENCES [edw].[dim_specimen] ([spe_ID]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [edw].[fact_cultivation] ADD CONSTRAINT [FK_fact_cultivation_dim_stage_age]
	FOREIGN KEY ([STA_ID]) REFERENCES [edw].[dim_age] ([age_ID]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [edw].[fact_cultivation] ADD CONSTRAINT [FK_fact_cultivation_entry_date]
	FOREIGN KEY ([ED_ID]) REFERENCES [edw].[dim_date] ([date_ID]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [edw].[fact_cultivation] ADD CONSTRAINT [FK_fact_cultivation_entry_time]
	FOREIGN KEY ([ET_ID]) REFERENCES [edw].[dim_time] ([time_ID]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [edw].[fact_cultivation] ADD CONSTRAINT [FK_fact_cultivation_planted_date]
	FOREIGN KEY ([PD_ID]) REFERENCES [edw].[dim_date] ([date_ID]) ON DELETE No Action ON UPDATE No Action
GO

ALTER TABLE [edw].[fact_cultivation] ADD CONSTRAINT [FK_fact_cultivation_planted_time]
	FOREIGN KEY ([PT_ID]) REFERENCES [edw].[dim_time] ([time_ID]) ON DELETE No Action ON UPDATE No Action
GO





/*---- ADD KEYS TO FACT TABLE -----*/

ALTER TABLE [edw].[fact_cultivation] 
 ADD CONSTRAINT [PK_fact_cultivation]
	PRIMARY KEY CLUSTERED ([PD_ID] ASC,[PT_ID] ASC,[ED_ID] ASC,[ET_ID] ASC,[SPE_ID] ASC,[MUA_ID] ASC,[STA_ID] ASC)
GO