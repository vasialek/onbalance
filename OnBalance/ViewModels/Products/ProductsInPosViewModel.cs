using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnBalance.Models;

namespace OnBalance.ViewModels.Products
{
    public class ProductsInPosViewModel
    {
        /// <summary>
        /// Shop we are listing
        /// </summary>
        public Pos Pos { get; set; }

        public IList<Product> Products { get; set; }

        public IList<Pos> Shops { get; set; }
    }
}
