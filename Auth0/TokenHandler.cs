using System.Net.Http.Headers;
using System.Text.Json;

namespace MauiAuth0App.Auth0 {
    public class TokenHandler : DelegatingHandler {

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", TokenHolder.IsPermission ? TokenHolder.PermissionToken : TokenHolder.AccessToken);

            return await base.SendAsync(request, cancellationToken);
        }

        public static async Task<T> ExecuteWithPermissionToken<T>(HttpClient client, Func<Task<T>> action) {
            if (TokenHolder.PermissionToken == null)
                await GetPermissionToken(client);

            TokenHolder.IsPermission = true;

            var result = await action.Invoke();

            TokenHolder.IsPermission = false;
            return result;
        }

        public static async Task GetPermissionToken(HttpClient client) {
            List<KeyValuePair<string, string>> postData = new() {
                new KeyValuePair<string, string>("grant_type", "urn:ietf:params:oauth:grant-type:uma-ticket"),
                new KeyValuePair<string, string>("audience", "corvina-platform"),
                new KeyValuePair<string, string>("permission", TokenHolder.ResourceId),
                new KeyValuePair<string, string>("client_id", "nextel-mobile-app")
            };
            var content = new FormUrlEncodedContent(postData);
            var response = await client.PostAsync(new Uri("https://auth.corvina.io/auth/realms/exor/protocol/openid-connect/token"), content);
            Token token = await JsonSerializer.DeserializeAsync<Token>(await response.Content.ReadAsStreamAsync());
            TokenHolder.PermissionToken = token.AccessToken;
        }

    }
}
