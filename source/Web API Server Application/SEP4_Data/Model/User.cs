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
        [JsonPropertyName("user_token")]
        public string Token { get; set; }
        
        public override string ToString()
        {
            return "User (key): " + Key;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is User))
                return false;
            User user = (User) obj;
            return Key == user.Key && Name == user.Name && Permission == user.Permission && Token == user.Token;
        }
        
        public override int GetHashCode()
        {
            // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
            return base.GetHashCode();
        }
    }
}