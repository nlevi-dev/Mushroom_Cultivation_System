using NUnit.Framework;
using SEP4_Data.Data;
using SEP4_Data.Model;
using SEP4_Data.Model.Exception;

namespace Test
{
    public class PersistenceServiceTest
    {
        public IPersistenceService PersistenceService;
        
        [OneTimeSetUp]
        public void Setup()
        {
            PersistenceService = new PersistenceService(new Config());
            PersistenceService.DropSchema();
            PersistenceService.InitSchema();
        }
        
        [OneTimeTearDown]
        public void Wipe()
        {
            PersistenceService.DropSchema();
            PersistenceService.InitSchema();
        }

        [Test, Order(0)]
        public void GetMushroomStages()
        {
            Assert.NotNull(PersistenceService.GetMushroomStages());
        }

        [Test, Order(0)]
        public void GetMushroomStageKey()
        {
            PersistenceService.GetMushroomStageKey("Dead");
            Assert.Throws(typeof(NotFoundException), () => PersistenceService.GetMushroomStageKey("nonexistent stage"));
        }
        
        [Test, Order(0)]
        public void GetMushroomTypes()
        {
            Assert.NotNull(PersistenceService.GetMushroomTypes());
        }

        [Test, Order(0)]
        public void GetMushroomTypeKey()
        {
            PersistenceService.GetMushroomTypeKey("Agaricus - bisporus");
            Assert.Throws(typeof(NotFoundException), () => PersistenceService.GetMushroomTypeKey("malformed mushroom name"));
            Assert.Throws(typeof(NotFoundException), () => PersistenceService.GetMushroomTypeKey("nonexistent - mushroom"));
        }
        
        [Test, Order(0)]
        public void GetGetPermissionKey()
        {
            PersistenceService.GetPermissionKey("user");
            Assert.Throws(typeof(NotFoundException), () => PersistenceService.GetPermissionKey("nonexistent permission"));
        }
        
        [Test, Order(1)]
        public void CreateUser()
        {
            User user = new User
            {
                Name = "test1",
                Password = "pw1",
                PermissionLevel = 1,
                Token = "token1"
            };
            Assert.AreEqual(1,PersistenceService.CreateUser(user));
            user = new User
            {
                Name = "test2",
                Password = "pw2",
                PermissionLevel = 2
            };
            Assert.AreEqual(2,PersistenceService.CreateUser(user));
            Assert.Throws(typeof(ConflictException), () => PersistenceService.CreateUser(user));
            user = new User
            {
                Password = "pw3",
                PermissionLevel = 2
            };
            Assert.Throws(typeof(ConflictException), () => PersistenceService.CreateUser(user));
            user = new User
            {
                Name = "test3",
                PermissionLevel = 2
            };
            Assert.Throws(typeof(ConflictException), () => PersistenceService.CreateUser(user));
        }
        
        [Test, Order(2)]
        public void CheckUserPassword()
        {
            Assert.IsTrue(PersistenceService.CheckUserPassword("test1", "pw1"));
            Assert.IsTrue(PersistenceService.CheckUserPassword("test2", "pw2"));
            Assert.IsFalse(PersistenceService.CheckUserPassword("test1", "pw2"));
            Assert.IsFalse(PersistenceService.CheckUserPassword("test2", "bad pw"));
            Assert.IsFalse(PersistenceService.CheckUserPassword("bad name", "pw1"));
            Assert.IsFalse(PersistenceService.CheckUserPassword("bad name", "bad pw"));
        }
        
        [Test, Order(2)]
        public void GetUserByName()
        {
            User user = new User
            {
                Key = 1,
                Name = "test1",
                Permission = "user",
                Token = "token1"
            };
            Assert.AreEqual(user,PersistenceService.GetUserByName("test1"));
            user = new User
            {
                Key = 2,
                Name = "test2",
                Permission = "admin"
            };
            Assert.AreEqual(user,PersistenceService.GetUserByName("test2"));
            Assert.Throws(typeof(NotFoundException), () => PersistenceService.GetUserByName("non existent user"));
        }
        
        [Test, Order(2)]
        public void GetUserByKey()
        {
            User user = new User
            {
                Key = 1,
                Name = "test1",
                Permission = "user",
                Token = "token1"
            };
            Assert.AreEqual(user,PersistenceService.GetUserByKey(1));
            user = new User
            {
                Key = 2,
                Name = "test2",
                Permission = "admin"
            };
            Assert.AreEqual(user,PersistenceService.GetUserByKey(2));
            Assert.Throws(typeof(NotFoundException), () => PersistenceService.GetUserByKey(3));
        }
        
