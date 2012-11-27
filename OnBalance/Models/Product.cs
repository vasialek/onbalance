using System.Linq;
using System.Collections.Generic;

namespace OnBalance.Models
{
    partial class Product
    {

        public string[] DetailsNames
        {
            get
            {
                string[] names = null;
                int total = ProductDetails == null ? 0 : ProductDetails.Count;

                if( total > 0 )
                {
                    names = new string[total];
                    for(int i = 0; i < total; i++)
                    {
                        names[i] = ProductDetails[i].parameter_name;
                    }
                }

                return names;
            }
        }

        public decimal GetPriceForSize(string size)
        {
            decimal price = 0m;
            var p = ProductDetails.SingleOrDefault(x => x.parameter_name.Equals("size") && x.parameter_value.Equals(size));
            if( p != null )
            {
                price = p.price_release_minor;
            }

            return price;
        }

    }
}
