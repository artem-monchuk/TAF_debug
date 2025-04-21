using Framework.Core.Utilities;
using Framework.Business.Pages;

namespace Framework.Tests.TestCases
{
    public class InsightsCarouselArticle : BaseTest
    {
        public InsightsCarouselArticle(string browser) : base(browser)
        {
        }

        [TestCase]
        public void ValidateCarouselArticle()
        {

            Logger.LogInfo("Starting carousel article validation test");

            InsightsPage = HomePage.NavigateToInsights();

            Logger.LogInfo("Swiping carousel to next item");
            InsightsPage.SwipeCarouselNext();

            Logger.LogInfo("Getting carousel article title");
            string carouselTitle = InsightsPage.GetCarouselArticleTitle();

            //click read more button
            var readMoreBtn = InsightsPage.GetReadMoreButton();
            ArticlePage = InsightsPage.ClickReadMoreButton(readMoreBtn);
            Logger.LogInfo("Navigated to article page");

            //validate article title
            var articleTitle = ArticlePage.GetArticleTitle();

            Assert.That(articleTitle, Is.EqualTo(carouselTitle),
                    $"Article title '{articleTitle}' does not match carousel title '{carouselTitle}'");

            Logger.LogInfo("Carousel article validation completed successfully");
        }
    }
}
