using JWTTokenApi.Models;
using JwtTokenDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using User = JWTTokenApi.Models.User;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace JwtTokenDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public INotyfService _notifyService { get; }

        private static string apiBaseUrl = "https://localhost:44374/api/Users";

        private readonly ApplicationDBContext _context;
        public HomeController(ILogger<HomeController> logger,ApplicationDBContext context, INotyfService notifyService)
        {
            _logger = logger;
            _context = context;
            _notifyService = notifyService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet] 
        public async Task<List<User>>GetUsers()
        { 
            var accessToken = HttpContext.Session.GetString("JWToken");
           
            var url = apiBaseUrl;

            HttpClient client = new HttpClient(); 
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken); 
            string jsonStr = await client.GetStringAsync(url); 
            var res = JsonConvert.DeserializeObject<List<User>>(jsonStr).ToList(); 
            return res; 
        
        }
        //https://localhost:44324/api/Token

        public async Task<IActionResult> LoginUser(User user)
        {
            using (var httpClient = new HttpClient())
            { 
                List<User> res = await GetUsers();
                var data = res.Where(e => e.Username == user.Username).SingleOrDefault();
                if (data != null) 
                { 
                    bool isvalid = (data.Username == user.Username && data.Password == user.Password); 
                    if (isvalid) 
                    { 
                        StringContent stringContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json"); 
                        using (var response = await httpClient.PostAsync("https://localhost:44374/api/Token", stringContent)) 
                        { 
                            string token = await response.Content.ReadAsStringAsync(); 
                            if (token == "Invalid credentials") 
                            { 
                                TempData["errorUserPass"] = "Incorrect username or password"; 
                                return Redirect("~/Home/Index"); 
                            } 
                            HttpContext.Session.SetString("JWToken", token); 
                        } 
                    } 
                    else
                    { 
                        TempData["errorUserPass"] = "Username or Password is incorrect!";
                        return Redirect("~/Home/Index");
                       
                    }
                } 
                else { 
                    TempData["error"] = "User Not Found";
                    return Redirect("~/Home/Index");
                }
                _notifyService.Custom("Successfully Logged In!", 3, "lightgreen", "fa fa-home");
                return Redirect("~/Dashboard/Index"); 
            }

        }


        public IActionResult Logoff()
        {
            HttpContext.Session.Clear();//clear token
            return RedirectToAction("LoginUser", "Home");
        }

        //Signup
        [HttpPost]
        public IActionResult SignUp(User user)
        {

            if (ModelState.IsValid)
            {

                User data = new User()
                {
                    Username = user.Username,
                    Email = user.Email,
                    Password = user.Password,
                    Mobile = user.Mobile
                };

                _context.Add(data);
                _context.SaveChanges();
                _notifyService.Custom("Successfully Registered!", 3, "lightgreen");
                return Redirect("~/Home/Index");
            }
            else
            {
               
                return View();
            }

        }

        public IActionResult SignUp()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
