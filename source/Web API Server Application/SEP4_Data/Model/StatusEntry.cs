using System;
using System.Text.Json.Serialization;

namespace SEP4_Data.Model
{
    public class StatusEntry
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
        [JsonPropertyName("stage_name")]
        public string Stage { get; set; }
        [JsonIgnore]
        public int? StageKey { get; set; }
        [JsonPropertyName("specimen_key")]
        public int? Specimen { get; set; }
    }
}