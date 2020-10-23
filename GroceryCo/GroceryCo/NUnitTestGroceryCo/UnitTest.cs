using GroceryCo.Services;
using NUnit.Framework;
using System;
using System.ComponentModel.Design;
using System.IO;

namespace NUnitTestGroceryCo
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestCheckoutEmptyFile()
        {
            var checkoutService = new CheckoutService();
            checkoutService.checkout("CheckoutEmpty.csv", new System.DateTime(2020,10,21));

            Assert.That(checkoutService.Basket, Is.Null);
            Assert.That(checkoutService.ReceiptFilePath, Is.Null);
            Assert.That(new FileInfo(checkoutService.LogFilePath), Does.Exist);
            Assert.That(new FileInfo(checkoutService.LogFilePath).ToString(), Is.Not.Empty);            
        }

        [Test]
        public void TestCheckoutNoFile()
        {
            var checkoutService = new CheckoutService();
            checkoutService.checkout("", new System.DateTime(2020, 10, 21));

            Assert.That(checkoutService.Basket, Is.Null);
            Assert.That(checkoutService.ReceiptFilePath, Is.Null);
            Assert.That(new FileInfo(checkoutService.LogFilePath), Does.Exist);
            Assert.That(new FileInfo(checkoutService.LogFilePath).ToString(), Is.Not.Empty);
        }

        [Test]
        public void TestCheckoutWrongFileName()
        {
            var checkoutService = new CheckoutService();
            checkoutService.checkout("Checkout555555x.csv", new System.DateTime(2020, 10, 21));

            Assert.That(checkoutService.Basket, Is.Null);
            Assert.That(checkoutService.ReceiptFilePath, Is.Null);
            Assert.That(new FileInfo(checkoutService.LogFilePath), Does.Exist);
            Assert.That(new FileInfo(checkoutService.LogFilePath).ToString(), Is.Not.Empty);
        }

        [Test]
        public void TestCheckoutNoConfigurationFilesForMonth()
        {
            var checkoutService = new CheckoutService();
            checkoutService.checkout("Checkout1.csv", new System.DateTime(2020, 5, 10));

            Assert.That(checkoutService.Basket, Is.Null);
            Assert.That(checkoutService.ReceiptFilePath, Is.Null);
            Assert.That(new FileInfo(checkoutService.LogFilePath), Does.Exist);
            Assert.That(new FileInfo(checkoutService.LogFilePath).ToString(), Is.Not.Empty);
        }
                
        [Test]
        public void TestCheckout1NoPromotion()
        {
            var checkoutService = new CheckoutService();
            checkoutService.checkout("Checkout1.csv", new System.DateTime(2020, 10, 21));
            
            Assert.That(Math.Round(checkoutService.Basket.GetTotal(),2), Is.EqualTo(239.26));
            Assert.That(Math.Round(checkoutService.Basket.GetTotalSaved(), 2), Is.EqualTo(0));
            Assert.That(new FileInfo(checkoutService.ReceiptFilePath), Does.Exist);
            Assert.That(new FileInfo(checkoutService.ReceiptFilePath).ToString(), Is.Not.Empty);
            Assert.That(checkoutService.LogFilePath, Is.Null);
        }

        [Test]
        public void TestCheckout2AdditionalProductDiscount()
        {
            var checkoutService = new CheckoutService();
            checkoutService.checkout("Checkout2.csv", new System.DateTime(2020, 10, 21));

            Assert.That(Math.Round(checkoutService.Basket.GetTotal(), 2), Is.EqualTo(152.61));
            Assert.That(Math.Round(checkoutService.Basket.GetTotalSaved(), 2), Is.EqualTo(1.82));
            Assert.That(new FileInfo(checkoutService.ReceiptFilePath), Does.Exist);
            Assert.That(new FileInfo(checkoutService.ReceiptFilePath).ToString(), Is.Not.Empty);
            Assert.That(checkoutService.LogFilePath, Is.Null);
        }

        [Test]
        public void TestCheckout3AdditionalProductDiscountAndExtraItem()
        {
            var checkoutService = new CheckoutService();
            checkoutService.checkout("Checkout3.csv", new System.DateTime(2020, 10, 21));

            Assert.That(Math.Round(checkoutService.Basket.GetTotal(), 2), Is.EqualTo(154.61));
            Assert.That(Math.Round(checkoutService.Basket.GetTotalSaved(), 2), Is.EqualTo(1.82));
            Assert.That(new FileInfo(checkoutService.ReceiptFilePath), Does.Exist);
            Assert.That(new FileInfo(checkoutService.ReceiptFilePath).ToString(), Is.Not.Empty);
            Assert.That(checkoutService.LogFilePath, Is.Null);
        }

        [Test]
        public void TestCheckout4GroupPromotionalPrice()
        {
            var checkoutService = new CheckoutService();
            checkoutService.checkout("Checkout4.csv", new System.DateTime(2020, 10, 21));

            Assert.That(Math.Round(checkoutService.Basket.GetTotal(), 2), Is.EqualTo(66.28));
            Assert.That(Math.Round(checkoutService.Basket.GetTotalSaved(), 2), Is.EqualTo(0.5));
            Assert.That(new FileInfo(checkoutService.ReceiptFilePath), Does.Exist);
            Assert.That(new FileInfo(checkoutService.ReceiptFilePath).ToString(), Is.Not.Empty);
            Assert.That(checkoutService.LogFilePath, Is.Null);
        }

        [Test]
        public void TestCheckout5GroupPromotionalPriceAndExtraItem()
        {
            var checkoutService = new CheckoutService();
            checkoutService.checkout("Checkout5.csv", new System.DateTime(2020, 10, 21));

            Assert.That(Math.Round(checkoutService.Basket.GetTotal(), 2), Is.EqualTo(71.28));
            Assert.That(Math.Round(checkoutService.Basket.GetTotalSaved(), 2), Is.EqualTo(0.5));
            Assert.That(new FileInfo(checkoutService.ReceiptFilePath), Does.Exist);
            Assert.That(new FileInfo(checkoutService.ReceiptFilePath).ToString(), Is.Not.Empty);
            Assert.That(checkoutService.LogFilePath, Is.Null);
        }

        [Test]
        public void TestCheckout6MultipleMixedPromotions()
        {
            var checkoutService = new CheckoutService();
            checkoutService.checkout("Checkout6.csv", new System.DateTime(2020, 10, 25));

            Assert.That(Math.Round(checkoutService.Basket.GetTotal(), 2), Is.EqualTo(81.51));
            Assert.That(Math.Round(checkoutService.Basket.GetTotalSaved(), 2), Is.EqualTo(9.77));
            Assert.That(new FileInfo(checkoutService.ReceiptFilePath), Does.Exist);
            Assert.That(new FileInfo(checkoutService.ReceiptFilePath).ToString(), Is.Not.Empty);
            Assert.That(checkoutService.LogFilePath, Is.Null);
        }
    }
}