using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using test_task_ak.Models;

namespace test_task_ak.Controllers
{
    [Route("/api/[controller]")]
    public class NewController : Controller
    {
        private ApplicationContext db;
        
        public NewController(ApplicationContext context)
        {
            db = context;
        }

        private static List<Product> products = new List<Product>(
            new[]
            {
                new Product() {Id = 1, Name = "Notebook", Price = 10000},
                new Product() {Id = 2, Name = "Car", Price = 20000},
                new Product() {Id = 2, Name = "Apple", Price = 30}
            });

        [HttpGet]
        public IEnumerable<Product> Get() => products;

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var product = products.SingleOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            products.Remove(products.SingleOrDefault(p => p.Id == id));
            return Ok();
        }

        private int NextProductId => products.Count() == 0 ? 1 : products.Max(x => x.Id) + 1;

        [HttpGet("GetNextProductId")]
        public int GetNextProductId()
        {
            return NextProductId;
        }

        [HttpPost("Create")] 
        public IActionResult Post(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            product.Id = NextProductId;
            products.Add(product);
            return CreatedAtAction(nameof(Get), new {id = product.Id}, product);
        }

        [HttpPost("AddProduct")]
        public IActionResult PostBody([FromBody] Product product) => Post(product);

        [HttpPut]
        public IActionResult Put(Product product)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

                var storedProduct = products.SingleOrDefault(p => p.Id == product.Id);
            if (storedProduct == null) return NotFound();

            storedProduct.Name = product.Name;
            storedProduct.Price = product.Price;
            return Ok(storedProduct);

        }
    }
}