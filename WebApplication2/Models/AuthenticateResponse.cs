using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class AuthenticateResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }


        public AuthenticateResponse(Login login, string token)
        {
            Id = login.id;
            Name = login.name;
            Token = token;
        }
    }
}
