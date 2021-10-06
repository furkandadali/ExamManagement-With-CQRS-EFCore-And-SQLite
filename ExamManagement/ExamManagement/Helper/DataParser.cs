using ExamManagement.Models;
using System;
using System.Collections.Generic;
using HtmlAgilityPack;
using System.Net;

namespace ExamManagement.Helper
{
    public class DataParser : Interfaces.IGetWebPageContent
    {
        public IDictionary<int, ContentModel> GetWebPageContent(int articleIndex)
        {
            try
            {
                HtmlWeb web = new HtmlWeb();
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                var giris_deg = web.Load(Encipher.DecryptString(SettingsFile.sourceUrl));
                var divList = giris_deg.DocumentNode.SelectNodes(".//div[@class ='cards-component']//div[contains(@class, 'card-component card-component')]");

                IDictionary<int, ContentModel> articleList = new Dictionary<int, ContentModel>();

                for (int i = 0; i < articleIndex; i++)
                {
                    var itemUrl = divList[i].SelectSingleNode(".//a").Attributes["href"].Value.Trim();
                    itemUrl = Encipher.DecryptString(SettingsFile.sourceUrl) + WebUtility.HtmlDecode(itemUrl);
                    var contentPage = web.Load(itemUrl);

                    if (contentPage != null)
                    {
                        string uniqueKey = string.Empty;
                        var article = string.Empty;

                        var header = WebUtility.HtmlDecode(contentPage.DocumentNode.SelectSingleNode(".//h1[@class = 'content-header__row content-header__hed']").InnerText.Trim());
                        var shortInfo = WebUtility.HtmlDecode(contentPage.DocumentNode.SelectSingleNode(".//div[@class ='content-header__row content-header__dek']").InnerText.Trim());
                        var articleSections = contentPage.DocumentNode.SelectNodes(".//div[@class = 'grid--item body body__container article__body grid-layout__content']");

                        if (!string.IsNullOrEmpty(shortInfo))
                        {
                            //uniqueKey = shortInfo.GetHashCode().ToString().Trim();
                            uniqueKey = shortInfo.Replace(" ","").Trim();
                        }

                        if (articleSections != null && articleSections.Count > 1)
                        {
                            foreach (var articleItem in articleSections)
                            {
                                article = article + WebUtility.HtmlDecode(articleItem.InnerText.Trim());
                            }
                        }
                        else if (articleSections != null && articleSections.Count == 1)
                        {
                            article = WebUtility.HtmlDecode(articleSections[0].InnerText.Trim());
                        }

                        articleList.Add(i, new ContentModel
                        {
                            ArticleKey = uniqueKey,
                            Header = header,
                            URL = itemUrl,
                            ShortInfo = shortInfo,
                            Content = article,
                        });
                    }
                }
                return articleList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
