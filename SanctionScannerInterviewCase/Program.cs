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
            var webService = new WebService();//Servisler çağırılıyor.
            var dataProccesorService = new DataProccesorService();

            var detailPageList = new List<DetailPageModel>();//Ürün sayfası için istenilen model oluşturuluyor.

            var homePageHtml = webService.DownloadData(string.Empty);//Anasayfa verileri indiriliyor.
            if (homePageHtml != null)
            {
                var homePageShowCase = dataProccesorService.BuildMainPage(homePageHtml);//Anasayfa vitrini oluşturuluyor.
                foreach (var item in homePageShowCase)
                {
                    var detailPageHtml = webService.DownloadData(item.Url);
                    var detailPage = dataProccesorService.BuildDetailPage(detailPageHtml);
                    if (detailPage != null)
                    {
                        detailPageList.Add(detailPage);//Ürün sayfasının detaylarını oluşturuluyor.
                        Thread.Sleep(10000);
                    }
                }
            }

            decimal avgPrice = 0m;//Ortalama fiyat için değişken
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
            });//İstenilen .TXT dosyası için fiyatlar ve isim çıktısı alınıyor
            File.WriteAllText(Environment.CurrentDirectory + @"\data.txt", JsonConvert.SerializeObject(txtModel)); //ve TXT dosyası oluşturuluyor.
        }
    }
}
