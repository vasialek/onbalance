using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBalance.Models
{

    /// <summary>
    /// Class containing some configuration of Organization/POS
    /// </summary>
    public class OrganizationConfig
    {

        public Uri EshopUri { get { return new Uri("http://www.gjsportland.com/"); } }

        /// <summary>
        /// Whether is possible to retrieve photos of product from Eshop
        /// </summary>
        public bool IsPhotosFromEshop { get { return true; } }

        public Uri PhotosUri
        {
            get
            {
                return new Uri("http://gjsportland.com/test.php/lt/image/bordered/f/d1122297a5defc3a12c4c59485d25d6e.png/type/centered/w/445/h/285");
            }
        }

        /// <summary>
        /// Whether is possible to get product HTML information to display in popup
        /// </summary>
        public bool IsProductInfoFromEshop { get { return true; } }

    }
}