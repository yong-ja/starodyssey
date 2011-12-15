using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace AvengersUtd.Odyssey.Utils
{
    public static class Text
    {
        public static string GetCapitalLetters(string value)
        {
            Contract.Requires<ArgumentException>(!string.IsNullOrEmpty(value)); 

            StringBuilder sb = new StringBuilder();

            foreach (char c in value.Where(Char.IsUpper))
            {
                sb.Append(c);
            }

            return sb.ToString();
        }
    }
}
