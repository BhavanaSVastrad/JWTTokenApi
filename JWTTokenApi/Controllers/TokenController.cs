using JWTTokenApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JWTTokenApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        public readonly ApplicationDBContext _db;
        public TokenController(IConfiguration configuration, ApplicationDBContext db)
        {
            _configuration = configuration;
            _db = db;
        }

        [HttpPost]
        public async Task<IActionResult> Post(User userInfo)
        {
            if (userInfo != null && userInfo.Username != null && userInfo.Password != null)
            {
                var user = await GetUser(userInfo.Username, userInfo.Password);
                if (user != null)
                {
                    var claims = new[]
                    {
                         new Claim(JwtRegisteredClaimNames.Sub,_configuration["Jwt:Subject"]),
                         new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                         new Claim(JwtRegisteredClaimNames.Iat,DateTime.UtcNow.ToString()),
                         new Claim("Id",user.Id.ToString()),
                         new Claim("Username",user.Username),
                         new Claim("Email",user.Email),
                         new Claim("Mobile",user.Mobile.ToString()),
                         new Claim("Password",user.Password),
                        
                         

                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                        _configuration["Jwt:Issuer"],
                        _configuration["Jwt:Audience"],
                        claims,
                        expires: DateTime.Now.AddMinutes(20),
                        signingCredentials: signIn);
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid user");
                }


            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public async Task<User> GetUser(string userName, string password)
        {
            return await _db.Users.FirstOrDefaultAsync(u => u.Username == userName && u.Password == password);
        }
    }
}
