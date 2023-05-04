using Microsoft.Maui.Controls.Internals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MauiAuth0App.Models
{
    public class Devices
    {
        [JsonPropertyName("number")]
        public int Number { get; set; }
        [JsonPropertyName("data")]
        public List<Device> Data { get; set; }
        [JsonPropertyName("totalElements")]
        public int TotalElements { get; set; }
        [JsonPropertyName("totalPages")]
        public int TotalPages{ get; set; }
        [JsonPropertyName("last")]
        public bool Last { get; set; }
    }

    public class Device
    {
        [JsonPropertyName("orgResourceId")]
        public string OrgResourceId { get; set; }
        [JsonPropertyName("realmId")]
        public string RealmId { get; set; }
        [JsonPropertyName("configurationSent")]
        public bool ConfigurationSent { get; set; }
        [JsonPropertyName("label")]
        public string Label { get; set; }
        [JsonPropertyName("creationDate")]
        public long CreationDate { get; set; }
        [JsonPropertyName("deviceId")]
        public string DeviceId { get; set; }
        [JsonPropertyName("tags")]
        public object Tags { get; set; }
        [JsonPropertyName("connected")]
        public bool? Connected { get; set; }
        [JsonPropertyName("deleted")]
        public bool Deleted { get; set; }
        [JsonPropertyName("configurationApplied")]
        public bool ConfigurationApplied { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("attributes")]
        public Attributes Attributes { get; set; }
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("updatedAt")]
        public long UpdatedAt { get; set; }
        [JsonPropertyName("lastConnUpdateAt")]
        public long LastConnUpdateAt { get; set; }
        [JsonPropertyName("modelId")]
        public string ModelId { get; set; }
        [JsonPropertyName("modelVersion")]
        public string ModelVersion { get; set; }
        [JsonPropertyName("configurationError")]
        public string ConfigurationError { get; set; }
        [JsonPropertyName("modelName")]
        public string ModelName { get; set; }
        [JsonPropertyName("presetName")]
        public string PresetName { get; set; }
        [JsonPropertyName("lastConfigUpdateAt")]
        public long LastConfigUpdateAt { get; set; }
        [JsonPropertyName("presetId")]
        public string PresetId { get; set; }
    }

    public class DeviceAlarms
    {
        [JsonPropertyName("n_active_1")]
        public int NActive1 { get; set; }
    }

    public class Attributes
    {
        [JsonPropertyName("vpn_connected")]
        public bool VpnConnected { get; set; }
        [JsonPropertyName("vpn_inuse")]
        public bool VpnInuse { get; set; }
        [JsonPropertyName("vpn_connected_ts")]
        public long VpnConnectedTs { get; set; }
        [JsonPropertyName("vpn_inuse_ts")]
        public long VpnInuseTs { get; set; }
        [JsonPropertyName("geoLocation")]
        public float[] GeoLocation { get; set; }
    }
}
