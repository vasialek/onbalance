using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace OnBalance.ParsersUnitTest
{
    [TestClass]
    public class ParseGjDressFile
    {
        private Parsers.IBalanceParser _gjParser;

        [TestInitialize]
        public void Init()
        {
            _gjParser = new Parsers.Parsers.GjExcelParserDress();
        }

        [TestMethod]
        public void Test_Dress_ProductName_And_Code()
        {
            string s = "REE  	Z80231	3	41											M			L	XL							65";

            var pi = _gjParser.ParseLine(s);

            // Assert
            Assert.AreEqual("REE", pi.ProductName, "Expected ProductName to be `REE`. Got: " + pi.ProductName);
        }
    }
}
