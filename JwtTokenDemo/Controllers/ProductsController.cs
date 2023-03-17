using JwtTokenDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using JWTTokenApi.Models;
namespace JwtTokenDemo.Controllers
{
    public class ProductsController : Controller
    {
        public static string baseUrl = "https://localhost:44374/api/products/";
        public async Task<IActionResult> Index()
        {
            var products = await GetProducts();
            return View(products);
        }

        [HttpGet]
        public async Task<List<JWTTokenApi.Models.Product>> GetProducts()
        {
            //Use the access token to call a protected web  API 

            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = baseUrl;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string jsonStr = await client.GetStringAsync(url);

            var res = JsonConvert.DeserializeObject<List<JWTTokenApi.Models.Product>>(jsonStr).ToList();

            return res;
        }


        public IActionResult Create()
        {
            return View();
        }

        // POST: Products1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Product_Image,Product_Name,Product_Description,Product_Price")] JWTTokenApi.Models.Product product)
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = baseUrl;
            //setting up the authorization header
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            var stringContent = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
            await client.PostAsync(url, stringContent);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = baseUrl + id;
            //setting up the authorization header
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            string jsonStr = await client.GetStringAsync(url);
            var res = JsonConvert.DeserializeObject<JWTTokenApi.Models.Product>(jsonStr);

            if (res == null)
            {
                return NotFound();
            }


            return View(res);
        }

        // GET: Products1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Product_Image,Product_Name,Product_Description,Product_Price")] JWTTokenApi.Models.Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = baseUrl + id;
            //setting up the authorization header
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var stringContent = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

            await client.PutAsync(url, stringContent);

            return RedirectToAction(nameof(Index));

            //var accessToken = HttpContext.Session.GetString("JWToken");
            //var url = baseUrl;
            ////setting up the authorization header
            //HttpClient client = new HttpClient();
            //client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            //string jsonStr = await client.GetStringAsync(url);


            //var stringContent = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");
            //await client.PutAsync(url, stringContent);
            //return RedirectToAction(nameof(Index));
        }


        // GET: Products1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = baseUrl + id;
            //setting up the authorization header
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            string jsonStr = await client.GetStringAsync(url);
            var res = JsonConvert.DeserializeObject<JWTTokenApi.Models.Product>(jsonStr);
            if (res == null)
            {
                return NotFound();
            }

            return View(res);
        }

        // POST: Products1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = baseUrl + id;
            //setting up the authorization header
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            await client.DeleteAsync(url);
            return RedirectToAction(nameof(Index));

        }

        //details

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var accessToken = HttpContext.Session.GetString("JWToken");
            var url = baseUrl + id;
            //setting up the authorization header
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            string jsonStr = await client.GetStringAsync(url);
            var products = JsonConvert.DeserializeObject<JWTTokenApi.Models.Product>(jsonStr);
            if (products == null)
            {
                return NotFound();
            }

            return View(products);
        }

        public async Task<IActionResult> Cards()
        {
            var products = await GetProducts();
            return View(products);
        }
    
    }
}
