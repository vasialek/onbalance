using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBalance.Models
{
    public class ProductNew
    {
        public int PosId { get; set; }
        public string PosName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string InternalCode { get; set; }
        public string ProductName { get; set; }
        public string PriceStr { get; set; }
        public string PriceReleaseStr { get; set; }
        public int ProductId { get; set; }
        public int TotalSizes { get; set; }
    }
}
