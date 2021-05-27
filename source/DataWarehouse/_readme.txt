to create the datawarehouse, either run the "dwh_init.sql" script
or create a database named "MushroomDWH" by hand

to init the staging and edw schemas run the following scripts:

schema_edw_delete.sql		(optional; wipes the edw schema)
schema_staging_delete.sql	(optional; wipes the staging schema)
schema_edw_init.sql
schema_staging_init.sql

to do the initial ETL run the following scripts:
extract.sql
transform.sql
load.sql
(WIP; temporary) temp_finalize_load.sql	NOTE: when finalized apped to the end of both load.sql and incremental_load.sql

to do the incremental ETL run the following scripts:
extract.sql
transform.sql
incremental_load.sql
(WIP; temporary) temp_finalize_load.sql	NOTE: when finalized apped to the end of both load.sql and incremental_load.sql