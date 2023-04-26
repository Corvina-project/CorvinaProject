using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiAuth0App.Auth0 {
    public static class TokenHolder {

        public static string AccessToken { get; set; }
        public static string RefreshToken { get; set; }

    }
}
