using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBalance.Models
{
    partial class Product
    {

        public string StatusName
        {
            get
            {
                return ((Status)status_id).ToString();
            }
        }


        public IList<ProductDetail> Details
        {
            get
            {
                if(ProductDetails == null)
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
            return new Dictionary<string, int>();
        }

    }
}