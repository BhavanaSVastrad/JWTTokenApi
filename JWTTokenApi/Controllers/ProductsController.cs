using JWTTokenApi.Models;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class ProductsController : ControllerBase
    {
        public readonly ApplicationDBContext _context;
        public ProductsController(ApplicationDBContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IEnumerable<Product> GetProducts()
        {
            return _context.Products.ToList();
        }


        [HttpGet("{Id}")]
        public ActionResult<Product> GetProducts(int Id)
        {
            var products = _context.Products.Find(Id);

            if (products == null)
            {
                return NotFound();
            }
            return products;
        }

        [HttpPost]
        public ActionResult PostProduct(Product product)
        {
            _context.Add(product);
            _context.SaveChangesAsync();
            return CreatedAtAction("GetProducts", new { id = product.Id }, product);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducts(int id, Product products)
        {
            if (id != products.Id)
            {
                return BadRequest();
            }

            _context.Entry(products).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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
        public ActionResult<Product> DeleteProducts(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return product;
        }


        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

}
}
