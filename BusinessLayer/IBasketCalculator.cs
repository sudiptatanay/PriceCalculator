using System;
using System.Collections.Generic;

namespace BusinessLayer
{
    interface IBasketCalculator
    {
        List<Product> GetProductDetails(string path);
        List<Product> GetProductAddCart(List<string> prods , List<Product> products);
        List<CalculatedProduct> GetCalculatedProducts(List<Product> chkoutProducts);
    }
}
