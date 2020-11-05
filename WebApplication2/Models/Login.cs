using JWTAuthenticationExample.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class Login
    {
        public Guid id{ get; set; }
        
        [Required]
        public string name { get; set; }

        [Required]
        public string pwd { get; set; }

        [Required]
        public string role { get; set; }
        public DateTime? lastlogin { get; set; }
    }
}
