using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBalance.ViewModels.Products
{
    public class ProductsInPosViewModel
    {
        /// <summary>
        /// Shop we are listing
        /// </summary>
        public OnBalance.Domain.Entities.Organization Pos { get; set; }

        public IList<OnBalance.Models.Product> Products { get; set; }

        public IList<OnBalance.Models.CategoryListItem> Categories { get; set; }

        public IList<OnBalance.Domain.Entities.Organization> Organizations { get; set; }

        public ProductsInPosViewModel()
        {
            Products = new List<OnBalance.Models.Product>();
            Categories = new List<OnBalance.Models.CategoryListItem>();
            Organizations = new List<OnBalance.Domain.Entities.Organization>();
        }
    }
}
