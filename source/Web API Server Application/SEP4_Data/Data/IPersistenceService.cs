using SEP4_Data.Model;

namespace SEP4_Data.Data
{
    public interface IPersistenceService
    {
        public string[] GetMushroomStages();
        public int GetMushroomStageKey(string name);
        public string[] GetMushroomTypes();
        public int GetMushroomTypeKey(string name);
        public bool CheckUserPassword(string username, string password);
        public int CreateUser(User user);
        public User GetUserByName(string username);
        public User GetUserByKey(int userKey);
        public void DeleteUser(int userKey);
        public void UpdateUsername(int userKey, string username);
        public void UpdateUserPassword(int userKey, string password);
        public int CreateHardware(Hardware hardware);
        public Hardware[] GetAllHardware(int userKey);
        public Hardware GetHardwareById(string hardwareId);
        public Hardware GetHardware(int hardwareKey);
        public void DeleteHardware(int hardwareKey);
        public void UpdateHardware(Hardware hardware);
        public int CreateSpecimen(Specimen specimen);
        public Specimen[] GetAllSpecimen(int userKey);
        public Specimen GetSpecimen(int specimenKey);
        public void DiscardSpecimen(int specimenKey);
        public void UpdateSpecimen(Specimen specimen);
        public int CreateSensorEntry(SensorEntry sensorEntry);
        public SensorEntry[] GetSensorHistory(int specimenKey, long? unixTimeFrom, long? unixTimeUntil);
        public int CreateStatusEntry(StatusEntry statusEntry);
        public StatusEntry[] GetAllStatusEntries(int specimenKey);
        public StatusEntry GetStatusEntry(int entryKey);
        public void DeleteStatusEntry(int entryKey);
        public void UpdateStatusEntry(StatusEntry statusEntry);
    }
}