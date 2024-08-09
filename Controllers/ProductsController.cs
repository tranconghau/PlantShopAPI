using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PlantShopAPI.Models;

namespace PlantShopAPI.Controllers
{
    // [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly CustomDbContext _context;

        public ProductsController(CustomDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet("api/v1/getProduct"), Authorize]
        public async Task<ActionResult<IEnumerable<Product>>> GetProduct()
        {
            var products = await _context.Product.ToListAsync();

            JsonResult json = new JsonResult(new Response
            {
                status = true,
                message = "Success",
                data = products
            });
            return json;
        }

        // GET: api/Products/5
        [HttpGet("api/v1/getProduct/{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            JsonResult json = new JsonResult(new Response
            {
                status = true,
                message = "success",
                data = product
            });
            /*OkObjectResult result = new OkObjectResult(json);
            result.StatusCode = 200;
            result.ContentTypes.Add("application/json");*/
            return json;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("api/v1/updateProduct/{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

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

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("api/v1/addProduct")]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, product);
        }

        // DELETE: api/Products/5
        [HttpDelete("api/v1/deleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
