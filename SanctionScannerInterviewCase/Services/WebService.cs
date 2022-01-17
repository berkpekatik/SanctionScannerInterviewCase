using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SanctionScannerInterviewCase.Service
{
    public class WebService
    {
        private WebClient webClient = new WebClient(); //Webclient'i çağırıyoruz.
        private string siteUrl = "https://www.sahibinden.com/";
        public WebService()
        {
            webClient.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; " +
                                     "Windows NT 5.2; .NET CLR 1.0.3705;)"); //Websitenin bizi bloklamaması için UserAgent atıyoruz.

        }
        public string DownloadData(string path)
        {
            if (path != null)
            {
                try
                {
                    return webClient.DownloadString(siteUrl + path);//Olmayan bir ilana gönderilmesi durumda yapılacaklar işlemler.
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return webClient.DownloadString(siteUrl);
        }
    }
}
