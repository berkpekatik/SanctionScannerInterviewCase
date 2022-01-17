using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SanctionScannerInterviewCase.Constants
{
    public class Config
    {
        public const string SiteUrl = "https://www.sahibinden.com/";
        public const string RegexSpaces = "[\\r|\\n|\\t]\\s\\s+";
        public const string RegexHtml = @"<[^>]+>|&nbsp;";
        public const string RegexNumber = @"[^0-9.]";
        public const string RegexLetter = @"[^A-Z]";
    }
}
