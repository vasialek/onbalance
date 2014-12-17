using OnBalance.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace OnBalance.Parsers.Parsers
{
    public class GjExcelParserShoes : IBalanceParser
    {
        protected IObLogger _logger = null;

        public GjExcelParserShoes()
        {
            _logger = new ObFakeLogger();
        }

        public GjExcelParserShoes(IObLogger logger)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            _logger = logger;
        }

        private IList<BalanceParseError> _errors = null;
        protected int _totalProcessedLines = 0;
        protected int _totalProcessedNonEmptyLines = 0;
        protected int _totalCategoryLines = 0;

        public IList<BalanceParseError> Errors { get { return _errors == null ? new List<BalanceParseError>() : _errors; } }

        public int TotalProcessedLines
        {
            get { return _totalProcessedLines; }
        }

        public int TotalProcessedNonEmptyLines
        {
            get { return _totalProcessedNonEmptyLines; }
        }

        public int TotalCategoryLines
        {
            get { return _totalCategoryLines; }
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
            _errors = new List<BalanceParseError>();

            for (int i = 0; i < lines.Length; i++)
            {
                try
                {
                    // Some statistics
                    _totalProcessedLines++;
                    if (String.IsNullOrWhiteSpace(lines[i]) == false)
                    {
                        _totalProcessedNonEmptyLines++;
                    }

                    cells = lines[i].Split(new char[] { '\t' });
                    if (IsLineCategoryName(cells))
                    {
                        currentCategoryName = cells[0].Trim();
                        _totalCategoryLines++;
                    }
                    else
                    {
                        var pi = ParseLine(cells);
                        if (pi != null)
                        {
                            pi.CategoryName = currentCategoryName;
                            parsed.Add(pi);
                        }
                        else
                        {
                            Errors.Add(new BalanceParseError(i, lines[i], "Could not parse line", null));
                        }
                    }
                }
                catch (Exception ex)
                {
                    _errors.Add(new BalanceParseError(i, lines[i], ex.Message, ex));
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
            if (string.IsNullOrEmpty(cells[0].Trim()))
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

                _logger.Debug("Parsing:");
                for (int i = 0; i < cells.Length; i++)
                {
                    _logger.DebugFormat("  cells[{0}]: {1}", i, cells[i]);
                    if (IsCodeAndProductNameField(i))
                    {
                        if (string.IsNullOrEmpty(cells[i]))
                        {
                            throw new ArgumentNullException("Product code/name field is empty");
                        }
                        pi.ProductName = cells[i].Trim();
                    }
                    else if (IsCodeField(i))
                    {
                        // If code cell is empty - extract from product name "super skate G 05415"
                        if (string.IsNullOrEmpty(cells[i]))
                        {
                            pi = ExtractInternalCodeFromName(pi);
                        }else
                        {
                            pi.InternalCode = cells[i].Trim();
                        }
                    }
                    else if (IsQuantityField(i))
                    {
                        if (String.IsNullOrEmpty(cells[i].Trim()))
                        {
                            _logger.WarnFormat("  quantity at index {0} is empty!", i);
                        }
                        else
                        {
                            pi.Quantity = ParseInt(cells[i], "Quantity");
                        }
                    }
                    else if (IsPriceField(i))
                    {
                        pi.Price = ParsePrice(cells[i], "Price");
                        //pi.Price = ParseDecimal(cells[i], "Price");
                    }
                    else if (IsPriceOfReleaseField(i))
                    {
                        if (string.IsNullOrWhiteSpace(cells[i]))
                        {
                            _logger.WarnFormat("    price of release at index {0} is empty!", i);
                        }
                        else
                        {
                            pi.PriceOfRelease = ParsePrice(cells[i].Trim(), "Price of release"); 
                            //pi.PriceOfRelease = ParseDecimal(cells[i].Trim(), "Price of release"); 
                        }
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

        // super skate G 05415
        private Regex _rxNameAndCode = new Regex(@"(\b)([A-Z])([\ ]{0,1})([\d]+)", RegexOptions.CultureInvariant | RegexOptions.Singleline);
        // NK 429716-104
        private Regex _rxNameCodeDashed = new Regex(@"(\s)(\d+)(\-)(\d+)", RegexOptions.CultureInvariant | RegexOptions.Singleline);
        // terex 043980
        private Regex _rxNameCodeDigits = new Regex(@"(\s)(\d+)", RegexOptions.CultureInvariant | RegexOptions.Singleline);
        // No NAME, internal code is in first cell: 488160-203
        private Regex _rxCodeWithDash = new Regex(@"^(\d{3,10})(\-)(\d+)$", RegexOptions.CultureInvariant | RegexOptions.Singleline);

        private Regex _rxPriceWithLtl = new Regex(@"\b(lt)(l?)\b", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Singleline);

        protected virtual ParsedItem ExtractInternalCodeFromName(ParsedItem pi)
        {
            if (pi.ProductName == null)
            {
                throw new ArgumentNullException("ProductName", "Could not extract Code from ProductName, ProductName is null");
            }

            // super skate G 05415
            // cc a.t. Q 23572
            Match m = _rxNameAndCode.Match(pi.ProductName);
            if (m.Success)
            {
                pi.InternalCode = string.Concat(m.Groups[2].Value.ToUpper(), " ", m.Groups[4].Value.Trim());
                pi.ProductName = pi.ProductName.Substring(0, m.Index).Trim();
                return pi;
            }

            // NK 429716-104
            m = _rxNameCodeDashed.Match(pi.ProductName);
            if (m.Success)
            {
                pi.InternalCode = string.Concat(m.Groups[2].Value.Trim(), "-", m.Groups[4].Value.Trim());
                pi.ProductName = pi.ProductName.Substring(0, m.Index).Trim();
                return pi;
            }

            // terex 043980
            m = _rxNameCodeDigits.Match(pi.ProductName);
            if (m.Success)
            {
                pi.InternalCode = m.Groups[2].Value.Trim();
                pi.ProductName = pi.ProductName.Substring(0, m.Index).Trim();
                return pi;
            }

            // Just code instead of name
            m = _rxCodeWithDash.Match(pi.ProductName);
            if (m.Success)
            {
                pi.InternalCode = pi.ProductName;
                pi.ProductName = "";
                return pi;
            }

            return pi;
        }

        protected virtual bool IsCodeAndProductNameField(int i)
        {
            return i == 0;
        }

        protected virtual bool IsCodeField(int i)
        {
            return i == 1;
        }

        protected virtual bool IsQuantityField(int i)
        {
            return i == 2;
        }

        protected virtual bool IsPriceField(int i)
        {
            return i == 3;
        }

        protected virtual bool IsPriceOfReleaseField(int i)
        {
            return i == 24;
        }

        //private void ParseCodeAndProductName(ParsedItem pi, string s)
        //{
        //    if (string.IsNullOrEmpty(s))
        //    {
        //        throw new ArgumentNullException("Product code/name field is empty");
        //    }

        //    string[] ar = s.Split(new char[] { ' ' });
        //    if (ar == null)
        //    {
        //        throw new ArgumentException("No separator (space) between product name and code: " + s);
        //    }

        //    pi.ProductName = ar[0].Trim();
        //    pi.InternalCode = string.Join(" ", ar, 1, ar.Length - 1);
        //}

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

        private decimal ParsePrice(string s, string fieldName)
        {
            // Strip `LT` or `Ltl` from price
            s = _rxPriceWithLtl.Replace(s, "");

            if (s.EndsWith(Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencySymbol, StringComparison.InvariantCultureIgnoreCase))
            {
                s = s.Substring(0, s.Length - Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencySymbol.Length);
            }

            // Try to fix "140.52" for LT locale
            char incorrectDecimalSeparator = Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator == "," ? '.' : ',';
            if (s.Length > 3 && s[s.Length - 3] == incorrectDecimalSeparator)
            {
                s = String.Concat(s.Substring(0, s.Length - 3), Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator, s.Substring(s.Length - 2));
            }
            return ParseDecimal(s, fieldName);
        }
    }
}
