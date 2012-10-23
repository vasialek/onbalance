using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBalance.Models
{
    /// <summary>
    /// Class to represent item for exchange between server-client (sync-ing)
    /// </summary>
    public class ExchangeItem
    {

        /// <summary>
        /// Code which seller use in his shop
        /// </summary>
        public string InternalCode { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        /// <summary>
        /// Price of item (not for selling)
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Price for sell in shop
        /// </summary>
        public decimal PriceOfRelease { get; set; }

    }
}
