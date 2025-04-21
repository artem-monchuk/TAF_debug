using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace Framework.Core.Utilities
{
    public static class WaitHelper
    {
        private const string Script = "return document.readyState";

        public static IWebElement WaitUntilElementVisible(this IWebDriver driver, By locator, int timeoutInSeconds = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            try
            {
                return wait.Until(ExpectedConditions.ElementIsVisible(locator));
            }
            catch (Exception ex)
            {
                Logger.LogError($"Element not visible: {locator}");
                throw new NoSuchElementException($"Element not visible: {locator}", ex);
            }
        }

        public static IWebElement WaitUntilElementClickable(this IWebDriver driver, By locator, int timeoutInSeconds = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            try
            {
                return wait.Until(ExpectedConditions.ElementToBeClickable(locator));
            }
            catch (Exception ex)
            {
                Logger.LogError($"Element not clickable: {locator}");
                throw new ElementNotInteractableException($"Element not clickable: {locator}", ex);
            }
        }

        public static IReadOnlyCollection<IWebElement> WaitUntilElementsPresent(this IWebDriver driver, By locator, int timeoutInSeconds = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            try
            {
                return wait.Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(locator));
            }
            catch (Exception ex)
            {
                Logger.LogError($"Elements not present: {locator}");
                throw new NoSuchElementException($"Elements not present: {locator}", ex);
            }
        }

        public static bool WaitUntilPageLoaded(this IWebDriver driver, int timeoutInSeconds = 10)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
            try
            {
                return wait.Until(d => ((IJavaScriptExecutor)d)
                    .ExecuteScript(Script)?.Equals("complete") == true);
            }
            catch (Exception ex)
            {
                Logger.LogError("Page load timeout");
                throw new WebDriverTimeoutException("Page load timeout", ex);
            }
        }
    }
}
