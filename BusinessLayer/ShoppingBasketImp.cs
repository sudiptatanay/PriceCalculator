using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace BusinessLayer
{
    public class ShoppingBasketImp : IBasketCalculator
    {

        /// <summary>
        /// Get All Product Details from Inventory
        /// </summary>
        /// <param name="path"></param>
        /// <returns>List of Product.</returns>
        public List<Product> GetProductDetails(string path)
        {
            List<Product> lstproductDetails = null;
            try
            {
                string productJson = string.Empty;
                using (StreamReader file = File.OpenText(path))
                {
                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        JObject jObject = (JObject)JToken.ReadFrom(reader);
                        productJson = jObject.ToString();

                        ProductDetails productDetails = JsonConvert.DeserializeObject<ProductDetails>(productJson);

                        if (productDetails.Products != null)
                        {
                            lstproductDetails = new List<Product>();

                            foreach (var item in productDetails.Products)
                            {
                                Product product = new Product();

                                product.name = item.name;
                                product.unit = item.unit;
                                product.price = item.price;
                                product.qty = item.qty;
                                product.has_offer = item.has_offer;
                                product.on_product = item.on_product;
                                product.on_qty = item.on_qty;
                                product.multiplier = item.multiplier;
                                product.adder = item.adder;

                                lstproductDetails.Add(product);
                            }
                        }

                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;
            }

            return lstproductDetails;
        }

        /// <summary>
        /// Get All Product which is being added to cart
        /// </summary>
        /// <param name="prods"></param>
        /// <param name="products"></param>
        /// <returns>List of Product.</returns>
        public List<Product> GetProductAddCart(List<string> prods, List<Product> products)
        {
            List<Product> lstProdCart = null;
            try
            {
                if (prods != null && products != null)
                {
                    lstProdCart = new List<Product>();
                    foreach (var item in prods)
                    {
                        Product prod = products.Find(x => x.name.ToLower() == item);
                        if (prod != null)
                        {
                            lstProdCart.Add(prod);
                        }
                    }

                }
            }
            catch(Exception Ex)
            {
                throw Ex;
            }
            return lstProdCart;
        }

        /// <summary>
        /// Get Calculated Information of all checkedout Products
        /// </summary>
        /// <param name="chkoutProducts"></param>
        /// <returns>List of CalculatedProduct.</returns>
        public List<CalculatedProduct> GetCalculatedProducts(List<Product> cartProducts)
        {
            List<CalculatedProduct> calculatedProducts = null;
            try
            {
                if (cartProducts != null)
                {
                    calculatedProducts = new List<CalculatedProduct>();

                    foreach (Product item in cartProducts)
                    {
                        CalculatedProduct calculatedProduct = new CalculatedProduct();
                        if (item.has_offer)
                        {
                            if (string.Equals(item.name, item.on_product)) //offer is on the item itself
                            {
                                //change the price accordingly
                                int offerInstaces = 0;
                                offerInstaces = item.qty / item.on_qty;
                                var actualPrice = item.price;
                                var afterDiscount = offerInstaces * (item.price * item.multiplier + item.adder) + (item.qty - offerInstaces) * actualPrice;

                                calculatedProduct.Name = item.name;
                                calculatedProduct.IsDiscounted = true;
                                calculatedProduct.TotalAmount = Convert.ToDecimal(item.qty * item.price);
                                calculatedProduct.Multiplier = item.multiplier;
                                calculatedProduct.DiscountAmount = calculatedProduct.TotalAmount - Convert.ToDecimal(afterDiscount);
                            }
                            else  //offer is on another item
                            {
                                //check if the offer is valid or not
                                // do this by checking in the other required items are in the cart
                                string onProduct = item.on_product;
                                int onQty = item.on_qty;
                                bool isOfferValid = false;
                                int offerInstaces = 0;
                                float afterDiscount = 0;
                                foreach (Product offerItem in cartProducts)
                                {
                                    if (offerItem.name.Contains(onProduct) && (offerItem.qty >= onQty))
                                    {
                                        isOfferValid = true;
                                        offerInstaces = offerItem.qty / onQty;
                                        break;
                                    }
                                }
                                // if offer is valid
                                if (isOfferValid)
                                {
                                    var actualPrice = item.price;
                                    afterDiscount = offerInstaces * (actualPrice * item.multiplier + item.adder) + (item.qty - offerInstaces) * actualPrice;
                                }
                                else
                                {
                                    item.price *= item.qty;
                                }

                                calculatedProduct.Name = item.name;
                                calculatedProduct.IsDiscounted = (isOfferValid) ? true : false;
                                calculatedProduct.TotalAmount = Convert.ToDecimal(item.qty * item.price);
                                calculatedProduct.Multiplier = item.multiplier;
                                calculatedProduct.DiscountAmount = (isOfferValid) ? calculatedProduct.TotalAmount - Convert.ToDecimal(afterDiscount) : 0;

                            }
                            calculatedProducts.Add(calculatedProduct);
                        }
                        else
                        {
                            calculatedProduct.Name = item.name;
                            calculatedProduct.IsDiscounted = false;
                            calculatedProduct.TotalAmount = Convert.ToDecimal(item.qty * item.price);
                            calculatedProduct.DiscountAmount = 0;
                            calculatedProduct.Multiplier = item.multiplier;

                            calculatedProducts.Add(calculatedProduct);
                        }
                    }
                }
            }
            catch(Exception Ex)
            {
                throw Ex;
            }

            return calculatedProducts;
        }
    }
}
