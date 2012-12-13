using System.Linq;
using System.Collections.Generic;

namespace OnBalance.Models
{
    partial class Product
    {

        public IList<ProductDetail> Details
        {
            get
            {
                if( ProductDetails == null )
                {
                    return new List<ProductDetail>();
                }
                return ProductDetails.ToList<ProductDetail>();
            }
        }

        public string[] DetailsNames
        {
            get
            {
                string[] names = null;
                int total = ProductDetails == null ? 0 : ProductDetails.Count;

                if(total > 0)
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

        public Dictionary<string, int> GetQuantityForAllSizes()
        {
            Dictionary<string, int> ar = new Dictionary<string, int>();

            foreach(var size in ProductRepository.GetAvailableNames("size"))
            {
                ar[size] = GetQuantityForSize(size);
            }

            return ar;
        }

        public decimal GetPriceForSize(string size)
        {
            decimal price = 0m;
            var p = ProductDetails.SingleOrDefault(x => x.product_id == this.id && x.parameter_name.Equals("size") && x.parameter_value.Equals(size));
            if( p != null )
            {
                price = p.price_release_minor;
            }

            return price;
        }

        public int GetQuantityForSize(string size)
        {
            //Log.DebugFormat("Searching for product #{0} quantity for size {1}...", this.id, size);
            int qnt = 0;
            var p = ProductDetails.SingleOrDefault(x => x.product_id == this.id && x.parameter_name.Equals("size") && x.parameter_value.Equals(size));
            if(p != null)
            {
                qnt = p.quantity;
            }

            //Log.DebugFormat("Got quantity (probably): {0}", qnt);
            return qnt;
        }
    }
}
