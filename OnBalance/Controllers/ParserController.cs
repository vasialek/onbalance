using OnBalance.Parsers;
using OnBalance.Parsers.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnBalance.Controllers
{


    public class ParserController : Controller
    {
        //
        // GET: /parser/

        public ActionResult Index()
        {
            return Edit("gj");
        }

        //
        // GET: /parser/edit

        public ActionResult Edit(string id)
        {
            return View("Edit", "_LayoutLogin");
        }

        //
        // GET: /parser/preview

        public ActionResult Preview(string id)
        {
            string data = Request["Data"];
            string[] lines = data.Split("\n".ToCharArray());
            IBalanceParser p = new GjExcelParser();
            var items = p.ParseFileContent(lines);
            return View("Preview", "_LayoutLogin", items);
        }

    }
}
