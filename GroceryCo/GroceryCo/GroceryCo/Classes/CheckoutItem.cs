using System;
using System.Collections.Generic;
using System.Text;

namespace GroceryCo.Classes
{
    public class CheckoutItem
    {
        public CheckoutItem(int productId, string description, int quantity, float weight)
        {
            this.ProductId = productId;
            this.Description = description;
            this.Quantity = quantity;
            this.Weight = weight;
        }

        public CheckoutItem() { 
        }

        private int _productId;
        private string _description;
        private int _quantity;
        private float _weight;

        public int ProductId { get => _productId; set => _productId = value; }
        public string Description { get => _description; set => _description = value; }
        public int Quantity { get => _quantity; set => _quantity = value; }
        public float Weight { get => _weight; set => _weight = value; }
    }
}
