using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Claims;
using System.Security;
using System.Security.Principal;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using BankEncryptionAPI.Models;
using System.Configuration;
using System.Text;

namespace BankEncryptionAPI.Services.Impl
{
    public class LoginService : ILoginService
    {
        public LoginService() { }
        public Boolean validateClientDetails(ClientModel client)
        {
            Boolean result = false;

            string clientId = ConfigurationManager.AppSettings["ClientId"];
            string clientSecret = ConfigurationManager.AppSettings["ClientSecret"];

            result = (clientId == client.clientId && clientSecret == client.clientSecret);

            return result;
        }
        public String GenererateTokenForClient(ClientModel client)
        {
            string communicationKey = ConfigurationManager.AppSettings["CommunicationKey"];
            double expirationTimeInMinutes = Convert.ToDouble(ConfigurationManager.AppSettings["ExpirationTimeInMinutes"]);
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(communicationKey));

            var signingCredentials = new SigningCredentials(signingKey,
               SecurityAlgorithms.HmacSha256Signature, SecurityAlgorithms.Sha256Digest);
            List<Claim> ClaimsList = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, client.clientSecret),
                new Claim(ClaimTypes.NameIdentifier, client.clientId),
            };
            var claimsIdentity = new ClaimsIdentity(ClaimsList, "Custom");
            var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Audience = ConfigurationManager.AppSettings["JWTAudience"],
                Issuer = "self",
                Expires = DateTime.UtcNow.AddMinutes(expirationTimeInMinutes),
                SigningCredentials = signingCredentials,
                IssuedAt = DateTime.UtcNow
            };
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var plainToken = tokenHandler.CreateToken(tokenDescriptor);
            var signedAndEncodedToken = tokenHandler.WriteToken(plainToken);
            //Log.Info("GenerateTokenForUser Token- " + signedAndEncodedToken);

            return signedAndEncodedToken;

        }

        public System.IdentityModel.Tokens.Jwt.JwtSecurityToken GenerateUserClaimFromJWT(string authToken)
        {
            List<String> audiences = new List<string>();
            string communicationKey = ConfigurationManager.AppSettings["CommunicationKey"];
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(communicationKey));
            audiences.Add(ConfigurationManager.AppSettings["JWTAudience"]);

            var tokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
            {
                ValidAudiences = audiences,
                ValidIssuers = new string[]
                  {
                      "self",
                  },
                ValidateLifetime = true,
                IssuerSigningKey = signingKey
            };
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();

            SecurityToken validatedToken;

            try
            {
                tokenHandler.ValidateToken(authToken, tokenValidationParameters, out validatedToken);
            }
            catch (Exception ex)
            {
                return null;

            }

            return validatedToken as System.IdentityModel.Tokens.Jwt.JwtSecurityToken;

        }

        public JWTAuthenticationIdentity PopulateUserIdentity(System.IdentityModel.Tokens.Jwt.JwtSecurityToken userPayloadToken)
        {
            string clientSecret = ((userPayloadToken)).Claims.FirstOrDefault(m => m.Type == "unique_name").Value;
            string clientId = ((userPayloadToken)).Claims.FirstOrDefault(m => m.Type == "nameid").Value;
            return new JWTAuthenticationIdentity(clientId) { clientId = clientId, clientSecret = clientSecret };

        }
    
    }
}