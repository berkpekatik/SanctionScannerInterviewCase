using RestSharp;
using System;
using System.Net;

namespace SanctionScannerInterviewCase.Service
{
    public class WebService
    {
        private RestClient client = null; //Webclient'i çağırıyoruz.
        private string siteUrl = "https://www.sahibinden.com/";

        public string DownloadData(string path)
        {
            client = new RestClient(siteUrl + path);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            return response.Content; //İstenilen url'de ki datayı indiriyor
        }
    }
}
