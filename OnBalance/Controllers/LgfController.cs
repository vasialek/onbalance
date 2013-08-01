using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnBalance.Import;
using OnBalance.ViewModels.lgf;

namespace OnBalance.Controllers
{
    public class LgfController : BaseController
    {
        //
        // GET: /lgf/

        public ActionResult Index()
        {
            return Content("L.G.F.");
        }

        public ActionResult List(GunsFilterViewModel? model)
        {
            var lgf = new ImportLgf();
            var filter = model.HasValue ? new GunsFilterViewModel(model) : new GunsFilterViewModel();
            filter.GunsTypes = new List<string> { "", "Pistoletas", "Revolveris" };
            
            InfoFormat("LGF was updated at {0}", ViewBag.UpdatedAt);

            lgf.GetData();
            var items = lgf.GetNewProducts().OrderByDescending(x => x.Price);

            int pos = 0;
            var producers = new List<string>();
            string producerName;
            foreach(var g in items)
            {
                producerName = "";
                pos = g.ProductName.IndexOf("pistoletas", StringComparison.InvariantCultureIgnoreCase);
                if(pos >= 0)
                {
                    producerName = g.ProductName.Substring(pos + "pistoletas".Length);
                }
                else if((pos = g.ProductName.IndexOf("pistoletas", StringComparison.InvariantCultureIgnoreCase)) >= 0)
                {
                    producerName = g.ProductName.Substring(pos + "pistoletas".Length);
                }

                if((pos = producerName.IndexOf(',')) > 0)
                {
                    producerName = producerName.Substring(0, pos);
                }

                if(!string.IsNullOrEmpty(producerName) && !producers.Contains(producerName))
                {
                    producers.Add(producerName);
                }
            }

            ViewBag.UpdatedAt = lgf.GetPosUpdatedAt().ToShortDateString();
            ViewBag.Filter = filter;
            if(Request.IsAjaxRequest())
            {
                return PartialView(items);
            }
            return View(items);
        }//

    }
}
