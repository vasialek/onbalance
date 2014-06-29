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
            var pi = new ParsedItem();
            return pi;
        }
    }
}
