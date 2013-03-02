using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBalance.Models
{
    partial class ProductDetail
    {
        partial void OnCreated()
        {
            this.updated_at = DateTime.Now;
            this.created_at = DateTime.Now;
        }

        partial void OnidChanged()
        {
            this.updated_at = DateTime.Now;
        }
    }

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
            Dictionary<string, int> ar = new Dictionary<string, int>();
            var sizes = new ProductRepository().GetAvailableSizes(this.category_id);
            sizes.ToList().ForEach(x => ar.Add(x, 0));
            return ar;
        }

    }
}