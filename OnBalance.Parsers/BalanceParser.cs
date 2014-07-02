using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnBalance.Parsers
{

    public interface IBalanceParser
    {
        IList<BalanceParseError> Errors { get; }
        IList<ParsedItem> ParseFileContent(string[] lines);
        ParsedItem ParseLine(string s);
    }

    public class BalanceParseError
    {
        public int LineNr { get; set; }
        public string Line { get; set; }
        public string Error { get; set; }
        public Exception Exception { get; set; }

        public BalanceParseError(int lineNr, string line, string error, Exception ex)
        {
            if (lineNr < 0)
            {
                throw new ArgumentOutOfRangeException("Line number could not be less than 0. Passed: " + lineNr);
            }
            LineNr = lineNr;
            Line = line;
            Error = error;
            Exception = ex;
        }
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
