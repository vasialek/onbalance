using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OnBalance.Models;
using OnBalance.Domain.Abstract;
using System.Text;
using Newtonsoft.Json;

namespace OnBalance.Controllers
{

    public class TestController : BaseController
    {

        class ExchangeJson
        {
            public string uid { get; set; }
            public string code { get; set; }
            public int pr { get; set; }
            public int posid { get; set; }
            public string name { get; set; }
            public Dictionary<string, string> sizes { get; set; }
        }

        private ICategoryRepository _repository = null;

        public TestController(ICategoryRepository repository)
        {
            _repository = repository;
        }

        //
        // GET: /test/

        public ActionResult Index()
        {
            _repository.Categories.ToList();
            return Content("OK");
            StringBuilder sb = new StringBuilder();
            foreach(var item in _repository.Categories.ToList())
            {
                sb.AppendLine(item.Name);
            }
            return Content(sb.ToString());
            //InfoFormat("Test/Index...");
            //BalanceItem bi = new BalanceItem();
            //return View(bi);
        }

        public ActionResult ChangeQuantity(int id)
        {
            bool status = false;
            string message = "";
            int quantity = 0;

            try
            {
                int dQnt = int.Parse(Request["dQnt"]);
                quantity += dQnt;
                status = true;
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

            return Json(new { 
                Staus = status,
                Message = message,
                Quantity = quantity
            }, JsonRequestBehavior.AllowGet);
        }

        //
        // POST: /test/index

        [HttpPost]
        public ActionResult Index(BalanceItem model)
        {
            return RedirectToAction("Index");
        }

        public ActionResult Grid()
        {
            return View();
        }

        public ActionResult Sync()
        {
            return View("Sync", Layout);
        }

        public ActionResult GetSync()
        {
            var model = new List<BalanceItem>();
            string json = "[{\"uid\":\"GJ_ES_97d79eefac074e54dc01cb28d454d76a\",\"code\":\"G61604\",\"pr\":7000,\"posid\":100001,\"name\":null,\"sizes\":{\"42\": \"2\", \"43\": \"4\", \"44\": \"6\", \"44.5\": \"4\", \"45\": \"2\", \"46\": \"2\", \"46.5\": \"2\", \"47\": \"2\"}}]";
            var exchanges = JsonConvert.DeserializeObject<List<ExchangeJson>>(json);
            model = exchanges.Select(x => new BalanceItem {
                InternalCode = x.uid,
                ProductName = string.Concat(x.code, ". ", x.name),
                Price = x.pr,
                QuantityForSizes = x.sizes
            }).ToList();
            return PartialView("", model);
        }

        public ActionResult Json()
        {
            string s = "";
            string json = "{\"uid\":\"GJ_ES_97d79eefac074e54dc01cb28d454d76a\",\"code\":\"G61604\",\"pr\":7000,\"posid\":100001,\"name\":null,\"sizes\":\"[42=2:43=4:44=6:44.5=4:45=2:46=2:46.5=2:47=2:]\"}";
            
            //json = "[{\"uid\":\"GJ_ES_97d79eefac074e54dc01cb28d454d76a\",\"code\":\"G61604\",\"pr\":7000,\"posid\":100001,\"name\":null,\"sizes\":\"[42=2:43=4:44=6:44.5=4:45=2:46=2:46.5=2:47=2:]\"},{\"uid\":\"GJ_ES_5bd2efb67b0bd94db7c7754a637c0379\",\"code\":\"G15627\",\"pr\":6000,\"posid\":100001,\"name\":null,\"sizes\":\"[42=2:43=4:44=3:44.5=3:45=1:]\"},{\"uid\":\"GJ_ES_e0ef680d0d962689be00ada040b1682b\",\"code\":\"G61595\",\"pr\":7000,\"posid\":100001,\"name\":null,\"sizes\":\"[42=1:43=2:44=1:44.5=1:45=1:46=1:]\"},{\"uid\":\"GJ_ES_556a70c1b83fc546bc807ff9f1a86812\",\"code\":\"G97553\",\"pr\":5500,\"posid\":100001,\"name\":null,\"sizes\":\"[38=2:39=1:40=1:]\"},{\"uid\":\"GJ_ES_9cc0dc69c06370d7f18b592c80dc7121\",\"code\":\"Q22307\",\"pr\":4500,\"posid\":100001,\"name\":null,\"sizes\":\"[38=2:39=1:]\"},{\"uid\":\"GJ_ES_647c89ea76763720bb56f05926b470c6\",\"code\":\"Q22306\",\"pr\":4500,\"posid\":100001,\"name\":null,\"sizes\":\"[38=2:39=3:40=2:]\"},{\"uid\":\"GJ_ES_78dbc95c33f337f483337e746360afe5\",\"code\":\"Q21834\",\"pr\":4000,\"posid\":100001,\"name\":null,\"sizes\":\"[41=2:42=1:43=3:44=1:45=3:46=1:]\"}]";
            json = "[{\"uid\":\"GJ_ES_97d79eefac074e54dc01cb28d454d76a\",\"code\":\"G61604\",\"pr\":7000,\"posid\":100001,\"name\":null,\"sizes\":{\"42\": \"2\", \"43\": \"4\", \"44\": \"6\", \"44.5\": \"4\", \"45\": \"2\", \"46\": \"2\", \"46.5\": \"2\", \"47\": \"2\"}}]";
            var list = JsonConvert.DeserializeObject<List<ExchangeJson>>(json);
            foreach (var item in list)
            {
                s += string.Format("{0}, {1}<br />", item.uid, item.code);
                foreach (var kvp in item.sizes)
                {
                    s += string.Format("&nbsp;&nbsp;{0}: {1}<br />", kvp.Key, kvp.Value);
                }
                s += "----------<br />";
            }

            //json = "{\"42\": \"2\", \"43\": \"4\", \"44\": \"6\", \"44.5\": \"4\", \"45\": \"2\", \"46\": \"2\", \"46.5\": \"2\", \"47\": \"2\"}";
            //var dict = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            //foreach (var kvp in dict)
            //{
            //    s += string.Format("{0}: {1}{2}<br />", kvp.Key, kvp.Value, Environment.NewLine);
            //}
            //return Content(s);

            //json = "{\"uid\":\"GJ_ES_97d79eefac074e54dc01cb28d454d76a\",\"code\":\"G61604\",\"pr\":7000,\"posid\":100001,\"name\":null,\"sizes\":\"[42=2:43=4:44=6:44.5=4:45=2:46=2:46.5=2:47=2:]\"}";
            //var obj = JsonConvert.DeserializeObject<ExchangeJson>(json);
            //return Content(string.Format("UID: {0}, code: {1}", obj.uid, obj.code));

            return Content(s);
        }

    }

}
