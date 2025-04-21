using OpenQA.Selenium;

namespace Framework.Core.Utilities
{
    public static class ElementHelper
    {
        public static void Click(this IWebElement element, IWebDriver driver)
        {
            element.Click();
            return;
        }

        public static void ScrollIntoView(this IWebElement element, IWebDriver driver)
        {
            //new Actions(driver).ScrollToElement(element).Perform();
            //js scroll is needed for Firefox
            var js = (IJavaScriptExecutor)driver;
            js.ExecuteScript("arguments[0].scrollIntoView({behavior: 'instant', block: 'center'});", element);
        }

        public static bool IsDisplayed(this IWebElement element)
        {
                return element.Displayed;
        }
    }
}
