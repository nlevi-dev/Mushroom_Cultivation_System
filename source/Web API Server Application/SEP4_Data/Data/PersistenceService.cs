using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using SEP4_Data.Model;
using SEP4_Data.Model.Exception;

namespace SEP4_Data.Data
{
    //methods do not support updating entities' associated user
    public class PersistenceService : IPersistenceService
    {
        private readonly string _connectionString;
        
        public PersistenceService(IConfigService config)
        {
            _connectionString = "Server=" + config.DbHost + "," + config.DbPort + ";Database=" + config.DbName + ";User Id=" + config.DbUser + ";Password=" + config.DbPassword + ";";
        }

        public string[] GetMushroomStages()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT stage_name FROM _mushroom_stage";
                var reader = command.ExecuteReader();
                var temp = new List<string>();
                while (reader.Read())
                    temp.Add(reader.GetString(0));
                reader.Close();
                return temp.ToArray();
            }
        }
        
        public int GetMushroomStageKey(string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =  "SELECT stage_key FROM _mushroom_stage WHERE stage_name = @name";
                command.Parameters.AddWithValue("@name", name);
                var reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    reader.Close();
                    throw new NotFoundException("stage does not exist with name of \"" + name + "\"");
                }
                var temp = reader.GetInt32(0);
                reader.Close();
                return temp;
            }
        }

        public string[] GetMushroomTypes()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT mushroom_genus, mushroom_name FROM _mushroom_type";
                var reader = command.ExecuteReader();
                var temp = new List<string>();
                while (reader.Read())
                    temp.Add(reader.GetString(0) + " - " + reader.GetString(1));
                reader.Close();
                return temp.ToArray();
            }
        }
        
        public int GetMushroomTypeKey(string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =  "SELECT type_key FROM _mushroom_type WHERE mushroom_genus = @genus AND mushroom_name = @name";
                var split = name.Split(" - ");
                if (split.Length != 2)
                    throw new NotFoundException("mushroom type does not exist with name of \"" + name + "\"");
                command.Parameters.AddWithValue("@genus", split[0]);
                command.Parameters.AddWithValue("@name", split[1]);
                var reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    reader.Close();
                    throw new NotFoundException("mushroom type does not exist with name of \"" + name + "\"");
                }
                var temp = reader.GetInt32(0);
                reader.Close();
                return temp;
            }
        }

        public bool CheckUserPassword(string username, string passwordHashed)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT password_hashed FROM _user WHERE username = @name";
                command.Parameters.AddWithValue("@name", username);
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    var temp = reader.GetString(0);
                    reader.Close();
                    return temp == passwordHashed;
                }
                reader.Close();
            }
            return false;
        }
        
        public int GetPermissionKey(string name)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =  "SELECT permission_key FROM _permission_level WHERE permission_type = @name";
                command.Parameters.AddWithValue("@name", name);
                var reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    reader.Close();
                    throw new NotFoundException("permission type does not exist with name of \"" + name + "\"");
                }
                var temp = reader.GetInt32(0);
                reader.Close();
                return temp;
            }
        }

        public int CreateUser(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO _user (username, password_hashed, permission_key, user_token) VALUES (@name, @password, @permission, @token)";
                command.Parameters.AddWithValue("@name", user.Name ?? throw new ConflictException("username can't be null"));
                command.Parameters.AddWithValue("@password", user.Password ?? throw new ConflictException("password can't be null"));
                command.Parameters.AddWithValue("@permission", (object) user.PermissionLevel ?? DateTime.Now.ToString("yyyy-MM-dd HH':'mm':'ss.fff"));
                command.Parameters.AddWithValue("@token", (object) user.Token ?? DBNull.Value);
                try {
                    command.ExecuteNonQuery();
                } catch (SqlException e) {
                    if (e.Message.Contains("duplicate key"))
                        throw new ConflictException("username taken");
                    throw;
                }
                command = connection.CreateCommand();
                command.CommandText = "SELECT TOP 1 user_key FROM _user WHERE username = @name ORDER BY user_key DESC";
                command.Parameters.AddWithValue("@name", user.Name);
                var reader = command.ExecuteReader();
                reader.Read();
                var temp = reader.GetInt32(0);
                reader.Close();
                return temp;
            }
        }

        public User[] GetAllUser()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT _user.user_key, _user.username, _user.permission_key, _permission_level.permission_type, _user.user_token FROM _user LEFT JOIN _permission_level ON (_user.permission_key = _permission_level.permission_key)";
                var reader = command.ExecuteReader();
                var temp = new List<User>();
                while (reader.Read())
                {
                    User item = new User
                    {
                        Key = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        PermissionLevel = reader.GetInt32(2),
                        Permission = reader.GetString(3)
                    };
                    if (!reader.IsDBNull(4))
                        item.Token = reader.GetString(4);
                    temp.Add(item);
                }
                reader.Close();
                return temp.ToArray();
            }
        }

        public User GetUserByName(string username)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT _user.user_key, _user.username, _user.permission_key, _permission_level.permission_type, _user.user_token FROM _user LEFT JOIN _permission_level ON (_user.permission_key = _permission_level.permission_key) WHERE _user.username = @name";
                command.Parameters.AddWithValue("@name", username ?? throw new ConflictException("username can't be null"));
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    User temp = new User
                    {
                        Key = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        PermissionLevel = reader.GetInt32(2),
                        Permission = reader.GetString(3)
                    };
                    if (!reader.IsDBNull(4))
                        temp.Token = reader.GetString(4);
                    reader.Close();
                    return temp;
                }
                reader.Close();
            }
            throw new NotFoundException("user \"" + username + "\" (name) not found");
        }

        public User GetUserByKey(int userKey)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT _user.user_key, _user.username, _user.permission_key, _permission_level.permission_type, _user.user_token FROM _user LEFT JOIN _permission_level ON (_user.permission_key = _permission_level.permission_key) WHERE _user.user_key = @key";
                command.Parameters.AddWithValue("@key", userKey);
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    User temp = new User
                    {
                        Key = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        PermissionLevel = reader.GetInt32(2),
                        Permission = reader.GetString(3)
                    };
                    if (!reader.IsDBNull(4))
                        temp.Token = reader.GetString(4);
                    reader.Close();
                    return temp;
                }
                reader.Close();
            }
            throw new NotFoundException("user \"" + userKey + "\" (key) not found");
        }

        public void DeleteUser(int userKey)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT user_key FROM _user WHERE user_key = @key";
                command.Parameters.AddWithValue("@key", userKey);
                var reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    reader.Close();
                    throw new NotFoundException("user \"" + userKey + "\" (key) not found");
                }
                reader.Close();
                command = connection.CreateCommand();
                command.CommandText = "DELETE FROM _user WHERE user_key = @key";
                command.Parameters.AddWithValue("@key", userKey);
                command.ExecuteNonQuery();
            }
        }

        public void UpdateUsername(int userKey, string username)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT user_key FROM _user WHERE user_key = @key";
                command.Parameters.AddWithValue("@key", userKey);
                var reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    reader.Close();
                    throw new NotFoundException("user \"" + userKey + "\" (key) not found");
                }
                reader.Close();
                command = connection.CreateCommand();
                command.CommandText = "UPDATE _user SET username = @name WHERE user_key = @key";
                command.Parameters.AddWithValue("@key", userKey);
                command.Parameters.AddWithValue("@name", username ?? throw new ConflictException("username can't be null"));
                try {
                    command.ExecuteNonQuery();
                } catch (SqlException e) {
                    if (e.Message.Contains("duplicate key"))
                        throw new ConflictException("username taken");
                    throw;
                }
            }
        }

        public void UpdateUserPassword(int userKey, string password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT user_key FROM _user WHERE user_key = @key";
                command.Parameters.AddWithValue("@key", userKey);
                var reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    reader.Close();
                    throw new NotFoundException("user \"" + userKey + "\" (key) not found");
                }
                reader.Close();
                command = connection.CreateCommand();
                command.CommandText = "UPDATE _user SET password_hashed = @pw WHERE user_key = @key";
                command.Parameters.AddWithValue("@key", userKey);
                command.Parameters.AddWithValue("@pw", password ?? throw new ConflictException("username can't be null"));
                command.ExecuteNonQuery();
            }
        }
        
        public void UpdateUserToken(int userKey, string token)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT user_key FROM _user WHERE user_key = @key";
                command.Parameters.AddWithValue("@key", userKey);
                var reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    reader.Close();
                    throw new NotFoundException("user \"" + userKey + "\" (key) not found");
                }
                reader.Close();
                command = connection.CreateCommand();
                command.CommandText = "UPDATE _user SET user_token = @token WHERE user_key = @key";
                command.Parameters.AddWithValue("@key", userKey);
                command.Parameters.AddWithValue("@token", (object) token ?? DBNull.Value);
                command.ExecuteNonQuery();
            }
        }

        public int CreateHardware(Hardware hardware)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO _hardware (hardware_id, desired_air_temperature, desired_air_humidity, desired_air_co2, desired_light_level, user_key) VALUES (@id, @airtemp, @airhum, @airco2, @light, @user)";
                command.Parameters.AddWithValue("@id", hardware.Id ?? throw new ConflictException("hardware id can't be null"));
                command.Parameters.AddWithValue("@airtemp", (object) hardware.DesiredAirTemperature ?? DBNull.Value);
                command.Parameters.AddWithValue("@airhum", (object) hardware.DesiredAirHumidity ?? DBNull.Value);
                command.Parameters.AddWithValue("@airco2", (object) hardware.DesiredAirCo2 ?? DBNull.Value);
                command.Parameters.AddWithValue("@light", (object) hardware.DesiredLightLevel ?? DBNull.Value);
                command.Parameters.AddWithValue("@user", hardware.UserKey ?? throw new ConflictException("user key can't be null"));
                try {
                    command.ExecuteNonQuery();
                } catch (SqlException e) {
                    if (e.Message.Contains("duplicate key"))
                        throw new ConflictException("hardware id taken");
                    throw;
                }
                command = connection.CreateCommand();
                command.CommandText = "SELECT TOP 1 hardware_key FROM _hardware WHERE hardware_id = @id ORDER BY hardware_key DESC";
                command.Parameters.AddWithValue("@id", hardware.Id);
                var reader = command.ExecuteReader();
                reader.Read();
                var temp = reader.GetInt32(0);
                reader.Close();
                return temp;
            }
        }

        public Hardware[] GetAllHardware(int? userKey)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT _hardware.hardware_key, _hardware.hardware_id, _hardware.desired_air_temperature, _hardware.desired_air_humidity, _hardware.desired_air_co2, _hardware.desired_light_level, _hardware.user_key, _specimen.specimen_key FROM _hardware LEFT JOIN _specimen ON (_hardware.hardware_key = _specimen.hardware_key)";
                if (userKey != null)
                {
                    command.CommandText += " WHERE _hardware.user_key = @user_key";
                    command.Parameters.AddWithValue("@user_key", userKey);
                }
                var reader = command.ExecuteReader();
                var temp = new List<Hardware>();
                while (reader.Read())
                {
                    Hardware item = new Hardware
                    {
                        Key = reader.GetInt32(0),
                        Id = reader.GetString(1),
                        UserKey = reader.GetInt32(6)
                    };
                    if (!reader.IsDBNull(2))
                        item.DesiredAirTemperature = reader.GetFloat(2);
                    if (!reader.IsDBNull(3))
                        item.DesiredAirHumidity = reader.GetFloat(3);
                    if (!reader.IsDBNull(4))
                        item.DesiredAirCo2 = reader.GetFloat(4);
                    if (!reader.IsDBNull(5))
                        item.DesiredLightLevel = reader.GetFloat(5);
                    if (!reader.IsDBNull(7))
                        item.SpecimenKey = reader.GetInt32(7);
                    temp.Add(item);
                }
                reader.Close();
                return temp.ToArray();
            }
        }

        public Hardware GetHardware(int hardwareKey)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT _hardware.hardware_key, _hardware.hardware_id, _hardware.desired_air_temperature, _hardware.desired_air_humidity, _hardware.desired_air_co2, _hardware.desired_light_level, _hardware.user_key, _specimen.specimen_key FROM _hardware LEFT JOIN _specimen ON (_hardware.hardware_key = _specimen.hardware_key) WHERE _hardware.hardware_key = @key";
                command.Parameters.AddWithValue("@key", hardwareKey);
                var reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    reader.Close();
                    throw new NotFoundException("hardware " + hardwareKey + " (key) not found");
                }
                Hardware item = new Hardware
                {
                    Key = reader.GetInt32(0),
                    Id = reader.GetString(1),
                    UserKey = reader.GetInt32(6)
                };
                if (!reader.IsDBNull(2))
                    item.DesiredAirTemperature = reader.GetFloat(2);
                if (!reader.IsDBNull(3))
                    item.DesiredAirHumidity = reader.GetFloat(3);
                if (!reader.IsDBNull(4))
                    item.DesiredAirCo2 = reader.GetFloat(4);
                if (!reader.IsDBNull(5))
                    item.DesiredLightLevel = reader.GetFloat(5);
                if (!reader.IsDBNull(7))
                    item.SpecimenKey = reader.GetInt32(7);
                reader.Close();
                return item;
            }
        }
        
        public Hardware GetHardwareById(string hardwareId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT _hardware.hardware_key, _hardware.hardware_id, _hardware.desired_air_temperature, _hardware.desired_air_humidity, _hardware.desired_air_co2, _hardware.desired_light_level, _hardware.user_key, _specimen.specimen_key FROM _hardware LEFT JOIN _specimen ON (_hardware.hardware_key = _specimen.hardware_key) WHERE _hardware.hardware_id = @id";
                command.Parameters.AddWithValue("@id", hardwareId ?? throw new ConflictException("hardware id can't be null"));
                var reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    reader.Close();
                    throw new NotFoundException("hardware " + hardwareId + " (id) not found");
                }
                Hardware item = new Hardware
                {
                    Key = reader.GetInt32(0),
                    Id = reader.GetString(1),
                    UserKey = reader.GetInt32(6)
                };
                if (!reader.IsDBNull(2))
                    item.DesiredAirTemperature = reader.GetFloat(2);
                if (!reader.IsDBNull(3))
                    item.DesiredAirHumidity = reader.GetFloat(3);
                if (!reader.IsDBNull(4))
                    item.DesiredAirCo2 = reader.GetFloat(4);
                if (!reader.IsDBNull(5))
                    item.DesiredLightLevel = reader.GetFloat(5);
                if (!reader.IsDBNull(7))
                    item.SpecimenKey = reader.GetInt32(7);
                reader.Close();
                return item;
            }
        }

        public void DeleteHardware(int hardwareKey)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT hardware_key FROM _hardware WHERE hardware_key = @key";
                command.Parameters.AddWithValue("@key", hardwareKey);
                var reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    reader.Close();
                    throw new NotFoundException("hardware " + hardwareKey + " (key) not found");
                }
                reader.Close();
                command = connection.CreateCommand();
                command.CommandText = "DELETE FROM _hardware WHERE hardware_key = @key";
                command.Parameters.AddWithValue("@key", hardwareKey);
                command.ExecuteNonQuery();
            }
        }

        public void UpdateHardware(Hardware hardware)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT hardware_key FROM _hardware WHERE hardware_key = @key";
                command.Parameters.AddWithValue("@key", hardware.Key ?? throw new ConflictException("hardware key can't be null"));
                var reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    reader.Close();
                    throw new NotFoundException("hardware " + hardware.Key + " (key) not found");
                }
                reader.Close();
                command = connection.CreateCommand();
                command.CommandText = "UPDATE _hardware SET hardware_id = @id, desired_air_temperature = @airtemp, desired_air_humidity = @airhum, desired_air_co2 = @airco2, desired_light_level = @light WHERE hardware_key = @key";
                var current = GetHardware((int) hardware.Key);
                command.Parameters.AddWithValue("@id", hardware.Id ?? current.Id);
                command.Parameters.AddWithValue("@airtemp", (object) hardware.DesiredAirTemperature ?? DBNull.Value);
                command.Parameters.AddWithValue("@airhum", (object) hardware.DesiredAirHumidity ?? DBNull.Value);
                command.Parameters.AddWithValue("@airco2", (object) hardware.DesiredAirCo2 ?? DBNull.Value);
                command.Parameters.AddWithValue("@light", (object) hardware.DesiredLightLevel ?? DBNull.Value);
                command.Parameters.AddWithValue("@key", hardware.Key);
                try {
                    command.ExecuteNonQuery();
                } catch (SqlException e) {
                    if (e.Message.Contains("duplicate key"))
                        throw new ConflictException("hardware id taken");
                    throw;
                }
            }
        }

        public int CreateSpecimen(Specimen specimen)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                SqlDataReader reader;
                if (specimen.HardwareKey != null)
                {
                    command.CommandText = "SELECT specimen_key FROM _specimen WHERE hardware_key = @hardware";
                    command.Parameters.AddWithValue("@hardware", specimen.HardwareKey);
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        var key = reader.GetInt32(0);
                        reader.Close();
                        throw new ConflictException("hardware is already assigned to specimen " + key + " (key)");
                    }
                    reader.Close();
                    command = connection.CreateCommand();
                }
                command.CommandText = "INSERT INTO _specimen (planted_date, specimen_name, specimen_description, desired_air_temperature, desired_air_humidity, desired_air_co2, desired_light_level, type_key, hardware_key, user_key) VALUES (@planted, @name, @description, @airtemp, @airhum, @airco2, @light, @type, @hardware, @user)";
                command.Parameters.AddWithValue("@planted", specimen.PlantedTsql ?? DateTime.Now.ToString("yyyy-MM-dd HH':'mm':'ss.fff"));
                command.Parameters.AddWithValue("@name", specimen.Name ?? "");
                command.Parameters.AddWithValue("@description", specimen.Description ?? "");
                command.Parameters.AddWithValue("@airtemp", (object) specimen.DesiredAirTemperature ?? DBNull.Value);
                command.Parameters.AddWithValue("@airhum", (object) specimen.DesiredAirHumidity ?? DBNull.Value);
                command.Parameters.AddWithValue("@airco2", (object) specimen.DesiredAirCo2 ?? DBNull.Value);
                command.Parameters.AddWithValue("@light", (object) specimen.DesiredLightLevel ?? DBNull.Value);
                command.Parameters.AddWithValue("@type", specimen.TypeKey ?? throw new ConflictException("type key can't be null"));
                command.Parameters.AddWithValue("@hardware", (object) specimen.HardwareKey ?? DBNull.Value);
                command.Parameters.AddWithValue("@user", specimen.UserKey ?? throw new ConflictException("user key can't be null"));
                command.ExecuteNonQuery();
                command = connection.CreateCommand();
                command.CommandText = "SELECT TOP 1 specimen_key FROM _specimen WHERE specimen_name = @name AND specimen_description = @description AND desired_air_temperature = @airtemp AND desired_air_humidity = @airhum AND desired_air_co2 = @airco2 AND desired_light_level = @light AND type_key = @type AND hardware_key = @hardware AND user_key = @user ORDER BY specimen_key DESC";
                command.Parameters.AddWithValue("@name", specimen.Name ?? "");
                command.Parameters.AddWithValue("@description", specimen.Description ?? "");
                AddNullableCondition(command, "@airtemp", specimen.DesiredAirTemperature);
                AddNullableCondition(command, "@airhum", specimen.DesiredAirHumidity);
                AddNullableCondition(command, "@airco2", specimen.DesiredAirCo2);
                AddNullableCondition(command, "@light", specimen.DesiredLightLevel);
                command.Parameters.AddWithValue("@type", specimen.TypeKey);
                AddNullableCondition(command, "@hardware", specimen.HardwareKey);
                command.Parameters.AddWithValue("@user", specimen.UserKey);
                reader = command.ExecuteReader();
                reader.Read();
                var temp = reader.GetInt32(0);
                reader.Close();
                return temp;
            }
        }

        public Specimen[] GetAllSpecimen(int userKey)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT _specimen.specimen_key, _specimen.planted_date, _specimen.specimen_name, _specimen.specimen_description, _specimen.desired_air_temperature, _specimen.desired_air_humidity, _specimen.desired_air_co2, _specimen.desired_light_level, _specimen.type_key, _specimen.hardware_key, _specimen.user_key, _hardware.hardware_id, _mushroom_type.mushroom_genus, _mushroom_type.mushroom_name FROM _specimen LEFT JOIN _hardware ON (_specimen.hardware_key = _hardware.hardware_key) LEFT JOIN _mushroom_type ON (_specimen.type_key = _mushroom_type.type_key) WHERE _specimen.user_key = @user_key";
                command.Parameters.AddWithValue("@user_key", userKey);
                var reader = command.ExecuteReader();
                var temp = new List<Specimen>();
                while (reader.Read())
                {
                    Specimen item = new Specimen
                    {
                        Key = reader.GetInt32(0),
                        PlantedDotnet = reader.GetDateTime(1),
                        Name = reader.GetString(2),
                        Description = reader.GetString(3),
                        TypeKey = reader.GetInt32(8),
                        UserKey = reader.GetInt32(10),
                        MushroomType = reader.GetString(12) + " - " + reader.GetString(13)
                    };
                    if (!reader.IsDBNull(4))
                        item.DesiredAirTemperature = reader.GetFloat(4);
                    if (!reader.IsDBNull(5))
                        item.DesiredAirHumidity = reader.GetFloat(5);
                    if (!reader.IsDBNull(6))
                        item.DesiredAirCo2 = reader.GetFloat(6);
                    if (!reader.IsDBNull(7))
                        item.DesiredLightLevel = reader.GetFloat(7);
                    if (!reader.IsDBNull(9))
                        item.HardwareKey = reader.GetInt32(9);
                    if (!reader.IsDBNull(11))
                        item.Hardware = reader.GetString(11);
                    temp.Add(item);
                }
                reader.Close();
                return temp.ToArray();
            }
        }

        public Specimen GetSpecimen(int specimenKey)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT _specimen.specimen_key, _specimen.planted_date, _specimen.specimen_name, _specimen.specimen_description, _specimen.desired_air_temperature, _specimen.desired_air_humidity, _specimen.desired_air_co2, _specimen.desired_light_level, _specimen.type_key, _specimen.hardware_key, _specimen.user_key, _hardware.hardware_id, _mushroom_type.mushroom_genus, _mushroom_type.mushroom_name FROM _specimen LEFT JOIN _hardware ON (_specimen.hardware_key = _hardware.hardware_key) LEFT JOIN _mushroom_type ON (_specimen.type_key = _mushroom_type.type_key) WHERE _specimen.specimen_key = @key";
                command.Parameters.AddWithValue("@key", specimenKey);
                var reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    reader.Close();
                    throw new NotFoundException("specimen " + specimenKey + " (key) not found");
                }
                Specimen item = new Specimen
                {
                    Key = reader.GetInt32(0),
                    PlantedDotnet = reader.GetDateTime(1),
                    Name = reader.GetString(2),
                    Description = reader.GetString(3),
                    TypeKey = reader.GetInt32(8),
                    MushroomType = reader.GetString(12) + " - " + reader.GetString(13)
                };
                if (!reader.IsDBNull(4))
                    item.DesiredAirTemperature = reader.GetFloat(4);
                if (!reader.IsDBNull(5))
                    item.DesiredAirHumidity = reader.GetFloat(5);
                if (!reader.IsDBNull(6))
                    item.DesiredAirCo2 = reader.GetFloat(6);
                if (!reader.IsDBNull(7))
                    item.DesiredLightLevel = reader.GetFloat(7);
                if (!reader.IsDBNull(9))
                    item.HardwareKey = reader.GetInt32(9);
                if (!reader.IsDBNull(10))
                    item.UserKey = reader.GetInt32(10);
                if (!reader.IsDBNull(11))
                    item.Hardware = reader.GetString(11);
                reader.Close();
                return item;
            }
        }

        public void DiscardSpecimen(int specimenKey)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT specimen_key FROM _specimen WHERE specimen_key = @key";
                command.Parameters.AddWithValue("@key", specimenKey);
                var reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    reader.Close();
                    throw new NotFoundException("specimen " + specimenKey + " (key) not found");
                }
                reader.Close();
                command = connection.CreateCommand();
                command.CommandText = "UPDATE _specimen SET discarded_date = @date, user_key = null, hardware_key = null WHERE specimen_key = @key";
                command.Parameters.AddWithValue("@date", DateTime.Now.ToString("yyyy-MM-dd HH':'mm':'ss.fff"));
                command.Parameters.AddWithValue("@key", specimenKey);
                command.ExecuteNonQuery();
            }
        }

        public void UpdateSpecimen(Specimen specimen)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlDataReader reader;
                var command = connection.CreateCommand();
                command.CommandText = "SELECT specimen_key FROM _specimen WHERE specimen_key = @key";
                command.Parameters.AddWithValue("@key", specimen.Key ?? throw new ConflictException("specimen key can't be null"));
                reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    reader.Close();
                    throw new NotFoundException("specimen " + specimen.Key + " (key) not found");
                }
                reader.Close();
                command = connection.CreateCommand();
                if (specimen.HardwareKey != null)
                {
                    command.CommandText = "SELECT specimen_key FROM _specimen WHERE hardware_key = @hardware";
                    command.Parameters.AddWithValue("@hardware", specimen.HardwareKey);
                    reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        var key = reader.GetInt32(0);
                        reader.Close();
                        if (key != specimen.Key)
                            throw new ConflictException("hardware is already assigned to specimen " + key + " (key)");
                    }
                    reader.Close();
                    command = connection.CreateCommand();
                }
                command.CommandText = "UPDATE _specimen SET specimen_name = @name, specimen_description = @description, desired_air_temperature = @airtemp, desired_air_humidity = @airhum, desired_air_co2 = @airco2, desired_light_level = @light, type_key = @type, hardware_key = @hardware WHERE specimen_key = @key";
                var current = GetSpecimen((int) specimen.Key);
                command.Parameters.AddWithValue("@name", specimen.Name ?? current.Name);
                command.Parameters.AddWithValue("@description", specimen.Description ?? current.Description);
                command.Parameters.AddWithValue("@airtemp", (object) specimen.DesiredAirTemperature ?? DBNull.Value);
                command.Parameters.AddWithValue("@airhum", (object) specimen.DesiredAirHumidity ?? DBNull.Value);
                command.Parameters.AddWithValue("@airco2", (object) specimen.DesiredAirCo2 ?? DBNull.Value);
                command.Parameters.AddWithValue("@light", (object) specimen.DesiredLightLevel ?? DBNull.Value);
                command.Parameters.AddWithValue("@type", specimen.TypeKey ?? current.TypeKey);
                command.Parameters.AddWithValue("@hardware", (object) specimen.HardwareKey ?? DBNull.Value);
                command.Parameters.AddWithValue("@key", specimen.Key);
                command.ExecuteNonQuery();
            }
        }

        public int CreateSensorEntry(SensorEntry sensorEntry)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO _sensor_entry (entry_time, air_temperature, air_humidity, air_co2, light_level, desired_air_temperature, desired_air_humidity, desired_air_co2, desired_light_level, specimen_key) VALUES (@date, @airtemp, @airhum, @airco2, @light, @desairtemp, @desairhum, @desairco2, @deslight, @specimen)";
                command.Parameters.AddWithValue("@date", sensorEntry.EntryTimeTsql ?? DateTime.Now.ToString("yyyy-MM-dd HH':'mm':'ss.fff"));
                command.Parameters.AddWithValue("@airtemp", sensorEntry.AirTemperature ?? throw new ConflictException("air temperature can't be null"));
                command.Parameters.AddWithValue("@airhum", sensorEntry.AirHumidity ?? throw new ConflictException("air humidity can't be null"));
                command.Parameters.AddWithValue("@airco2", sensorEntry.AirCo2 ?? throw new ConflictException("air co2 can't be null"));
                command.Parameters.AddWithValue("@light", sensorEntry.LightLevel ?? throw new ConflictException("light level can't be null"));
                command.Parameters.AddWithValue("@desairtemp", (object) sensorEntry.DesiredAirTemperature ?? DBNull.Value);
                command.Parameters.AddWithValue("@desairhum", (object) sensorEntry.DesiredAirHumidity ?? DBNull.Value);
                command.Parameters.AddWithValue("@desairco2", (object) sensorEntry.DesiredAirCo2 ?? DBNull.Value);
                command.Parameters.AddWithValue("@deslight", (object) sensorEntry.DesiredLightLevel ?? DBNull.Value);
                command.Parameters.AddWithValue("@specimen", sensorEntry.Specimen ?? throw new ConflictException("specimen key can't be null"));
                command.ExecuteNonQuery();
                command = connection.CreateCommand();
                command.CommandText = "SELECT TOP 1 entry_key FROM _sensor_entry WHERE specimen_key = @specimen ORDER BY entry_key DESC";
                command.Parameters.AddWithValue("@specimen", sensorEntry.Specimen);
                var reader = command.ExecuteReader();
                reader.Read();
                var temp = reader.GetInt32(0);
                reader.Close();
                return temp;
            }
        }

        public SensorEntry[] GetSensorHistory(int specimenKey, long? unixTimeFrom, long? unixTimeUntil)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT entry_key, entry_time, air_temperature, air_humidity, air_co2, light_level, desired_air_temperature, desired_air_humidity, desired_air_co2, desired_light_level, specimen_key FROM _sensor_entry WHERE specimen_key = @specimen";
                if (unixTimeFrom != null)
                {
                    command.CommandText += " AND entry_time >= @from";
                    command.Parameters.AddWithValue("@from", new DateTime((long) unixTimeFrom * 10000 + DateTime.UnixEpoch.Ticks).ToString("yyyy-MM-dd HH':'mm':'ss.fff"));
                }
                if (unixTimeUntil != null)
                {
                    command.CommandText += " AND entry_time <= @until";
                    command.Parameters.AddWithValue("@until", new DateTime((long) unixTimeUntil * 10000 + DateTime.UnixEpoch.Ticks).ToString("yyyy-MM-dd HH':'mm':'ss.fff"));
                }
                command.Parameters.AddWithValue("@specimen", specimenKey);
                var reader = command.ExecuteReader();
                var temp = new List<SensorEntry>();
                while (reader.Read())
                {
                    SensorEntry item = new SensorEntry
                    {
                        Key = reader.GetInt32(0),
                        EntryTimeDotnet = reader.GetDateTime(1),
                        AirTemperature = reader.GetFloat(2),
                        AirHumidity = reader.GetFloat(3),
                        AirCo2 = reader.GetFloat(4),
                        LightLevel = reader.GetFloat(5),
                        Specimen = reader.GetInt32(10)
                    };
                    if (!reader.IsDBNull(6))
                        item.DesiredAirTemperature = reader.GetFloat(6);
                    if (!reader.IsDBNull(7))
                        item.DesiredAirHumidity = reader.GetFloat(7);
                    if (!reader.IsDBNull(8))
                        item.DesiredAirCo2 = reader.GetFloat(8);
                    if (!reader.IsDBNull(9))
                        item.DesiredLightLevel = reader.GetFloat(9);
                    temp.Add(item);
                }
                reader.Close();
                return temp.ToArray();
            }
        }

        public SensorEntry GetLastEntry(int specimenKey)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT TOP(1) entry_key, entry_time, air_temperature, air_humidity, air_co2, light_level, desired_air_temperature, desired_air_humidity, desired_air_co2, desired_light_level, specimen_key FROM _sensor_entry WHERE specimen_key = @specimen ORDER BY entry_time DESC";
                command.Parameters.AddWithValue("@specimen", specimenKey);
                var reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    reader.Close();
                    throw new NotFoundException("sensor entry for specimen " + specimenKey + " (key) not found");
                }
                SensorEntry item = new SensorEntry
                {
                    Key = reader.GetInt32(0), 
                    EntryTimeDotnet = reader.GetDateTime(1),
                    AirTemperature = reader.GetFloat(2),
                    AirHumidity = reader.GetFloat(3),
                    AirCo2 = reader.GetFloat(4),
                    LightLevel = reader.GetFloat(5),
                    Specimen = reader.GetInt32(10)
                };
                if (!reader.IsDBNull(6))
                    item.DesiredAirTemperature = reader.GetFloat(6);
                if (!reader.IsDBNull(7))
                    item.DesiredAirHumidity = reader.GetFloat(7);
                if (!reader.IsDBNull(8))
                    item.DesiredAirCo2 = reader.GetFloat(8);
                if (!reader.IsDBNull(9))
                    item.DesiredLightLevel = reader.GetFloat(9);
                reader.Close();
                return item;
            }
        }

        public int CreateStatusEntry(StatusEntry statusEntry)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO _status_entry (entry_time, stage_key, specimen_key) VALUES (@entry_time, @stage_key, @specimen_key)";
                command.Parameters.AddWithValue("@entry_time", (object) statusEntry.EntryTimeTsql ?? DateTime.Now.ToString("yyyy-MM-dd HH':'mm':'ss.fff"));
                command.Parameters.AddWithValue("@specimen_key", statusEntry.Specimen ?? throw new ConflictException("specimen key can't be null"));
                command.Parameters.AddWithValue("@stage_key", statusEntry.StageKey ?? throw new ConflictException("stage key can't be null"));
                command.ExecuteNonQuery();
                command = connection.CreateCommand();
                command.CommandText = "SELECT TOP 1 entry_key FROM _status_entry WHERE stage_key = @stage AND specimen_key = @specimen ORDER BY entry_key DESC";
                command.Parameters.AddWithValue("@specimen", statusEntry.Specimen);
                command.Parameters.AddWithValue("@stage", statusEntry.StageKey);
                var reader = command.ExecuteReader();
                reader.Read();
                var temp = reader.GetInt32(0);
                reader.Close();
                return temp;
            }
        }

        public StatusEntry[] GetAllStatusEntries(int specimenKey)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT _status_entry.entry_key, _status_entry.entry_time, _status_entry.stage_key, _status_entry.specimen_key, _mushroom_stage.stage_name FROM _status_entry LEFT JOIN _mushroom_stage ON (_status_entry.stage_key = _mushroom_stage.stage_key) WHERE specimen_key = @specimen_key";
                command.Parameters.AddWithValue("@specimen_key", specimenKey);
                var reader = command.ExecuteReader();
                var temp = new List<StatusEntry>();
                while (reader.Read())
                {
                    StatusEntry item = new StatusEntry
                    {
                        Key = reader.GetInt32(0),
                        EntryTimeDotnet = reader.GetDateTime(1),
                        StageKey = reader.GetInt32(2),
                        Specimen = reader.GetInt32(3),
                        Stage = reader.GetString(4)
                    };
                    temp.Add(item);
                }
                reader.Close();
                return temp.ToArray();
            }
        }
        

    public StatusEntry GetStatusEntry(int entryKey)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT _status_entry.entry_key, _status_entry.entry_time, _status_entry.stage_key, _status_entry.specimen_key, _mushroom_stage.stage_name FROM _status_entry LEFT JOIN _mushroom_stage ON (_status_entry.stage_key = _mushroom_stage.stage_key) WHERE entry_key = @key";
                command.Parameters.AddWithValue("@key", entryKey);
                var reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    reader.Close();
                    throw new NotFoundException("status entry " + entryKey + " (key) not found");
                }
                StatusEntry temp = new StatusEntry
                {
                    Key = reader.GetInt32(0),
                    EntryTimeDotnet = reader.GetDateTime(1),
                    StageKey = reader.GetInt32(2),
                    Specimen = reader.GetInt32(3),
                    Stage = reader.GetString(4)
                };
                reader.Close();
                return temp;
            }
        }

        public void DeleteStatusEntry(int entryKey)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT entry_key FROM _status_entry WHERE entry_key = @key";
                command.Parameters.AddWithValue("@key", entryKey);
                var reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    reader.Close();
                    throw new NotFoundException("status entry " + entryKey + " (key) not found");
                }
                reader.Close();
                command = connection.CreateCommand();
                command.CommandText = "DELETE FROM _status_entry WHERE entry_key = @key";
                command.Parameters.AddWithValue("@key", entryKey);
                command.ExecuteNonQuery();
            }
        }

        public void UpdateStatusEntry(StatusEntry statusEntry)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT entry_key FROM _status_entry WHERE entry_key = @key";
                command.Parameters.AddWithValue("@key", statusEntry.Key ?? throw new ConflictException("entry key can't be null"));
                var reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    reader.Close();
                    throw new NotFoundException("status entry " + statusEntry.Key + " (key) not found");
                }
                reader.Close();
                command = connection.CreateCommand();
                command.CommandText = "UPDATE _status_entry SET entry_time = @date, stage_key = @stage WHERE entry_key = @key";
                var current = GetStatusEntry((int) statusEntry.Key);
                command.Parameters.AddWithValue("@date", statusEntry.EntryTimeTsql ?? current.EntryTimeTsql);
                command.Parameters.AddWithValue("@stage", statusEntry.StageKey ?? current.StageKey);
                command.Parameters.AddWithValue("@key", statusEntry.Key);
                command.ExecuteNonQuery();
            }
        }

        public void DropSchema()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "IF OBJECT_ID('dbo._sensor_entry') IS NOT NULL TRUNCATE TABLE dbo._sensor_entry; IF OBJECT_ID('dbo._sensor_entry') IS NOT NULL DROP TABLE dbo._sensor_entry; IF OBJECT_ID('dbo._status_entry') IS NOT NULL TRUNCATE TABLE dbo._status_entry; IF OBJECT_ID('dbo._status_entry') IS NOT NULL DROP TABLE dbo._status_entry; IF OBJECT_ID('dbo._specimen') IS NOT NULL TRUNCATE TABLE dbo._specimen; IF OBJECT_ID('dbo._specimen') IS NOT NULL DROP TABLE dbo._specimen; IF OBJECT_ID('dbo._mushroom_type') IS NOT NULL TRUNCATE TABLE dbo._mushroom_type; IF OBJECT_ID('dbo._mushroom_type') IS NOT NULL DROP TABLE dbo._mushroom_type; IF OBJECT_ID('dbo._mushroom_stage') IS NOT NULL TRUNCATE TABLE dbo._mushroom_stage; IF OBJECT_ID('dbo._mushroom_stage') IS NOT NULL DROP TABLE dbo._mushroom_stage; IF OBJECT_ID('dbo._hardware') IS NOT NULL TRUNCATE TABLE dbo._hardware; IF OBJECT_ID('dbo._hardware') IS NOT NULL DROP TABLE dbo._hardware; IF OBJECT_ID('dbo._user') IS NOT NULL TRUNCATE TABLE dbo._user; IF OBJECT_ID('dbo._user') IS NOT NULL DROP TABLE dbo._user; IF OBJECT_ID('dbo._permission_level') IS NOT NULL TRUNCATE TABLE dbo._permission_level; IF OBJECT_ID('dbo._permission_level') IS NOT NULL DROP TABLE dbo._permission_level;";
                command.ExecuteNonQuery();
            }
        }
        
        public void InitSchema()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "CREATE TABLE [_hardware]( [hardware_key] int IDENTITY (1, 1) NOT NULL, [hardware_id] nvarchar(16) NOT NULL, [desired_air_temperature] real NULL, [desired_air_humidity] real NULL, [desired_air_co2] real NULL, [desired_light_level] real NULL, [user_key] int NOT NULL); CREATE TABLE [_mushroom_stage] ( [stage_key] int IDENTITY (1, 1) NOT NULL, [stage_name] nvarchar(32) NOT NULL ); CREATE TABLE [_mushroom_type] ( [type_key] int IDENTITY (1, 1) NOT NULL, [mushroom_name] nvarchar(32) NOT NULL, [mushroom_genus] nvarchar(32) NOT NULL ); CREATE TABLE [_permission_level] ( [permission_key] int IDENTITY (1, 1) NOT NULL, [permission_type] nvarchar(16) NOT NULL ); CREATE TABLE [_sensor_entry] ( [entry_key] int IDENTITY (1, 1) NOT NULL, [entry_time] datetime2(3) NOT NULL DEFAULT GETDATE(), [air_temperature] real NOT NULL, [air_humidity] real NOT NULL, [air_co2] real NOT NULL, [light_level] real NOT NULL, [desired_air_temperature] real NULL, [desired_air_humidity] real NULL, [desired_air_co2] real NULL, [desired_light_level] real NULL, [specimen_key] int NULL ); CREATE TABLE [_specimen] ( [specimen_key] int IDENTITY (1, 1) NOT NULL, [planted_date] datetime2(3) NOT NULL DEFAULT GETDATE(), [discarded_date] datetime2(3) NULL, [specimen_name] nvarchar(32) NOT NULL, [specimen_description] nvarchar(256) NOT NULL, [desired_air_temperature] real NULL, [desired_air_humidity] real NULL, [desired_air_co2] real NULL, [desired_light_level] real NULL, [type_key] int NOT NULL, [hardware_key] int NULL, [user_key] int NULL ); CREATE TABLE [_status_entry] ( [entry_key] int IDENTITY (1, 1) NOT NULL, [entry_time] datetime2(3) NOT NULL DEFAULT GETDATE(), [stage_key] int NOT NULL, [specimen_key] int NOT NULL ); CREATE TABLE [_user] ( [user_key] int IDENTITY (1, 1) NOT NULL, [username] nvarchar(32) NOT NULL, [password_hashed] nvarchar(64) NOT NULL, [permission_key] int NOT NULL DEFAULT 1, [user_token] nvarchar(64) NULL ); ALTER TABLE [_hardware] ADD CONSTRAINT [PK_hardware] PRIMARY KEY CLUSTERED ([hardware_key] ASC); ALTER TABLE [_mushroom_stage] ADD CONSTRAINT [PK_mushroom_stage] PRIMARY KEY CLUSTERED ([stage_key] ASC); ALTER TABLE [_mushroom_stage] ADD CONSTRAINT [mushroom_stage] UNIQUE NONCLUSTERED ([stage_name] ASC); ALTER TABLE [_mushroom_type] ADD CONSTRAINT [PK_mushroom_type] PRIMARY KEY CLUSTERED ([type_key] ASC); ALTER TABLE [_permission_level] ADD CONSTRAINT [PK_permission_level] PRIMARY KEY CLUSTERED ([permission_key] ASC); ALTER TABLE [_sensor_entry] ADD CONSTRAINT [PK_sensor_entry] PRIMARY KEY CLUSTERED ([entry_key] ASC); ALTER TABLE [_specimen] ADD CONSTRAINT [PK_specimen] PRIMARY KEY CLUSTERED ([specimen_key] ASC); ALTER TABLE [_status_entry] ADD CONSTRAINT [PK_status_entry] PRIMARY KEY CLUSTERED ([entry_key] ASC); ALTER TABLE [_user] ADD CONSTRAINT [PK_user] PRIMARY KEY CLUSTERED ([user_key] ASC); ALTER TABLE [_user] ADD CONSTRAINT [username] UNIQUE NONCLUSTERED ([username] ASC); ALTER TABLE [_hardware] ADD CONSTRAINT [FK_hardware_user] FOREIGN KEY ([user_key]) REFERENCES [_user] ([user_key]) ON DELETE No Action ON UPDATE No Action; ALTER TABLE [_sensor_entry] ADD CONSTRAINT [FK_sensor_entry_specimen] FOREIGN KEY ([specimen_key]) REFERENCES [_specimen] ([specimen_key]) ON DELETE Cascade ON UPDATE No Action; ALTER TABLE [_specimen] ADD CONSTRAINT [FK_specimen_hardware] FOREIGN KEY ([hardware_key]) REFERENCES [_hardware] ([hardware_key]) ON DELETE No Action ON UPDATE No Action; ALTER TABLE [_specimen] ADD CONSTRAINT [FK_specimen_mushroom_type] FOREIGN KEY ([type_key]) REFERENCES [_mushroom_type] ([type_key]) ON DELETE No Action ON UPDATE No Action; ALTER TABLE [_specimen] ADD CONSTRAINT [FK_specimen_user] FOREIGN KEY ([user_key]) REFERENCES [_user] ([user_key]) ON DELETE No Action ON UPDATE No Action; ALTER TABLE [_status_entry] ADD CONSTRAINT [FK_status_entry_mushroom_stage] FOREIGN KEY ([stage_key]) REFERENCES [_mushroom_stage] ([stage_key]) ON DELETE No Action ON UPDATE No Action; ALTER TABLE [_status_entry] ADD CONSTRAINT [FK_status_entry_specimen] FOREIGN KEY ([specimen_key]) REFERENCES [_specimen] ([specimen_key]) ON DELETE Cascade ON UPDATE No Action; ALTER TABLE [_user] ADD CONSTRAINT [FK_user_permission_level] FOREIGN KEY ([permission_key]) REFERENCES [_permission_level] ([permission_key]) ON DELETE Set Default ON UPDATE No Action;";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TRIGGER dbo._user_delete_cascade ON dbo._user INSTEAD OF DELETE AS BEGIN SET NOCOUNT ON; UPDATE dbo._specimen SET user_key = NULL WHERE user_key IN(SELECT user_key FROM DELETED); DELETE FROM dbo._hardware WHERE user_key IN (SELECT user_key FROM DELETED); DELETE FROM dbo._user WHERE user_key IN (SELECT user_key FROM DELETED); END";
                command.ExecuteNonQuery();
                command.CommandText = "CREATE TRIGGER dbo._hardware_delete_cascade ON dbo._hardware INSTEAD OF DELETE AS BEGIN SET NOCOUNT ON; UPDATE dbo._specimen SET hardware_key = NULL WHERE hardware_key IN(SELECT hardware_key FROM DELETED); DELETE FROM dbo._hardware WHERE hardware_key IN (SELECT hardware_key FROM DELETED); END";
                command.ExecuteNonQuery();
                command.CommandText = "INSERT INTO dbo._permission_level(permission_type) VALUES ('user'); INSERT INTO dbo._permission_level (permission_type) VALUES ('admin'); INSERT INTO dbo._permission_level (permission_type) VALUES ('developer'); INSERT INTO dbo._mushroom_stage (stage_name) VALUES ('Inoculation'); INSERT INTO dbo._mushroom_stage (stage_name) VALUES ('Casing'); INSERT INTO dbo._mushroom_stage (stage_name) VALUES ('Spawn Run'); INSERT INTO dbo._mushroom_stage (stage_name) VALUES ('Pinning'); INSERT INTO dbo._mushroom_stage (stage_name) VALUES ('Fruiting'); INSERT INTO dbo._mushroom_stage (stage_name) VALUES ('Dead'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Agaricus','bisporus'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Lentinula','edodes'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Auricularia','auricula-judae'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Volvariella','volvacea'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Flammulina','velutipes'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Tremella','fuciformis'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Hypsizygus','tessellatus'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Stropharia','rugosoannulata'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Cyclocybe','aegerita'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Hericium','erinaceus'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Phallus','indusiatus'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Boletus','edulis'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Calbovista','subsculpta'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Calvatia','gigantea'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Cantharellus','cibarius'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Craterellus','tubaeformis'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Clitocybe','nuda'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Cortinarius','caperatus'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Craterellus','cornucopioides'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Grifola','frondosa'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Gyromitra','esculenta'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Hericium','erinaceus'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Hydnum','repandum'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Lactarius','deliciosus'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Morchella','conica'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Morchella','esculenta'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Tricholoma','matsutake'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Tuber','aestivum'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Tuber','borchii'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Tuber','brumale'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Tuber','indicum'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Tuber','macrosporum'); INSERT INTO dbo._mushroom_type (mushroom_genus, mushroom_name) VALUES ('Tuber','mesentericum'); ";
                command.ExecuteNonQuery();
            }
        }
        
        private static void AddNullableCondition(SqlCommand command, string parameterName, object value)
        {
            if (value == null)
                command.CommandText = command.CommandText.Replace("= " + parameterName, "IS NULL");
            else
                command.Parameters.AddWithValue(parameterName, value);
        }
    }
}