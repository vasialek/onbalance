using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnBalance.Parsers.Parsers
{
    public class GjExcelParser : IBalanceParser
    {
        protected const int QNT_FIELD = 2;
        protected const int PRICE_FIELD = 3;

        public IList<ParsedItem> ParseFileContent(string[] lines)
        {
            throw new NotImplementedException();
        }

        public ParsedItem ParseLine(string s)
        {
            try
            {
                ParsedItem pi = new ParsedItem();
                string[] ar = s.Split(new char[] { '\t' });
                string sizeName;

                for(int i = 0; i < ar.Length; i++)
                {
                    if(IsCodeAndProductNameField(i))
                    {
                        ParseCodeAndProductName(pi, ar[i].Trim());
                    } else if(IsQuantityField(i))
                    {
                        pi.Quantity = ParseInt(ar[i], "Quantity");
                    }else if(IsPriceField(i))
                    {
                        pi.Price = ParseDecimal(ar[i], "Price");
                    }else if(IsPriceOfReleaseField(i))
                    {
                        pi.PriceOfRelease = ParseDecimal(ar[i].Trim(), "Price of release");
                    } else
                    {
                        sizeName = ar[i].Trim();
                        if(string.IsNullOrEmpty(sizeName) == false)
                        {
                            Console.WriteLine("Index: {0} = {1}", i, ar[i]);
                            pi.AddSize(sizeName); 
                        }
                    }
                }

                return pi;
            } catch(Exception ex)
            {
                throw;
            }
        }

        private bool IsCodeAndProductNameField(int i)
        {
            return i == 0;
        }

        private bool IsQuantityField(int i)
        {
            return i == 2;
        }

        private bool IsPriceField(int i)
        {
            return i == 3;
        }

        private bool IsPriceOfReleaseField(int i)
        {
            return i == 25;
        }

        private void ParseCodeAndProductName(ParsedItem pi, string s)
        {
            if(string.IsNullOrEmpty(s))
            {
                throw new ArgumentNullException("Product code/name field is empty");
            }

            string[] ar = s.Split(new char[] { ' ' });
            if(ar == null)
            {
                throw new ArgumentException("No separator (space) between product name and code: " + s);
            }

            pi.ProductName = ar[0].Trim();
            pi.InternalCode = string.Join(" ", ar, 1, ar.Length - 1);
        }

        private int ParseInt(string s, string fieldName)
        {
            if(string.IsNullOrEmpty(s))
            {
                throw new ArgumentNullException("`" + fieldName + "` field is empty");
            }
            int v;
            if(int.TryParse(s, out v) == false)
            {
                throw new InvalidCastException("Could not parse `" + fieldName + "` from: " + s);
            }
            return v;
        }

        private decimal ParseDecimal(string s, string fieldName)
        {
            if(string.IsNullOrEmpty(s))
            {
                throw new ArgumentNullException("`" + fieldName + "` field is empty");
            }
            decimal v;
            if(decimal.TryParse(s, out v) == false)
            {
                throw new InvalidCastException("Could not parse `" + fieldName + "` from: " + s);
            }
            return v;
        }
    }
}
