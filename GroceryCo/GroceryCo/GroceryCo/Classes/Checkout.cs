using System;
using System.Collections.Generic;
using System.Text;

namespace GroceryCo.Classes
{
    public class Checkout
    {
        public Checkout () {
            CheckoutItemList = new List<CheckoutItem>();
        }

        private List<CheckoutItem> _checkoutItemList;

        public List<CheckoutItem> CheckoutItemList { get => _checkoutItemList; set => _checkoutItemList = value; }
    }
}
