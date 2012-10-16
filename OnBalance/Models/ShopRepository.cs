using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBalance.Models
{
    public class ShopRepository
    {

        public IList<Shop> Shops
        {
            get
            {
                return new List<Shop> 
                {
                    new Shop{ Name = "GJ Sportland.com", UserId = "GJ" }
                };
            }
        }

    }
}
