using System;
using System.Linq;
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

        public Product()
        {

        }

        public Product(OnBalance.Domain.Entities.Product product)
        {
            Id = product.Id;
            CategoryId = product.CategoryId;
            CreatedAt = product.CreatedAt;
            InternalCode = product.InternalCode;
            Name = product.Name;
            PosId = product.PosId;
            Price = product.Price;
            StatusId = product.StatusId;
            Uid = product.Uid;
            UserId = product.UserId;
            ProductDetails = product.ProductDetails.Select(x => new OnBalance.Models.ProductDetail {
                id = x.Id,
                parameter_value = x.ParameterValue,
                quantity = x.Quantity,
                price_minor = x.PriceMinor,
                price_release_minor = x.PriceReleaseMinor,
                product_id = x.ProductId,

            }).ToList();
        }

        public int QuantityCalculated
        {
            get
            {
                return ProductDetails == null ? 0 : ProductDetails.Sum(x => x.quantity);
            }
        }

        public decimal PriceFirst { get { return ProductDetails == null ? 0m : ProductDetails.First().price_minor / 100; } }

        public decimal PriceReleaseFirst { get { return ProductDetails == null ? 0m : ProductDetails.First().price_release_minor / 100; } }
    }
}