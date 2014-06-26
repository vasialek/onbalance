using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnBalance.Parsers
{

    public interface IBalanceParser
    {
        IList<ParsedItem> ParseFileContent(string[] lines);
        ParsedItem ParseLine(string s);
    }

    public class BalanceParser
    {
        public enum ParserType { GjExcel }

        public static IBalanceParser Get(ParserType type)
        {
            switch (type)
            {
                case ParserType.GjExcel:
                    return new Parsers.GjExcelParser();
            }
            throw new NotImplementedException("No Parser is defined for type: " + type);
        }
    }
}
