using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Framework.Core.Pages;
using Framework.Core.Utilities;

namespace Framework.Business.Pages
{
    public class InsightsPage(IWebDriver driver) : BasePage(driver)
    {
        private readonly By _carouselNextButton = By.CssSelector("button.owl-next");
        private readonly By _activeSlide = By.CssSelector("div.owl-item.active");
        private readonly By _titleContainer = By.CssSelector("div.text-ui-23");
        private readonly By _readMoreButton = By.CssSelector("a.link-with-bottom-arrow, a.link-with-right-arrow");
        private readonly WebDriverWait wait = new(driver, TimeSpan.FromSeconds(10));
        private const int DEFAULT_SWIPE_COUNT = 2;

        public void SwipeCarouselNext()
        {
            var nextButton = wait.Until(driver => driver.FindElement(_carouselNextButton));
            //nextButton.Click();
            ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].click();", nextButton);
            Logger.LogInfo("Swiped carousel to next item");
        }

        public string GetCarouselArticleTitle()
        {
            var slide = WaitAndFindElement(_activeSlide);
            var titleElement = slide.FindElement(_titleContainer);
            string carouselTitle = titleElement.Text.Trim();

            Assert.That(carouselTitle, Is.Not.Empty, "Carousel title should not be empty");
            Logger.LogInfo($"Found carousel article: {carouselTitle}");

            return carouselTitle;
        }

        public IWebElement GetReadMoreButton()
        {
            var slide = WaitAndFindElement(_activeSlide);
            return slide.FindElement(_readMoreButton);
        }

        public ArticlePage ClickReadMoreButton(IWebElement readMoreButton)
        {
            ScrollToElement(readMoreButton);
            ClickElement(readMoreButton);
            Logger.LogInfo("Clicked Read More button");
            return new ArticlePage(Driver);
        }
    }
}
