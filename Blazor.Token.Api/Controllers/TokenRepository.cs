using Blazor.Token.Api.Model;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Token.Api.Controllers
{
    public class TokenRepository : ITokenRepository
    {
        // TODO: Add Users repository
        static List<User> Users = new List<User>
        {
            new User{ Email = "vasia@123.com", Role="guest"},
            new User{ Email = "alex@123.com", Role="admin"},
            new User{ Email = "nik@123.com", Role="guest"}
        };
        public Task CreateAsync(TokenRequest token)
        {
            var user = Users.FirstOrDefault(u => u.Email == token.email);
            if (user == null)
                throw new Exception($"User not found, use allowed e-mail: {string.Join("; ", Users.Select(u => u.Email)) }");
            if(string.IsNullOrEmpty(user.Token))
            {
                CreatePasswordHash(token.password, out byte[] hash, out byte[] salt);
                user.Hash = hash;
                user.Salt = salt;
                user.Token = CreateToken(user.Id.ToString(), user.Email, user.Role);
            }
            return Task.CompletedTask;
        }

        public Task<string> RefreshAsync(string token)
        {
            // TODO: implement Refresh method
            throw new System.NotImplementedException();
        }

        public Task<TokenResponse> VerifyAsync(TokenRequest token)
        {
            var user = Users.FirstOrDefault(u => u.Email == token.email);
            if (user == null)
                throw new Exception("User not found");
            if (VerifyPasswordHash(token.password, user.Hash, user.Salt))
                return Task.FromResult(new TokenResponse {Token = user.Token, RefreshToken = user.RefreshToken });
            
            return Task.FromResult(default(TokenResponse));
         }
        private string CreateToken(string tokenKey, string email, string role)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role)
            };

            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
                SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials);
            
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
