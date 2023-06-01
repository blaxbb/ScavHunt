using System.Security.Cryptography;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

public static class TokenGenerator
{
    public static string CreateNewToken(ConfigurationManager config)
    {
        var iss = config["Authentication:Apple:TeamId"]; // your accounts team ID found in the dev portal
        var aud = "https://appleid.apple.com";
        var sub = config["Authentication:Apple:ClientId"]; // same as client_id
        var now = DateTime.UtcNow;
        
        var privateKey = config["Authentication:Apple:PrivateKey"];
        var ecdsa = ECDsa.Create();
        ecdsa?.ImportPkcs8PrivateKey(Convert.FromBase64String(privateKey), out _);

        var handler = new JsonWebTokenHandler();
        return handler.CreateToken(new SecurityTokenDescriptor
        {
            Issuer = iss,
            Audience = aud,
            Claims = new Dictionary<string, object> {{"sub", sub}},
            Expires = now.AddMinutes(5), // expiry can be a maximum of 6 months - generate one per request or re-use until expiration
            IssuedAt = now,
            NotBefore = now,
            SigningCredentials = new SigningCredentials(new ECDsaSecurityKey(ecdsa), SecurityAlgorithms.EcdsaSha256)
        });
    }
}