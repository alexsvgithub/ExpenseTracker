using ExpenseTracker.Business.Interface;
using ExpenseTracker.Core.DTOs;
using ExpenseTracker.Core.Models;
using ExpenseTracker.Data.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Business.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _config;

        public UserService(IUserRepository userRepo, IConfiguration config)
        {
            _userRepo = userRepo;
            _config = config;
        }

        public async Task<string> RegisterUser(RegisterDto user)
        {
            try
            {
                User login = new User()
                {
                    emailId = user.emailId,
                    userName = user.userName
                };
                if(login == null || 
                    string.IsNullOrEmpty(user.password) ||
                    string.IsNullOrEmpty(user.emailId)  || 
                    string.IsNullOrEmpty(user.userName))
                {
                    return "Fill in the mandatory Fields";
                }

                (login.passwordHash,login.salt) = SaltAndHashPassword(user);
                return await _userRepo.RegisterNewUser(login);


            }
            catch(Exception ex)
            {
                return $"{ex}";
            }
        }

        private (string,string) SaltAndHashPassword(RegisterDto register)
        {
            var salt = BCrypt.Net.BCrypt.GenerateSalt();  // Optional: You can specify a work factor (e.g., BCrypt.Net.BCrypt.GenerateSalt(12))
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(register.password, salt);
            return (hashedPassword, salt);

        }

        public async Task<string> LoginAsync(LoginDto login)
        {
            var user = await _userRepo.GetByEmailAsync(login.EmailId);
            if (user == null || !VerifyPassword(login.Password, user.passwordHash))
                return null;

            return GenerateJwtToken(user);
        }

        public async Task<User> GetByIdAsync(string userId)
        {
            return await _userRepo.GetByIdAsync(userId);
        }

        public async Task<bool> UpdateProfileAsync(UpdateProfileDto dto)
        {
            var user = await _userRepo.GetByIdAsync(dto.UserId);
            if (user == null) return false;

            user.userName = dto.Username;
            user.emailId = dto.Email;
            user.UpdatedAt = DateTime.UtcNow;

            return await _userRepo.UpdateAsync(user);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
            //using var sha256 = SHA256.Create();
            //var hash = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
            //return hash == storedHash;
        }

        //private string GenerateJwtToken(User user)
        //{
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(_config["Jwt:Secret"]);
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new[]
        //        {
        //            new Claim(ClaimTypes.NameIdentifier, user._id),
        //            new Claim(ClaimTypes.Email, user.emailId)
        //        }),
        //        Expires = DateTime.UtcNow.AddDays(7),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //    };
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    return tokenHandler.WriteToken(token);
        //}

        public string GenerateJwtToken(User user)
        {
            // Retrieve JWT settings from configuration
            var jwtSettings = _config.GetSection("JwtSettings");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));

            // Define the signing credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Define the claims
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user._id),   // Unique identifier
                new Claim(JwtRegisteredClaimNames.Email, user.emailId),  // User's email
                new Claim(ClaimTypes.NameIdentifier, user._id),       // Name identifier (e.g., userId)
                new Claim(ClaimTypes.Role, user.role),                // User's role (e.g., Admin, User)
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unique token identifier
            };

            // Create the JWT token descriptor with claims, expiry, issuer, and audience
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["ExpiryInMinutes"])), // Configurable expiry
                Issuer = jwtSettings["Issuer"],       // Issuer
                Audience = jwtSettings["Audience"],   // Audience
                SigningCredentials = creds
            };

            // Use JwtSecurityTokenHandler to create the token
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Return the JWT as a string
            return tokenHandler.WriteToken(token);
        }


    }
}
