using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBalance.Models
{
    public class OrganizationConfigurationRepository
    {

        private List<OrganizationConfiguration> _items = new List<OrganizationConfiguration>
        {
            new OrganizationConfiguration{ OrganizationId = 500000, EshopUri = "http://www.gjsportland.com/", PhotosUri = "http://gjsportland.com/%CULTURE%/balance/getphoto/uid/%UID%" },
            new OrganizationConfiguration{ OrganizationId = 500014, EshopUri = "http://eshop.interateitis.lt/public_html/test.php", PhotosUri = "http://eshop.interateitis.lt/public_html/test.php/%CULTURE%/balance/getphoto/uid/%UID%" },
            new OrganizationConfiguration{ OrganizationId = 500017, EshopUri = "http://www.nerpigiau.lt/", PhotosUri = "http://www.nerpigiau.lt/%CULTURE%/balance/getphoto/uid/%UID%" }
        };

        public IList<OrganizationConfiguration> Items { get { return _items; } }
    }
}
