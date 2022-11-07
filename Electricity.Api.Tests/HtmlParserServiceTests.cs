using Electricity.Api.Services.HtmlParser;
using FluentAssertions;

namespace Electricity.Api.Tests
{
    public class HtmlParserServiceTests
    {
        private readonly HtmlParserService _sut;
        public HtmlParserServiceTests()
        {
            _sut = new HtmlParserService();
        }
        [Fact]
        public void GetCSVPublicUrlsFromParsedHtml_Should_Return_Urls()
        {
            int parseRowCount = 2;
            var result = _sut.GetCSVPublicUrlsFromParsedHtml(parseRowCount);

            result.Should().Contain(url => url.EndsWith(".csv"));
        }
    }
}
