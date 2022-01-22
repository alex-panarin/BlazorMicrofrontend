using System;

namespace Blazor.Register
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RegistrationAttribute : Attribute
    {
        public Type RegistrationType { get; set; }
    }
}
