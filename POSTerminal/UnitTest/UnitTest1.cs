using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using POSTerminal;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace UnitTest
{

    [TestClass]
    public class UnitTest1
    {
        Product product = new Product()
        {
            Price = 1.99,
            ProductCata = "Fry Item",
            ProductCode = "F3",
            ProductDesc = "4.50 ounce serving of delicious yummy fries",
            ProductName = "Large Fry"
        };      

        public static string AdjustedValidateItemCode(List<Product> menu, string userInput)
        {
            bool valid = false;
            while (!valid)
            {
                foreach (Product product in menu)
                {
                    if (product.ProductCode == userInput)
                    {
                        valid = true;
                    }
                }

                if (!valid)
                {
                    return "Wrong";
                }

            }

            return userInput;
        }

        public static int AdjustedValidateQuantity(string userInput)
        {
            int quantity;
            while (!int.TryParse(userInput, out quantity) || quantity < 1)
            {
                return -1;
            }

            return quantity;
        }



        [TestMethod]
        public void Validate_ItemCode_ReturnsTrue()
        {
            List<Product> menu = new List<Product>() { product };
            string result = AdjustedValidateItemCode(menu, "F3");
            string result2 = AdjustedValidateItemCode(menu, "S4");

            Assert.AreEqual("F3", result);

            Assert.AreEqual("Wrong", result2);
        }

        [TestMethod]
        public void Validate_Quantity_ReturnsValidQuantity()
        {
            int result = AdjustedValidateQuantity("2");
            int result2 = AdjustedValidateQuantity("a");
            int result3 = AdjustedValidateQuantity("-4");

            Assert.AreEqual(2, result);

            Assert.AreEqual(-1, result2);

            Assert.AreEqual(-1, result3);
        }
    }
}
