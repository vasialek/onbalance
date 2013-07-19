using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OnBalance.Models;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace OnBalance.Import
{
    public class ImportLgf : IPosImport
    {

        protected string _url = "http://www.lgf.lt/index.php?1565547090";
        //protected string _url = "D:\\temp\\lgf.htm";

        protected DateTime _documentDownloadedAt = DateTime.MinValue;
        protected HtmlDocument _document = null;

        protected bool _isPosUpdated = false;

        protected List<BalanceItem> _loaded = new List<BalanceItem>();

        public void GetData()
        {
            var htmlWeb = new HtmlWeb();
            _document = htmlWeb.Load(_url);
        }

        public void GetData(int ttlS)
        {
            
        }

        public IList<BalanceItem> GetNewProducts()
        {
            if(_document != null)
            {
                ParseData(_document);
                return _loaded;
            }

            return null;
        }

        public bool IsPosUpdated()
        {
            return _isPosUpdated;
        }

        public DateTime GetPosUpdatedAt()
        {
            DateTime updated = DateTime.MinValue;
            GetData();
            if(_document != null)
            {
                updated = ExtractDateOfUpdate(_document);
            }
            return updated;
        }

        protected DateTime ExtractDateOfUpdate(HtmlDocument _document)
        {
            HtmlNode dateNode = _document.DocumentNode.SelectNodes("//h3").First(x => x.InnerText.IndexOf("Realizacijos skyrius") != -1);
            // 4 digits, separator, 2 digits, separator, 2 digits
            Regex rx = new Regex(@"(\d{4})(\S)(\d\d)(\S)(\d\d)");
            Match m = rx.Match(dateNode.InnerText);
            if(m.Success)
            {
                return DateTime.Parse(m.Value);
            }
            return DateTime.MinValue;
        }

        private void ParseData(HtmlDocument document)
        {
            string[] ar;

            string tempName = "", tempCode, tempPrice;
            var td = document.DocumentNode.SelectNodes("//table/tbody/tr/td/p").FirstOrDefault(x => x.InnerText.IndexOf("kodas ir pavadinimas") > 0);

            _loaded.Clear();
            if(td != null)
            {
                //Log4cs.Log("Got header of guns list...");
                var tbody = td.ParentNode.ParentNode.ParentNode;
                if(tbody != null)
                {
                    var trs = tbody.SelectNodes(".//tr");
                    //Log4cs.Log("Got {0} rows in guns table", trs == null ? "NO" : trs.Count.ToString());
                    // Skip first row - header
                    for(int i = 1; i < trs.Count; i++)
                    {
                        var tds = trs[i].SelectNodes(".//td");
                        if(tds.Count > 2)
                        {
                            try
                            {
                                BalanceItem bi = new BalanceItem();

                                tempName = tds[0].InnerText.Replace("&nbsp;", "").Trim();
                                tempCode = tds[1].InnerText.Replace("&nbsp;", "").Trim();
                                tempPrice = tds[2].InnerText.Replace("&nbsp;", "").Trim();
                                //Log4cs.Log("Name: {0}, price: {1}, code: {2}", tempName, tempPrice, tempCode);
                                ar = tempName.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                try
                                {
                                    bi.Quantity = 1;
                                    bi.InternalCode = tempCode;
                                    bi.ProductName = tempName;
                                    //g.TypeOfGun = GetTypeByName(ar[1]);
                                    //g.Name = tempName;
                                    //g.Code = tempCode;
                                } catch(KeyNotFoundException)
                                {
                                    //g = ParseLineHarder(tempName);
                                    //if(g == null)
                                    //{
                                    //    sbIncorrectLines.AppendLine(tempName);
                                    //    throw;
                                    //}
                                }

                                bi.Price = decimal.Parse(tempPrice);

                                _loaded.Add(bi);
                                //AddOrUpdate(g);
                            } catch(Exception ex)
                            {
                                //Log4cs.Log(Importance.Error, "Error parsing and saving Gun object!");
                                //Log4cs.Log(Importance.Debug, ex.ToString());
                            }
                        }
                    }
                }
            }

        }

        //private object GetTypeByName(string p)
        //{
        //    throw new NotImplementedException();
        //}



        public bool CheckNewProducts()
        {
            return false;
        }
    }
}
