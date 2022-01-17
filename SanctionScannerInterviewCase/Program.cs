using Newtonsoft.Json;
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

            decimal avgPrice = 0m;
            foreach (var item in detailPageList)
            {
                Console.WriteLine($"{item.Title} {item.Price} {item.Currency}");
                avgPrice += item.Price;
            }
            Console.WriteLine($"Ortalama: {avgPrice / detailPageList.Count(x => x != null)}");

            var txtModel = detailPageList.Where(x => x != null).Select(x => new
            {
                Title = x.Title,
                Price = x.Price + " " + x.Currency
            });
            File.WriteAllText(Environment.CurrentDirectory + @"\data.txt", JsonConvert.SerializeObject(txtModel));
        }
    }
}