        [Test, Order(3)]
        public void UpdateUsername()
        {
            PersistenceService.UpdateUsername(1, "updated 1");
            User user = new User
            {
                Key = 1,
                Name = "updated 1",
                Permission = "user",
                Token = "token1"
            };
            Assert.AreEqual(user,PersistenceService.GetUserByKey(1));
            user = new User
            {
                Key = 2,
                Name = "test2",
                Permission = "admin"
            };
            Assert.AreEqual(user,PersistenceService.GetUserByKey(2));
            Assert.Throws(typeof(NotFoundException), () => PersistenceService.UpdateUsername(3, "updated 3"));
            Assert.Throws(typeof(ConflictException), () => PersistenceService.UpdateUsername(2, "updated 1"));
            PersistenceService.UpdateUsername(1, "test1");
        }
        
        [Test, Order(4)]
        public void UpdateUserPassword()
        {
            PersistenceService.UpdateUserPassword(1, "updated 1");
            Assert.IsTrue(PersistenceService.CheckUserPassword("test1", "updated 1"));
            Assert.Throws(typeof(NotFoundException), () => PersistenceService.UpdateUserPassword(3, "updated 3"));
            Assert.Throws(typeof(ConflictException), () => PersistenceService.UpdateUsername(2, null));
            PersistenceService.UpdateUserPassword(1, "pw1");
        }
        
        [Test, Order(4)]
        public void UpdateUserToken()
        {
            PersistenceService.UpdateUserToken(1, "updated 1");
            User user = new User
            {
                Key = 1,
                Name = "test1",
                Permission = "user",
                Token = "updated 1"
            };
            Assert.AreEqual(user,PersistenceService.GetUserByKey(1));
            user = new User
            {
                Key = 2,
                Name = "test2",
                Permission = "admin"
            };
            Assert.AreEqual(user,PersistenceService.GetUserByKey(2));
            PersistenceService.UpdateUserToken(1, null);
            user = new User
            {
                Key = 1,
                Name = "test1",
                Permission = "user"
            };
            Assert.AreEqual(user,PersistenceService.GetUserByKey(1));
            Assert.Throws(typeof(NotFoundException), () => PersistenceService.UpdateUserToken(3, "updated 3"));
            PersistenceService.UpdateUserToken(1, "token1");
        }
        
        [Test, Order(5)]
        public void DeleteUser()
        {
            User user = new User
            {
                Name = "test3",
                Password = "pw3",
                PermissionLevel = 3,
                Token = "token3"
            };
            int key = PersistenceService.CreateUser(user);
            PersistenceService.GetUserByKey(key);
            PersistenceService.DeleteUser(key);
            Assert.Throws(typeof(NotFoundException), () => PersistenceService.GetUserByKey(key));
            Assert.Throws(typeof(NotFoundException), () => PersistenceService.DeleteUser(key));
        }

        [Test, Order(6)]
        public void CreateHardware()
        {
            Hardware hardware = new Hardware
            {
                Id = "id1",
                DesiredAirTemperature = 1.1f,
                DesiredAirHumidity = 1.2f,
                DesiredAirCo2 = 1.3f,
                DesiredLightLevel = 1.4f,
                UserKey = 1
            };
            Assert.AreEqual(1,PersistenceService.CreateHardware(hardware));
            hardware = new Hardware
            {
                Id = "id2",
                DesiredAirTemperature = 2.1f,
                DesiredAirHumidity = 2.2f,
                DesiredAirCo2 = 2.3f,
                DesiredLightLevel = 2.4f,
                UserKey = 2
            };
            Assert.AreEqual(2,PersistenceService.CreateHardware(hardware));
            hardware = new Hardware
            {
                Id = "id3",
                DesiredAirTemperature = 3.1f,
                DesiredAirHumidity = 3.2f,
                DesiredAirCo2 = null,
                DesiredLightLevel = 3.4f,
                UserKey = 2
            };
            Assert.AreEqual(3,PersistenceService.CreateHardware(hardware));
            hardware = new Hardware
            {
                DesiredAirTemperature = 4.1f,
                DesiredAirHumidity = 4.2f,
                DesiredAirCo2 = 4.3f,
                DesiredLightLevel = 4.4f,
                UserKey = 2
            };
            Assert.Throws(typeof(ConflictException), () => PersistenceService.CreateHardware(hardware));
            hardware = new Hardware
            {
                Id = "id4",
                DesiredAirTemperature = 4.1f,
                DesiredAirHumidity = 4.2f,
                DesiredAirCo2 = 4.3f,
                DesiredLightLevel = 4.4f
            };
            Assert.Throws(typeof(ConflictException), () => PersistenceService.CreateHardware(hardware));
        }

