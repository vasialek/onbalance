using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnBalance.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Display(Name = "Status")]
        public byte StatusId { get; set; }

        [Display(Name = "Point of Service")]
        public int PosId { get; set; }

        [Display(Name = "Code used in POS")]
        public string InternalCode { get; set; }

        [Display(Name = "Unique code in OBS")]
        public string Uid { get; set; }

        [Display(Name = "Product owner")]
        public string UserId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        [Display(Name = "Created at")]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Product category")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        [Display(Name = "Product details")]
        public IList<ProductDetail> ProductDetails { get; set; }

        public OnBalance.Domain.Entities.Organization Pos { get; set; }

        public string PhotosUri { get; set; }

        public string StatusName { get { return StatusId.ToString(); } }
    }
}