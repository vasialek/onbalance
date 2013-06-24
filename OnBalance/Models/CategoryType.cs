using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBalance.Models
{
    public class CategoryType
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string Text { get; set; }
    }

    public class CategoryTypeRepository
    {

        public IList<CategoryType> Items
        {
            get
            {
                return new List<CategoryType>
                {
                    new CategoryType{ Id = 1, Name = "Ordinal product", Text = "Simple product - price, quantity and name" },
                    new CategoryType{ Id = 2, Name = "Extended product", Text = "Product with one dimension - name and list of price/quantity divided by parameter (size, color, etc)" },
                    new CategoryType{ Id = 3, Name = "Bidimensional product", Text = "Product with two dimensions - (size & color, size & weigth" },
                    new CategoryType{ Id = 4, Name = "Multidimensional product", Text = "Complex product - name and several list of price/quantity divided by parameter" }
                };
            }
        }

    }

}
