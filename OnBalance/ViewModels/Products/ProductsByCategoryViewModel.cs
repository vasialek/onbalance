using OnBalance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace OnBalance.ViewModels.Products
{

    public class ProductsByCategoryViewModel
    {
        public int PosId { get; set; }

        public bool CouldViewPrice { get; set; }

        public IList<ProductsAndSizesViewModel> ProductsByCategories { get; private set; }

        /// <summary>
        /// Decorations (background) for products
        /// </summary>
        public IList<ProductDecoratorColor> Decorators
        {
            get
            {
                return new List<ProductDecoratorColor>
                {
                    new ProductDecoratorColor { ProductId = 100000, SizeName = "100", BackgroundColor = "#a00" },
                    new ProductDecoratorColor { ProductId = 100000, SizeName = "115", BackgroundColor = "#0a0" },
                };
            }
        }
        //{ get; private set; }

        public string GetDecoratorsAsJs(string jsArrayName = "xxx")
        {
            var sb = new StringBuilder();

            sb.AppendFormat("var {0} = [];", jsArrayName).AppendLine();
            foreach (var pd in Decorators)
            {
                sb.AppendFormat("{0}[{0}.length] = {1};", jsArrayName, pd.ToJs())
                    .AppendLine();
            }

            return sb.ToString();
        }

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
            
            c._sizes = new List<ProductSizeQuantity>();
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
            var size = productSizes._sizes.FirstOrDefault(x => x.SizeName.Equals(sizeName, StringComparison.InvariantCultureIgnoreCase));
            if (size == null)
            {
                // Create new size with one product
                productSizes._sizes.Add(new ProductSizeQuantity { SizeName = sizeName, Quantity = 1 });
            }
        }

    }

    public class ProductsAndSizesViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public IList<ProductSizeQuantity> _sizes { get; set; }
        public ProductSizeQuantity[] SizesOrdered { get { return _sizes == null ? new ProductSizeQuantity[0] : _sizes.OrderBy(x => x.SizeName).ToArray(); } }
        public IList<Product> Products { get; set; }

        public ProductsAndSizesViewModel()
        {
            _sizes = new List<ProductSizeQuantity>();
            Products = new List<Product>();
        }
    }
}
