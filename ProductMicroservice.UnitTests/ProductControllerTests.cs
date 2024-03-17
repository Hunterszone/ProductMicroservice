using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ProductMicroservice.Controllers;
using ProductMicroservice.Models;
using ProductMicroservice.Repository;

namespace ProductMicroservice.UnitTests
{
    [TestClass]
    public class ProductControllerTests
    {
        private Mock<IProductRepository> _mockProductRepository;
        private ProductController _productController;

        [TestInitialize]
        public void Setup()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _productController = new ProductController(_mockProductRepository.Object);
        }

        [TestMethod]
        public void Get_ReturnsProducts()
        {
            // Arrange
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product 1" },
                new Product { Id = 2, Name = "Product 2" }
            };
            _mockProductRepository.Setup(repo => repo.GetProducts()).Returns(products);

            // Act
            var result = _productController.GetAllProducts();

            // Assert
            Assert.IsNotNull(result);
            var productResult = result.ToList();
            Assert.AreEqual(products.Count, productResult.Count);
        }

        [TestMethod]
        public void Get_ReturnsOkResult_WithValidId()
        {
            // Arrange
            int id = 1;
            var expectedProduct = new Product { Id = id, Name = "Product 1" };
            _mockProductRepository.Setup(repo => repo.GetProductByID(id)).Returns(expectedProduct);

            // Act
            var result = _productController.Get(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
            Assert.AreEqual(expectedProduct, okResult.Value);
        }

        [TestMethod]
        public void Get_ReturnsNotFound_WithInvalidId()
        {
            // Arrange
            int id = 1;
            _mockProductRepository.Setup(repo => repo.GetProductByID(id)).Returns((Product)null);

            // Act
            var result = _productController.Get(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
            var notFoundResult = result as NotFoundResult;
            Assert.IsNotNull(notFoundResult);
            Assert.AreEqual(404, notFoundResult.StatusCode);
        }

        [TestMethod]
        public void Post_ReturnsCreatedAtAction_WithValidProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1" };

            _mockProductRepository.Setup(repo => repo.InsertProduct(product));

            // Act
            var result = _productController.Post(product);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CreatedAtActionResult));
            var createdAtActionResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual("Get", createdAtActionResult.ActionName);
            Assert.AreEqual(product.Id, createdAtActionResult.RouteValues["id"]);
            Assert.AreEqual(product, createdAtActionResult.Value);
        }

        [TestMethod]
        public void Put_ReturnsOkResult_WithValidProduct()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Updated product" };

            _mockProductRepository.Setup(repo => repo.UpdateProduct(product));

            // Act
            var result = _productController.Put(product);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
            var okResult = result as OkResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }

        [TestMethod]
        public void Put_ReturnsOkResult_WithNullProduct()
        {
            // Arrange
            _mockProductRepository.Setup(repo => repo.UpdateProduct((Product)null));

            // Act
            var result = _productController.Put((Product)null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NoContentResult));
            var noContentResult = result as NoContentResult;
            Assert.IsNotNull(noContentResult);
            Assert.AreEqual(204, noContentResult.StatusCode);
        }

        [TestMethod]
        public void Delete_ReturnsOkResult_WithId()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Product 1" };
            _mockProductRepository.Setup(repo => repo.DeleteProduct(product.Id));

            // Act
            var result = _productController.Delete(product.Id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
            var okResult = result as OkResult;
            Assert.IsNotNull(okResult);
            Assert.AreEqual(200, okResult.StatusCode);
        }
    }
}