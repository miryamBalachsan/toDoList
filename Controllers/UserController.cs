using Microsoft.AspNetCore.Mvc;
using Task.Interfaces;
using User.Models;
using Task.Models;
using System.Security.Claims;
using Task.Services;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Text.Json;
using System.Text;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace User.Controllers
{
    [ApiController]
    // [Route("/user")]
    [Route("User")]
    public class UserController : ControllerBase
    {
        private IUserService UserService;
        // private ITokenService VTokenService;
      //  private static user CurrentUser;
         
        public UserController(IUserService UserService)
        {
            this.UserService = UserService;
            // this.VTokenService=ToDoTokenService;
        }
        [HttpGet]
       [Authorize(Policy = "Admin")]
        public ActionResult<List<user>> GetAll() =>
              UserService.GetAll();

        [HttpGet("{id}")]
        public ActionResult<user> Get(int id)
        {
            var myUser = UserService.Get(id);

            if (myUser == null)
                return NotFound();

            return myUser;
        }
        [HttpPost]
        [Route("[action]")]
        public ActionResult<String> Login([FromBody] user User)
        {
         var getuser=UserService.GetAll().FirstOrDefault(c=>c.Name ==User.Name && c.Password == User.Password);
            if(getuser == null)
                {
                    return Unauthorized();
                }
            else if(getuser.IsAdmin){
                 var claims = new List<Claim> {
                    new Claim("type", "Admin"),
                    new Claim("Id", getuser.Id.ToString()) 
                };
                
                var token = TokenService.GetToken(claims);
                return new OkObjectResult(TokenService.WriteToken(token));
            }
            else{
                 var claims = new List<Claim> {
                    new Claim("type", "User"),
                    new Claim("Id", getuser.Id.ToString())
                };
                
                var token = TokenService.GetToken(claims);
                return new OkObjectResult(TokenService.WriteToken(token));
            }
        }
        [HttpPost]

        [Authorize(Policy = "Admin")]
        public IActionResult Create(user myUser)
        {
            UserService.Add(myUser);
            return CreatedAtAction(nameof(Create), new { id = myUser.Id }, myUser);

        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public IActionResult Delete(int id)
        {
            var myUser = UserService.Get(id);
            if (myUser is null)
                return NotFound();

            UserService.Delete(id);

            return Content(UserService.Count.ToString());
        }
        [HttpPut("{id}")]
        [Authorize(Policy = "Admin")]
        public IActionResult Update(int id, user myUser)
        {
            if (id != myUser.Id)
                return BadRequest();

            var existinguser = UserService.Get(id);
            if (existinguser is null)
                return NotFound();

            UserService.Update(myUser);

            return NoContent();
        }
    }
}