using Microsoft.AspNetCore.Mvc;
using Moq;
using SportStore.Controllers;
using SportStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class OrderControllerTests
    {
        [Fact]
        public void Cannot_Checkout_Empty_Cart()
        {
            // Przygotowanie — utworzenie imitacji repozytorium.
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            // Przygotowanie — utworzenie pustego koszyka.
            Cart cart = new Cart();
            // Przygotowanie — utworzenie zamówienia.
            Order order = new Order();
            // Przygotowanie — utworzenie egzemplarza kontrolera.
            OrderController target = new OrderController(mock.Object, cart);

            // Działanie.
            ViewResult result = target.Checkout(order) as ViewResult;

            // Asercje — sprawdzenie, czy zamówienie zostało umieszczone w repozytorium.
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            // Asercje — sprawdzenie, czy metoda zwraca domyślny widok.
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            // Asercje — sprawdzenie, czy przekazujemy prawidłowy model do widoku.
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            // Przygotowanie — tworzenie imitacji repozytorium.
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            // Przygotowanie — tworzenie koszyka z produktem.
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            // Przygotowanie — tworzenie egzemplarza kontrolera.
            OrderController target = new OrderController(mock.Object, cart);
            // Przygotowanie — dodanie błędu do modelu.
            target.ModelState.AddModelError("error", "error");

            // Działanie — próba zakończenia zamówienia.
            ViewResult result = target.Checkout(new Order()) as ViewResult;

            // Asercje — sprawdzenie, czy zamówienie nie zostało przekazane do repozytorium.
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Never);
            // Asercje — sprawdzenie, czy metoda zwraca domyślny widok.
            Assert.True(string.IsNullOrEmpty(result.ViewName));
            // Asercje — sprawdzenie, czy przekazujemy nieprawidłowy model do widoku.
            Assert.False(result.ViewData.ModelState.IsValid);
        }

        [Fact]
        public void Can_Checkout_And_Submit_Order()
        {
            // Przygotowanie — tworzenie imitacji repozytorium.
            Mock<IOrderRepository> mock = new Mock<IOrderRepository>();
            // Przygotowanie — tworzenie koszyka z produktem.
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            // Przygotowanie — tworzenie egzemplarza kontrolera.
            OrderController target = new OrderController(mock.Object, cart);

            // Działanie — próba zakończenia zamówienia.
            RedirectToActionResult result =
                target.Checkout(new Order()) as RedirectToActionResult;

            // Asercje — sprawdzenie, czy zamówienie nie zostało przekazane do repozytorium.
            mock.Verify(m => m.SaveOrder(It.IsAny<Order>()), Times.Once);

            // Asercje — sprawdzenie, czy metoda przekierowuje do metody akcji Completed().
            Assert.Equal("Completed", result.ActionName);

        }
    }
}