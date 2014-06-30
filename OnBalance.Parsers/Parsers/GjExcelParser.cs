using OnBalance.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnBalance.Parsers.Parsers
{
    public class GjExcelParser : IBalanceParser
    {
        protected IObLogger _logger = null;

        public GjExcelParser()
        {
            _logger = new ObFakeLogger();
        }

        public GjExcelParser(IObLogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            _logger = logger;
        }

        public IList<ParsedItem> ParseFileContent(string[] lines)
        {
            if (lines == null || lines.Length < 1)
            {
                _logger.Error("File to parse content is NULL or empty!");
                throw new ArgumentNullException("GjExcelParser - could not parse NULL or empty file content");
            }

            string currentCategoryName = "";
            var parsed = new List<ParsedItem>();
            string[] cells;
            for (int i = 0; i < lines.Length; i++)
            {
                cells = lines[i].Split(new char[] { '\t' });
                if (IsLineCategoryName(cells))
                {
                    currentCategoryName = cells[0].Trim();
                }
                else
                {
                    var pi = ParseLine(cells);
                    if (pi != null)
                    {
                        pi.CategoryName = currentCategoryName;
                    }
                }
            }

            return parsed;
        }

        public bool IsLineCategoryName(string[] cells/*, ref string currentCategoryName*/)
        {
            if (cells == null || cells.Length < 1)
            {
                return false;
            }

            // First cell should be category name
            if (string.IsNullOrEmpty(cells[0]))
            {
                return false;
            }

            bool isEmptyOrNonNumber = true;
            decimal d;
            for (int i = 1; isEmptyOrNonNumber && i < cells.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(cells[i]) && decimal.TryParse(cells[i].Trim(), out d))
                {
                    isEmptyOrNonNumber = false;
                }
            }

            return isEmptyOrNonNumber;
        }

        public ParsedItem ParseLine(string s)
        {
            string[] ar = s.Split(new char[] { '\t' });
            return ParseLine(ar);
        }

        public ParsedItem ParseLine(string[] cells)
        {
            try
            {
                ParsedItem pi = new ParsedItem();
                string sizeName;

                for (int i = 0; i < cells.Length; i++)
                {
                    if (IsCodeAndProductNameField(i))
                    {
                        ParseCodeAndProductName(pi, cells[i].Trim());
                    }
                    else if (IsCodeField(i))
                    {
                        if (string.IsNullOrEmpty(cells[i]) == false)
                        {
                            pi.InternalCode = cells[i].Trim();
                        }
                    }
                    else if (IsQuantityField(i))
                    {
                        pi.Quantity = ParseInt(cells[i], "Quantity");
                    }
                    else if (IsPriceField(i))
                    {
                        pi.Price = ParseDecimal(cells[i], "Price");
                    }
                    else if (IsPriceOfReleaseField(i))
                    {
                        pi.PriceOfRelease = ParseDecimal(cells[i].Trim(), "Price of release");
                    }
                    else
                    {
                        sizeName = cells[i].Trim();
                        if (string.IsNullOrEmpty(sizeName) == false)
                        {
                            Console.WriteLine("Index: {0} = {1}", i, cells[i]);
                            pi.AddSize(sizeName);
                        }
                    }
                }

                return pi;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private bool IsCodeAndProductNameField(int i)
        {
            return i == 0;
        }

        private bool IsCodeField(int i)
        {
            return i == 1;
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
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentNullException("Product code/name field is empty");
            }

            string[] ar = s.Split(new char[] { ' ' });
            if (ar == null)
            {
                throw new ArgumentException("No separator (space) between product name and code: " + s);
            }

            pi.ProductName = ar[0].Trim();
            pi.InternalCode = string.Join(" ", ar, 1, ar.Length - 1);
        }

        private int ParseInt(string s, string fieldName)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentNullException("`" + fieldName + "` field is empty");
            }
            int v;
            if (int.TryParse(s, out v) == false)
            {
                throw new InvalidCastException("Could not parse `" + fieldName + "` from: " + s);
            }
            return v;
        }

        private decimal ParseDecimal(string s, string fieldName)
        {
            if (string.IsNullOrEmpty(s))
            {
                throw new ArgumentNullException("`" + fieldName + "` field is empty");
            }
            decimal v;
            if (decimal.TryParse(s, out v) == false)
            {
                throw new InvalidCastException("Could not parse `" + fieldName + "` from: " + s);
            }
            return v;
        }

    }
}
