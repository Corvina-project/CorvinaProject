namespace MauiAuth0App.Auth0 {
    public static class TokenHolder {
        public static string AccessToken { get; set; }
        public static string RefreshToken { get; set; }
        public static string PermissionToken { get; set; }
        public static bool IsPermission { get; set; }
        public static string ResourceId { get; set; }
        public static IDispatcherTimer Timer { get; set; }
    }
}
