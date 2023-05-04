using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MauiAuth0App.Models {
    public class Alarms {

        [JsonPropertyName("number")]
        public int Number { get; set; }
        [JsonPropertyName("data")]
        public List<Alarm> Data { get; set; }
        [JsonPropertyName("totalElements")]
        public int TotalElements { get; set; }
        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }
        [JsonPropertyName("last")]
        public bool Last { get; set; }
    }

    public class Alarm {
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("description")]
        public string Description { get; set; }
        [JsonPropertyName("severity")]
        public int Severity { get; set; }
        [JsonPropertyName("realmId")]
        public string RealmId { get; set; }
        [JsonPropertyName("tag")]
        public string Tag { get; set; }
        [JsonPropertyName("orgResourceId")]
        public string OrgResourceId { get; set; }
        [JsonPropertyName("enabled")]
        public string Enabled { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("action")]
        public string Action { get; set; }
        [JsonPropertyName("ack")]
        public string Ack { get; set; }
        [JsonPropertyName("reset")]
        public string Reset { get; set; }
        [JsonPropertyName("updatedAt")]
        public long UpdatedAt { get; set; }
        [JsonPropertyName("eventTimestamp")]
        public long EventTimestamp { get; set; }
        [JsonPropertyName("deviceId")]
        public string DeviceId { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("value_double")]
        public float ValueDouble { get; set; }
    }
}
