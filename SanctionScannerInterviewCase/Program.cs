using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SanctionScannerInterviewCase.App_Start;
using SanctionScannerInterviewCase.Interfaces;
using SanctionScannerInterviewCase.Models;
using SanctionScannerInterviewCase.Service;
using SanctionScannerInterviewCase.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace SanctionScannerInterviewCase
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var container = Startup.ConfigureService();
            var _webService = container.GetRequiredService<IWebService>();
            var _dataProccesorService = container.GetRequiredService<IDataProccesorService>();
            var _loggerService = container.GetRequiredService<ILoggerService>();

            var detailPageList = new List<DetailPageModel>();

            var homePageHtml = _webService.DownloadData();
            if (homePageHtml != null)
            {
                var homePageShowCase = _dataProccesorService.BuildMainPage(homePageHtml);
                foreach (var item in homePageShowCase)
                {
                    var detailPageHtml = _webService.DownloadData(item.Url);
                    var detailPage = _dataProccesorService.BuildDetailPage(detailPageHtml);
                    if (detailPage != null)
                    {
                        detailPageList.Add(detailPage);
                    }
                }
            }

            decimal avgPrice = 0m;
            foreach (var item in detailPageList)
            {
                Console.WriteLine($"{item.Title} {item.Price} {item.Currency}");
                avgPrice += item.Price;
            }
            var countDetailPage = detailPageList.Count(x => x != null);
            var calcAvg = avgPrice / countDetailPage;
            _loggerService.Log($"Ortalama:  {calcAvg}");

            var txtModel = detailPageList.Where(x => x != null).Select(x => new
            {
                Title = x.Title,
                Price = x.Price + " " + x.Currency
            });
            _loggerService.ExternalLog(txtModel);
        }

    }
}
