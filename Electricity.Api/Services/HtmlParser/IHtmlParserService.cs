namespace Electricity.Api.Services.HtmlParser
{
    public interface IHtmlParserService
    {
        IEnumerable<string> GetCSVPublicUrlsFromParsedHtml(int parseRowCountDesc);
    }
}