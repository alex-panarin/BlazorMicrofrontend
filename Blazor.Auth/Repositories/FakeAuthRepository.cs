using Blazor.Auth.Models;
using Blazor.Auth.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Auth.Repositories
{
    internal class FakeAuthRepository : IAuthRepository
    {
        private readonly ILocalStorageService _localStorage;

        public FakeAuthRepository(ILocalStorageService localStorage)
        {
            _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
        }
        public Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            if (request.Email != CashedUser.Name)
            {
                return Task.FromResult( new LoginResponse { IsSuccess = false, Error = "Bad request" });
            }

            if (!VerifyPasswordHash(request.Password, CashedUser.PasswordHash, CashedUser.Salt))
            {
                return Task.FromResult(new LoginResponse { IsSuccess = false, Error = "Wrong password" });
            }

            string token = CreateToken(CashedUser.TokenValue);
            return  Task.FromResult(new LoginResponse { IsSuccess = true, Token = token });
        }

        public Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] salt);
            CashedUser.Name = request.Email;
            CashedUser.PasswordHash = passwordHash;
            CashedUser.Salt = salt;

            return Task.FromResult(new RegisterResponse { IsSuccess = true });
        }
        public static string CreateToken(byte[] tokenValue)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, CashedUser.Name),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenValue),
                SecurityAlgorithms.EcdsaSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);
            // Blazor webAssembly not supported symmetric key
            // new JwtSecurityTokenHandler().WriteToken(token);
            var jwt = "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiMTIzQDEyMy5ydSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkFkbWluIiwiZXhwIjozMTY4NTQ5OTk5fQ.pveWTRTAFcQiTuwIvAEQR4PrP-3680Y1G1W9pnqLOTdwXem1zJAnZBIu8f2nCsPgiJGg3vTDL7t5N-7p7bZlCg"; 

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = SHA256.Create())
            {
                passwordSalt = hmac.Hash;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            //using (var hmac = new HMACSHA256(passwordSalt))
            //{
            //    var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            //    return computedHash.SequenceEqual(passwordHash);
            //}
            using (var hmac = SHA256.Create())
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
