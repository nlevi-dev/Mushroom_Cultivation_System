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
        [JsonPropertyName("desired_air_temperature")]
        public float? DesiredAirTemperature { get; set; }
        [JsonPropertyName("desired_air_humidity")]
        public float? DesiredAirHumidity { get; set; }
        [JsonPropertyName("desired_air_co2")]
        public float? DesiredAirCo2 { get; set; }
        [JsonPropertyName("ambient_air_temperature")]
        public float? AmbientAirTemperature { get; set; }
        [JsonPropertyName("ambient_air_humidity")]
        public float? AmbientAirHumidity { get; set; }
        [JsonPropertyName("ambient_air_co2")]
        public float? AmbientAirCo2 { get; set; }
        [JsonPropertyName("specimen_key")]
        public int? Specimen { get; set; }
    }
}