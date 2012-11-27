using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBalance
{
    public class MyMessages
    {

        public const string Login = "Login";

        public class Parser
        {
            public const string LineIsTooShortFmt = "Line #%NR% is to short (%LENGTH%)!";

            public const string SplittedLineHasTooLessArgumentsFmt = "Line #%NR%  does not contain enough information!";

            public const string CouldNotParseCodeForLineFmt = "Could not parse code of product for line #%NR%";
        }

        public class Errors
        {
            public const string DataIsEmpty = "Data is empty!";
        }

    }
}