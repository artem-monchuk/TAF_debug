using OpenQA.Selenium;
using Framework.Core.Pages;
using Framework.Core.Utilities;

namespace Framework.Business.Pages
{
    public class HomePage(IWebDriver driver) : BasePage(driver)
    {
        //locators
        private readonly By _cookieAcceptButton = By.Id("onetrust-accept-btn-handler");
        private readonly By _searchIcon = By.CssSelector(".search-icon.dark-icon.header-search__search-icon");
        private readonly By _searchInput = By.Id("new_form_search");
        private readonly By _findButton = By.CssSelector(".custom-search-button");
        private readonly By _careersLink = By.XPath("//a[contains(@href, '/careers') and text()='Careers']");
        private readonly By _insightsLink = By.CssSelector("a.top-navigation__item-link[href='/insights']");
        private readonly By _aboutLink = By.CssSelector("a.top-navigation__item-link[href='/about']");

        public void AcceptCookies()
        {
            var cookieButton = WaitAndFindClickableElement(_cookieAcceptButton);
            ClickElement(cookieButton);
            Logger.LogInfo("Cookies accepted");
        }

        public SearchResultsPage PerformSearch(string searchText)
        {
            OpenSearchBox();
            EnterSearchText(searchText);
            ClickFindButton();
            return new SearchResultsPage(Driver);
        }

        private void OpenSearchBox()
        {
            var searchIconElement = WaitAndFindClickableElement(_searchIcon);
            ClickElement(searchIconElement);
            Logger.LogInfo("Clicked search icon");
        }

        private void EnterSearchText(string searchText)
        {
            var searchInputElement = WaitAndFindElement(_searchInput);
            searchInputElement.SendKeys(searchText);
            Logger.LogInfo($"Entered search text: {searchText}");
        }

        private void ClickFindButton()
        {
            var findButtonElement = WaitAndFindClickableElement(_findButton);
            ClickElement(findButtonElement);
            Logger.LogInfo("Clicked Find button");
            WaitForPageLoad();
        }

        public CareersPage NavigateToCareers()
        {
            var careers = WaitAndFindClickableElement(_careersLink);
            ClickElement(careers);
            WaitForPageLoad();
            Logger.LogInfo("Navigated to Careers page");
            return new CareersPage(Driver);
        }

        public InsightsPage NavigateToInsights()
        {
            var insights = WaitAndFindClickableElement(_insightsLink);
            ClickElement(insights);
            WaitForPageLoad();
            Logger.LogInfo("Navigated to Insights page");
            return new InsightsPage(Driver);
        }

        public AboutPage NavigateToAbout()
        {
            var aboutLinkElement = WaitAndFindClickableElement(_aboutLink);
            ClickElement(aboutLinkElement);
            WaitForPageLoad();
            Logger.LogInfo("Navigated to About page");
            return new AboutPage(Driver, GetDownloadPath());
        }
        private static string GetDownloadPath()
        {
            return Path.Combine(Environment.GetFolderPath(
                Environment.SpecialFolder.UserProfile), "Downloads");
        }
    }
}
