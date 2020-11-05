using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using WebApplication2.Models;
using JWTAuthenticationExample.Models;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly tempdbContext context;
        public UserController(tempdbContext _context)
        {
            context = _context;
        }

        [Route("/")]
        [HttpGet]
        [ApiVersion("1.0",Deprecated = true)]
        public IActionResult Index() {
            return StatusCode(200,"NetCore 3.0 API");
        }
        
        /// <summary>
        /// get all users
        /// </summary>
        /// <returns></returns>
        [Route("users")]
        [HttpGet]
        public async Task<object> GetUsers() {
            var res = await context.GetUsersAsync();
            return res;
        }

        /// <summary>
        /// get user by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [Route("{name?}")]
        [HttpGet("{name}")]
        public async IAsyncEnumerable<User> GetUser(string name)
        {
           yield return await context.GetUserAsync(name);
        }

        /// <summary>
        /// add new user
        /// </summary>
        /// <param name="_newuser"></param>
        /// <returns></returns>
        [Authorize(Policy = Policies.Admin)]
        [HttpPost("add")]
        public async Task<IActionResult> AddUser([FromBody] User _newuser) {
            //check if info is empty
            if (_newuser.Name == ""||_newuser.Description == "") {
                return BadRequest("User information is empty");
            }
            var createdUr = await context.GetUserAsync(_newuser.Name);
            //check if user exist in db
            if (createdUr != null)
                return BadRequest("User Already Exists");
            //add user in db
            var res = context.AddUser(_newuser);
            return Ok(_newuser);
        }

        /// <summary>
        /// Delete user from db
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Policy = Policies.Admin)]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteUser(int id) {
            try
            {
                var _ur = await this.context.GetUserAsync(id);
                if (_ur == null)
                    return NotFound("User does not exists");
                this.context.DeleteUser(id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error " + ex.Message);
            }
            return Ok("User id "+id+" deleted.");
        }
    }
}
