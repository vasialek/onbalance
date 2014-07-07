using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnBalance.Parsers.Parsers
{
    public class GjExcelParserDress : GjExcelParserShoes, IBalanceParser
    {

        protected override bool IsCodeAndProductNameField(int i)
        {
            // No product name
            return false;
        }

        protected override bool IsCodeField(int i)
        {
            return i == 0;
        }

        protected override bool IsPriceOfReleaseField(int i)
        {
            return i == 25;
        }

    }
}
