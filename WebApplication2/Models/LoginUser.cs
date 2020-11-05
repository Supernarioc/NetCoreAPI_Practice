using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class LoginUser
    {
        [Required]
        public string LoginName { get; set; }
        
        [Required]
        public string LoginPwd { get; set; }
    }
}
