using System.Text.Json.Serialization;

namespace SEP4_Data.Model
{
    public class User
    {
        [JsonPropertyName("user_key")]
        public int? Key { get; set; }
        [JsonPropertyName("username")]
        public string Name { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
        [JsonIgnore]
        public int? PermissionLevel { get; set; }
        [JsonPropertyName("user_type")]
        public string Permission { get; set; }
    }
}