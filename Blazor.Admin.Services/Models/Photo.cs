using System;

namespace Blazor.Services.Models
{
    public class Photo
    {
        public string id { get; } = Guid.NewGuid().ToString();
        public int Width { get; set; } = 225;
        public int Heigh { get; set; } = 300;
        public string Url { get; set; }
        public string Title { get; set; }
    }
}
