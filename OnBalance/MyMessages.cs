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
            public const string BackToEdit = "Back to edit";
            public const string BackToList = "Back to list";
            public const string Create = "Create";
            public const string AreYouSure = "Are you sure?!";
            public const string Reset = "Reset";
            public const string Search = "Search";
            public const string Loading = "Loading...";
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

        public class Categories
        {
            public const string ConfirmStructureReset = "Confirm structure reset and products deletion!";
        }

        public class Products
        {
            public const string PosIsNotFound = "Requested POS in not found!";
        }

        public class Errors
        {
            public const string DataIsEmpty = "Data is empty!";
            public const string NoRecordsFound = "No records found!";
            public const string AjaxLoadingError = "Error loading Ajax, please try a bit later!";
        }

    }
}