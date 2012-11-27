using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq.Mapping;

namespace OnBalance.Models
{
    /// <summary>
    /// Class to represent item for exchange between server-client (sync-ing)
    /// </summary>
    [Table(Name = "balance_item")]
    public class BalanceItem
    {
        [Column(Name = "id", IsDbGenerated = true, IsPrimaryKey = true)]
        public int Id { get; set; }

        [Column(Name = "status_id")]
        public byte StatusId { get; set; }

        public Status Status
        {
            get
            {
                switch(StatusId)
                {
                    case (byte)Status.Approved:
                        return Status.Approved;
                    case (byte)Status.Completed:
                        return Status.Completed;
                    case (byte)Status.Deleted:
                        return Status.Deleted;
                }
                return Status.Unknown;
            }

            set { StatusId = (byte)value; }
        }

        /// <summary>
        /// Id of PointOfService where product is
        /// </summary>
        [Column(Name = "pos_id")]
        public int PosId { get; set; }

        /// <summary>
        /// Code which seller use in his shop
        /// </summary>
        [Column(Name = "internal_code")]
        public string InternalCode { get; set; }

        [Column(Name = "product_name")]
        public string ProductName { get; set; }

        [Column(Name = "quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Price of item (not for selling)
        /// </summary>
        [Column(Name = "price")]
        public decimal Price { get; set; }

        /// <summary>
        /// Price for sell in shop
        /// </summary>
        public decimal PriceOfRelease { get; set; }

        [Column(Name = "is_new")]
        public char _DbFieldIsNew
        {
            get{ return IsNew ? 'Y' : 'N'; }
            set { IsNew = value == 'Y'; }
        }

        public bool IsNew { get; set; }

        /// <summary>
        /// Gets/sets whether changes from local should be synced with remote (shop, POS)
        /// </summary>
        public bool HasLocalChanges { get; set; }

        /// <summary>
        /// Gets/sets whether product was changed on remote side (shop, POS) and need be updated
        /// </summary>
        public bool HasRemoteChanges { get; set; }

        //public BalanceItem()
        //{
        //    IsNew = true;
        //    HasLocalChanges = false;
        //    HasRemoteChanges = false;
        //}
    }
}
