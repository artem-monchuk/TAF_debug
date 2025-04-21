using OpenQA.Selenium;
using Framework.Core.Utilities;

namespace Framework.Core.Pages
{
    public abstract class BasePage(IWebDriver driver)
    {
        protected readonly IWebDriver Driver = driver;
        protected const int DEFAULT_TIMEOUT = 10;
        protected const int EXTENDED_TIMEOUT = 30;
        protected const int SHORT_TIMEOUT = 5;

        protected IWebElement WaitAndFindElement(By locator, int timeoutInSeconds = DEFAULT_TIMEOUT, int timeout = 0)
        {
            return Driver.WaitUntilElementVisible(locator, timeoutInSeconds);
        }

        protected IWebElement WaitAndFindClickableElement(By locator, int timeoutInSeconds = DEFAULT_TIMEOUT)
        {
            return Driver.WaitUntilElementClickable(locator, timeoutInSeconds);
        }

        protected IReadOnlyCollection<IWebElement> WaitAndFindElements(By locator, int timeoutInSeconds = DEFAULT_TIMEOUT)
        {
            return Driver.WaitUntilElementsPresent(locator, timeoutInSeconds);
        }

        protected void WaitForPageLoad()
        {
            Driver.WaitUntilPageLoaded();
        }

        protected void ClickElement(IWebElement element)
        {
            element.Click(Driver);
        }

        protected void ScrollToElement(IWebElement element)
        {
            element.ScrollIntoView(Driver);
        }
    }
}
