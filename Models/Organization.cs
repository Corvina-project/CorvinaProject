using System.Text.Json.Serialization;

namespace MauiAuth0App.Models {
    public class Organization {

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("label")]
        public string Label { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("resourceId")]
        public string ResourceId { get; set; }

        [JsonPropertyName("privateAccess")]
        public bool PrivateAccess { get; set; }

        [JsonPropertyName("allowDisablePrivateAccess")]
        public bool AllowDisablePrivateAccess { get; set; }

        [JsonPropertyName("hostname")]
        public object Hostname { get; set; }

        [JsonPropertyName("hostnameAllowed")]
        public bool HostnameAllowed { get; set; }

        [JsonPropertyName("dataEnabled")]
        public bool DataEnabled { get; set; }

        [JsonPropertyName("vpnEnabled")]
        public bool VpnEnabled { get; set; }

        [JsonPropertyName("vpnPairingMode")]
        public string VpnPairingMode { get; set; }

        [JsonPropertyName("vpnOtpRequired")]
        public bool VpnOtpRequired { get; set; }

        [JsonPropertyName("ipAddressesWhitelist")]
        public List<object> IpAddressesWhitelist { get; set; }

        [JsonPropertyName("userCanAccess")]
        public bool UserCanAccess { get; set; }

        [JsonPropertyName("storeEnabled")]
        public bool StoreEnabled { get; set; }

    }


}
