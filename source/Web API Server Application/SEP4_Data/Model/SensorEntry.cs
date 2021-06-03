using System;
using System.Text.Json.Serialization;

namespace SEP4_Data.Model
{
    public class SensorEntry
    {
        [JsonPropertyName("entry_key")]
        public int? Key { get; set; }
        [JsonPropertyName("entry_time")]
        public long? EntryTimeUnix { get; set; }
        [JsonIgnore]
        public DateTime? EntryTimeDotnet
        {
            get
            {
                if (EntryTimeUnix == null)
                    return null;
                return new DateTime((long) EntryTimeUnix * 10000 + DateTime.UnixEpoch.Ticks);
            }
            set
            {
                if (value == null)
                    EntryTimeUnix = null;
                else
                    EntryTimeUnix = (((DateTime) value).Ticks - DateTime.UnixEpoch.Ticks) / 10000;
            }
        }
        [JsonIgnore]
        public string EntryTimeTsql
        {
            get => EntryTimeDotnet?.ToString("yyyy-MM-dd HH':'mm':'ss.fff");
            set => EntryTimeDotnet = DateTime.Parse(value);
        }
        [JsonPropertyName("air_temperature")]
        public float? AirTemperature { get; set; }
        [JsonPropertyName("air_humidity")]
        public float? AirHumidity { get; set; }
        [JsonPropertyName("air_co2")]
        public float? AirCo2 { get; set; }
        [JsonPropertyName("light_level")]
        public float? LightLevel { get; set; }
        [JsonPropertyName("desired_air_temperature")]
        public float? DesiredAirTemperature { get; set; }
        [JsonPropertyName("desired_air_humidity")]
        public float? DesiredAirHumidity { get; set; }
        [JsonPropertyName("desired_air_co2")]
        public float? DesiredAirCo2 { get; set; }
        [JsonPropertyName("desired_light_level")]
        public float? DesiredLightLevel { get; set; }
        [JsonPropertyName("specimen_key")]
        public int? Specimen { get; set; }
        [JsonPropertyName("hardware_id")]
        public string Id { get; set; }
        
        public override string ToString()
        {
            return "SensorEntry (key): " + Key;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is SensorEntry))
                return false;
            SensorEntry sensorEntry = (SensorEntry) obj;
            return Key == sensorEntry.Key && EntryTimeUnix == sensorEntry.EntryTimeUnix && AirTemperature == sensorEntry.AirTemperature && AirHumidity == sensorEntry.AirHumidity && AirCo2 == sensorEntry.AirCo2 && LightLevel == sensorEntry.LightLevel && DesiredAirTemperature == sensorEntry.DesiredAirTemperature && DesiredAirHumidity == sensorEntry.DesiredAirHumidity && DesiredAirCo2 == sensorEntry.DesiredAirCo2 && DesiredLightLevel == sensorEntry.DesiredLightLevel && Specimen == sensorEntry.Specimen;
        }
        
        public override int GetHashCode()
        {
            // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
            return base.GetHashCode();
        }
    }
}