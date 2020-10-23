using System;
using System.Collections.Generic;
using System.Text;

namespace GroceryCo.Classes
{
    public class Product
    {
        public Product(int productId, string description)
        {
            this.ProductId = productId;
            this.Description = description;
        }

        private int _productId;
        private string _description;

        public int ProductId { get => _productId; set => _productId = value; }
        public string Description { get => _description; set => _description = value; }
    }
}
