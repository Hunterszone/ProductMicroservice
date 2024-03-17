using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using ProductMicroservice.Models;
using ProductMicroservice.Repository;
using PagedList;

namespace ProductMicroservice.Controllers
{
    [Route("/api")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpGet("products")]
        public IEnumerable<Product> GetAllProducts()
        {
            var products = _productRepository.GetProducts();
            return products;
        }

        [HttpGet("products/paged")]
        public IPagedList<Product> GetAllProductsPerPage([FromQuery(Name = "page")] int page, 
                                                         [FromQuery(Name = "size")] int size)
        {
            var products = _productRepository.GetProductsPerPage(page, size);
            return products;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var product = _productRepository.GetProductByID(id);

            if (product == null)
            {
                return NotFound();
            }

            return new OkObjectResult(product);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Product product)
        {
            using (var scope = new TransactionScope())
            {
                _productRepository.InsertProduct(product);
                scope.Complete();
                return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
            }
        }

        [HttpPut]
        public IActionResult Put([FromBody] Product product)
        {
            if (product != null)
            {
                using (var scope = new TransactionScope())
                {
                    _productRepository.UpdateProduct(product);
                    scope.Complete();
                    return new OkResult();
                }
            }
            return new NoContentResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var product = _productRepository.GetProductByID(id);

            if (product != null)
            {
                _productRepository.DeleteProduct(id);
                return new OkResult();
            }

            return new NotFoundResult();
        }
    }
}
