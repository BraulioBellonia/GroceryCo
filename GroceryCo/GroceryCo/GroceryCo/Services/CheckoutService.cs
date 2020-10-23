using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Configuration;
using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using GroceryCo.Classes;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Runtime.InteropServices;

namespace GroceryCo.Services
{
    public class CheckoutService
    {
        public CheckoutService ()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }

        private IConfigurationRoot _configuration;
        private string _logFilePath;
        private string _receiptFilePath;
        private string _priceListFilePath;
        private string _promotionsFilePath;
        private string _checkoutFilePath;
        private string _productsFilePath;
        private Basket _basket;
        private DateTime checkoutDate;

        private List<Price> priceList;
        private List<Promotion> promotionList;
        private List<Product> productList;
        private Checkout checkoutList;

        #region Properties
        public string LogFilePath { get => _logFilePath; set => _logFilePath = value; }
        public string ReceiptFilePath { get => _receiptFilePath; set => _receiptFilePath = value; }
        public string PriceListFilePath { get => _priceListFilePath; set => _priceListFilePath = value; }
        public string PromotionsFilePath { get => _promotionsFilePath; set => _promotionsFilePath = value; }
        public string CheckoutFilePath { get => _checkoutFilePath; set => _checkoutFilePath = value; }
        public string ProductsFilePath { get => _productsFilePath; set => _productsFilePath = value; }
        public Basket Basket { get => _basket; set => _basket = value; }
        public IConfigurationRoot Configuration { get => _configuration; set => _configuration = value; }
        #endregion

        public decimal checkout(string fileName, DateTime checkoutDate)
        {
            if (!loadConfigurations(fileName, checkoutDate)) return 0;

            loadMarketingFiles();
            processCheckout();

            return Basket.GetTotal();
        }

        public void processCheckout() 
        {
            this.checkoutList = loadCheckoutFile();

            //grouping checkout records from checkout file
            var checkoutGroup = (from checkoutItem in this.checkoutList.CheckoutItemList
                                 group checkoutItem by new { checkoutItem.ProductId, checkoutItem.Description} into g
                                 select new { ProductId = g.Key.ProductId,
                                              Description = g.Key.Description,
                                              Quantity = g.Sum(q => q.Quantity),
                                              Weight = g.Sum(w => w.Weight) }).ToList();

            //adding products to the basket
            Basket = new Basket();
            foreach (var item in checkoutGroup)
            {
                Basket.BasketItemList.Add(new BasketItem(item.ProductId,
                                                         item.Description,
                                                         item.Quantity,
                                                         item.Weight,
                                                         priceList.Where(p => p.ProductId == item.ProductId).FirstOrDefault(),
                                                         null));
            }

            //adding promotions to the basket
            var activePromotionList = promotionList.Where(p => p.StartDate <= checkoutDate && checkoutDate <= p.EndDate).ToList();
            foreach (var activePromotion in activePromotionList)
            {
                var basketItem = Basket.BasketItemList.Find(b => b.ProductId == activePromotion.ProductId && b.ItemType == BasketItemType.Product);
                if (basketItem != null)
                {
                    if (activePromotion.PromotionType == PromotionType.AdditionalProductDiscount)
                    {
                        if (basketItem.Quantity > activePromotion.Quantity)
                        {
                            var promotionBasket = new Basket();

                            int i = activePromotion.Quantity;
                            while (i < basketItem.Quantity)
                            {
                                promotionBasket.BasketItemList.Add(new BasketItem (basketItem.ProductId,
                                                                        basketItem.Description,
                                                                        0,
                                                                        0,
                                                                        basketItem.Price,
                                                                        activePromotion));
                                i += activePromotion.Quantity + 1;
                            }

                            Basket.BasketItemList.AddRange(promotionBasket.BasketItemList);
                        }
                    }
                    else
                    {
                        if (basketItem.Quantity >= activePromotion.Quantity)
                        {
                            var promotionBasket = new Basket();

                            int i = activePromotion.Quantity;
                            while (i <= basketItem.Quantity)
                            {
                                promotionBasket.BasketItemList.Add(new BasketItem(basketItem.ProductId,
                                                                        basketItem.Description,
                                                                        0,
                                                                        0,
                                                                        basketItem.Price,
                                                                        activePromotion));
                                i += activePromotion.Quantity;
                            }

                            Basket.BasketItemList.AddRange(promotionBasket.BasketItemList);
                        }
                    }
                }                
            }

            //printing results into receipt file and console
            Basket.BasketItemList = Basket.BasketItemList.OrderBy(b => b.ProductId).ThenBy(b => b.ItemType).ToList();

            StringWriter stringWriter = new StringWriter();
            
            string line = "------------------------------------------------------------------------------------------------------------------------";
            stringWriter.WriteLine(line);
            stringWriter.WriteLine();
            stringWriter.WriteLine("GROCERY CO");
            stringWriter.WriteLine();
            stringWriter.WriteLine(line);
            foreach (var basketItem in Basket.BasketItemList)
            {
                stringWriter.WriteLine(basketItem.ToString());
            }
            stringWriter.WriteLine(line);
            stringWriter.WriteLine();

            string description = "                                                                                ";
            string sTotal = "TOTAL: " + Basket.GetTotal().ToString("C2");
            do
            {
                sTotal += " ";
            }
            while (sTotal.Length < 40);

            stringWriter.WriteLine(description + sTotal);
            stringWriter.WriteLine();

            string sTotalSaving = "TOTAL SAVINGS: "  + Basket.GetTotalSaved().ToString("C2");
            do
            {
                sTotalSaving += " ";
            }
            while (sTotalSaving.Length < 40);

            stringWriter.WriteLine(description + sTotalSaving);
            stringWriter.WriteLine();
            stringWriter.WriteLine(line);
            stringWriter.WriteLine("Checkout Date:" + checkoutDate.ToString("yyyy-MM-dd HH:mm:ss"));
            stringWriter.WriteLine("Processing Date:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            stringWriter.WriteLine(line);

            Console.Write(stringWriter.ToString());

            ReceiptFilePath = Path.Combine(Configuration["receiptfolder"], String.Format(Configuration["receiptfilename"], Path.GetFileNameWithoutExtension(CheckoutFilePath), DateTime.Now.ToString("yyyy-MM-dd_HHmmss")));
            System.IO.File.WriteAllText(ReceiptFilePath, stringWriter.ToString());

        }

        #region LOAD FILES AND CONFIGURATIONS

        private Boolean loadConfigurations(string fileName, DateTime checkoutDateTime)
        {
            if (checkoutDateTime != DateTime.MinValue)
                checkoutDate = checkoutDateTime;
            else
                checkoutDate = DateTime.Now;


            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(Configuration["checkoutfolder"]) || !File.Exists(Path.Combine(Configuration["checkoutfolder"], fileName)))
            {
                Console.WriteLine("Invalid file name.");
                return false;
            }
            else
                CheckoutFilePath = Path.Combine(Configuration["checkoutfolder"], fileName);

            if (string.IsNullOrEmpty(Configuration["productsfilename"]) || string.IsNullOrEmpty(Configuration["configurationfolder"])
                    || !File.Exists(Path.Combine(Configuration["configurationfolder"], Configuration["productsfilename"])))
            {
                Console.WriteLine("Invalid Product file configuration.");
                return false;
            }
            else
                ProductsFilePath = Path.Combine(Configuration["configurationfolder"], Configuration["productsfilename"]);

            if (string.IsNullOrEmpty(Configuration["pricelistfilename"]) || string.IsNullOrEmpty(Configuration["configurationfolder"])
                    || !File.Exists(Path.Combine(Configuration["configurationfolder"], String.Format(Configuration["pricelistfilename"], (checkoutDate.Year + "-" + checkoutDate.Month.ToString("00"))))))
            {
                Console.WriteLine("Invalid Price List file configuration.");
                return false;
            }
            else
                PriceListFilePath = Path.Combine(Configuration["configurationfolder"], String.Format(Configuration["pricelistfilename"], (checkoutDate.Year + "-" + checkoutDate.Month.ToString("00"))));

            if (string.IsNullOrEmpty(Configuration["promotionfilename"]) || string.IsNullOrEmpty(Configuration["configurationfolder"])
                    || !File.Exists(Path.Combine(Configuration["configurationfolder"], String.Format(Configuration["promotionfilename"], (checkoutDate.Year + "-" + checkoutDate.Month.ToString("00"))))))
            {
                Console.WriteLine("Invalid Promotion file configuration.");
                return false;
            }
            else
                PromotionsFilePath = Path.Combine(Configuration["configurationfolder"], String.Format(Configuration["promotionfilename"], (checkoutDate.Year + "-" + checkoutDate.Month.ToString("00"))));

            return true;
        }

