using Framework.Business.Pages;
using Framework.Core.Utilities;

namespace Framework.Tests.TestCases
{
    public class PositionSearch : BaseTest
    {
        public PositionSearch(string browser) : base(browser)
        {
        }

        [TestCase("Java", "All Locations", false)]
        [TestCase("Python", "All Locations", true)]
        public void ValidatePositionSearch(string keyword, string location, bool isHeadless)
        {
            Logger.LogInfo($"Starting position search test with keyword: {keyword}, location: {location}");

            Logger.LogInfo("Navigating to Careers page");
            CareersPage = HomePage.NavigateToCareers();

            Logger.LogInfo("Performing position search with keyword");
            CareersPage.FillSearchCriteria(keyword, location);

            Logger.LogInfo("Selecting last position and validating job description");
            JobDetailsPage = CareersPage.SearchAndSelectLastPosition();

            Assert.That(JobDetailsPage.ValidateJobDescription(keyword), Is.True,
                $"Job description does not contain the keyword: {keyword}");

            Logger.LogInfo("Position search test completed successfully");
        }
    }
}
