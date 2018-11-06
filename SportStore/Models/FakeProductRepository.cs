using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.Models
{
    public class FakeProductRepository /* : IProductRepository */
    {
        public IQueryable<Product> Products => new List<Product>
        {
            new Product{ Name = "Piłka Nożna", Price = 25},
            new Product{ Name = "Deska surfingowa", Price = 179},
            new Product{ Name = "Buty do biegania", Price = 95}
        }.AsQueryable<Product>();
    }
}
