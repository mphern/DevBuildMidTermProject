using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSTerminal
{
    class Validator
    {
        public static string ValidateItemCode(string message, List<Product> menu)
        {
            bool valid = false;
            Console.Write(message);
            string productCode = Console.ReadLine().ToUpper();
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
                    productCode = Console.ReadLine().ToUpper();
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
            Console.Write(message);
            int choice;
            while(!int.TryParse(Console.ReadLine(), out choice) || choice < min || choice > max)
            {
                Console.Write("Invalid selection. " + message);
            }
            return choice;
        }

    }
}