        [Test, Order(7)]
        public void GetAllHardware()
        {
            Assert.AreEqual(1, PersistenceService.GetAllHardware(1).Length);
            Assert.AreEqual(2, PersistenceService.GetAllHardware(2).Length);
            Hardware hardware = new Hardware
            {
                Key = 1,
                Id = "id1",
                DesiredAirTemperature = 1.1f,
                DesiredAirHumidity = 1.2f,
                DesiredAirCo2 = 1.3f,
                DesiredLightLevel = 1.4f,
                UserKey = 1
            };
            Assert.AreEqual(hardware, PersistenceService.GetAllHardware(1)[0]);
            Assert.AreEqual(0, PersistenceService.GetAllHardware(3).Length);
        }
        
        [Test, Order(7)]
        public void GetHardwareById()
        {
            Hardware hardware = new Hardware
            {
                Key = 1,
                Id = "id1",
                DesiredAirTemperature = 1.1f,
                DesiredAirHumidity = 1.2f,
                DesiredAirCo2 = 1.3f,
                DesiredLightLevel = 1.4f,
                UserKey = 1
            };
            Assert.AreEqual(hardware, PersistenceService.GetHardwareById("id1"));
            hardware = new Hardware
            {
                Key = 3,
                Id = "id3",
                DesiredAirTemperature = 3.1f,
                DesiredAirHumidity = 3.2f,
                DesiredAirCo2 = null,
                DesiredLightLevel = 3.4f,
                UserKey = 2
            };
            Assert.AreEqual(hardware, PersistenceService.GetHardwareById("id3"));
            Assert.Throws(typeof(NotFoundException), () => PersistenceService.GetHardwareById("non existent id"));
        }
        
        [Test, Order(7)]
        public void GetHardwareByKey()
        {
            Hardware hardware = new Hardware
            {
                Key = 1,
                Id = "id1",
                DesiredAirTemperature = 1.1f,
                DesiredAirHumidity = 1.2f,
                DesiredAirCo2 = 1.3f,
                DesiredLightLevel = 1.4f,
                UserKey = 1
            };
            Assert.AreEqual(hardware, PersistenceService.GetHardware(1));
            hardware = new Hardware
            {
                Key = 3,
                Id = "id3",
                DesiredAirTemperature = 3.1f,
                DesiredAirHumidity = 3.2f,
                DesiredAirCo2 = null,
                DesiredLightLevel = 3.4f,
                UserKey = 2
            };
            Assert.AreEqual(hardware, PersistenceService.GetHardware(3));
            Assert.Throws(typeof(NotFoundException), () => PersistenceService.GetHardware(4));
        }

        [Test, Order(8)]
        public void UpdateHardware()
        {
            Hardware hardware = new Hardware
            {
                Key = 1,
                Id = "updated 1",
                DesiredAirTemperature = 10.1f,
                DesiredAirHumidity = 10.2f,
                DesiredAirCo2 = null,
                DesiredLightLevel = 10.4f,
                UserKey = 1
            };
            PersistenceService.UpdateHardware(hardware);
            Assert.AreEqual(hardware, PersistenceService.GetHardware(1));
            Assert.Throws(typeof(NotFoundException), () => PersistenceService.GetHardware(4));
            hardware = new Hardware
            {
                Key = 1,
                Id = "id1",
                DesiredAirTemperature = 1.1f,
                DesiredAirHumidity = 1.2f,
                DesiredAirCo2 = 1.3f,
                DesiredLightLevel = 1.4f,
                UserKey = 1
            };
            PersistenceService.UpdateHardware(hardware);
            Assert.AreEqual(hardware, PersistenceService.GetHardware(1));
        }
        
        [Test, Order(9)]
        public void DeleteHardware()
        {
            Hardware hardware = new Hardware
            {
                Id = "id4",
                DesiredAirTemperature = 4.1f,
                DesiredAirHumidity = 4.2f,
                DesiredAirCo2 = 4.3f,
                DesiredLightLevel = 4.4f,
                UserKey = 1
            };
            int key = PersistenceService.CreateHardware(hardware);
            PersistenceService.GetHardware(key);
            PersistenceService.DeleteHardware(key);
            Assert.Throws(typeof(NotFoundException), () => PersistenceService.GetHardware(key));
            Assert.Throws(typeof(NotFoundException), () => PersistenceService.DeleteHardware(key));
        }
        
