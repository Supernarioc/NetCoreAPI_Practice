using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;
using WebApplication2.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Schema;

namespace WebApplication2.Service
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model,tempdbContext context);
        //IEnumerable<User> GetAll();
        //User GetById(int id);
    }
    public class UserService:IUserService
    {
        //AuthenticateResponse authenticateResult(AuthenticateRequest model);
        private readonly AppSettings _appSettings;
        public AuthenticateResponse Authenticate(AuthenticateRequest model,tempdbContext context)
        {
            //var login = new Login { id = Guid.NewGuid(), name = "admin", pwd = "admin", lastlogin = Convert.ToDateTime("2020-1-1") };
            var login = context.GetLogin(model);
            // return null if user not found
            if (login == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(login);

            return new AuthenticateResponse(login, token);
        }

        private string generateJwtToken(Login login)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", login.id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        
    }
}
