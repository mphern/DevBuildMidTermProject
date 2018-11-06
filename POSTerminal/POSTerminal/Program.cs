using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace POSTerminal
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("                      Welcome to Hernski's!\n");
            List<string> customerOrder = new List<string>();
            List<double> orderPrices = new List<double>();
            bool orderDone = false;
            while (!orderDone)
            {
                List<string> choices = new List<string>() { "1. Look at Menu", "2. Create an order", "3. Display current order and order total", "4. Pay for order" };
                string currentDirectory = Directory.GetCurrentDirectory();
                DirectoryInfo directory = new DirectoryInfo(currentDirectory);
                var fileName = Path.Combine(directory.FullName, "FoodMenu.csv");
                List<Product> menu = ReadMenu(fileName);
                DisplayChoices(choices);
                int choice = Validator.ValidateChoice("What would you like to do? (Enter 1-" + choices.Count + "): ", 1, choices.Count);

                if(choice == 1)
                {
                    DisplayMenu(menu);
                }

                else if(choice == 2)
                {
                    GetCustomerOrder(menu, ref customerOrder, ref orderPrices);
                }

                else if(choice == 3)
                {
                    DisplayOrder(customerOrder, orderPrices);
                }

                else if(choice == 4)
                {
                    PayForOrder(customerOrder, orderPrices);
                    orderDone = true;
                }

                else
                {
                    break;
                }
 
            }

            Console.ReadKey();

        }

        public static List<Product> ReadMenu(string fileName)
        {

            List<Product> menu = new List<Product>();

            using (var reader = new StreamReader(fileName))
            {
                string line = "";
                reader.ReadLine();
                while ((line = reader.ReadLine()) != null)
                {
                    Product product = new Product();
                    string[] values = line.Split(';');

                    product.ProductName = values[0];
                    double price;
                    if (double.TryParse(values[1], out price))
                    {
                        product.Price = price;
                    }
                    product.ProductCata = values[2];
                    product.ProductDesc = values[3];
                    product.ProductCode = values[4];

                    menu.Add(product);
                }
            }

            return menu;
        }

        public static void DisplayMenu(List<Product> menu)
        {
            List<string> fries = new List<string>();
            List<string> burgers = new List<string>();
            List<string> drinks = new List<string>();
            List<string> meals = new List<string>();
            List<string> desserts = new List<string>();

            foreach(Product product in menu)
            {
                if(product.ProductCata == "Fry Item")
                {
                    fries.Add("("+ product.ProductCode+ ") " + product.ProductName + " - " + product.Price);
                }
                if (product.ProductCata == "Drink Item")
                {
                    drinks.Add("(" + product.ProductCode + ") " + product.ProductName + " - " + product.Price);
                }
                if (product.ProductCata == "Burger Item")
                {
                    burgers.Add("(" + product.ProductCode + ") " + product.ProductName + " - " + product.Price);
                }
                if (product.ProductCata == "Meal")
                {
                    meals.Add("(" + product.ProductCode + ") " + product.ProductName + " - " + product.Price);
                }
                if (product.ProductCata == "Dessert Item")
                {
                    desserts.Add("(" + product.ProductCode + ") " + product.ProductName + " - " + product.Price);
                }

            }


            Console.WriteLine("\n                             MENU                               ");
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("           Fries                          Burgers           ");
            Console.WriteLine("      ~~~~~~~~~~~~~~~~~~~~~~         ~~~~~~~~~~~~~~~~~~~~~~~~");
            for (int i = 0; i < Math.Max(fries.Count, burgers.Count); i++)
            {
                try
                {
                    Console.WriteLine(string.Format("{0,-5} {1,-30} {2,0}","", fries[i], burgers[i]));
                }
                catch
                {
                    try
                    {
                        Console.WriteLine(string.Format("{0,-5} {1,0}", "", fries[i]));
                    }
                    catch
                    {
                        Console.WriteLine(string.Format("{0, -35} {1,0}", "", burgers[i]));
                    }

                }

            }
            Console.WriteLine("\n");
            Console.WriteLine("          Drinks                         Meals             ");
            Console.WriteLine("      ~~~~~~~~~~~~~~~~~~~~~~~~       ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            for (int i = 0; i < Math.Max(drinks.Count, meals.Count); i++)
            {
                try
                {
                    Console.WriteLine(string.Format("{0,-5} {1,-30} {2,0}", "", drinks[i], meals[i]));
                }
                catch
                {
                    try
                    {
                        Console.WriteLine(string.Format("{0,-5} {1,0}", "", drinks[i]));
                    }
                    catch
                    {
                        Console.WriteLine(string.Format("{0, -35} {1,0}", "", meals[i]));
                    }

                }

            }
            Console.WriteLine("\n");
            Console.WriteLine("          Desserts");
            Console.WriteLine("      ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            for(int i = 0; i < desserts.Count; i++)
            {
                Console.WriteLine(string.Format("{0,-5} {1,0}", "", desserts[i]));
            }



        }

        public static void DisplayChoices(List<string> choices)
        {
            Console.Write("\n");
            foreach(string choice in choices)
            {
                Console.WriteLine(choice);
            }
        }

        public static void GetCustomerOrder(List<Product> menu, ref List<string> customerOrder, ref List<double> orderTotal)
        {
            bool orderMore = true;
            while (orderMore)
            {
                Console.Clear();
                DisplayMenu(menu);
                string productCode = Validator.ValidateItemCode("\nPlease provide Item Code for the item you would like to order (i.e. F1, B2, etc.): ", menu);
                int quantity = Validator.ValidateQuantity("How many of this item would you like to order? ");
                foreach (Product product in menu)
                {
                    if (product.ProductCode == productCode)
                    {
                        for (int i = 0; i < quantity; i++)
                        {
                            customerOrder.Add(product.ProductName);
                            orderTotal.Add(product.Price);
                        }
                        Console.WriteLine($"\n{quantity} {product.ProductName}{(quantity > 1 ? "s" : "")} {(quantity > 1 ? "have" : "has")} been added to your order.\n");
                        break;
                    }
       
                }

                orderMore = Validator.ValidateYesNo("Would you like to order more? (y/n): ");
            }
            Console.Clear();

        }

        public static void DisplayOrder(List<string> order, List<double> prices)
        {
            Console.Clear();
            int count = 1;
            double tax = Math.Round(prices.Sum() * 0.06, 2);
            string taxString = string.Format("{0:0.00}", tax);
            double total = prices.Sum();
            string totalString = string.Format("{0:0.00}", total);
            double grandTotal = prices.Sum() + tax;
            string grandTotalString = string.Format("{0:0.00}", grandTotal);
            if (order.Count == 0)
            {
                Console.WriteLine("***You currently have no items in your order.***");
            }
            else
            {
                foreach (string item in order)
                {
                    Console.WriteLine(string.Format("{0,0}.  {1, -25} ${2,0}", count, item, prices[count - 1]));
                    count++;
                }
                Console.WriteLine("====================================");
                Console.WriteLine(string.Format("{0,-3} {1, -25} ${2, 0}", "", "Subtotal:", totalString));
                Console.WriteLine("====================================");
                Console.WriteLine(string.Format("{0,-3} {1, -25} ${2, 0}", "", "Sales Tax", taxString));
                Console.WriteLine("====================================");
                Console.WriteLine("{0,-3} {1,-25} ${2,0}", "", "Grand Total:", grandTotalString);
            }
        }

        public static void PayForOrder(List<string> order, List<double> prices)
        {
            Console.WriteLine("\nHow would you like to pay:");
            Console.WriteLine("1. Cash");
            Console.WriteLine("2. Credit");
            Console.WriteLine("3. Check");
            int choice = Validator.ValidateChoice("Enter choice (1-3): ", 1, 3);
            if(choice == 1)
            {
                PayWithCash(order, prices);
            }

        }

        public static void PayWithCash(List<string> order, List<double> prices)
        {
            double totalPaymentNeeded = prices.Sum() * 1.06;
            DisplayOrder(order, prices);
            double payment = Validator.ValidateCashPayment("\nEnter amount tendered: ", totalPaymentNeeded);
            if(payment == totalPaymentNeeded)
            {
                Console.WriteLine("Thank you! Enjoy your meal.");
            }
            else
            {
                Console.WriteLine("\n$" + Math.Round(payment - totalPaymentNeeded, 2) + " is your change. Thank you! Enjoy your meal.");
            }
      
        }


    }
}
