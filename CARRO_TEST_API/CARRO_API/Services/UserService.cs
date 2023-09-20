using CARRO_API.Common.Utilities;
using CARRO_API.Entities;
using CARRO_API.Models;
using CARRO_API.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CARRO_API.Services
{
    public class UserService : IUserService
    {
        private JwtSettings _jwtSettings;
        private readonly ILogger _logger;
        public DbDataContext _db;
        private IEmailService _emailService;

        public UserService(
            JwtSettings jwtSettings
            , DbDataContext db
            , IEmailService emailService
            )
        {
            _jwtSettings = jwtSettings;
            _db = db;
            _emailService = emailService;
        }
        public UserCredential Sigin(SiginQuery req)
        {
            try
            {
                var email = req.Email;
                var password = req.Password;

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                    throw new Exception($"Email and password are required");

                var query = (from _data in _db.Users
                             where _data.Email == req.Email && _data.Active == true
                             select _data).FirstOrDefault();

                if (query == null)
                {
                    throw new Exception($"Email incorrect");
                }

                var encryptPasswpord = CryptoUtilities.sha256_hash(password);
                bool isValid = query.Password == encryptPasswpord;
                if (!isValid)
                {
                    throw new Exception($"Password incorrect");
                }

                UserCredential user = new UserCredential() 
                { 
                    Email = query.Email,
                    FirstName = query.FirstName,
                    LastName = query.LastName,
                };

                return user;
            }
            catch (Exception oEx)
            {
                _logger.LogError(oEx.Message);
                throw;
            }
        }

        public UserCredential Sigup(SigupQuery req)
        {
            try
            {
                var email = req.Email;
                var password = req.Password;

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                    throw new Exception($"Email and password are required");

                if (!int.TryParse(req.PhoneNumber, out int phoneNumber))
                    throw new Exception("Phone number must be a valid number");

                var query = (from _data in _db.Users
                             where _data.Email == req.Email && _data.Active == true
                             select _data).FirstOrDefault();

                if (query == null)
                {
                    throw new Exception($"Email incorrect");
                }

                var encryptPasswpord = CryptoUtilities.sha256_hash(req.Password);

                User data = new User()
                {
                    Email = email,
                    Password = encryptPasswpord,
                    FirstName = req.FirstName,
                    LastName = req.LastName,
                    PhoneNumber = req.PhoneNumber,
                    Brithdate = req.Brithdate,
                    Active = true,
                    ModifyBy = "Sigup",
                    ModifyDate = DateTime.Now,
                };
                _db.Users.Add(data);
                _db.SaveChanges();

                _emailService.SendEmailSigupSuccess(data);

                UserCredential user = new UserCredential()
                {
                    Email = data.Email,
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                };

                return user;
            }
            catch (Exception oEx)
            {
                _logger.LogError(oEx.Message);
                throw;
            }
        }

    }
}
