﻿using ApplicationCore.Entities;
using ApplicationCore.Interfaces;
using Web.Angular.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;

// Helpful pages for unit testing:
// https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices
// https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/testing?view=aspnetcore-3.1

namespace UnitTests.Web.Angular.Controllers
{
    [TestClass]
    public class ProductsControllerTests
    {

        [TestMethod]
        public async Task GetAllProductsAsync_ShouldReturnOk()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            productServiceStub.Setup(ps => ps.GetAllProductsAsync())
                .ReturnsAsync(new List<Product>());
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            ActionResult<IEnumerable<Product>> actionResult = await productsController.GetAllProductsAsync();

            // Assert
            actionResult.Result.ShouldBeOfType<OkObjectResult>();
        }


        [TestMethod]
        public async Task GetProductByIdAsync_GivenProductIdThatExists_ShouldReturnTypeOk()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            int productIdThatExists = 0;
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatExists))
                .ReturnsAsync(true);
            productServiceStub.Setup(ps => ps.GetProductByIdAsync(productIdThatExists))
                .ReturnsAsync(new Product());
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            ActionResult<Product> actionResult = await productsController.GetProductByIdAsync(productIdThatExists);

            // Assert
            actionResult.Result.ShouldBeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task GetProductByIdAsync_GivenProductIdThatDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            int productIdThatDoesNotExist = 0;
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatDoesNotExist))
                .ReturnsAsync(false);
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            ActionResult<Product> actionResult = await productsController.GetProductByIdAsync(productIdThatDoesNotExist);

            // Assert
            actionResult.Result.ShouldBeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task BuyProductByIdAsync_GivenProductIdThatExists_ShouldReturnOk()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            int productIdThatExists = 0;
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatExists))
                .ReturnsAsync(true);
            productServiceStub.Setup(ps => ps.BuyProductByIdAsync(productIdThatExists));
            productServiceStub.Setup(ps => ps.GetProductByIdAsync(productIdThatExists))
                .ReturnsAsync(new Product());
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            ActionResult<int> actionResult = await productsController.BuyProductByIdAsync(productIdThatExists);

            // Assert
            actionResult.Result.ShouldBeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task BuyProductByIdAsync_GivenProductIdThatDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            int productIdThatDoesNotExist = 0;
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatDoesNotExist))
                .ReturnsAsync(false);
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            ActionResult<int> actionResult = await productsController.BuyProductByIdAsync(productIdThatDoesNotExist);

            // Assert
            actionResult.Result.ShouldBeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task AddProductAsync_GivenProduct_ShouldReturnCreated()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            productServiceStub.Setup(ps => ps.AddProductAsync(It.IsAny<Product>()));
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            ActionResult<Product> actionResult = await productsController.AddProductAsync(new Product());

            // Assert
            actionResult.Result.ShouldBeOfType<CreatedAtActionResult>();
        }

        [TestMethod]
        public async Task DeleteProductByIdAsync_GivenProductIdThatExists_ShouldReturnOk()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            var productIdThatExists = 0;
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatExists))
                .ReturnsAsync(true);
            productServiceStub.Setup(ps => ps.DeleteProductByIdAsync(productIdThatExists));
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            ActionResult actionResult = await productsController.DeleteProductByIdAsync(productIdThatExists);

            // Assert
            actionResult.ShouldBeOfType<OkResult>();
        }

        [TestMethod]
        public async Task DeleteProductByIdAsync_GivenProductIdThatDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            var productServiceStub = new Mock<IProductService>();
            var productIdThatDoesNotExist = 0;
            productServiceStub.Setup(ps => ps.GetProductByIdAsync(productIdThatDoesNotExist))
                .ReturnsAsync((Product)null);
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            ActionResult actionResult = await productsController.DeleteProductByIdAsync(productIdThatDoesNotExist);

            // Assert
            actionResult.ShouldBeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task UpdateProductAsync_GivenProductWithKnownId_ShouldReturnOk()
        {
            // Arrange
            var productIdThatExists = 1;
            var productWithKnownId = new Product()
            {
                ProductId = productIdThatExists
            };
            var productServiceStub = new Mock<IProductService>();
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatExists))
                .ReturnsAsync(true);
            productServiceStub.Setup(ps => ps.UpdateProductAsync(It.IsAny<Product>()));
            productServiceStub.Setup(ps => ps.GetProductByIdAsync(productIdThatExists))
                .ReturnsAsync(new Product());
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            ActionResult<Product> actionResult = await productsController.UpdateProductAsync(productWithKnownId);

            // Assert
            actionResult.Result.ShouldBeOfType<OkObjectResult>();
        }

        [TestMethod]
        public async Task UpdateProductAsync_GivenProductWithUnknownId_ShouldReturnNotFound()
        {
            // Arrange
            var productIdThatDoesNotExist = 0;
            var productWithUnknownId = new Product()
            {
                ProductId = productIdThatDoesNotExist
            };
            var productServiceStub = new Mock<IProductService>();
            productServiceStub.Setup(ps => ps.DoesProductIdExist(productIdThatDoesNotExist))
                .ReturnsAsync(false);
            var productsController = new ProductsController(productServiceStub.Object);

            // Act
            ActionResult<Product> actionResult = await productsController.UpdateProductAsync(productWithUnknownId);

            // Assert
            actionResult.ShouldBeOfType<ActionResult<Product>>();
        }
    }
}