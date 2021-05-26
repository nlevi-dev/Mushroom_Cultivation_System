to create the datawarehouse, either run the "dwh_init.sql" script
or create a database named "MushroomDWH" by hand

to init the staging and edw schema run the following scripts:

schema_edw_delete.sql		(optional; wipes the edw schema)
schema_staging_delete.sql	(optional; wipes the staging schema)
schema_edw_init.sql
schema_staging_init.sql
schema_main_populate.sql
dummy_data_insert.sql

to do the initial ETL run the following scripts:
extract.sql
(WIP; missing) transform.sql		NOTE: check for possible nulls, replace with default shit idk include smthing
load.sql
(WIP; temporary) temp_finalize_load.sql	NOTE: when finalized apped to the end of both load.sql and incremental_load.sql

to do the incremental ETL run the following scripts:
extract.sql
(WIP; missing) transform.sql		NOTE: check for possible nulls, replace with default shit idk include smthing
incremental_load.sql
(WIP; temporary) temp_finalize_load.sql	NOTE: when finalized apped to the end of both load.sql and incremental_load.sql