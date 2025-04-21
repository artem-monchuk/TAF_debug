using Framework.Business.Pages;
using Framework.Core.Utilities;

namespace Framework.Tests.TestCases
{
    public class AboutDownloadFile : BaseTest
    {
        public AboutDownloadFile(string browser) : base(browser)
        {
        }

        [TestCase("EPAM_Corporate_Overview_Q4FY-2024.pdf")]
        public void ValidateFileDownload(string expectedFileName)
        {
            Logger.LogInfo($"Starting file download test for: {expectedFileName}");

            HomePage = new HomePage(Driver);

            Logger.LogInfo("Navigating to About page");
            AboutPage = HomePage.NavigateToAbout();

            //download file
            AboutPage.ScrollToGlanceSection();
            Logger.LogInfo("Downloading corporate overview file");
            AboutPage.DownloadCorporateOverview();

            //verify download
            var downloadPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Downloads",
                expectedFileName);

            Assert.That(WaitForFileDownload(downloadPath), Is.True,
                $"File {expectedFileName} was not downloaded successfully");

            Logger.LogInfo("File download test completed successfully");
        }

        private static bool WaitForFileDownload(string filePath, int timeoutInSeconds = EXTENDED_TIMEOUT)
        {
            var timeout = TimeSpan.FromSeconds(timeoutInSeconds);
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            while (stopwatch.Elapsed < timeout)
            {
                if (File.Exists(filePath))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
