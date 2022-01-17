using HtmlAgilityPack;
using SanctionScannerInterviewCase.Constants;
using SanctionScannerInterviewCase.Interfaces;
using SanctionScannerInterviewCase.Models;
using SanctionScannerInterviewCase.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SanctionScannerInterviewCase.Services
{
    public class DataProccesorService : IDataProccesorService
    {
        public List<MainPageModel> BuildMainPage(string data)
        {
            try
            {
                var model = new List<MainPageModel>();
                var doc = new HtmlDocument();
                doc.LoadHtml(data);
                var findedHtml = doc.DocumentNode.SelectNodes(@"//*[@id='container']/div[3]/div/div[3]/div[3]/ul/li").Descendants("a");
                foreach (var item in findedHtml)
                {
                    if (item.Attributes["href"].Value.StartsWith(@"/ilan"))
                        model.Add(new MainPageModel
                        {
                            Url = item.Attributes["href"].Value,
                            Text = item.InnerText.Replace(Config.RegexSpaces),
                        });
                }
                return model;
            }
            catch
            {
                return new List<MainPageModel>();
            }

        }

        public DetailPageModel BuildDetailPage(string data)
        {
            try
            {
                var attrList = new List<AttributeModel>();
                var detailPage = new DetailPageModel();
                var doc = new HtmlDocument();
                doc.LoadHtml(data);
                var findedAttrHtml = doc.DocumentNode.SelectNodes(@"//*[@id='classifiedDetail']/div/div[2]/div[2]/ul/li");
                foreach (var item in findedAttrHtml)
                {
                    var title = item.Descendants("strong").FirstOrDefault();
                    var value = item.Descendants("span").FirstOrDefault();
                    attrList.Add(new AttributeModel
                    {
                        Title = title != null ? title.InnerText.Replace(Config.RegexSpaces) : "",
                        Value = value != null ? value.InnerText.Replace(Config.RegexSpaces).Replace(Config.RegexHtml) : "",
                    });
                }
                var price = "";
                var divFinder = 2;
                if (doc.DocumentNode.SelectSingleNode($"//*[@id='classifiedDetail']/div/div[{divFinder}]/div[2]/h3/text()") != null)
                {
                    price = doc.DocumentNode.SelectSingleNode($"//*[@id='classifiedDetail']/div/div[{divFinder}]/div[2]/h3/text()").InnerText;
                }
                else
                {
                    divFinder = 3;
                    price = doc.DocumentNode.SelectSingleNode($"//*[@id='classifiedDetail']/div/div[{divFinder}]/div[2]/h3/text()").InnerText;
                }
                var newPrice = price.Replace(Config.RegexNumber);
                var currency = price.Replace(Config.RegexLetter);
                var detailTitle = doc.DocumentNode.SelectSingleNode("//*[@id='classifiedDetail']/div/div[1]/h1").InnerText.Replace(Config.RegexSpaces);
                var city = doc.DocumentNode.SelectSingleNode($"//*[@id='classifiedDetail']/div/div[{divFinder}]/div[2]/h2/a[1]").InnerText.Replace(Config.RegexSpaces);
                var region = doc.DocumentNode.SelectSingleNode($"//*[@id='classifiedDetail']/div/div[{divFinder}]/div[2]/h2/a[2]").InnerText.Replace(Config.RegexSpaces);
                var state = "Diğer";
                if (doc.DocumentNode.SelectSingleNode($"//*[@id='classifiedDetail']/div/div[{divFinder}]/div[2]/h2/a[3]") != null)
                {
                    state = doc.DocumentNode.SelectSingleNode($"//*[@id='classifiedDetail']/div/div[{divFinder}]/div[2]/h2/a[3]").InnerText.Replace(Config.RegexSpaces);
                }
                var desc = doc.DocumentNode.SelectSingleNode("//*[@id='classifiedDescription']").InnerHtml;
                detailPage.AttributeList = attrList;
                detailPage.City = city;
                detailPage.Currency = currency;
                detailPage.Description = desc;
                detailPage.Price = decimal.Parse(newPrice);
                detailPage.Region = region;
                detailPage.State = state;
                detailPage.Title = detailTitle;
                return detailPage;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
