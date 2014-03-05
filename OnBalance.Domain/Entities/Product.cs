using System;
using System.Collections.Generic;
using System.Linq;
using OnBalance.Domain.Primitives;
using System.ComponentModel.DataAnnotations;

namespace OnBalance.Domain.Entities
{

    public class Product
    {
        public int Id { get; set; }

        public byte StatusId { get; set; }

        public int PosId { get; set; }

        public string InternalCode { get; set; }

        public string Uid { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public DateTime CreatedAt { get; set; }

        public int CategoryId { get; set; }

        public Organization Pos { get { return new Organization(); } }

        public string PhotosUri { get { return ""; } }

        public string StatusName
        {
            get
            {
                IQueryable<Status> statuses = Enum.GetValues(typeof(Status)).AsQueryable().Cast<Status>();
                string name = statuses.SingleOrDefault(x => (int)x == StatusId).ToString();
                return name ?? StatusId.ToString();
            }
        }

        public IEnumerable<KeyValuePair<object, object>> GetQuantityForAllSizes()
        {
            throw new NotImplementedException();
        }
    }
}
