using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Loading.Models
{
    public class AssemblyLayout
    {
        public string[] Assemblies { get; set; }
        public string[] Css { get; set; }
        public string[] Js { get; set; }
        public string Key { get; set; }
    }
}
