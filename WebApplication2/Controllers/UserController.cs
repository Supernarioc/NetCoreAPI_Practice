using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly tempdbContext context;
        
        public UserController()
        {
            context = new tempdbContext();
        }

        [Route("/")]
        [HttpGet]
        public IActionResult Index() {
            return StatusCode(200,"NetCore 3.0 API");
        }
        [Route("[controller]")]
        [HttpGet]
        public async Task<object> GetUsers() {
            return await context.GetUsersAsync();
        }

        [Route("[controller]/{name?}")]
        [HttpGet("{name}")]
        public async IAsyncEnumerable<User> GetUser(string name)
        {
           yield return await context.GetUserAsync(name);
        }

        //[Authorize] TODO:add authorize
        [Route("[controller]")]
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
            return StatusCode(200, _newuser);
        }

        [Route("[controller]")]
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
            return StatusCode(200,"User id "+id+" deleted.");
        }
    }
}
