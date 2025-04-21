using OpenQA.Selenium;
using Framework.Core.Pages;
using Framework.Core.Utilities;

namespace Framework.Business.Pages
{
    public class SearchResultsPage(IWebDriver driver) : BasePage(driver)
    {
        private readonly By _searchResults = By.CssSelector(".search-results__item");
        private readonly By _resultTitleLink = By.CssSelector("a.search-results__title-link");

        public IReadOnlyCollection<IWebElement> GetSearchResults()
        {
            var results = WaitAndFindElements(_searchResults);

            if (results.Count == 0)
            {
                throw new Exception("No search results found");
            }

            Logger.LogInfo($"Found {results.Count} search results");
            return results;
        }

        public List<string> GetFailingLinks(string searchText)
        {
            var results = GetSearchResults();

            var failingLinks = results
                .Where(result => !result.Text.Contains(searchText))
                .Select(result => result.FindElement(_resultTitleLink).Text)
                .ToList();

            if (failingLinks.Any())
            {
                foreach (var link in failingLinks)
                {
                    Logger.LogError($"Failed link: {link}");
                }
            }

            return failingLinks;
        }
    }
}
