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
            _gjExcelParser = new GjExcelParserShoes(new ObLog4NetLogger(typeof(ParseLineTest).Name));
        }

        [TestMethod]
        public void Test_Parse_Price()
        {
            string s = "G 75901		20	106																						160						M	M	L	L	L	L	XXS	XS	XS	XS	XS	S	S	S	S	M	M	M	M";

            ParsedItem p = _gjExcelParser.ParseLine(s);

            Assert.AreEqual(106m, p.Price, "Price is not good");
        }

        [TestMethod]
        public void Test_Parse_Price_With_Lt()
        {
            string s = "essence G 96433		5	135				42	42		42,5					44,5	44,5								220,00 Lt";

            var p = _gjExcelParser.ParseLine(s);

            Assert.AreEqual(220m, p.PriceOfRelease, "Expected price of release to be 220.00. Got: " + p.PriceOfRelease);
        }

        [TestMethod]
        public void Test_Parse_Price_With_Dot_Separator()
        {
            string s = "team feather G 56837		0	140.52";

            var p = _gjExcelParser.ParseLine(s);

            Assert.AreEqual(140.52m, p.Price, "Expected price to be 140.52. Got: " + p.Price);
        }

        [TestMethod]
        public void Test_Parse_Price_With_Lt_No_Space()
        {
            string s = "Kalisto 385716-104		1	164,5lt			43";

            var p = _gjExcelParser.ParseLine(s);

            Assert.AreEqual(164.50m, p.Price, "Expected price to be 164.50. Got: " + p.Price);
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

        [TestMethod]
        public void Test_InternalCode_Is_Only_Digits()
        {
            string s = "terex 043980		1	170							36,5		38												240																									";

            ParsedItem pi = _gjExcelParser.ParseLine(s);

            string expS;
            decimal expD;

            expS = "terex";
            Assert.AreEqual(expS, pi.ProductName, "Expected name: " + expS + ", got: " + pi.ProductName);
            expS = "043980";
            Assert.AreEqual(expS, pi.InternalCode, "Expected internal code: " + expS + ", got: " + pi.InternalCode);
            expD = 170m;
            Assert.AreEqual(expD, pi.Price, "Expected price: " + expD + ", got: " + pi.Price);
            expD = 240;
            Assert.AreEqual(expD, pi.PriceOfRelease, "Expected price of release: " + expD + ", got: " + pi.PriceOfRelease);
        }

        [TestMethod]
        public void Test_InternalCode_Is_Dash_Digits()
        {
            string s = "NK 429716-104		1	144,49				42,5									48,5								260																													";

            ParsedItem pi = _gjExcelParser.ParseLine(s);

            string expS;
            decimal expD;

            expS = "NK";
            Assert.AreEqual(expS, pi.ProductName, "Expected name: " + expS + ", got: " + pi.ProductName);
            expS = "429716-104";
            Assert.AreEqual(expS, pi.InternalCode, "Expected internal code: " + expS + ", got: " + pi.InternalCode);
            expD = 144.49m;
            Assert.AreEqual(expD, pi.Price, "Expected price: " + expD + ", got: " + pi.Price);
            expD = 260;
            Assert.AreEqual(expD, pi.PriceOfRelease, "Expected price of release: " + expD + ", got: " + pi.PriceOfRelease);
        }

        [TestMethod]
        public void Test_Only_Code_Is_Present()
        {
            string s = "488160-203		57	49				7	7	7	7		8	8";

            ParsedItem pi = _gjExcelParser.ParseLine(s);

            Assert.AreEqual("488160-203", pi.InternalCode, "Expected InternalCode to be 488160-203");
        }

        [TestMethod]
        public void Test_Empty_Quantity()
        {
            string s = "44      			67																						120																																																					";

            var pi = _gjExcelParser.ParseLine(s);

            Assert.AreEqual(0, pi.Quantity, "Expected Quantity to be 0. Got: " + pi.Quantity);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Test_Line_Of_Empty_Fields()
        {
            string s = "																									";

            var pi = _gjExcelParser.ParseLine(s);
        }
    }
}
