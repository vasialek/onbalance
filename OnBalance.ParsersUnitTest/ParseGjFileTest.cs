using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OnBalance.Parsers.Parsers;

namespace OnBalance.ParsersUnitTest
{
    [TestClass]
    public class ParseGjFileTest
    {
        private GjExcelParser _gjParser = null;

        [TestInitialize]
        public void Init()
        {
            _gjParser = new GjExcelParser(new ObLog4NetLogger("GjParserTest"));
        }

        [TestMethod]
        public void Test_Line_Contains_CategoryName()
        {
            string[] cells = "ADIDAS vyr.laisvalaikio																								kaina																									".Split(new char[] { '\t' });

            bool isCategory = _gjParser.IsLineCategoryName(cells);

            Assert.AreEqual(true, isCategory, "Line expected to be category name");
        }

        [TestMethod]
        public void Test_Line_Is_Not_CategoryName()
        {
            string[] cells = new string[] {
                "duramo", "F32234", "8", "103"	, "43", "43", "44", "44", "44,5", "44,5", "45", "45"
            };

            bool isCategory = _gjParser.IsLineCategoryName(cells);

            Assert.AreEqual(false, isCategory, "Line expected to be NOT a category name");
        }

        [TestMethod]
        public void Test_Parse_One_Category_Products()
        {
            string[] lines = new string[] {
                "ADIDAS vyr.laisvalaikio																								kaina																									",
                "duramo                               	F32234	8	103		43	43	44	44	44,5	44,5	45	45												170																									",
                "Adipure  	G98582	3	190		42	43	44																	300																									"
            };

            var products = _gjParser.ParseFileContent(lines);
            Assert.AreEqual(lines.Length, products.Count, "Parsed products should be: " + lines.Length);
        }

        public void Test_One_Line_Is_Bad()
        {
            string[] lines = new string[]{
                "CC a.t  120	D66143	13	125		42,5	42,5	43	43	43	43	44	44	44	44		44,5	44,5	45	45					200																																																																																																																																																																																																																					",
			    "185																					280																																																																																																																																																																																																																					",
                "PLImcana clean	D65630	18	152		41	41		42,5	43	43	43	43		44	44	44	44,5	44,5	44,5	44,5		45	45	240		46	46	107	39	39	42	42		195																																																																																																																																																																																																											",
            };

        }
    }
}
