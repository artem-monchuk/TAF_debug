using OpenQA.Selenium;
using Framework.Core.Pages;
using Framework.Core.Utilities;

namespace Framework.Business.Pages
{
    public class AboutPage : BasePage
    {
        private readonly By _glanceSection = By.XPath("//span[contains(text(),'EPAM at')]");
        private readonly By _downloadButton = By.XPath("//span[contains(@class,'button__content--desktop') and contains(text(),'DOWNLOAD')]");

        private readonly string _downloadPath;
        public AboutPage(IWebDriver driver, string downloadPath) : base(driver)
        {
            _downloadPath = downloadPath;
        }

        public AboutPage(IWebDriver driver) : base(driver)
        {
            _downloadPath = string.Empty; //initialize _downloadPath to avoid CS8618
        }

        public void ScrollToGlanceSection()
        {
            var glanceSectionElement = WaitAndFindElement(_glanceSection);
            ScrollToElement(glanceSectionElement);
            Logger.LogInfo("Scrolled to EPAM at Glance section");
        }

        public void DownloadCorporateOverview()
        {
            var downloadButtonElement = WaitAndFindClickableElement(_downloadButton);
            ClickElement(downloadButtonElement);
            Logger.LogInfo("Clicked download button");
        }

        public bool VerifyFileDownload(string expectedFileName)
        {
            var filePath = Path.Combine(_downloadPath, expectedFileName);
            var isFileDownloaded = WaitForDownload(filePath);

            if (isFileDownloaded)
                Logger.LogInfo($"File {expectedFileName} downloaded successfully");
            else
                Logger.LogError($"File {expectedFileName} was not downloaded successfully");

            return isFileDownloaded;
        }

        private static bool WaitForDownload(string filePath, int timeoutInSeconds = EXTENDED_TIMEOUT)
        {
            var endTime = DateTime.Now.AddSeconds(timeoutInSeconds);
            while (DateTime.Now < endTime)
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
