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
        [JsonPropertyName("desired_light_level")]
        public float? DesiredLightLevel { get; set; }
        [JsonPropertyName("user_key")]
        public int? UserKey { get; set; }

        public override string ToString()
        {
            return "Hardware (key): " + Key;
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is Hardware))
                return false;
            Hardware hardware = (Hardware) obj;
            return Key == hardware.Key && Id == hardware.Id && SpecimenKey == hardware.SpecimenKey && DesiredAirTemperature == hardware.DesiredAirTemperature && DesiredAirHumidity == hardware.DesiredAirHumidity && DesiredAirCo2 == hardware.DesiredAirCo2 && DesiredLightLevel == hardware.DesiredLightLevel;
        }
    }
}