using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SportStore.Controllers;
using SportStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class AdminControllerTests
    {
        [Fact]
        public void Index_Contains_All_Products()
        {
            // Przygotowanie — tworzenie imitacji repozytorium.
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
            }.AsQueryable<Product>());

            // Przygotowanie — utworzenie kontrolera.
            AdminController target = new AdminController(mock.Object);

            // Działanie.
            Product[] result =
                GetViewModel<IEnumerable<Product>>(target.Index())?.ToArray();

            // Asercje.
            Assert.Equal(3, result.Length);
            Assert.Equal("P1", result[0].Name);
            Assert.Equal("P2", result[1].Name);
            Assert.Equal("P3", result[2].Name);
        }

        private T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }

        [Fact]
        public void Can_Edit_Product()
        {
            // Przygotowanie — tworzenie imitacji repozytorium.
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
            }.AsQueryable<Product>());

            // Przygotowanie — utworzenie kontrolera.
            AdminController target = new AdminController(mock.Object);

            // Działanie.
            Product p1 = GetViewModel<Product>(target.Edit(1));
            Product p2 = GetViewModel<Product>(target.Edit(2));
            Product p3 = GetViewModel<Product>(target.Edit(3));

            // Asercje.
            Assert.Equal(1, p1.ProductID);
            Assert.Equal(2, p2.ProductID);
            Assert.Equal(3, p3.ProductID);
        }

        [Fact]
        public void Cannot_Edit_Nonexistent_Product()
        {
            // Przygotowanie — tworzenie imitacji repozytorium.
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1"},
                new Product {ProductID = 2, Name = "P2"},
                new Product {ProductID = 3, Name = "P3"},
            }.AsQueryable<Product>());

            // Przygotowanie — utworzenie kontrolera.
            AdminController target = new AdminController(mock.Object);

            // Działanie.
            Product result = GetViewModel<Product>(target.Edit(4));

            // Asercje.
            Assert.Null(result);
        }

        [Fact]
        public void Can_Save_Valid_Changes()
        {
            // Przygotowanie — tworzenie imitacji repozytorium.
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            // Przygotowanie — tworzenie imitacji kontenera TempData.
            Mock<ITempDataDictionary> tempData = new Mock<ITempDataDictionary>();
            // Przygotowanie — tworzenie kontrolera.
            AdminController target = new AdminController(mock.Object)
            {
                TempData = tempData.Object
            };
            // Przygotowanie — tworzenie produktu.
            Product product = new Product { Name = "Test" };

            // Działanie — próba zapisania produktu.
            IActionResult result = target.Edit(product);

            // Asercje — sprawdzenie, czy zostało wywołane repozytorium.
            mock.Verify(m => m.SaveProduct(product));
            // Asercje — sprawdzenie typu zwracanego z metody.
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", (result as RedirectToActionResult).ActionName);
        }

        [Fact]
        public void Cannot_Save_Invalid_Changes()
        {
            // Przygotowanie — tworzenie imitacji repozytorium.
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            // Przygotowanie — tworzenie kontrolera.
            AdminController target = new AdminController(mock.Object);
            // Przygotowanie — tworzenie produktu.
            Product product = new Product { Name = "Test" };
            // Przygotowanie — dodanie błędu do stanu modelu.
            target.ModelState.AddModelError("error", "error");

            // Działanie — próba zapisania produktu.
            IActionResult result = target.Edit(product);

            // Asercje — sprawdzenie, czy nie zostało wywołane repozytorium.
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());
            // Asercje — sprawdzenie typu zwracanego z metody.
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Can_Delete_Valid_Products()
        {
            // Przygotowanie — tworzenie produktu.
            Product prod = new Product { ProductID = 2, Name = "Test" };

            // Przygotowanie — tworzenie imitacji repozytorium.
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID = 1, Name = "P1"},
                prod,
                new Product {ProductID = 3, Name = "P3"},
            }.AsQueryable<Product>());

            // Przygotowanie — tworzenie kontrolera.
            AdminController target = new AdminController(mock.Object);

            // Działanie — usunięcie produktu.
            target.Delete(prod.ProductID);

            // Asercje — upewnienie się, że metoda repozytorium
            // została wywołana z właściwym produktem.
            mock.Verify(m => m.DeleteProduct(prod.ProductID));
        }
    }
}
