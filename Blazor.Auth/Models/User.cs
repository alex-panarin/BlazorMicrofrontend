using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Auth.Models
{
    internal class User
    {
        public string Name { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] Salt { get; set; }
        public byte[] TokenValue { get; } = Encoding.UTF8.GetBytes("7B3AE85A-BB8C-4EBB-8F74-C566AFAF1AE2");
    }
}
