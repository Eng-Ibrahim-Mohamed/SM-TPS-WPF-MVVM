using System.Collections.Generic;
using SM_TPS_.Models;

namespace SM_TPS_.Services
{
    public class ProductService
    {
        private readonly List<Product> _products = new List<Product>
        {
            new Product { Name = "Milk", Price = 20 },
            new Product { Name = "Bread", Price = 15 },
            new Product { Name = "Rice", Price = 50 }
        };

        public List<Product> GetAll()
        {
            return new List<Product>(_products);
        }

        public void Add(Product product)
        {
            _products.Add(product);
        }

        public void Update(Product product)
        {
        }

        public void Delete(Product product)
        {
            _products.Remove(product);
        }
    }
}