namespace MauiAuth0App.Auth0 {
    public static class TokenHolder {

        private static object _lock = new();
        private static bool isPermission;

        public static string AccessToken { get; set; }
        public static string RefreshToken { get; set; }
        public static string PermissionToken { get; set; }
        public static bool IsPermission {
            get {
                lock (_lock) {
                    return isPermission;
                }
            }
            set {
                lock (_lock) {
                    isPermission = value;
                }
            }
        }
        public static string ResourceId { get; set; }
        public static IDispatcherTimer Timer { get; set; }

    }
}
