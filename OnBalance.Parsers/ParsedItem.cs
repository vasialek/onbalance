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

        public IList<ItemSizeQuantity> Sizes { get; set; }

        public ParsedItem()
        {
            Sizes = new List<ItemSizeQuantity>();
        }

        /// <summary>
        /// Adds 1 product to specific size.
        /// </summary>
        public void AddSize(string sizeName)
        {
            var size = Sizes.FirstOrDefault(x => x.Size.Equals(sizeName, StringComparison.InvariantCultureIgnoreCase));
            if(size == null)
            {
                // Create new size with one product
                Sizes.Add(new ItemSizeQuantity { Size = sizeName, Quantity = 1 });
            } else
            {
                // Increase quantity of existing size
                size.Quantity = size.Quantity + 1;
            }
        }

        public int CalculateTotalQuantity()
        {
            return Sizes.Sum(x => x.Quantity);
        }
    }
}
