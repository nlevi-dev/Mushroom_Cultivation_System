USE MushroomPP
GO

INSERT INTO dbo._permission_level (permission_type) VALUES ('user');
INSERT INTO dbo._permission_level (permission_type) VALUES ('admin');
INSERT INTO dbo._permission_level (permission_type) VALUES ('developer');
GO

/*temporary placeholder, research needed*/
INSERT INTO dbo._mushroom_stage (stage_name) VALUES ('Inoculation');
INSERT INTO dbo._mushroom_stage (stage_name) VALUES ('Casing');
INSERT INTO dbo._mushroom_stage (stage_name) VALUES ('Spawn Run');
INSERT INTO dbo._mushroom_stage (stage_name) VALUES ('Pinning');
INSERT INTO dbo._mushroom_stage (stage_name) VALUES ('Fruiting');
INSERT INTO dbo._mushroom_stage (stage_name) VALUES ('Dead');
GO

/*temporary placeholder, research needed*/
INSERT INTO dbo._mushroom_type (mushroom_name) VALUES ('Agaricus bisporus');
INSERT INTO dbo._mushroom_type (mushroom_name) VALUES ('Pleurotus');
INSERT INTO dbo._mushroom_type (mushroom_name) VALUES ('Lentinula edodes');