using System;
using System.Linq;
using BusinessLayer;
using System.Threading;
using System.Collections.Generic;

namespace shoppingbasket
{
    class Program
    {
        static void Main(string[] args)
        {

            List<string> argParse = new List<string>();
            // parse command line args
            foreach (var item in args)
            {
                try
                {
                    argParse.Add(item.ToString().ToLower());
                }
                catch (Exception Ex)
                {
                    Console.WriteLine(Ex.ToString()+"Issue with argument passed");
                }
            }

            try
            {
                string pathInfo = Environment.CurrentDirectory + "\\Inventory\\" + "Inventory.json";
                ShoppingBasketImp shoppingBasketImp = new ShoppingBasketImp();
                // read product
                // read the JSON file and put in in the list called lstproducts
                List<Product> lstproducts = shoppingBasketImp.GetProductDetails(pathInfo);
                // add valid items to cart
                if (argParse != null && argParse.Count > 0)
                {
                    List<Product> lstCartProducts = shoppingBasketImp.GetProductAddCart(argParse, lstproducts);

                    if (lstCartProducts.Count > 0)
                    {
                        List<Product> lstCart = new List<Product>();
                        foreach (Product item in lstCartProducts)
                        {
                            Console.Write(item.unit.ToString() + " of " + item.name + ": ");
                            string qty = Console.ReadLine();
                            item.qty = int.Parse(qty);

                            lstCart.Add(item);
                        }
                        //process cart
                        //process items one by one and add to checkout
                        List<CalculatedProduct> calculatedProducts = shoppingBasketImp.GetCalculatedProducts(lstCart);
                        var calculatedProduct = calculatedProducts.FindAll(x => x.IsDiscounted);
                        decimal total = calculatedProducts.Select(x => x.TotalAmount).Sum();
                        // Print Calculated amount 
                        if (calculatedProduct != null)
                        {
                            decimal discount = calculatedProducts.Select(x => x.DiscountAmount).Sum();
                            Console.WriteLine("SubTotal :£" + total);
                            foreach (var item in calculatedProducts)
                            {
                                if (item.IsDiscounted)
                                    Console.WriteLine(item.Name + (1 - item.Multiplier) * 100 + "  % off :- " + item.DiscountAmount);
                            }
                            Console.WriteLine("Total Price:£" + (total - discount));
                        }
                        else
                        {

                            Console.WriteLine("SubTotal :£" + total);
                            Console.WriteLine("(No offers available)");
                            Console.WriteLine(" Total Price:£" + total);
                        }
                    }

                    Thread.Sleep(50000);

                }
            }
            catch (Exception ex)
            {
                ExceptionLogging.SendErrorToText(ex);
            }

        }
    }
}
