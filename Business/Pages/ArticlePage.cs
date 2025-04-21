using OpenQA.Selenium;
using Framework.Core.Pages;
using Framework.Core.Utilities;

namespace Framework.Business.Pages
{
    public class ArticlePage(IWebDriver driver) : BasePage(driver)
    {
        private readonly By _articleTitle = By.CssSelector("div.text-ui-23 span.museo-sans-light");

        public string GetArticleTitle()
        {
            WaitForPageLoad();
            var titleElement = WaitAndFindElement(_articleTitle);
            var title = titleElement.Text.Trim();
            Logger.LogInfo($"Found article title: {title}");
            return title;
        }
    }
}