        public void loadMarketingFiles() 
        {
            productList = loadProductFile();
            priceList = loadPriceFile();
            promotionList = loadPromotionFile();
        }

        public List<Product> loadProductFile()
        {
            using (var reader = new StreamReader(ProductsFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = new List<Product>();
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var record = new Product(csv.GetField<int>("ProductId"), 
                                             csv.GetField("Description"));
                    records.Add(record);
                }
                return records;
            }
        }
        public List<Price> loadPriceFile()
        {
            using (var reader = new StreamReader(PriceListFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = new List<Price>();
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var record = new Price(csv.GetField<int>("ProductId"),
                                             csv.GetField("Description"),
                                             csv.GetField("Type") == "Weight" ? PriceType.Weight : PriceType.Each,
                                             csv.GetField<decimal>("Price"),
                                             csv.GetField("Unit"));
                    records.Add(record);
                }
                return records;
            }
        }
        public List<Promotion> loadPromotionFile() 
        {
            using (var reader = new StreamReader(PromotionsFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var records = new List<Promotion>();
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var record = new Promotion(csv.GetField<int>("ProductId"),
                                              csv.GetField("Description"),
                                              csv.GetField("PromotionType") == "AdditionalProductDiscount" ? PromotionType.AdditionalProductDiscount : PromotionType.GroupPromotionalPriceByQuantity,
                                              csv.GetField<decimal>("PriceAfterDiscount"),
                                              csv.GetField<int>("Quantity"),
                                              csv.GetField<float>("DiscountNextItem"),                                              
                                              csv.GetField<DateTime>("StartDate"),
                                              csv.GetField<DateTime>("EndDate"));
                    records.Add(record);
                }
                return records;
            }
        }

        public Checkout loadCheckoutFile()
        {
            using (var reader = new StreamReader(CheckoutFilePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var checkout = new Checkout();
                csv.Read();
                csv.ReadHeader();
                while (csv.Read())
                {
                    var record = new CheckoutItem(csv.GetField<int>("ProductId"),
                                              csv.GetField("Description"),
                                              csv.GetField<int>("Quantity"),
                                              csv.GetField<float>("Weight"));
                    checkout.CheckoutItemList.Add(record);
                }
                return checkout;
            }
        }
        #endregion

    }
}
