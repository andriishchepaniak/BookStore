using BookStoreModels;
using BookStoreModels.AuthenticationModels;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreCore.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly BookStoreContext _context;
        private readonly AuthSettings _authSettings;

        public AuthService(BookStoreContext context, IOptions<AuthSettings> options)
        {
            _context = context;
            _authSettings = options.Value;
        }

        public async Task<AuthResponse> Login(LoginModel model)
        {
            var identity = await GetIdentity(model);
            if (identity == null)
            {
                throw new ArgumentException("Invalid username or password.");
            }

            var jwt = GenerateSecurityToken(identity);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            var authResponse = new AuthResponse
            {
                Token = encodedJwt,
                User = user
            };

            return authResponse;
        }

        public async Task<User> Register(RegisterModel model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user != null)
            {
                throw new ArgumentException("Email is already taken.");
            }

            var newUser = new User
            {
                FullName = model.FullName,
                Email = model.Email,
                Password = HashPassword(model.Password)
            };

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);

            return user;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
        private Task<bool> CheckPasswordAsync(User user, string password)
        {
            if (password != null && user.Password == HashPassword(password))
            {
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        private async Task<ClaimsIdentity> GetIdentity(LoginModel model)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(user => user.Email == model.Email);

            var checkPassword = user != null
                ? await CheckPasswordAsync(user, model.Password)
                : false;

            if (checkPassword)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Token",
                ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

                return claimsIdentity;
            }

            return null;

        }
        
        private JwtSecurityToken GenerateSecurityToken(ClaimsIdentity identity)
        {
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: _authSettings.Issuer,
                    audience: _authSettings.Audience,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromMinutes(_authSettings.Expires)),
                    signingCredentials: new SigningCredentials(
                        _authSettings.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            return jwt;
        }

    }
}
