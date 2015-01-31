using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnBalance.Parsers.Parsers
{
    public class GjExcelParserDress : GjExcelParserShoes, IBalanceParser
    {

        public override void ValidateLine(string[] cells)
        {
            base.ValidateLine(cells);

            // Deny code and product name, dress has only code
            if (String.IsNullOrWhiteSpace(cells[0]) == false && String.IsNullOrWhiteSpace(cells[1]) == false)
            {
                throw new BalanceCellException("Line of dress contains both product name and code"/* + cells[0] + " {TAB} " + cells[1]*/);
            }
        }

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
