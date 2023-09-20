using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using WANYEN.API.COMMON.Attributes;

namespace WANYEN.API.COMMON.Authentications
{
    [TransientPriorityRegistration]
    public class TokenManager : ITokenManager
    {
        private readonly AppSettings _appSettings;

        public TokenManager(AppSettings appSettings, IHttpContextAccessor contextAccessor)
        {
            _appSettings = appSettings;
        }


        public string GetClaim(ClaimStore key, string token)
        {
            var val = GetPrincipal(token).Claims.Where(s => s.Type == key.ToString()).FirstOrDefault();
            if (val == null) return "";


            return val.Value;
        }

        public List<string> GetClaimList(ClaimStore key, string token)
        {
            var list = GetPrincipal(token).Claims.Where(s => s.Type == key.ToString()).Select(s => s.Value).ToList();
            return list;
        }

        public string ValidateToken(string token)
        {
            string username = null;
            ClaimsPrincipal principal = GetPrincipal(token);

            if (principal == null)
                return null;

            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }

            Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim.Value;

            return username;
        }


        public ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                token = token.StartsWith("Bearer ") ? token.Substring(7) : token;
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);

                if (jwtToken == null)
                    return null;

                //byte[] key = Convert.FromBase64String(Secret);
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JwtKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _appSettings.JwtIssuer,
                    ValidAudience = _appSettings.JwtAudience,
                    IssuerSigningKey = creds.Key

                };

                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token, parameters, out securityToken);
                return principal;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
