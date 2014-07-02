using System;
using System.Linq;
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
            _gjExcelParser = new GjExcelParser(new ObLog4NetLogger(typeof(ParseLineTest).Name));
        }

        [TestMethod]
        public void Test_Parse_Price()
        {
            string s = "G 75901		20	106																						160						M	M	L	L	L	L	XXS	XS	XS	XS	XS	S	S	S	S	M	M	M	M";

            ParsedItem p = _gjExcelParser.ParseLine(s);

            Assert.AreEqual(106m, p.Price, "Price is not good");
        }

        [TestMethod]
        public void Test_Parse_ProductName_And_Code_Together()
        {
            string s = "super skate G 05415		3	133					44	44,5		41					42	43							220																									";

            ParsedItem pi = _gjExcelParser.ParseLine(s);
            string expectedName = "super skate";
            Assert.AreEqual(expectedName, pi.ProductName, "Name is `" + pi.ProductName + "`expected to be: " + expectedName);

            expectedName = "G 05415";
            Assert.AreEqual(expectedName, pi.InternalCode, "Internal code is `" + pi.InternalCode + "`expected to be: " + expectedName);
        }

        [TestMethod]
        public void Test_Product_Sizes()
        {
            string s = "plimcana clean	D65623	31	152		41	41	42	42	43	43	43	43	43	43			44	44	44	44	44,5	44,5	40,5	240		44,5	44,,5	44;5				45	45	45	45			46	46	46;5	46;5		47	48		48,5	48,5		";

            ParsedItem pi = _gjExcelParser.ParseLine(s);

            Assert.AreEqual(32, pi.CalculateTotalQuantity(), "Expected 32 sizes, got: " + pi.CalculateTotalQuantity());
            ItemSizeQuantity size = pi.Sizes.First(x => x.Size == "43");
            Assert.AreEqual(6, size.Quantity, "Expected to 6 products with size 43. Got: " + size.Quantity);
        }
    }
}
