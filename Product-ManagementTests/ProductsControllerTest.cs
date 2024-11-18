using Microsoft.AspNetCore.Mvc;
using Product_Management_API.Controllers;
using Product_Management_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product_ManagementTests
{
    public class ProductsControllerTests
    {
        [Fact]
        public void GetProducts_ReturnsOkResult()
        {
            var controller = new ProductsController();

            var result = controller.GetProducts();

            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var products = Assert.IsType<List<Product>>(okResult.Value);
            Assert.NotNull(products);
        }
    }
}
