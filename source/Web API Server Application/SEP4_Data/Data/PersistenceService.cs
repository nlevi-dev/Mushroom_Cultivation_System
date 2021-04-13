using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.SqlClient;
using SEP4_Data.Model;
using SEP4_Data.Model.Exception;

namespace SEP4_Data.Data
{
    public class PersistenceService : IPersistenceService
    {
        private readonly IConfigService _config;
        private readonly string _connectionString;
        
        public PersistenceService(IConfigService config)
        {
            _config = config;
            _connectionString = "Server=" + _config.DbHost + "," + _config.DbPort + ";Database=" + _config.DbName + ";User Id=" + _config.DbUser + ";Password=" + _config.DbPassword + ";";
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
                return temp.ToArray();
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
                return temp.ToArray();
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
                    return reader.GetString(0) == passwordHashed;
            }
            return false;
        }

        public int CreateUser(User user)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO _user (username, password_hashed) VALUES (@name, @password)";
                command.Parameters.AddWithValue("@name", user.Name);
                command.Parameters.AddWithValue("@password", user.Password);
                try {
                    command.ExecuteNonQuery();
                } catch {
                    throw new ConflictException("username taken");
                }
                command = connection.CreateCommand();
                command.CommandText = "SELECT TOP 1 user_key FROM _user WHERE username = @name AND password_hashed = @password ORDER BY user_key DESC;";
                command.Parameters.AddWithValue("@name", user.Name);
                command.Parameters.AddWithValue("@password", user.Password);
                var reader = command.ExecuteReader();
                reader.Read();
                return reader.GetInt32(0);
            }
        }

        public User GetUserByName(string username)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    "SELECT _user.user_key, _user.username, _user.permission_key, _permission_level.permission_type FROM _user LEFT JOIN _permission_level ON (_user.permission_key = _permission_level.permission_key) WHERE _user.username = @name";
                command.Parameters.AddWithValue("@name", username);
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
                    return temp;
                }
            }
            throw new NotFoundException("User \"" + username + "\" (name) not found!");
        }

        public User GetUserByKey(int userKey)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    "SELECT _user.user_key, _user.username, _user.permission_key, _permission_level.permission_type FROM _user LEFT JOIN _permission_level ON (_user.permission_key = _permission_level.permission_key) WHERE _user.user_key = @key";
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
                    return temp;
                }
            }
            throw new NotFoundException("User \"" + userKey + "\" (key) not found!");
        }

        public void DeleteUser(int userKey)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "";
                throw new NotImplementedException();
            }
        }

        public void UpdateUsername(int userKey, string username)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "";
                throw new NotImplementedException();
            }
        }

        public void UpdateUserPassword(int userKey, string password)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "";
                throw new NotImplementedException();
            }
        }

        public int CreateHardware(Hardware hardware)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = 
                    "INSERT INTO _hardware (hardware_key, hardware_id, specimen_key,desired_air_temperature,desired_air_humidity,desired_air_co2) VALUES (@key, @id,@specimen,@airtemp,@airhumidity,@air_c02)";
                command.Parameters.AddWithValue("@key", hardware.Key);
                command.Parameters.AddWithValue("@id", hardware.Id);
                command.Parameters.AddWithValue("@specimen", hardware.SpecimenKey);
                command.Parameters.AddWithValue("@airtemp", hardware.DesiredAirTemperature);
                command.Parameters.AddWithValue("@airhumidity", hardware.DesiredAirHumidity);
                command.Parameters.AddWithValue("@air_co2", hardware.DesiredAirCo2);
                try {
                    command.ExecuteNonQuery();
                } catch {
                    throw new ConflictException("hardware already registered");
                }
                var reader = command.ExecuteReader();
                reader.Read();
                return reader.GetInt32(0);
            }
            
        }

        public Hardware[] GetAllHardware(int userKey)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * from _hardware WHERE user_key = @user_key";
                command.Parameters.AddWithValue("@user_key", userKey);
                var reader = command.ExecuteReader();
                var temp = new List<Hardware>();
                while (reader.Read())
                {
                    Hardware item = new Hardware()
                    {
                        Key = (int) reader["hardware_key"],
                        Id = (string) reader["hardware_id"],
                        SpecimenKey = (int) reader["specimen_key"],
                        DesiredAirTemperature = (float) reader["desired_air_temperature"],
                        DesiredAirCo2 = (float) reader["desired_air_co2"],
                        DesiredAirHumidity = (float) reader["desired_air_humidity"]
                    };
                    temp.Add(item);
                }

                return temp.ToArray();
            }
        }

        public Hardware GetHardware(int hardwareKey)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT * from _hardware WHERE hardware_key = @key";
                command.Parameters.AddWithValue("@key", hardwareKey);
                var reader = command.ExecuteReader();
                
                if (reader.Read())
                { 
                    Hardware temp = new Hardware()
                    {
                        Key = (int) reader["hardware_key"],
                        Id = (string) reader["hardware_id"],
                        SpecimenKey = (int) reader["specimen_key"],
                        DesiredAirTemperature = (float) reader["desired_air_temperature"],
                        DesiredAirCo2 = (float) reader["desired_air_co2"],
                        DesiredAirHumidity = (float) reader["desired_air_humidity"]
                    };
                    return temp;
                }
            }
            throw new NotFoundException("hardware \"" + hardwareKey + "\" not found!");
        }

        public void DeleteHardware(int hardwareKey)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "DELETE * FROM _hardware WHERE hardware_key = @key";
                command.Parameters.AddWithValue("@key", hardwareKey);
                try {
                    command.ExecuteNonQuery();
                } catch (Exception e) {
                    if (e.Message.Contains("duplicate key"))
                        throw new ConflictException("liking already exists");
                    throw;
                }
            }
        }

        public void UpdateHardware(Hardware hardware)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    "UPDATE _hardware SET desired_air_temperature = @field1, desired_air_humidity = @field2, desired_air_co2 = @field3 WHERE id = @id";
                command.Parameters.AddWithValue("@Field1", hardware.DesiredAirTemperature);
                command.Parameters.AddWithValue("@Field2", hardware.DesiredAirHumidity);
                command.Parameters.AddWithValue("@Field1", hardware.DesiredAirCo2);
                command.ExecuteNonQuery();
            }
        }

        public int CreateSpecimen(Specimen specimen)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "";
                throw new NotImplementedException();
            }
        }

        public Specimen[] GetAllSpecimen(int userKey)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "";
                throw new NotImplementedException();
            }
        }

        public Specimen GetSpecimen(int specimenKey)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "";
                throw new NotImplementedException();
            }
        }

        public void DiscardSpecimen(int specimenKey)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "";
                throw new NotImplementedException();
            }
        }

        public void UpdateSpecimen(Specimen specimen)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "";
                throw new NotImplementedException();
            }
        }

        public int CreateSensorEntry(SensorEntry sensorEntry)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "";
                throw new NotImplementedException();
            }
        }

        public SensorEntry[] GetSensorHistory(int specimenKey, long? unixTimeFrom, long? unixTimeUntil)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "";
                throw new NotImplementedException();
            }
        }

        public int CreateStatusEntry(StatusEntry statusEntry)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "";
                throw new NotImplementedException();
            }
        }

        public StatusEntry[] GetAllStatusEntries(int specimenKey)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "";
                throw new NotImplementedException();
            }
        }

        public StatusEntry GetStatusEntry(int entryKey)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "";
                throw new NotImplementedException();
            }
        }

        public void DeleteStatusEntry(int entryKey)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "";
                throw new NotImplementedException();
            }
        }

        public void UpdateStatusEntry(StatusEntry statusEntry)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "";
                throw new NotImplementedException();
            }
        }
    }
}