using System;
using System.Collections.Generic;
using System.Text;

namespace GroceryCo.Classes
{
    public class Price
    {
        public Price(int productId, string description, PriceType priceType, decimal productPrice, string unit)
        {
            this.ProductId = productId;
            this.Description = description;
            this.PriceType = priceType;
            this.ProductPrice = productPrice;
            this.Unit = unit;
        }

        private int _productId;
        private string _description;
        private PriceType _priceType;
        private decimal _productPrice;
        private string _unit;
        
        public int ProductId { get => _productId; set => _productId = value; }
        public string Description { get => _description; set => _description = value; }
        public PriceType PriceType { get => _priceType; set => _priceType = value; }
        public decimal ProductPrice { get => _productPrice; set => _productPrice = value; }
        public string Unit { get => _unit; set => _unit = value; }
    }
}