        [Test, Order(10)]
        public void CreateSpecimen()
        {
            Specimen specimen = new Specimen
            {
                PlantedUnix = 1492992000000,
                Name = "spe1",
                TypeKey = 1,
                Description = "dsc1",
                DesiredAirTemperature = 1.1f,
                DesiredAirHumidity = 1.2f,
                DesiredAirCo2 = 1.3f,
                DesiredLightLevel = 1.4f,
                UserKey = 1
            };
            Assert.AreEqual(1,PersistenceService.CreateSpecimen(specimen));
            specimen = new Specimen
            {
                PlantedUnix = 2492992000000,
                Name = "spe2",
                TypeKey = 1,
                Description = "dsc2",
                DesiredAirTemperature = 2.1f,
                DesiredAirHumidity = 2.2f,
                DesiredAirCo2 = 2.3f,
                DesiredLightLevel = 2.4f,
                UserKey = 2
            };
            Assert.AreEqual(2,PersistenceService.CreateSpecimen(specimen));
            specimen = new Specimen
            {
                PlantedUnix = 3492992000000,
                Name = "spe3",
                TypeKey = 1,
                Description = "dsc3",
                DesiredAirTemperature = 3.1f,
                DesiredAirHumidity = 3.2f,
                DesiredAirCo2 = null,
                DesiredLightLevel = 3.4f,
                UserKey = 2
            };
            Assert.AreEqual(3,PersistenceService.CreateSpecimen(specimen));
        }

        [Test, Order(11)]
        public void GetAllSpecimen()
        {
            Assert.AreEqual(1, PersistenceService.GetAllSpecimen(1).Length);
            Assert.AreEqual(2, PersistenceService.GetAllSpecimen(2).Length);
            Specimen specimen = new Specimen
            {
                Key = 1,
                PlantedUnix = 1492992000000,
                Name = "spe1",
                MushroomType = "Agaricus - bisporus",
                TypeKey = 1,
                Description = "dsc1",
                DesiredAirTemperature = 1.1f,
                DesiredAirHumidity = 1.2f,
                DesiredAirCo2 = 1.3f,
                DesiredLightLevel = 1.4f,
                UserKey = 1
            };
            Assert.AreEqual(specimen, PersistenceService.GetAllSpecimen(1)[0]);
            Assert.AreEqual(0, PersistenceService.GetAllSpecimen(3).Length);
        }
        
        [Test, Order(11)]
        public void GetSpecimen()
        {
            Specimen specimen = new Specimen
            {
                Key = 1,
                PlantedUnix = 1492992000000,
                Name = "spe1",
                MushroomType = "Agaricus - bisporus",
                TypeKey = 1,
                Description = "dsc1",
                DesiredAirTemperature = 1.1f,
                DesiredAirHumidity = 1.2f,
                DesiredAirCo2 = 1.3f,
                DesiredLightLevel = 1.4f,
                UserKey = 1
            };
            Assert.AreEqual(specimen, PersistenceService.GetSpecimen(1));
            specimen = new Specimen
            {
                Key = 2,
                PlantedUnix = 2492992000000,
                Name = "spe2",
                MushroomType = "Agaricus - bisporus",
                TypeKey = 1,
                Description = "dsc2",
                DesiredAirTemperature = 2.1f,
                DesiredAirHumidity = 2.2f,
                DesiredAirCo2 = 2.3f,
                DesiredLightLevel = 2.4f,
                UserKey = 2
            };
            Assert.AreEqual(specimen, PersistenceService.GetSpecimen(2));
            specimen = new Specimen
            {
                Key = 3,
                PlantedUnix = 3492992000000,
                Name = "spe3",
                MushroomType = "Agaricus - bisporus",
                TypeKey = 1,
                Description = "dsc3",
                DesiredAirTemperature = 3.1f,
                DesiredAirHumidity = 3.2f,
                DesiredAirCo2 = null,
                DesiredLightLevel = 3.4f,
                UserKey = 2
            };
            Assert.AreEqual(specimen, PersistenceService.GetSpecimen(3));
            Assert.Throws(typeof(NotFoundException), () => PersistenceService.GetSpecimen(4));
        }
        
        
    }

    public class Config : IConfigService
    {
        public int Port { get; set; }
        public bool Https { get; set; }
        public string DbHost { get => "127.0.0.1"; set {} }
        public int DbPort { get => 1433; set {} }
        public string DbName { get => "MushroomPP"; set {} }
        public string DbUser { get => "Admin"; set {} }
        public string DbPassword { get => "mssql"; set {} }
        public byte[] JwtKey { get; set; }
        public byte[] Salt { get; set; }
        public int UserPostPermissionLevel { get; set; }
        public int TokenExpire { get; set; }
        public bool Swagger { get; set; }
        public bool ReInitializeDb { get; set; }
        public int SampleInterval { get; set; }
    }
}