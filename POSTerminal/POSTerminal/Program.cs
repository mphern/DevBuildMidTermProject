﻿using System;
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
            Console.WriteLine("             WELCOME TO HERNSKI'S DINER!");
            Console.Write("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~\n");
            List<string> customerOrder = new List<string>();
            List<double> orderPrices = new List<double>();
            bool orderDone = false;
            List<string> choices = new List<string>() { "1. Look at Menu", "2. Create/Add to an Order", "3. Display Current Order and Order Total", "4. Pay for Order",
                                                        "5. Cancel/Edit Current Order", "6. Learn about the Menu Items", "7. Quit" };
            string currentDirectory = Directory.GetCurrentDirectory();
            DirectoryInfo directory = new DirectoryInfo(currentDirectory);
            var fileName = Path.Combine(directory.FullName, "FoodMenu.csv");
            List<Product> menu = ReadMenu(fileName);
            while (!orderDone)
            {
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
                    if (customerOrder.Count == 0)
                    {
                        Console.WriteLine("\nYou have not placed an order yet.");
                    }
                    else
                    {
                        DisplayOrder(customerOrder, orderPrices);
                    }
                }

                else if(choice == 4)
                {
                    if (customerOrder.Count == 0)
                    {
                        Console.WriteLine("\nYou have not placed an order yet.");
                    }
                    else
                    {
                        PayForOrder(ref customerOrder, ref orderPrices);
                    }
                }

                else if(choice == 5)
                {
                    if (customerOrder.Count == 0)
                    {
                        Console.WriteLine("\nYou have not placed an order yet.");
                    }
                    else
                    {
                        CancelEditOrder(ref customerOrder, ref orderPrices);
                    }
                }

                else if(choice == 6)
                {
                    DisplayItemDesc(menu);
                }

                else
                {
                    if(customerOrder.Count != 0)
                    {
                        Console.WriteLine("\nYou cannot leave without paying for your food.");
                    }
                    else
                    {
                        orderDone = true;
                    }
                }
 
            }

            Console.WriteLine("\nThanks for coming to Hernski's. Have a nice day. Come again!");
            Console.ReadKey();
        }

        /*This method goes through the FoodMenu.csv file line by line and creates instances of the Product class and then adds the
         * instance to the Products List variable menu which it returns.*/
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
            Console.Clear();
            List<string> fries = new List<string>();
            List<string> burgers = new List<string>();
            List<string> drinks = new List<string>();
            List<string> meals = new List<string>();
            List<string> desserts = new List<string>();

            foreach (Product product in menu)
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


            Console.WriteLine("\n                         HERNSKI'S MENU                           ");
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
            Console.WriteLine("What would you like to do?");
            foreach(string choice in choices)
            {
                Console.WriteLine(choice);
            }
            Console.Write("\n> ");
        }

        public static void GetCustomerOrder(List<Product> menu, ref List<string> customerOrder, ref List<double> orderTotal)
        {
            bool orderMore = true;
            while (orderMore)
            {
                Console.Clear();
                DisplayMenu(menu);
                string productCode = Validator.ValidateItemCode("\nPlease provide Item Code (F1, B2, etc.) for the item you would like to order or enter 0 to go back: ", menu);
                if(productCode == "0")
                {
                    return;
                }
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
                        if (product.ProductCata != "Fry Item")
                        {
                            Console.WriteLine($"\n{quantity} {product.ProductName}{(quantity > 1 ? "s" : "")} {(quantity > 1 ? "have" : "has")} been added to your order.\n");
                        }
                        else
                        {
                            string[] fryArray = product.ProductName.Split();
                            if(quantity == 1)
                            {
                                Console.WriteLine($"\n{quantity} {product.ProductName} has been added to your order.\n");
                            }
                            else
                            {
                                Console.WriteLine($"\n{quantity} {fryArray[0]} Fries have been added to your order.\n");
                            }
                        }
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
            foreach (string item in order)
            {
                Console.WriteLine(string.Format("{0,0}.  {1, -26} ${2,0}", count, item, prices[count - 1]));
                count++;
            }
            Console.WriteLine("=====================================");
            Console.WriteLine(string.Format("{0,-3} {1, -26} ${2, 0}", "", "Subtotal:", totalString));
            Console.WriteLine("=====================================");
            Console.WriteLine(string.Format("{0,-3} {1, -26} ${2, 0}", "", "Sales Tax", taxString));
            Console.WriteLine("=====================================");
            Console.WriteLine(string.Format("{0,-3} {1,-26} ${2,0}", "", "Grand Total:", grandTotalString));

        }

        public static void PayForOrder(ref List<string> order, ref List<double> prices)
        {
            Console.WriteLine("\nHow would you like to pay:");
            Console.WriteLine("1. Cash");
            Console.WriteLine("2. Credit");
            Console.WriteLine("3. Check");
            Console.WriteLine("4. Go back");
            Console.Write("> ");
            int choice = Validator.ValidateChoice("Enter choice (1-4): ", 1, 4);

            if(choice == 1)
            {
                PayWithCash(ref order, ref prices);
            }

            else if(choice == 2)
            {
                PayWithCredit(ref order, ref prices);
            }

            else if(choice == 3)
            {
                PayWithCheck(ref order, ref prices);
            }

            else
            {
                return;
            }

        }

        public static void PayWithCash(ref List<string> order, ref List<double> prices)
        {
            double totalPaymentNeeded = Math.Round(prices.Sum() * 1.06, 2);
            DisplayOrder(order, prices);
            double payment = Validator.ValidateCashPayment("\nEnter amount tendered or enter 0 to go back: ", totalPaymentNeeded);
            if(payment == 0)
            {
                return;
            }
            CashReceipt(ref order, ref prices, payment);
        }

        public static void PayWithCheck(ref List<string> order, ref List<double> prices)
        {
            double totalPaymentNeeded = Math.Round(prices.Sum() * 1.06, 2);
            DisplayOrder(order, prices);
            double payment = Validator.ValidateCheckPayment("\nPlease provide amount on check or enter 0 to go back to menu: ", totalPaymentNeeded);
            if(payment == 0)
            {
                return;
            }
            string routingNumber = Validator.ValidateRoutingNumber("Please provide 9 digit Routing Number or enter 0 to go back to menu: ");
            if(routingNumber == "0")
            {
                return;
            }
            string accountNumber = Validator.ValidateAccountCheckNumber("Please provide account number (leave out dashes(-) and spaces) or enter 0 to go back to menu: ");
            if(accountNumber == "0")
            {
                return;
            }
            string checkNumber = Validator.ValidateAccountCheckNumber("Please provide check number: ");
            if(checkNumber == "0")
            {
                return;
            }
            CheckReceipt(ref order, ref prices, checkNumber);
        }

        public static void PayWithCredit(ref List<string> order, ref List<double> prices)
        {
            double totalPaymentNeeded = Math.Round(prices.Sum() * 1.06, 2);
            DisplayOrder(order, prices);
            string creditCardNumber = Validator.ValidateCreditCardNumber("\nPlease provide Credit Card Number (no spaces) or enter 0 to go back to menu: ");
            if(creditCardNumber == "0")
            {
                return;
            }
            string date = Validator.ValidateExpirationDate("Please provide Expiration Date (mm/yy) or enter 0 to go back to menu: ");
            if(date == "0")
            {
                return;
            }
            string cvvNumber = Validator.ValidateCVVNumber("Please provide CVV number or enter 0 to go back to menu: ");
            if(cvvNumber == "0")
            {
                return;
            }
            CreditReceipt(ref order, ref prices, creditCardNumber);
        }

        public static void CashReceipt(ref List<string> order, ref List<double> prices, double payment)
        {
            double totalPaymentNeeded = Math.Round(prices.Sum() * 1.06, 2);
            DisplayOrder(order, prices);
            Console.WriteLine("{0,-3} {1,-26} ${2,0}", "", "Payment:", string.Format("{0:0.00}", payment));
            Console.WriteLine("{0,-3} {1,-26} ${2,0}", "", "Change:", string.Format("{0:0.00}", payment-totalPaymentNeeded));
            Console.WriteLine("***************RECEIPT***************");
            Console.WriteLine("\nThank you! Enjoy your meal!");
            order = new List<string>();
            prices = new List<double>();
        }

        public static void CreditReceipt(ref List<string> order, ref List<double> prices, string creditCardNumber)
        {
            double totalPaymentNeeded = Math.Round(prices.Sum() * 1.06, 2);
            string hiddenCardNumber = string.Concat(Enumerable.Repeat("*", creditCardNumber.Length - 4)) + creditCardNumber.Substring(creditCardNumber.Length - 4);
            DisplayOrder(order, prices);
            Console.WriteLine("{0,-3} {1,-26} ${2,0}", "", "Payment:", string.Format("{0:0.00}", Math.Round(prices.Sum() * 1.06, 2)));
            Console.WriteLine("{0,-3} {1,-16} {2,0}", "", "Paid w/ Card #:", hiddenCardNumber);
            Console.WriteLine("***************RECEIPT***************");
            Console.WriteLine("\nThank you! Enjoy your meal!");
            order = new List<string>();
            prices = new List<double>();

        }

        public static void CheckReceipt(ref List<string> order, ref List<double> prices, string checkNumber)
        {
            double totalPaymentNeeded = Math.Round(prices.Sum() * 1.06, 2);
            DisplayOrder(order, prices);
            Console.WriteLine("{0,-3} {1,-25}  ${2,0}", "", "Payment:", string.Format("{0:0.00}", Math.Round(prices.Sum() * 1.06, 2)));
            Console.WriteLine("{0,-3} {1,-28} {2,0}", "", "Paid w/ Check #:", checkNumber);
            Console.WriteLine("***************RECEIPT***************");
            Console.WriteLine("\nThank you! Enjoy your meal!");
            order = new List<string>();
            prices = new List<double>();

        }

        public static void CancelEditOrder(ref List<string> order, ref List<double> prices)
        {
            bool goAgain = true;
            while (goAgain)
            {
                DisplayOrder(order, prices);
                Console.Write("\nEnter number of item you would like removed or 0 to cancel entire order: ");
                int choice = Validator.ValidateChoice("\nEnter number of item you would like removed or 0 to cancel entire order: ", 0, order.Count);
                if (choice == 0)
                {
                    order = new List<string>();
                    prices = new List<double>();
                    Console.WriteLine("\nOrder has been canceled.");
                    break;
                }
                else
                {
                    string item = order[choice-1];
                    order.RemoveAt(choice-1);
                    prices.RemoveAt(choice-1);
                    Console.WriteLine("\n" + item + " has been removed from the order.");
                }
                goAgain = Validator.ValidateYesNo("\nWould you like to remove another item? (y/n): ");

            }
        }

        public static void DisplayItemDesc(List<Product> menu)
        {
            bool goAgain = true;
            while(goAgain)
            {
                Console.Clear();
                DisplayMenu(menu);
                string choice = Validator.ValidateItemCode("\nEnter Item Code (F1, B2, etc.) of Item you would like to learn about or enter 0 to go back: ", menu);
                if(choice == "0")
                {
                    return;
                }
                foreach(Product product in menu)
                {
                    if(product.ProductCode == choice)
                    {
                        Console.Write("\n");
                        Console.WriteLine(product.ProductName + " - " + product.ProductDesc);
                        break;
                    }

                }
                goAgain = Validator.ValidateYesNo("\nWould you like to learn more? (y/n): ");
            }
        }

    }
}
