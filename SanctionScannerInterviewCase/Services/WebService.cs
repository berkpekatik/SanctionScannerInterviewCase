using RestSharp;
using SanctionScannerInterviewCase.Constants;
using SanctionScannerInterviewCase.Interfaces;
using System;
using System.Net;

namespace SanctionScannerInterviewCase.Service
{
    public class WebService : IWebService
    {
        private RestClient client = null;

        public string DownloadData(string path = null)
        {
            client = new RestClient(Config.SiteUrl + path);
            client.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)";
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            return response.Content;
        }
    }
}
