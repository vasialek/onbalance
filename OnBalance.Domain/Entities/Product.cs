using System;
using System.Collections.Generic;
using System.Linq;

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

    }
}
