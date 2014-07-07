using OnBalance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBalance.ViewModels.Products
{

    public class ProductsByCategoryViewModel
    {
        public IList<ProductsAndSizesViewModel> ProductsByCategories { get; private set; }

        public ProductsByCategoryViewModel()
        {
            ProductsByCategories = new List<ProductsAndSizesViewModel>();
        }

        public ProductsByCategoryViewModel(IList<OnBalance.Domain.Entities.Product> products)
        {
            var productsByCategories = new List<ProductsAndSizesViewModel>();
            foreach (var p in products)
            {
                var c = productsByCategories.FirstOrDefault(x => x.CategoryId == p.CategoryId);
                if (c == null)
                {
                    productsByCategories.Add(AddNewCategory(p));
                }
                else
                {
                    c.Products.Add(new Product(p));
                    foreach (var s in p.ProductDetails)
                    {
                        AddSizeName(c, s.ParameterValue);
                    }
                }
            }

            this.ProductsByCategories = productsByCategories;
        }

        private ProductsAndSizesViewModel AddNewCategory(Domain.Entities.Product p)
        {
            var c = new ProductsAndSizesViewModel();
            c.CategoryId = p.CategoryId;
            
            c.Sizes = new List<ProductSizeQuantity>();
            foreach (var s in p.ProductDetails)
            {
                AddSizeName(c, s.ParameterValue);
            }
            
            c.Products = new List<Product>();
            c.Products.Add(new Product(p));

            return c;
        }

        private void AddSizeName(ProductsAndSizesViewModel productSizes, string sizeName)
        {
            var size = productSizes.Sizes.FirstOrDefault(x => x.SizeName.Equals(sizeName, StringComparison.InvariantCultureIgnoreCase));
            if (size == null)
            {
                // Create new size with one product
                productSizes.Sizes.Add(new ProductSizeQuantity { SizeName = sizeName, Quantity = 1 });
            }
        }

    }

    public class ProductsAndSizesViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public IList<ProductSizeQuantity> Sizes { get; set; }
        public IList<Product> Products { get; set; }

        public ProductsAndSizesViewModel()
        {
            Sizes = new List<ProductSizeQuantity>();
            Products = new List<Product>();
        }
    }
}
