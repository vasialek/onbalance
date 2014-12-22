using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Linq.Mapping;
using System.ComponentModel.DataAnnotations;
using OnBalance.Core;

namespace OnBalance.Models
{
    /// <summary>
    /// Class to represent item for exchange between server-client (sync-ing)
    /// </summary>
    public class BalanceItem : BaseModel
    {

        public int Id { get; set; }

        public new byte StatusId { get; set; }

        /// <summary>
        /// Gets status as enum value rather than byte
        /// </summary>
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
                    case (byte)Status.Pending:
                        return Status.Pending;
                    case (byte)Status.Failed:
                        return Status.Failed;
                }
                return Status.Unknown;
            }

            set { StatusId = (byte)value; }
        }

        public string StatusName { get { return Status.ToString(); } }

        /// <summary>
        /// Id of PointOfService where product is
        /// </summary>
        public int PosId { get; set; }

        /// <summary>
        /// Code which seller use in his shop
        /// </summary>
        [Display(Name = "Internal code")]
        public string InternalCode { get; set; }

        [Display(Name = "Product name")]
        public string ProductName { get; set; }

        [Display(Name = "Quantity change")]
        public int Quantity { get; set; }

        /// <summary>
        /// Price of item (not for selling)
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Price for sell in shop
        /// </summary>
        [Display(Name = "Price of release")]
        public decimal PriceOfRelease { get; set; }

        /// <summary>
        /// Gets/sets quantity for each size ( 41: 3, 45: 1 )
        /// </summary>
        public Dictionary<string, string> QuantityForSizes { get; set; }

        public bool IsNew { get; set; }

        /// <summary>
        /// Gets/sets whether changes from local should be synced with remote (shop, POS)
        /// </summary>
        //public bool HasLocalChanges { get; set; }

        /// <summary>
        /// Gets/sets whether product was changed on remote side (shop, POS) and need be updated
        /// </summary>
        public bool IsChangedLocally { get; set; }

        [Display(Name = "Size")]
        public string SizeName { get; set; }

        public BalanceItem()
        {
            QuantityForSizes = new Dictionary<string, string>();
        //    IsNew = true;
        //    HasLocalChanges = false;
        //    HasRemoteChanges = false;
        }

        public BalanceItem(OnBalance.Domain.Entities.BalanceItem x)
        {
            Id = x.Id;
            InternalCode = x.InternalCode;
            PosId = x.PosId;
            Price = x.Price;
            PriceOfRelease = x.PriceOfRelease;
            ProductName = x.ProductName;
            SizeName = x.SizeName;
            Quantity = x.Quantity;
            StatusId = x.StatusId;
            IsChangedLocally = x.ChangedFrom.Equals('L') || x.ChangedFrom.Equals('l');
        }
    }
}
