using System;
using System.Collections.Generic;
using System.Linq;
using OnBalance.Domain.Primitives;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OnBalance.Domain.Entities
{

    [Table("vasialek_onbalance_user.product")]
    public class Product
    {
        public int Id { get; set; }

        [Column("status_id")]
        public byte StatusId { get; set; }

        [Column("pos_id")]
        public int PosId { get; set; }

        [Column("internal_code")]
        public string InternalCode { get; set; }

        [Column("uid")]
        public string Uid { get; set; }

        [Column("user_id")]
        public string UserId { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("price")]
        public decimal Price { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        [Column("category_id")]
        public int CategoryId { get; set; }

        [Column("data_json")]
        public string DataJson { get; set; }

        private IList<ProductDetail> _productDetails = null;
        public IList<ProductDetail> ProductDetails
        {
            get
            {
                if (_productDetails == null)
                {
                    //_productDetails = new OnBalance.Domain.Concrete.EfProductRepository().GetDetailsByProduct(Id);
                    _productDetails = new List<ProductDetail>();
                }
                return _productDetails;
            }
        }
        //public ICollection<ProductDetail> ProductDetails { get; set; }

        //public Organization Pos { get { return new Organization(); } }

        //public string PhotosUri { get { return ""; } }

        //public string StatusName
        //{
        //    get
        //    {
        //        IQueryable<Status> statuses = Enum.GetValues(typeof(Status)).AsQueryable().Cast<Status>();
        //        string name = statuses.SingleOrDefault(x => (int)x == StatusId).ToString();
        //        return name ?? StatusId.ToString();
        //    }
        //}

        public IEnumerable<KeyValuePair<object, object>> GetQuantityForAllSizes()
        {
            throw new NotImplementedException();
        }
    }
}
