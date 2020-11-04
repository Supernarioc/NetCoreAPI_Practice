using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class Login
    {
        public Guid id{ get; set; }
        public string name { get; set; }
        public string pwd { get; set; }
        public DateTime lastlogin { get; set; }
    }
}
