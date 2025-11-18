using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Helper
{
    public class JwtSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessTokenExpiration { get; set; }
        public int RefreshTokenExpirationDays { get; set; }
        public bool ValidateLifetime { get; set; }
        public bool ValidateAudience { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }



    }
}
