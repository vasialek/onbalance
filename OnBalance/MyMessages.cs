using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnBalance
{
    public class MyMessages
    {

        public class ButtonText
        {
            public const string Login = "Login";
            public const string Save = "Save";
            public const string Back = "Back";
        }

        public class Parser
        {
            public const string LineIsTooShortFmt = "Line #%NR% is to short (%LENGTH%)!";

            public const string SplittedLineHasTooLessArgumentsFmt = "Line #%NR%  does not contain enough information!";

            public const string CouldNotParseCodeForLineFmt = "Could not parse code of product for line #%NR%";
        }

        public class Balancer
        {
            public const string NoPendingLocalChanges = "No pending changes to be send to e-shop";
        }

        public class Errors
        {
            public const string DataIsEmpty = "Data is empty!";
            public const string NoRecordsFound = "No records found!";
        }

    }
}