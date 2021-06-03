using System;
using System.Collections;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using SEP4_Data.Model;
using SEP4_Data.Model.Exception;

namespace SEP4_Data.Data
{
    public class SampleService : ISampleService
    {
        private readonly ILogService _log;
        private readonly IPersistenceService _persistence;
        private readonly HttpClient _client;
        private readonly ArrayList _lastEntriesByHardware;

        public SampleService(IConfigService config, ILogService log, IPersistenceService persistence)
        {
            _log = log;
            _persistence = persistence;
            _client = new HttpClient {BaseAddress = new Uri("http://127.0.0.1:41003/")};
            _lastEntriesByHardware = new ArrayList();
            var hardwares = _persistence.GetAllHardware(null);
            foreach (Hardware hardware in hardwares)
                if (hardware.Id != null)
                    SetDesiredHardwareValues(hardware);
            new Thread(() =>
            {
                Thread.Sleep(60000);
                try
                {
                    while (true)
                    {
                        var users = _persistence.GetAllUser();
                        foreach (User user in users)
                            GetLatestEntries((int) user.Key);
                        Thread.Sleep(config.SampleInterval * 60000);
                    }
                } catch (Exception e) {
                    _log.Log(e.ToString());
                }
            }).Start();
        }

        public SensorEntry GetLatestEntry(int userKey, string hardwareId)
        {
            try {
                var res = GetEntriesBase(userKey);
                if (res == null) return null;
                SaveEntries(SortOutLatest(res));
                SensorEntry compare = new SensorEntry{EntryTimeUnix = 0};
                foreach (SensorEntry entry in res)
                    if (hardwareId == entry.Id)
                        if (compare.EntryTimeUnix <= entry.EntryTimeUnix)
                            compare = entry;
                if (compare.Id != hardwareId)
                    return null;
                return compare;
            } catch (Exception e) {
                _log.Log(e.ToString());
                return null;
            }
        }
        
        public SensorEntry[] GetLatestEntries(int userKey)
        {
            try {
                var res = GetEntriesBase(userKey);
                if (res == null) return Array.Empty<SensorEntry>();
                var temp = SortOutLatest(res);
                SaveEntries(temp);
                return temp;
            } catch (Exception e) {
                _log.Log(e.ToString());
                return Array.Empty<SensorEntry>();
            }
        }
        
        public SensorEntry[] GetEntries(int userKey)
        {
            try {
                var res = GetEntriesBase(userKey);
                SaveEntries(SortOutLatest(res));
                return res;
            } catch (Exception e) {
                _log.Log(e.ToString());
                return Array.Empty<SensorEntry>();
            }
        }

        private SensorEntry[] GetEntriesBase(int userKey)
        {
            HttpResponseMessage response = _client.GetAsync("/hardware/" + userKey).Result;
            if (!response.IsSuccessStatusCode) throw new GatewayException("gateway error");
            var res = (SensorEntry[]) JsonSerializer.Deserialize(response.Content.ReadAsStringAsync().Result, typeof(SensorEntry[]));
            return res;
        }

        public void SetDesiredHardwareValues(Hardware hardware)
        {
            try {
                var json = JsonSerializer.Serialize(hardware);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync("/hardware", data).Result;
                if (!response.IsSuccessStatusCode) throw new GatewayException("gateway error");
            } catch (Exception e) {
                _log.Log(e.ToString());
            }
        }
        
        public void AddUser(User user)
        {
            try {
                var json = JsonSerializer.Serialize(user);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = _client.PostAsync("/user", data).Result;
                if (!response.IsSuccessStatusCode) throw new GatewayException("gateway error");
            } catch (Exception e) {
                _log.Log(e.ToString());
            }
        }
        
        public void DeleteUser(int userKey)
        {
            try {
                HttpResponseMessage response = _client.DeleteAsync("/user/" + userKey).Result;
                if (!response.IsSuccessStatusCode) throw new GatewayException("gateway error");
            } catch (Exception e) {
                _log.Log(e.ToString());
            }
        }

        public void UpdateUser(User user)
        {
            try {
                if (user.Key == null)
                    throw new ConflictException("user key can not be null");
                DeleteUser((int) user.Key);
                AddUser(user);
            } catch (Exception e) {
                _log.Log(e.ToString());
            }
        }

        private static SensorEntry[] SortOutLatest(SensorEntry[] entries)
        {
            if (entries.Length == 0)
                return Array.Empty<SensorEntry>();
            ArrayList latest = new ArrayList();
            foreach (SensorEntry entry in entries)
            {
                bool hasCategory = false;
                foreach (SensorEntry category in latest)
                {
                    if (category.Id != entry.Id) continue;
                    hasCategory = true;
                    if (category.EntryTimeUnix < entry.EntryTimeUnix)
                    {
                        latest.Remove(category);
                        latest.Add(entry);
                    }
                    break;
                }
                if (!hasCategory)
                    latest.Add(entry);
            }
            return (SensorEntry[]) latest.ToArray(typeof(SensorEntry));
        }
        
        private void SaveEntries(SensorEntry[] entries)
        {
            if (entries.Length == 0)
                return;
            foreach (SensorEntry entry in entries)
            {
                try
                {
                    Hardware hardware = _persistence.GetHardwareById(entry.Id);
                    entry.Specimen = hardware.SpecimenKey;
                    entry.DesiredAirTemperature = hardware.DesiredAirTemperature;
                    entry.DesiredAirHumidity = hardware.DesiredAirHumidity;
                    entry.DesiredAirCo2 = hardware.DesiredAirCo2;
                    entry.DesiredLightLevel = hardware.DesiredLightLevel;
                    //quickfix
                    entry.EntryTimeDotnet = DateTime.Now;
                    //--------
                    _persistence.CreateSensorEntry(entry);
                } catch (NotFoundException) {/* ignored */}
            }
            foreach (SensorEntry entry in entries)
            {
                bool hasCategory = false;
                foreach (SensorEntry category in _lastEntriesByHardware)
                {
                    if (category.Id != entry.Id) continue;
                    hasCategory = true;
                    if (category.EntryTimeUnix < entry.EntryTimeUnix)
                    {
                        _lastEntriesByHardware.Remove(category);
                        _lastEntriesByHardware.Add(entry);
                    }
                    break;
                }
                if (!hasCategory)
                    _lastEntriesByHardware.Add(entry);
            }
        }

        public SensorEntry GetCached(string hardwareId)
        {
            foreach (SensorEntry category in _lastEntriesByHardware)
                if (category.Id == hardwareId)
                    return category;
            return null;
        }
    }
}