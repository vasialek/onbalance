using OnBalance.Parsers.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnBalance.ParsersUnitTest.Mocked
{
    public class GjExcelParserTst : GjExcelParserShoes
    {
        public bool IsEmptyLineTest(string[] cells)
        {
            return IsEmptyLine(cells);
        }
    }
}
