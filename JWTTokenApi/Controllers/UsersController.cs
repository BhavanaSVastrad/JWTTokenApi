using JWTTokenApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWTTokenApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
     
            public readonly ApplicationDBContext _context;
            public UsersController(ApplicationDBContext context)
            {
                _context = context;
            }

            [HttpGet]
            public IEnumerable<User> GetUsers()
            {
                return _context.Users.ToList();
            }


            [HttpGet("{Id}")]
            public ActionResult<User> GetUsers(int Id)
            {
                var users = _context.Users.Find(Id);

                if (users == null)
                {
                    return NotFound();
                }
                return users;
            }

            [HttpPost]
            public ActionResult PostUser(User user)
            {
                _context.Add(user);
                _context.SaveChangesAsync();
                return CreatedAtAction("GetUsers", new { id = user.Id }, user);
            }

            [HttpPut("{id}")]
            public IActionResult PutUsers(int id, User user)
            {
                if (id != user.Id)
                {
                    return BadRequest();
                }

                _context.Entry(user).State = EntityState.Modified;

                try
                {
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }


            [HttpDelete("{id}")]
            public ActionResult<User> DeleteUsers(int id)
            {
                var user = _context.Users.Find(id);
                if (user == null)
                {
                    return NotFound();
                }

                _context.Users.Remove(user);
                _context.SaveChanges();

                return user;
            }


            private bool UserExists(int id)
            {
                return _context.Users.Any(e => e.Id == id);
            }
        }
 }

