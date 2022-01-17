using SanctionScannerInterviewCase.Models;
using SanctionScannerInterviewCase.Service;
using SanctionScannerInterviewCase.Services;
using System;
using System.Collections.Generic;

namespace SanctionScannerInterviewCase
{
    class Program
    {
        static void Main(string[] args)
        {
            var webService = new WebService();
            var dataProccesorService = new DataProccesorService();

            var detailPageList = new List<DetailPageModel>();

            var homePageHtml = webService.DownloadData(string.Empty);
            if (homePageHtml != null)
            {
                var homePageShowCase = dataProccesorService.BuildMainPage(homePageHtml);
                foreach (var item in homePageShowCase)
                {
                    var detailPageHtml = webService.DownloadData(item.Url);
                    detailPageList.Add(dataProccesorService.BuildDetailPage(detailPageHtml));
                }
            }


        }
    }
}
