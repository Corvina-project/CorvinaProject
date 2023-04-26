using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MauiAuth0App.Auth0 {
    public class Token {

        [JsonPropertyName("upgraded")]
        public bool Upgraded { get; set; }
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonPropertyName("refresh_expires_in")]
        public int RefreshExpiresIn { get; set; }
        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }
        [JsonPropertyName("notbeforepolicy")]
        public int NotBeforePolicy { get; set; }
        [JsonPropertyName("id_token")]
        public string IdToken { get; set; }
        [JsonPropertyName("session_state")]
        public string SessionState { get; set; }
        [JsonPropertyName("scope")]
        public string Scope { get; set; }

    }
}
