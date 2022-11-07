using HtmlAgilityPack;

namespace Electricity.Api.Services.HtmlParser
{
    public class HtmlParserService : IHtmlParserService
    {
        private const string UrlToParse = "https://data.gov.lt/dataset/siame-duomenu-rinkinyje-pateikiami-atsitiktinai-parinktu-1000-buitiniu-vartotoju-automatizuotos-apskaitos-elektriniu-valandiniai-duomenys";
        public IEnumerable<string> GetCSVPublicUrlsFromParsedHtml(int parseRowCountDesc)
        {
            var web = new HtmlWeb();
            var doc = web.Load(UrlToParse);

            var rows = doc.DocumentNode.SelectNodes($"//table[@id='resource-table']//tr[position() > last()-{parseRowCountDesc}]//a[2]");

            return rows.Select(r => $"https://data.gov.lt/{r.Attributes["href"].Value}");
        }
    }
}
