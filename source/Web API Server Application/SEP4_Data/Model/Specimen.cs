using System;
using System.Text.Json.Serialization;

namespace SEP4_Data.Model
{
    public class Specimen
    {
        [JsonPropertyName("specimen_key")]
        public int? Key { get; set; }
        [JsonPropertyName("planted_date")]
        public long? PlantedUnix { get; set; }
        [JsonIgnore]
        public DateTime? PlantedDotnet
        {
            get
            {
                if (PlantedUnix == null)
                    return null;
                return new DateTime((long) PlantedUnix * 10000 + DateTime.UnixEpoch.Ticks);
            }
            set
            {
                if (value == null)
                    PlantedUnix = null;
                else
                    PlantedUnix = (((DateTime) value).Ticks - DateTime.UnixEpoch.Ticks) / 10000;
            }
        }
        [JsonIgnore]
        public string PlantedTsql
        {
            get => PlantedDotnet?.ToString("yyyy-MM-dd HH':'mm':'ss.fff");
            set => PlantedDotnet = DateTime.Parse(value);
        }
        [JsonPropertyName("specimen_name")]
        public string Name { get; set; }
        [JsonPropertyName("specimen_type")]
        public string MushroomType { get; set; }
        [JsonIgnore]
        public int? TypeKey { get; set; }
        [JsonPropertyName("specimen_description")]
        public string Description { get; set; }
        [JsonPropertyName("desired_air_temperature")]
        public float? DesiredAirTemperature { get; set; }
        [JsonPropertyName("desired_air_humidity")]
        public float? DesiredAirHumidity { get; set; }
        [JsonPropertyName("desired_air_co2")]
        public float? DesiredAirCo2 { get; set; }
        [JsonPropertyName("desired_light_level")]
        public float? DesiredLightLevel { get; set; }
        [JsonPropertyName("hardware_id")]
        public string Hardware { get; set; }
        [JsonIgnore]
        public int? HardwareKey { get; set; }
        [JsonIgnore]
        public int? UserKey { get; set; }
        
        public override string ToString()
        {
            return "Specimen (key): " + Key;
        }

        public override bool Equals(object? obj)
        {
            if (!(obj is Specimen))
                return false;
            Specimen specimen = (Specimen) obj;
            return Key == specimen.Key && PlantedUnix == specimen.PlantedUnix && Name == specimen.Name && MushroomType == specimen.MushroomType && Description == specimen.Description && DesiredAirTemperature == specimen.DesiredAirTemperature && DesiredAirHumidity == specimen.DesiredAirHumidity && DesiredAirCo2 == specimen.DesiredAirCo2 && DesiredLightLevel == specimen.DesiredLightLevel && Hardware == specimen.Hardware;
        }
    }
}