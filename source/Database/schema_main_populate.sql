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
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Agaricus','bisporus');
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Lentinula','edodes');
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Auricularia','auricula-judae');
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Volvariella','volvacea');
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Flammulina','velutipes');
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Tremella','fuciformis');
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Hypsizygus','tessellatus');
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Stropharia','rugosoannulata');
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Cyclocybe','aegerita');
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Hericium','erinaceus');
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Phallus','indusiatus');
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Boletus','edulis');
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Calbovista','subsculpta');
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Calvatia','gigantea');
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Cantharellus','cibarius');
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Craterellus','tubaeformis');
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Clitocybe','nuda');
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Cortinarius','caperatus');
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Craterellus','cornucopioides');
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Grifola','frondosa');
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Gyromitra','esculenta');
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Hericium','erinaceus');
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Hydnum','repandum');
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Lactarius','deliciosus');
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Morchella','conica');
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Morchella','esculenta');
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Tricholoma','matsutake');
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Tuber','aestivum');
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Tuber','borchii');
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Tuber','brumale');
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Tuber','indicum');
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Tuber','macrosporum');
INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Tuber','mesentericum');
