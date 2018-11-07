using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace POSTerminal
{
    class Validator
    {
        public static string ValidateItemCode(string message, List<Product> menu)
        {
            bool valid = false;
            Console.Write(message);
            string productCode = Console.ReadLine().ToUpper().Trim();
            if(productCode == "0")
            {
                return productCode;
            }
            while (!valid)
            {
                foreach (Product product in menu)
                {
                    if (product.ProductCode == productCode)
                    {
                        valid = true;
                    }
                }

                if (!valid)
                {
                    Console.Write("Invalid selection. " + message);
                    productCode = Console.ReadLine().ToUpper().Trim();
                }

            }

            return productCode;
        }

        public static int ValidateQuantity(string message)
        {
            Console.Write(message);
            int quantity;
            while (!int.TryParse(Console.ReadLine(), out quantity) || quantity < 1)
            {
                Console.Write("Invalid input. " + message);
            }

            return quantity;
        }

        public static bool ValidateYesNo(string message)
        {
            Console.Write(message);
            string answer = Console.ReadLine().ToLower().Trim();
            while(answer != "yes" && answer != "y" && answer != "no" && answer != "n")
            {
                Console.Write("Invalid input. " + message);
                answer = Console.ReadLine().ToLower().Trim();
            }
            if(answer == "yes" || answer == "y")
            {
                return true;
            }
            return false;
        }

        public static int ValidateChoice(string message, int min, int max)
        {
            int choice;
            while(!int.TryParse(Console.ReadLine(), out choice) || choice < min || choice > max)
            {
                Console.Write("Invalid selection. " + message);
            }
            return choice;
        }

        public static double ValidateCashPayment(string message, double minPaymentNeeded)
        {
            Console.Write(message);
            double payment;
            while(!double.TryParse(Console.ReadLine(), out payment) || payment < minPaymentNeeded)
            {
                if(payment == 0)
                {
                    return 0;
                }
                Console.WriteLine("Invalid input. Please provide sufficient funds.");
                Console.WriteLine(message);
            }
            return payment;
        }

        public static double ValidateCheckPayment(string message, double paymentNeeded)
        {
            Console.Write(message);
            double payment;
            while (!double.TryParse(Console.ReadLine(), out payment) || payment != paymentNeeded)
            {
                if(payment == 0)
                {
                    return payment;
                }
                Console.WriteLine("Invalid input. Please provide funds equal to total.");
                Console.Write(message);
            }
            return payment;
        }

        public static string ValidateRoutingNumber(string message)
        {
            Console.Write(message);
            Regex pattern = new Regex(@"^\d{9}$");
            string routingNumber = Console.ReadLine().Trim();
            if(routingNumber == "0")
            {
                return routingNumber;
            }
            while(!pattern.IsMatch(routingNumber))
            {
                Console.Write("Invalid Routing Number. "+message);
                routingNumber = Console.ReadLine().Trim();
            }

            return routingNumber;    
        }

        public static string ValidateAccountCheckNumber(string message)
        {
            Console.Write(message);
            Regex pattern = new Regex(@"^\d+$");
            string number = Console.ReadLine().Trim();
            if(number == "0")
            {
                return number;
            }
            while (!pattern.IsMatch(number))
            {
                Console.Write("Invalid Number. " + message);
                number = Console.ReadLine().Trim();
            }

            return number;
        }

        public static string ValidateCreditCardNumber(string message)
        {
            Console.Write(message);
            Regex pattern = new Regex(@"^\d{8,19}$");
            string creditCardNumber = Console.ReadLine().Trim();
            if(creditCardNumber == "0")
            {
                return creditCardNumber;
            }
            while (!pattern.IsMatch(creditCardNumber))
            {
                Console.Write("Invalid Credit Card Number. " + message);
                creditCardNumber = Console.ReadLine().Trim();
            }

            return creditCardNumber;
        }

        public static string ValidateExpirationDate(string message)

        {
            Console.Write(message);
            bool validDate = false;
            DateTime date = new DateTime();
            Regex pattern = new Regex(@"^\d{2}[/][1-9][0-9]$");
            string expirationDate = Console.ReadLine().Trim();
            while(!validDate)
            {
                while(!pattern.IsMatch(expirationDate))
                {
                    if(expirationDate == "0")
                    {
                        return expirationDate;
                    }
                    Console.Write("Invalid date. " + message);
                    expirationDate = Console.ReadLine().Trim();
                
}

                expirationDate = expirationDate.Substring(0, 3) + "01/20" + expirationDate.Substring(3, 2);
                try
                {
                    date = DateTime.Parse(expirationDate);                   
                }
                catch
                {
                    continue;
                }
                DateTime tempDate = DateTime.Today.AddMonths(-1);
                DateTime testDate = new DateTime(tempDate.Year, tempDate.Month, DateTime.DaysInMonth(tempDate.Year, tempDate.Month));
                if(date <= testDate)
                {
                    expirationDate = "";
                    continue;
                }
                validDate = true;
            }
            return expirationDate;

        }

        public static string ValidateCVVNumber(string message)
        {
            Console.Write(message);
            Regex pattern = new Regex(@"^\d{3}$");
            string number = Console.ReadLine().Trim();
            if(number == "0")
            {
                return number;
            }
            while (!pattern.IsMatch(number))
            {
                Console.Write("Invalid Number. " + message);
                number = Console.ReadLine().Trim();
            }

            return number;

        }


    }
}
