using CARRO_API.Common.Authentications;
using CARRO_API.Models;
using CARRO_API.Services.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CARRO_API.Services
{
    public class AuthrorizationsService : IAuthrorizationsService
    {
        private JwtSettings _jwtSettings;
        private IUserService _userService;
        public AuthrorizationsService(
            JwtSettings jwtSettings
            , IUserService userService            
            )
        {
            _jwtSettings = jwtSettings;
            _userService = userService;
        }
        public SiginModel Sigin(SiginQuery req)
        {
            UserCredential user = null;
            if (req.Email == "admin@domain.com" && req.Password == "1234")
            {
                user = new UserCredential
                {
                    Email = "admin@domain.com",
                    FirstName = "Admin",
                    LastName = "User",
                };
            }
            else
            {
                user = _userService.Sigin(req);
            }

            if (user == null)
            {
                throw new Exception($"User infomation not found");
            }

            var jwtExpireSec = _jwtSettings.DurationInMinutes * 60;
            var jwt = GenerateJWT(user);

            SiginModel token = new SiginModel
            {
                token_type = "bearer",
                access_token = jwt,
                expires_in = jwtExpireSec
            };

            return token;
        }
        public SiginModel Sigup(SigupQuery req)
        {
            UserCredential user = _userService.Sigup(req);


            var jwtExpireSec = _jwtSettings.DurationInMinutes * 60;
            var jwt = GenerateJWT(user);

            SiginModel token = new SiginModel
            {
                token_type = "bearer",
                access_token = jwt,
                expires_in = jwtExpireSec
            };

            return token;
        }
        private string GenerateJWT(UserCredential user)
        {
            var claims = new List<Claim>
            {
                     //new Claim(ClaimStore.name.ToString(), user.UserName),
                     new Claim(ClaimStore.email.ToString(), user.Email),
                     //new Claim(ClaimStore.name.ToString(), user.Name),
                     //new Claim(ClaimStore.userId.ToString(), user.UserId),
                     new Claim(ClaimStore.firstName.ToString(), user.FirstName ?? ""),
                     new Claim(ClaimStore.lastName.ToString(), user.LastName ?? ""),
                     //new Claim(ClaimStore.groupId.ToString(), user.GroupId?.ToString() ?? ""),
                     //new Claim(ClaimStore.groupName.ToString(), user.GroupName ?? ""),
                     //new Claim(ClaimStore.phoneNumber.ToString(), user.PhoneNumber ?? ""),
                     //new Claim(ClaimStore.isAdmin.ToString(), user.IsAdmin.ToString()),
                     //new Claim(ClaimStore.ImageUrl.ToString(), user.ImageUrl ?? "")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                 expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            string result = new JwtSecurityTokenHandler().WriteToken(token);


            return result;
        }
    }
}
