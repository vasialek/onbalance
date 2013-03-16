using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace OnBalance.Helpers
{
    public static class PasswordHelper
    {

        public static string Generate(int length, bool useSpecialSymbols)
        {
            // Without letter O and digit 0!
            string simpleSymbols = "_abcdefghijklmnpqrstuvwxyz123456789ABCDEFGHIJKLMNPQRSTUVWXYZ";
            string specialSymbols = "~!@#$%^&*-+=| /?";

            char[] ar = useSpecialSymbols ? string.Concat(simpleSymbols, specialSymbols).ToCharArray() : simpleSymbols.ToCharArray();
            StringBuilder password = new StringBuilder(length);
            Random r = new Random((int)DateTime.Now.Ticks);

            for(int i = 0; i < length; i++)
            {
                password[i] = ar[r.Next(ar.Length - 1)];
            }
            return password.ToString();
        }

    }
}
