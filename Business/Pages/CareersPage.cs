using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Framework.Core.Pages;
using Framework.Core.Utilities;

namespace Framework.Business.Pages
{
    public class CareersPage(IWebDriver driver) : BasePage(driver)
    {
        private readonly By _keywordInput = By.Id("new_form_job_search-keyword");
        private readonly By _locationDropdown = By.CssSelector("span[aria-labelledby='select2-new_form_job_search-location-container']");
        private readonly By _allLocationsOption = By.XPath("//li[contains(text(), 'All Locations')]");
        private readonly By _remoteCheckbox = By.CssSelector("input[name='remote']");
        private readonly By _findButton = By.CssSelector("button[class*='job-search-button']");
        private readonly By _searchResults = By.CssSelector(".search-result__item");
        private readonly By _applyButton = By.XPath(".//a[contains(@class, 'search-result__item-apply')]");
        private readonly WebDriverWait wait = new(driver, TimeSpan.FromSeconds(DEFAULT_TIMEOUT));
        public void FillSearchCriteria(string keyword, string location)
        {
            EnterKeyword(keyword);
            SelectLocation(location);
            CheckRemoteOption();

            Logger.LogInfo($"Filled search criteria - Keyword: {keyword}, Location: {location}");
        }

        private void EnterKeyword(string keyword)
        {
            var keywordField = WaitAndFindElement(_keywordInput);
            keywordField.Clear();
            keywordField.SendKeys(keyword);

            Assert.That(keywordField.GetAttribute("value"), Is.EqualTo(keyword), "Keyword field value is not as expected");
            Logger.LogInfo($"Entered keyword: {keyword}");
        }

        private void SelectLocation(string location)
        {
            var locationDropdownElement = WaitAndFindClickableElement(_locationDropdown);
            ClickElement(locationDropdownElement);

            var locationOption = WaitAndFindClickableElement(_allLocationsOption);
            ClickElement(locationOption);

            Assert.That(locationDropdownElement.Text, Is.EqualTo(location), "Location dropdown value is not as expected");
            Logger.LogInfo($"Selected location: {location}");
        }

        private void CheckRemoteOption()
        {
            var remote = wait.Until(driver => driver.FindElement(_remoteCheckbox));
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", remote);

            Assert.That(remote.Selected, Is.True, "Remote checkbox is not checked");
            Logger.LogInfo("Checked Remote checkbox");
        }

        public JobDetailsPage SearchAndSelectLastPosition()
        {
            ClickFindButton();
            var searchResults = WaitAndFindElements(_searchResults);
            var lastResult = GetLastSearchResult(searchResults);
            ClickViewAndApply(lastResult);
            return new JobDetailsPage(Driver);
        }

        private void ClickFindButton()
        {
            var findButtonElement = WaitAndFindClickableElement(_findButton);
            ClickElement(findButtonElement);
            WaitForPageLoad();

            Logger.LogInfo("Clicked Find button");
        }

        private IWebElement GetLastSearchResult(IReadOnlyCollection<IWebElement> searchResults)
        {
            if (searchResults.Count == 0)
            {
                throw new Exception("No search results found");
            }
            Logger.LogInfo($"Found {searchResults.Count} search results");
            return searchResults.Last();
        }

        //native click works in Firefox only
        //private void ClickViewAndApply(IWebElement lastResult)
        //{
        //    var applyButtonElement = lastResult.FindElement(_applyButton);
        //    ScrollToElement(applyButtonElement);
        //    ClickElement(applyButtonElement);
        //    WaitForPageLoad();

        //    Logger.LogInfo("Clicked View and Apply button");
        //}
        private void ClickViewAndApply(IWebElement lastResult)
        {
            var applyButtonElement = lastResult.FindElement(_applyButton);
            ScrollToElement(applyButtonElement);

            try
            {
                applyButtonElement.Click();
            }
            catch (ElementClickInterceptedException)
            {
                Logger.LogWarning("First click attempt failed, trying alternative approach");
                ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", applyButtonElement);
            }
            WaitForPageLoad();

            Logger.LogInfo("Successfully clicked View and Apply button");
        }
    }
}
