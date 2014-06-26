using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnBalance.Parsers
{

    public class ItemSizeQuantity
    {
        public string Size { get; set; }
        public int Quantity { get; set; }
    }

    public class ParsedItem
    {

        public bool IsOk { get; set; }

        /// <summary>
        /// In parsed file
        /// </summary>
        public int LineNr { get; set; }

        public string CategoryName { get; set; }

        public string ProductName { get; set; }

        public string InternalCode { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal PriceOfRelease { get; set; }

    }
}
