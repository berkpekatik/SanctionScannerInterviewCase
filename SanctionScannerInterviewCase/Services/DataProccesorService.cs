using HtmlAgilityPack;
using SanctionScannerInterviewCase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SanctionScannerInterviewCase.Services
{
    public class DataProccesorService
    {
        private string regexSpaces = "[\\r|\\n|\\t]\\s\\s+";
        private string regexHtml = @"<[^>]+>|&nbsp;";
        private string regexNumber = @"[^0-9.]";
        private string regexLetter = @"[^A-Z]";
        public List<MainPageModel> BuildMainPage(string data)
        {
            try
            {
                var model = new List<MainPageModel>();
                var doc = new HtmlDocument();
                doc.LoadHtml(data);
                var findedHtml = doc.DocumentNode.SelectNodes(@"//*[@id='container']/div[3]/div/div[3]/div[3]/ul/li").Descendants("a");//Anasayfa vitrin içerisinde ki ul içerisinde li listesi getiriliyor.
                foreach (var item in findedHtml)
                {
                    if (item.Attributes["href"].Value.StartsWith(@"/ilan")) //reklamları atlamak için /ilan path olanlar ayıklanıyor.
                        model.Add(new MainPageModel
                        {
                            Url = item.Attributes["href"].Value,
                            Text = regexParser(item.InnerText, regexSpaces),//regex yöntemiyle text sadeleştiriliyor.
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
                var findedAttrHtml = doc.DocumentNode.SelectNodes(@"//*[@id='classifiedDetail']/div/div[2]/div[2]/ul/li");//İlan içerisindeki detaylar getiriliyor.
                foreach (var item in findedAttrHtml)
                {
                    var title = item.Descendants("strong").FirstOrDefault(); //strong ve span türünde olduğu için sınırlandırılıyor.
                    var value = item.Descendants("span").FirstOrDefault();
                    attrList.Add(new AttributeModel
                    {
                        Title = title != null ? regexParser(title.InnerText, regexSpaces) : "",
                        Value = value != null ? regexParser(regexParser(value.InnerText, regexSpaces), regexHtml) : "",
                    });
                }
                var price = "";
                var divFinder = 2;
                if (doc.DocumentNode.SelectSingleNode($"//*[@id='classifiedDetail']/div/div[{divFinder}]/div[2]/h3/text()") != null) //site içerisinde anlamadığım bir olay var test ederek çözdüm bazı sayfalarda xpath içerisinde div numarası değişiyor bu yüzden böyle bir şey ile çözdüm.
                {
                    price = doc.DocumentNode.SelectSingleNode($"//*[@id='classifiedDetail']/div/div[{divFinder}]/div[2]/h3/text()").InnerText;
                }
                else
                {
                    divFinder = 3;
                    price = doc.DocumentNode.SelectSingleNode($"//*[@id='classifiedDetail']/div/div[{divFinder}]/div[2]/h3/text()").InnerText;
                }
                var newPrice = regexParser(price, regexNumber);
                var currency = regexParser(price, regexLetter);
                var detailTitle = regexParser(doc.DocumentNode.SelectSingleNode("//*[@id='classifiedDetail']/div/div[1]/h1").InnerText, regexSpaces);
                var city = regexParser(doc.DocumentNode.SelectSingleNode($"//*[@id='classifiedDetail']/div/div[{divFinder}]/div[2]/h2/a[1]").InnerText, regexSpaces);
                var region = regexParser(doc.DocumentNode.SelectSingleNode($"//*[@id='classifiedDetail']/div/div[{divFinder}]/div[2]/h2/a[2]").InnerText, regexSpaces);
                var state = "Diğer";
                if (doc.DocumentNode.SelectSingleNode("//*[@id='classifiedDetail']/div/div[{divFinder}]/div[2]/h2/a[3]") != null)
                {
                    state = regexParser(doc.DocumentNode.SelectSingleNode($"//*[@id='classifiedDetail']/div/div[{divFinder}]/div[2]/h2/a[3]").InnerText, regexSpaces);
                }
                var desc = doc.DocumentNode.SelectSingleNode("//*[@id='classifiedDescription']").InnerHtml;
                detailPage.AttributeList = attrList;
                detailPage.City = city;
                detailPage.Currency = currency;
                detailPage.Description = desc;
                detailPage.Price = decimal.Parse(price);
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

        private string regexParser(string text, string regex)
        {
            return Regex.Replace(text, regex, string.Empty);
        }
    }
}
