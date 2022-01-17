using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SanctionScannerInterviewCase.Utils
{
    public static class Extension
    {
        public static string Replace(this string text, string regex)
        {
            return Regex.Replace(text, regex, string.Empty);
        }
    }
}
