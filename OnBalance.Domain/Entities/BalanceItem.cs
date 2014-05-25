using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using OnBalance.Domain.Primitives;

namespace OnBalance.Domain.Entities
{
    /// <summary>
    /// Class to represent item for exchange between server-client (sync-ing)
    /// </summary>
    [Table("vasialek_onbalance_user.balance_item")]
    public class BalanceItem : BaseModel
    {
        //[Column("id", IsDbGenerated = true, IsPrimaryKey = true)]
        public int Id { get; set; }

        [Column("status_id")]
        public new byte StatusId { get; set; }

        /// <summary>
        /// Gets status as enum value rather than byte
        /// </summary>
        [NotMapped]
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

        [NotMapped]
        public string StatusName { get { return Status.ToString(); } }

        /// <summary>
        /// Id of PointOfService where product is
        /// </summary>
        [Column("pos_id")]
        public int PosId { get; set; }

        /// <summary>
        /// Code which seller use in his shop
        /// </summary>
        [Column("internal_code")]
        public string InternalCode { get; set; }

        [Column("product_name")]
        public string ProductName { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Price of item (not for selling)
        /// </summary>
        [Column("price")]
        public decimal Price { get; set; }

        /// <summary>
        /// Price for sell in shop
        /// </summary>
        [NotMapped]
        public decimal PriceOfRelease { get; set; }

        [Column("is_new")]
        public char _DbFieldIsNew
        {
            get{ return IsNew ? 'Y' : 'N'; }
            set { IsNew = value == 'Y'; }
        }

        [NotMapped]
        public bool IsNew { get; set; }
    }
}
