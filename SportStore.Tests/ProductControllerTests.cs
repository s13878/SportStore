using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Moq;
using SportStore.Controllers;
using SportStore.Models;
using Xunit;

namespace SportStore.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void Can_Paginate()
        {
            //Przygotowanie
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 1, Name = "P2"},
                new Product {ProductID = 1, Name = "P3"},
                new Product {ProductID = 1, Name = "P4"},
                new Product {ProductID = 1, Name = "P5"}

            }).AsQueryable<Product>());

            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //Działanie
            IEnumerable<Product> result =
                controller.List(2).ViewData.Model as IEnumerable<Product>;

            //Asercje
            Product[] prodArray = result.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("P4", prodArray[0].Name);
            Assert.Equal("P5", prodArray[1].Name);
        }
    }
}
