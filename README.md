# GroceryCo
GroceryCo

Limitations/design choices
- I am only keeping the prices and promotions on files to keep it simple for this exercise, in a real scenario they would be tables on a PoS, or 
they would be part of the integration between PoS and the Marketing system. In the exercise, the marketing and product supply staff will edit
the promotions and prices records straight on the csv files (which is dangerous) using Excel or Notepad. In real life they would input the data on the specific pages of the application with proper security and validations.
- In real life I would also save the items and results of the checkout in the database to be traceable, to be used for accountability and taxes collection by the company, and also in case a customer come back to return a item.
For this example I am just printing the results in a txt file and on console.
- Since this application will be a console, so there will be no insert, delete and update on any UI, I'll also keep the products in a csv file to avoid the need to create a database just for one table.

Assumptions
- In the real world the system should charge sales taxes on the products according to province (gst,pst,hst,qst), some products would be exempt.
Also some products should have extra fees/levies regarding environment issues and others. For example Water bottles and milk. 
None of these rules were described on the exercise, so I'll not calculate them to keep the solution simple.
- Grocery stores usually checkout the product in two possible ways, it could be by weight (e.g. bananas, tomatoes) or by unit/each (e.g. green onions bunch, box of cereals),
so I added this behaviour to be more accurate.
- I kept the product file simple, but it could have other fields such as brand, department, size, perishable, packaging and etc.
- I only added the product description on PriceList and Promotion file to facilitate the reading for marketing and product supply staff. In a real system the description should be only on Product table.
- There will be one Pricelist file per month, I did it to avoid having a huge csv file after a year with multiple prices for the same products. I added only 20 products in the list, but a real grocery store would have thousands of products in the list per month.
- In a real system there would be a table with the list of Pricelists with its own start/end dates and in each of them there would be the products saved as an item of the list. I didn't add a PriceListItemId to the Pricelist file to simplify the maintenance for marketing staff.
- I am not considering coupons in the application, regardless if it's a general coupon or if it's a counpon generated specificaly for the member and the item purchased.
- I am also not considering membership redeemings to give discount in the final price or to give extra discount in products.
- There will be one Promotion file per month to avoid huge files, and the items themselves will be limited by start and end date because usually promotions have a duration of days.
- I didn't added barcode fields into the products, but usually real systems uses them to locate the products with the scanning.
- My prototype also do not care about the stock, some PoS sometimes use SKU to track the products in the stock, specially if the stock is shared between the store itself and the e-commerce.

Notes
- I know promotion type and price should be a enum or a table, but I let it as a string to help the merketing person with the files. If there was an UI to input promotions and price, I would not let it like that.
- I am passing the date on the arguments just to be able to test the checkout program in multiple dates inclusind in the past and the future.