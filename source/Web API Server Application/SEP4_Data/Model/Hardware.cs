using System.Text.Json.Serialization;

namespace SEP4_Data.Model
{
    public class Hardware
    {
        [JsonPropertyName("hardware_key")]
        public int? Key { get; set; }
        [JsonPropertyName("hardware_id")]
        public string Id { get; set; }
        [JsonPropertyName("specimen_key")]
        public int? SpecimenKey { get; set; }
        [JsonPropertyName("desired_air_temperature")]
        public float? DesiredAirTemperature { get; set; }
        [JsonPropertyName("desired_air_humidity")]
        public float? DesiredAirHumidity { get; set; }
        [JsonPropertyName("desired_air_co2")]
        public float? DesiredAirCo2 { get; set; }
        [JsonIgnore]
        public int? UserKey { get; set; }
    }
}