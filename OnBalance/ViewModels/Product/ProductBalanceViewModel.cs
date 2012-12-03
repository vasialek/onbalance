using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnBalance.Models;

namespace OnBalance.ViewModels.Product
{
    public class ProductBalanceViewModel
    {

        public IList<Models.Product> Products { get; set; }

        public IList<Pos> Shops { get; set; }
    }
}
