using System;
using System.Collections.Generic;
using static System.Console;

namespace SalesBasket
{
    class MainClass
    {
        public class Receipt
        {
            public double TotalCost { get
                {
                    double result = 0.0;
                    foreach(var product in Products)
                    {
                        result += product.TotalPrice;
                    }
                    return result;
                }
            }
            public double TotalTax { get
                {
                    double result = 0.0;
                    foreach (var product in Products)
                    {
                        result += product.TaxCost;
                    }
                    return result;
                }
            }
            public IEnumerable<Product> Products { get; set; }
            public static int Count { get; set; }

            public Receipt(IEnumerable<Product> products)
            {
                Products = products;
                Count += 1;
            }

            public void PrintReceipt()
            {
                WriteLine($"Output {Count}");
                foreach (Product product in Products)
                {
                    WriteLine("Item: {0:C}, Price: {1:C}", product.Name, product.TotalPrice);
                }
                WriteLine("Sales Tax: {0:C}", TotalTax);
                WriteLine("Total: {0:C}", TotalCost);
                WriteLine();
            }
        }

        public class Product
        {
            public string Name { get; set; }
            public double ShelfPrice { get; set; }
            public double TotalPrice => ShelfPrice + TaxCost;
            public bool IsTaxExempt { get; set; } // decide whether or not a product will have sales tax 
            public bool IsImportedTaxExempt { get; set; } // decide whether or not a product with have an import tax 
            public double TaxCost
            {
                get
                {
                    if (!IsTaxExempt && !IsImportedTaxExempt)
                        return (ShelfPrice * 5 + RoundFix(5)) / 100 + (ShelfPrice * 10 + RoundFix(10))/100;
                    if (!IsTaxExempt && IsImportedTaxExempt)
                        return (ShelfPrice * 10 + RoundFix(10))/100;
                    if (IsTaxExempt && !IsImportedTaxExempt)
                        return (ShelfPrice * 5 + RoundFix(5)) / 100;
                    return 0.0;
                }
            }

            public Product(string name, double shelfprice, bool isTaxExempt, bool isImportTaxExempt) // Product Constructor
            {
                Name = name;
                ShelfPrice = shelfprice;
                IsTaxExempt = isTaxExempt;
                IsImportedTaxExempt = isImportTaxExempt;
            }
            
            public double RoundFix(double n)
            {
                double remainder = (ShelfPrice * n) % 5;
                if (remainder == 0) return 0;
                return 5-remainder;
            }
        }

        public static void Main(string[] args)
        {
            // Receipt One
            new Receipt(new List<Product>{
                new Product("Book", 12.49, true, true), // Both Sales Tax and import tax exempt 
				new Product("CD", 14.99, false, true),  // only import tax exempt 
				new Product("Chocolate Bar", 0.85, true, true) // Both Sales Tax and import tax exempt 
			}).PrintReceipt();
            
            // Receipt Two
            new Receipt(new List<Product>{
                new Product("Imported Box of Chocolates", 10.00, true, false), // only sales tax exempt 
				new Product("Imported Perfume", 47.50, false, false), // Taxed both times 
			}).PrintReceipt();

            // Receipt Three
            new Receipt(new List<Product>{
                new Product("Imported Perfume", 27.99, false, false), // taxed both times 
				new Product("Perfume", 18.99, false, true), // sales tax only 
				new Product("Headache Pills", 9.75, true, true), // tax exempt 
				new Product("Imported Box of Chocolates", 11.25, true, false) // sales tax exempt 
			}).PrintReceipt();
            
        }
    }
}