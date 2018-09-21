using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Moq;
using SportStore.Controllers;
using SportStore.Models;
using Xunit;
using SportStore.Models.ViewModels;

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
            ProductListViewModel result =
                controller.List(null, 2).ViewData.Model as ProductListViewModel;

            //Asercje
            Product[] prodArray = result.Products.ToArray();
            Assert.True(prodArray.Length == 2);
            Assert.Equal("P4", prodArray[0].Name);
            Assert.Equal("P5", prodArray[1].Name);
        }
        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            //Przygotowanie
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
                new Product {ProductID = 4, Name = "P4"},
                new Product {ProductID = 5, Name = "P5"}
            }).AsQueryable<Product>());

            ProductController controller = new ProductController(mock.Object) { PageSize = 3 };

            //Działanie.
            ProductListViewModel result =
                controller.List(null, 2).ViewData.Model as ProductListViewModel;

            //assercje
            PagingInfo pageInfo = result.PagingInfo;
            Assert.Equal(2, pageInfo.CurrentPage);
            Assert.Equal(3, pageInfo.ItemsPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(2, pageInfo.TotalPages);
        }
        [Fact]
        public void Can_Filter_Products()
        {
            //Przygotowanie
            //Utworzenie imitacji repozytorium.
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns((new Product[]
            {
                new Product {ProductID = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductID = 2, Name = "P2", Category = "Cat2"},
                new Product {ProductID = 3, Name = "P3", Category = "Cat1"},
                new Product {ProductID = 4, Name = "P4", Category = "Cat2"},
                new Product {ProductID = 5, Name = "P5", Category = "Cat3"}
            }).AsQueryable<Product>());

            //Przygotowanie - utworzenie kontrolera i ustawienie 3-elementowej strony.
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            //Działanie
            Product[] result =
                (controller.List("Cat2", 1).ViewData.Model as ProductListViewModel)
                    .Products.ToArray();

            //Assercje
            Assert.Equal(2, result.Length);
            Assert.True(result[0].Name == "P2" && result[0].Category == "Cat2");
            Assert.True(result[1].Name == "P4" && result[1].Category == "Cat2");
        }
    }
}
