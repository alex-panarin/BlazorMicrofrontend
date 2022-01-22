using System;

namespace Blazor.Token.Api.Model
{
    internal class User
    {
        public Guid Id { get; set; } = new Guid("7B3AE85A-BB8C-4EBB-8F74-C566AFAF1AE2");
        public string Email { get; set; }
        public byte[] Salt { get; set; }
        public byte[] Hash { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
