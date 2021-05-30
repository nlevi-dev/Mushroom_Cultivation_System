using SEP4_Data.Model;

namespace SEP4_Data.Data
{
    public interface ISampleService
    {
        public SensorEntry GetLatestEntry(int userKey, string hardwareId);
        public SensorEntry[] GetLatestEntries(int userKey);
        public SensorEntry[] GetEntries(int userKey);
        public void SetDesiredHardwareValues(Hardware hardware);
        public void AddUser(User user);
        public void DeleteUser(int userKey);
        public void UpdateUser(User user);
        public SensorEntry GetCached(string hardwareId);
    }
}