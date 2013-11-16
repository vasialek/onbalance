using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnBalance.Domain.Entities;

namespace OnBalance.ViewModels.Products
{
    public class ProductsInPosViewModel
    {
        /// <summary>
        /// Shop we are listing
        /// </summary>
        public Organization Pos { get; set; }

        public IList<Product> Products { get; set; }

        public IList<Category> Categories { get; set; }

        public IList<Organization> Organizations { get; set; }

        public ProductsInPosViewModel()
        {
            Products = new List<Product>();
            Categories = new List<Category>();
            Organizations = new List<Organization>();
        }
    }
}
