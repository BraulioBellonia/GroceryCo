using System;
using System.Collections.Generic;
using System.Text;

namespace GroceryCo.Classes
{
    public class Promotion
    {
        public Promotion(int productId, string description, PromotionType promotionType, decimal productPriceAfterDiscount, 
            int quantity, float discountNextItem, DateTime startDate, DateTime endDate)
        {
            this.ProductId = productId;
            this.Description = description;
            this.PromotionType = promotionType;
            this.ProductPriceAfterDiscount = productPriceAfterDiscount;
            this.Quantity = quantity;
            this.DiscountNextItem = discountNextItem;
            this.StartDate = startDate;
            this.EndDate = endDate;
        }

        private int _productId;
        private string _description;
        private PromotionType _promotionType;
        private decimal _productPriceAfterDiscount;
        private int _quantity;
        private float _discountNextItem;
        private DateTime _startDate;
        private DateTime _endDate;

        public int ProductId { get => _productId; set => _productId = value; }
        public string Description { get => _description; set => _description = value; }
        public PromotionType PromotionType { get => _promotionType; set => _promotionType = value; }
        public decimal ProductPriceAfterDiscount { get => _productPriceAfterDiscount; set => _productPriceAfterDiscount = value; }
        public int Quantity { get => _quantity; set => _quantity = value; }
        public DateTime StartDate { get => _startDate; set => _startDate = value; }
        public DateTime EndDate { get => _endDate; set => _endDate = value; }
        public float DiscountNextItem { get => _discountNextItem; set => _discountNextItem = value; }

        public override string ToString()
        {
            if (PromotionType == PromotionType.AdditionalProductDiscount && DiscountNextItem == Math.Floor(DiscountNextItem))
                return ($"Buy {Quantity} {Description} get {DiscountNextItem} free");
            else if (PromotionType == PromotionType.AdditionalProductDiscount)
                return ($"Buy {Quantity} {Description} get one for {DiscountNextItem * 100}% off");
            else
                return ($"Buy {Quantity} {Description} for ${ProductPriceAfterDiscount}.");
        }
    }
}
