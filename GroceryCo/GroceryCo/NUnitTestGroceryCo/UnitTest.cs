using GroceryCo.Services;
using NUnit.Framework;
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
            var total = checkoutService.checkout("CheckoutEmpty.csv", new System.DateTime(2020,10,21));

            Assert.That(total, Is.EqualTo(0));
        }

        [Test]
        public void TestCheckoutNoFile()
        {
            var checkoutService = new CheckoutService();
            var total = checkoutService.checkout("", new System.DateTime(2020, 10, 21));

            Assert.That(total, Is.EqualTo(0));
        }

        [Test]
        public void TestCheckoutWrongFileName()
        {
            var checkoutService = new CheckoutService();
            var total = checkoutService.checkout("Checkout555555x.csv", new System.DateTime(2020, 10, 21));

            Assert.That(total, Is.EqualTo(0));
        }

        [Test]
        public void TestCheckoutNoConfigurationFilesForMonth()
        {
            var checkoutService = new CheckoutService();
            var total = checkoutService.checkout("Checkout1.csv", new System.DateTime(2020, 5, 10));

            Assert.That(total, Is.EqualTo(0));
        }
                
        [Test]
        public void TestCheckout1NoPromotion()
        {
            var checkoutService = new CheckoutService();
            var total = checkoutService.checkout("Checkout1.csv", new System.DateTime(2020, 10, 21));

            Assert.That(total, Is.GreaterThan(0));
            //Assert.That(new FileInfo(checkoutService.ReceiptFilePath), Does.Exist);
            //Assert.That(new FileInfo(checkoutService.ReceiptFilePath).ToString(), Is.Not.Empty);
            //Assert.That(new FileInfo(checkoutService.LogFilePath), Does.Exist);
            //Assert.That(new FileInfo(checkoutService.LogFilePath).ToString(), Is.Not.Empty);
        }

        [Test]
        public void TestCheckout2AdditionalProductDiscount()
        {
            var checkoutService = new CheckoutService();
            var total = checkoutService.checkout("Checkout2.csv", new System.DateTime(2020, 10, 21));

            Assert.That(total, Is.EqualTo(0));
            Assert.That(new FileInfo(checkoutService.ReceiptFilePath), Does.Exist);
            Assert.That(new FileInfo(checkoutService.ReceiptFilePath).ToString(), Is.Not.Empty);
            Assert.That(new FileInfo(checkoutService.LogFilePath), Does.Exist);
            Assert.That(new FileInfo(checkoutService.LogFilePath).ToString(), Is.Not.Empty);
        }

        [Test]
        public void TestCheckout3AdditionalProductDiscountAndExtraItem()
        {
            var checkoutService = new CheckoutService();
            var total = checkoutService.checkout("Checkout3.csv", new System.DateTime(2020, 10, 21));

            Assert.That(total, Is.EqualTo(0));
            Assert.That(new FileInfo(checkoutService.ReceiptFilePath), Does.Exist);
            Assert.That(new FileInfo(checkoutService.ReceiptFilePath).ToString(), Is.Not.Empty);
            Assert.That(new FileInfo(checkoutService.LogFilePath), Does.Exist);
            Assert.That(new FileInfo(checkoutService.LogFilePath).ToString(), Is.Not.Empty);
        }

        [Test]
        public void TestCheckout4GroupPromotionalPrice()
        {
            var checkoutService = new CheckoutService();
            var total = checkoutService.checkout("Checkout4.csv", new System.DateTime(2020, 10, 21));

            Assert.That(total, Is.EqualTo(0));
            Assert.That(new FileInfo(checkoutService.ReceiptFilePath), Does.Exist);
            Assert.That(new FileInfo(checkoutService.ReceiptFilePath).ToString(), Is.Not.Empty);
            Assert.That(new FileInfo(checkoutService.LogFilePath), Does.Exist);
            Assert.That(new FileInfo(checkoutService.LogFilePath).ToString(), Is.Not.Empty);
        }

        [Test]
        public void TestCheckout5GroupPromotionalPriceAndExtraItem()
        {
            var checkoutService = new CheckoutService();
            var total = checkoutService.checkout("Checkout5.csv", new System.DateTime(2020, 10, 21));

            Assert.That(total, Is.EqualTo(0));
            Assert.That(new FileInfo(checkoutService.ReceiptFilePath), Does.Exist);
            Assert.That(new FileInfo(checkoutService.ReceiptFilePath).ToString(), Is.Not.Empty);
            Assert.That(new FileInfo(checkoutService.LogFilePath), Does.Exist);
            Assert.That(new FileInfo(checkoutService.LogFilePath).ToString(), Is.Not.Empty);
        }

        [Test]
        public void TestCheckout6MultipleMixedPromotions()
        {
            var checkoutService = new CheckoutService();
            var total = checkoutService.checkout("Checkout6.csv", new System.DateTime(2020, 10, 21));

            Assert.That(total, Is.EqualTo(0));
            Assert.That(new FileInfo(checkoutService.ReceiptFilePath), Does.Exist);
            Assert.That(new FileInfo(checkoutService.ReceiptFilePath).ToString(), Is.Not.Empty);
            Assert.That(new FileInfo(checkoutService.LogFilePath), Does.Exist);
            Assert.That(new FileInfo(checkoutService.LogFilePath).ToString(), Is.Not.Empty);
        }
    }
}