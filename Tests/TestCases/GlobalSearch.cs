using Framework.Business.Pages;
using Framework.Core.Utilities;

namespace Framework.Tests.TestCases
{
    public class GlobalSearch : BaseTest
    {
        public GlobalSearch(string browser) : base(browser)
        {
        }

        [TestCase("Automation")]
        [TestCase("Cloud")]
        public void ValidateGlobalSearch(string searchText)
        {
            //perform search and get results page
            Logger.LogInfo($"Starting global search test with search term: {searchText}");
            SearchResultsPage = HomePage.PerformSearch(searchText);

            Logger.LogInfo("Validating search results");
            var failingLinks = SearchResultsPage.GetFailingLinks(searchText);

            Assert.That(failingLinks, Is.Empty,
                $"{failingLinks.Count} links did not contain the search term: {searchText}");

            Logger.LogInfo("Global search test completed successfully");
        }
    }
}
