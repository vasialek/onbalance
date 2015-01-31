using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnBalance.Parsers
{

    public interface IBalanceParser
    {
        IList<BalanceParseError> Errors { get; }
        int TotalProcessedLines { get; }
        int TotalProcessedNonEmptyLines { get; }
        int TotalCategoryLines { get; }
        IList<ParsedItem> ParseFileContent(string[] lines);
        ParsedItem ParseLine(string s);
        void ValidateLine(string[] cells);
        bool AllowEmptyPrice { get; set; }
    }

    public class BalanceParseError
    {
        public int LineNr { get; set; }
        
        public string Line { get; set; }
        
        public string Error { get; set; }
        
        /// <summary>
        /// Line contains only TABs or spaces
        /// </summary>
        public bool IsLineEmpty { get; set; }
        
        public Exception Exception { get; set; }

        public BalanceParseError(int lineNr, string line, string error, Exception ex)
            : this(lineNr, line, error, false, ex)
        { }

        public BalanceParseError(int lineNr, string line, string error, bool isEmptyLine, Exception ex)
        {
            if (lineNr < 0)
            {
                throw new ArgumentOutOfRangeException("Line number could not be less than 0. Passed: " + lineNr);
            }
            LineNr = lineNr;
            Line = line;
            Error = error;
            Exception = ex;
            IsLineEmpty = isEmptyLine;
        }
    }

    public class BalanceCellException : Exception
    {
        /// <summary>
        /// Index of invalid cell
        /// </summary>
        public int Index { get; private set; }

        /// <summary>
        /// Value of invalid cell
        /// </summary>
        public string Value { get; private set; }

        public string Error { get; private set; }

        public BalanceCellException()
            : this(-1, null, "")
        {
        }

        public BalanceCellException(string msg)
            : this(-1, null, msg)
        {
        }

        public BalanceCellException(int index, string value, string error)
        {
            Index = index;
            Value = value;
            Error = error;
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
                    return new Parsers.GjExcelParserShoes();
            }
            throw new NotImplementedException("No Parser is defined for type: " + type);
        }
    }
}
