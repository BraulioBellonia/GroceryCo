using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GroceryCo.Classes
{
    public class Basket
    {
        public Basket()
        {
            BasketItemList = new List<BasketItem>();
        }

        private List<BasketItem> _basketItemList;

        public List<BasketItem> BasketItemList { get => _basketItemList; set => _basketItemList = value; }

        public decimal GetTotal()
        {
            decimal total = (from basketItem in BasketItemList select basketItem.GetValue()).Sum();

            return total;
        }

        public decimal GetTotalNoDiscounts()
        {
            decimal total = (from basketItem in BasketItemList where basketItem.ItemType==BasketItemType.Product select basketItem.GetValue()).Sum();

            return total;
        }

        public decimal GetTotalSaved()
        {
            return GetTotalNoDiscounts() - GetTotal();
        }
    }
}
