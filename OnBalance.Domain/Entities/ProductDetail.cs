using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace OnBalance.Domain.Entities
{
    [Table("vasialek_onbalance_user.product_detail")]
    public class ProductDetail
    {
        public int Id { get; set; }

        [Column("status_id")]
        public byte StatusId { get; set; }

        //[ForeignKey("Product")]
        [Column("product_id")]
        public int ProductId { get; set; }

        //[ForeignKey("ProductId")]
        //public virtual Product Product { get; set; }

        [Column("parameter_name")]
        public string ParameterName { get; set; }

        [Column("parameter_value")]
        public string ParameterValue { get; set; }

        [Column("price_minor")]
        public decimal PriceMinor { get; set; }

        [Column("price_release_minor")]
        public decimal PriceReleaseMinor { get; set; }

        [Column("quantity")]
        public int Quantity { get; set; }

        [Column("data_json")]
        public string DataJson { get; set; }

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

    }
}
