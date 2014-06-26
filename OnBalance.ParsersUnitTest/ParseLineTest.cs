using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnBalance.Parsers;
using OnBalance.Parsers.Parsers;

namespace OnBalance.ParsersUnitTest
{

    [TestClass]
    public class ParseLineTest
    {
        private IBalanceParser _gjExcelParser;

        [TestInitialize]
        public void Init()
        {
            _gjExcelParser = new GjExcelParser();
        }

        [TestMethod]
        public void Test_Parse_Price()
        {
            string s = "G 75901		20	106																						160						M	M	L	L	L	L	XXS	XS	XS	XS	XS	S	S	S	S	M	M	M	M";

            ParsedItem p = _gjExcelParser.ParseLine(s);

            Assert.AreEqual(106m, p.Price, "Price is not good");
        }

    }
}
