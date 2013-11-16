using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBalance.Domain.Entities
{

    /// <summary>
    /// Class containing some configuration of Organization/POS
    /// </summary>
    public class OrganizationConfiguration
    {

        public int Id { get; set; }

        public int OrganizationId { get; set; }

        public string EshopUri { get; set; }

        /// <summary>
        /// Whether is possible to retrieve photos of product from Eshop
        /// </summary>
        public bool IsPhotosFromEshop { get { return EshopUri != null; } }

        public string PhotosUri { get; set; }

        /// <summary>
        /// Whether is possible to get product HTML information to display in popup
        /// </summary>
        public bool IsProductInfoFromEshop { get { return PhotosUri != null; } }

    }
}