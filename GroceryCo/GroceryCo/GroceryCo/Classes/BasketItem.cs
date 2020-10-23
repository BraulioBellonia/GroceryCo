using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace GroceryCo.Classes
{
    public class BasketItem
    {
        public BasketItem(int productId, string description, int quantity, float weight, Price price, Promotion promotion)
        {
            this.ProductId = productId;
            this.Description = description;
            this.Quantity = quantity;
            this.Weight = weight;
            this.Price = price;
            this.Promotion = promotion;

            if (promotion==null)
                this.ItemType = BasketItemType.Product;
            else
                this.ItemType = BasketItemType.Promotion;
        }        

        public decimal GetValue()
        {
            if (this.ItemType == BasketItemType.Product)
            {
                if (this.Price.PriceType == PriceType.Each)
                {
                    return Math.Round((decimal) this.Quantity * this.Price.ProductPrice,2);
                }
                else 
                { 
                    return Math.Round((decimal) this.Weight * this.Price.ProductPrice, 2);
                }
            }
            else 
            {
                if (Promotion.PromotionType == PromotionType.AdditionalProductDiscount)
                {
                    return Math.Round((decimal) ((float) Price.ProductPrice * Promotion.DiscountNextItem) * -1, 2);
                }
                else 
                {
                    decimal originalTotalPrice = (decimal) Promotion.Quantity * Price.ProductPrice;
                    return Math.Round((decimal) (originalTotalPrice - Promotion.ProductPriceAfterDiscount) * -1, 2);
                }
            }
        }

        private int _productId;
        private string _description;
        private int _quantity;
        private float _weight;
        private BasketItemType _itemType;
        private Price _price;
        private Promotion _promotion;

        public int ProductId { get => _productId; set => _productId = value; }
        public string Description { get => _description; set => _description = value; }
        public int Quantity { get => _quantity; set => _quantity = value; }
        public float Weight { get => _weight; set => _weight = value; }
        public BasketItemType ItemType { get => _itemType; set => _itemType = value; }
        public Price Price { get => _price; set => _price = value; }
        public Promotion Promotion { get => _promotion; set => _promotion = value; }

        public override string ToString()
        {
            if (this.ItemType == BasketItemType.Product)
            {
                string sQuantity = "";
                if (Price.PriceType == PriceType.Each)
                    sQuantity = "  " + Quantity.ToString();
                else
                    sQuantity = "  " + Weight.ToString() + Price.Unit;
                
                string sDescription = Description;
                string sValue = this.GetValue().ToString("C2");

                do
                {
                    sQuantity += " ";
                }
                while (sQuantity.Length < 20);

                do
                {
                    sDescription += " ";
                }
                while (sDescription.Length < 80);

                do
                {
                    sValue += " ";
                }
                while (sValue.Length < 20);

                return sQuantity + sDescription + sValue;
            }
            else
            {
                string sDescription = "  " + Promotion.ToString();
                string sValue = this.GetValue().ToString("C2");

                do
                {
                    sDescription += " ";
                }
                while (sDescription.Length < 100);

                do
                {
                    sValue += " ";
                }
                while (sValue.Length < 20);

                return sDescription + sValue; ;
            }            
        }

    }
}
