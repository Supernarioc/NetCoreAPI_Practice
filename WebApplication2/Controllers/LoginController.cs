using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JWTAuthenticationExample.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly tempdbContext _tempdbContext;

        //private List<Login> appUsers = new List<Login>
        //{
        //    new User {  FullName = "Vaibhav Bhapkar",  UserName = "admin", Password = "1234", UserRole = "Admin" },
        //    new User {  FullName = "Test User",  UserName = "user", Password = "1234", UserRole = "User" }
        //};

        public LoginController(IConfiguration config,tempdbContext tempdbContext)
        {
            _config = config;
            _tempdbContext = tempdbContext;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginUser login)
        {
            IActionResult response = Unauthorized();
            Login _login = AuthenticateUser(login);
            if (_login != null)
            {
                var tokenString = GenerateJWTToken(_login);
                response = Ok(new
                {
                    token = tokenString,
                    userDetails = _login,
                });
            }
            return response;
        }

        [Route("add")]
        [HttpPost]
        [Authorize(Policy = Policies.Admin)]
        public Login AddNewLogin([FromBody] Login login) {
            
            _tempdbContext.AddLogin(login);
            return login;
        }
        Login AuthenticateUser(LoginUser loginCredentials)
        {
            var login_dt = _tempdbContext.GetLogin(loginCredentials);
            return login_dt;
        }
        /// <summary>
        /// generate token based on login user 
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        string GenerateJWTToken(Login userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.name),
                //new Claim("fullName", userInfo.FullName.ToString()),
                new Claim("role",userInfo.role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
